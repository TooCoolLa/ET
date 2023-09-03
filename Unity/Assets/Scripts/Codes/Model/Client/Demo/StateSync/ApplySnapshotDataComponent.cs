using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ApplySnapshotDataComponent: Entity, IUpdate, IAwake
    {
        public M2C_Snapshot Snapshot;
        //public M2C_Snapshot LastFrameSnapshot;
        public long myUnitID;
        //public int StartFrameCount;
        public int AuthFrameCount;
        public Dictionary<long, StateInfo> StateBuff;
        //public StateInfo AllDelta;
        public int FrameInterval;
        //public int MaxPredictFrameCount;
        public float moveSpeed;
        public bool inPredict;
    }
}