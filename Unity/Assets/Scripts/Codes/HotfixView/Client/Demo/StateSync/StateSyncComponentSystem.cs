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
            long frameTime = (long)(self.frameTime * 1000f);
            while (true)
            {
                var start = TimeHelper.ClientNow();
                var scene = self.DomainScene();
                SessionComponent session = scene.GetComponent<SessionComponent>();
                Unit MyUnit = UnitHelper.GetMyUnitFromClientScene(scene);
                MoveByInput input = scene.GetComponent<MoveByInput>();
                var MyState = new StateInfo()
                {
                    Rotation = MyUnit.Rotation, Position = MyUnit.Position, UnitID = MyUnit.Id, Input = input.GetInput(),
                };
                M2C_Snapshot snapshot = await session.Session.Call(new C2M_StateSync() { MyState = MyState, }) as M2C_Snapshot;
                self.Snapshot = snapshot;
                self.lastTimeState = MyState;
                ApplySnapshotDataComponent applySnapshotDataComponent = scene.GetComponent<ApplySnapshotDataComponent>();
                applySnapshotDataComponent.SetSnapshotData(snapshot, MyUnit.Id);
                var finish = TimeHelper.ClientNow();
                var delta = finish - start;
                var wait = frameTime - delta;
                if (wait > 0)
                    await TimerComponent.Instance.WaitAsync(wait);
                //Logger.Instance.Debug($"接收到服务器快照数据{snapshot}");
            }
        }
    }
}