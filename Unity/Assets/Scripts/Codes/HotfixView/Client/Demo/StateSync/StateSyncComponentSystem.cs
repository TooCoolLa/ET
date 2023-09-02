namespace ET.Client
{
    [FriendOf(typeof (StateSyncComponent))]
    public static class StateSyncComponentSystem
    {
        [ObjectSystem]
        public class StateSyncComponentAwakeSystem: AwakeSystem<StateSyncComponent>
        {
            protected override void Awake(StateSyncComponent self)
            {
                self.StateSync().Coroutine();
            }
        }

        private static async ETTask StateSync(this StateSyncComponent self)
        {
            while (true)
            {
                await TimerComponent.Instance.WaitAsync((long)self.frameTime * 1000l);
                var scene = self.DomainScene();
                SessionComponent session = scene.GetComponent<SessionComponent>();
                Unit MyUnit = UnitHelper.GetMyUnitFromClientScene(scene);
                var MyState = new StateInfo() { Rotation = MyUnit.Rotation, Position = MyUnit.Position };
                M2C_Snapshot snapshot = await session.Session.Call(new C2M_StateSync() { MyState = MyState, }) as M2C_Snapshot;
                self.Snapshot = snapshot;
                self.lastTimeState = MyState;
                ApplySnapshotDataComponent applySnapshotDataComponent = scene.GetComponent<ApplySnapshotDataComponent>();
                applySnapshotDataComponent.SetSnapshotData(snapshot);
                Logger.Instance.Debug($"接收到服务器快照数据{snapshot}");
            }
        }
    }
}