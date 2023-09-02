namespace ET.Client
{
    [FriendOf(typeof (ApplySnapshotDataComponent))]
    public static class AppleSnapshotDataComponentSystem
    {
        [ObjectSystem]
        public class AppleSnapshotDataComponentAwakeSystem: AwakeSystem<ApplySnapshotDataComponent>
        {
            protected override void Awake(ApplySnapshotDataComponent self)
            {
            }
        }

        [ObjectSystem]
        public class AppleSnapshotDataComponentUpdateSystem: UpdateSystem<ApplySnapshotDataComponent>
        {
            protected override void Update(ApplySnapshotDataComponent self)
            {
                var snapshort = self.Snapshot;
                UnitComponent unitComponent = self.DomainScene().GetComponent<UnitComponent>();
                if (snapshort != null && snapshort.OtherUnits != null && snapshort.OtherUnits.Count > 0)
                    foreach (var stateInfo in snapshort.OtherUnits)
                    {
                        Unit unit = unitComponent.Get(stateInfo.UnitID);
                        if (unit != null)
                        {
                            unit.Position = stateInfo.Position;
                            unit.Rotation = stateInfo.Rotation;
                        }
                        else
                        {
                            unit = UnitFactory.Create(self.DomainScene(), stateInfo);
                        }
                    }
            }
        }

        public static void SetSnapshotData(this ApplySnapshotDataComponent self, M2C_Snapshot snapshot)
        {
            self.Snapshot = snapshot;
        }
    }
}