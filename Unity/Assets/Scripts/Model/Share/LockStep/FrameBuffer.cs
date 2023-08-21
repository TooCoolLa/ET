using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    public class FrameBuffer
    {
        public int MaxFrame { get; private set; }
        private readonly List<OneFrameInputs> frameInputs;
        private readonly List<MemoryBuffer> snapshots;
        private readonly List<long> hashs;

        public FrameBuffer(int frame = 0, int capacity = LSConstValue.FrameCountPerSecond * 60)
        {
            this.MaxFrame = frame + LSConstValue.FrameCountPerSecond * 30;
            this.frameInputs = new List<OneFrameInputs>(capacity);
            this.snapshots = new List<MemoryBuffer>(capacity);
            this.hashs = new List<long>(capacity);
            
            for (int i = 0; i < this.snapshots.Capacity; ++i)
            {
                this.hashs.Add(0);
                this.frameInputs.Add(new OneFrameInputs());
                MemoryBuffer memoryBuffer = new(10240);
                memoryBuffer.SetLength(0);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                this.snapshots.Add(memoryBuffer);
            }
        }

        public void SetHash(int frame, long hash)
        {
            EnsureFrame(frame);
            this.hashs[frame % this.frameInputs.Capacity] = hash;
        }
        
        public long GetHash(int frame)
        {
            EnsureFrame(frame);
            return this.hashs[frame % this.frameInputs.Capacity];
        }
        /// <summary>
        /// 检查输入帧是否有效
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public bool CheckFrame(int frame)
        {
            if (frame < 0)
            {
                return false;
            }

            if (frame > this.MaxFrame)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 检查一下输入帧的有效性，如果无效，则来个警告
        /// </summary>
        /// <param name="frame"></param>
        /// <exception cref="Exception"></exception>
        private void EnsureFrame(int frame)
        {
            if (!CheckFrame(frame))
            {
                throw new Exception($"frame out: {frame}, maxframe: {this.MaxFrame}");
            }
        }
        /// <summary>
        /// 获取输入帧的玩家输入情况
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public OneFrameInputs FrameInputs(int frame)
        {
            EnsureFrame(frame);
            OneFrameInputs oneFrameInputs = this.frameInputs[frame % this.frameInputs.Capacity];
            return oneFrameInputs;
        }
        /// <summary>
        /// 保持最大帧与输入帧至少差1秒，如果差值不足一秒，则最大帧自增，并且清空对应的帧输入
        /// </summary>
        /// <param name="frame"></param>
        public void MoveForward(int frame)
        {
            if (this.MaxFrame - frame > LSConstValue.FrameCountPerSecond) // 至少留出1秒的空间
            {
                return;
            }
            
            ++this.MaxFrame;
            
            OneFrameInputs oneFrameInputs = this.FrameInputs(this.MaxFrame);
            oneFrameInputs.Inputs.Clear();
        }

        public MemoryBuffer Snapshot(int frame)
        {
            EnsureFrame(frame);
            MemoryBuffer memoryBuffer = this.snapshots[frame % this.snapshots.Capacity];
            return memoryBuffer;
        }
    }
}