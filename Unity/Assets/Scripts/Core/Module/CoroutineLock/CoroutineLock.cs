using System;

namespace ET
{
    /// <summary>
    /// 协程锁类型，记录类型，key以及level
    /// </summary>
    public class CoroutineLock: IDisposable
    {
        private int type;
        private long key;
        private int level;
        
        public static CoroutineLock Create(int type, long k, int count)
        {
            CoroutineLock coroutineLock = ObjectPool.Instance.Fetch<CoroutineLock>();
            coroutineLock.type = type;
            coroutineLock.key = k;
            coroutineLock.level = count;
            return coroutineLock;
        }
        /// <summary>
        /// 回收时候调用RunNextCoroutine
        /// </summary>
        public void Dispose()
        {
            CoroutineLockComponent.Instance.RunNextCoroutine(this.type, this.key, this.level + 1);
            
            this.type = CoroutineLockType.None;
            this.key = 0;
            this.level = 0;
            
            ObjectPool.Instance.Recycle(this);
        }
    }
}