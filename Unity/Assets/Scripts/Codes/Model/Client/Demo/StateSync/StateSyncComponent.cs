namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class StateSyncComponent: Entity, IUpdate, IAwake
    {
        public float frameTime = 1f / 20f;
        public M2C_Snapshot Snapshot;
        public StateInfo lastTimeState;
    }
}