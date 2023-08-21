using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class Replay
    {
        /// <summary>
        /// 所有单位的初始位置
        /// </summary>
        [MemoryPackOrder(1)]
        public List<LockStepUnitInfo> UnitInfos;
        /// <summary>
        /// 记录每一个单位在每帧的输入
        /// </summary>
        [MemoryPackOrder(2)]
        public List<OneFrameInputs> FrameInputs = new();
        /// <summary>
        /// 快照缓存（帧）
        /// </summary>
        [MemoryPackOrder(3)]
        public List<byte[]> Snapshots = new();
    }
}