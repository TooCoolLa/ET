namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ApplySnapshotDataComponent: Entity, IUpdate, IAwake
    {
        public M2C_Snapshot Snapshot;
        public long myUnitID;
    }
}