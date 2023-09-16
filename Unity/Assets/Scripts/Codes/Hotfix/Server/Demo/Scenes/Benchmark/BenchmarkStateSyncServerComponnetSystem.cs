namespace ET.Server;

public static class BenchmarkStateSyncServerComponnetSystem
{
    [ObjectSystem]
    public class AwakeSystem : AwakeSystem<BenchmarkStateSyncServerComponent>
    {
        protected override void Awake(BenchmarkStateSyncServerComponent self)
        {
            
        }
    }
}