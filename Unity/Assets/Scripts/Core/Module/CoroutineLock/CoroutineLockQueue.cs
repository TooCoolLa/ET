using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 协程锁队列
    /// </summary>
    public class CoroutineLockQueue
    {
        private int type;
        private long key;
        
        public static CoroutineLockQueue Create(int type, long key)
        {
            CoroutineLockQueue coroutineLockQueue = ObjectPool.Instance.Fetch<CoroutineLockQueue>();
            coroutineLockQueue.type = type;
            coroutineLockQueue.key = key;
            return coroutineLockQueue;
        }
        /// <summary>
        /// 当前协程锁
        /// </summary>
        private CoroutineLock currentCoroutineLock;
        /// <summary>
        /// 等待协程锁队列
        /// </summary>
        private readonly Queue<WaitCoroutineLock> queue = new Queue<WaitCoroutineLock>();

        public int Count
        {
            get
            {
                return this.queue.Count;
            }
        }
        /// <summary>
        /// 等待方法
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public async ETTask<CoroutineLock> Wait(int time)
        {
            //如果当前没有协程锁，则创建一个，从1开始
            if (this.currentCoroutineLock == null)
            {
                this.currentCoroutineLock = CoroutineLock.Create(type, key, 1);
                return this.currentCoroutineLock;
            }
            //创建一个等待协程锁
            WaitCoroutineLock waitCoroutineLock = WaitCoroutineLock.Create();
            //等待协程锁队列入列
            this.queue.Enqueue(waitCoroutineLock);
            //在waitCoroutineLock对象上创建一个计时器，
            if (time > 0)
            {
                long tillTime = TimeHelper.ClientFrameTime() + time;
                TimerComponent.Instance.NewOnceTimer(tillTime, TimerCoreInvokeType.CoroutineTimeout, waitCoroutineLock);
            }
            this.currentCoroutineLock = await waitCoroutineLock.Wait();
            return this.currentCoroutineLock;
        }

        public void Notify(int level)
        {
            // 有可能WaitCoroutineLock已经超时抛出异常，所以要找到一个未处理的WaitCoroutineLock
            while (this.queue.Count > 0)
            {
                WaitCoroutineLock waitCoroutineLock = queue.Dequeue();

                if (waitCoroutineLock.IsDisposed())
                {
                    continue;
                }

                CoroutineLock coroutineLock = CoroutineLock.Create(type, key, level);

                waitCoroutineLock.SetResult(coroutineLock);
                break;
            }
        }

        public void Recycle()
        {
            this.queue.Clear();
            this.key = 0;
            this.type = 0;
            this.currentCoroutineLock = null;
            ObjectPool.Instance.Recycle(this);
        }
    }
}