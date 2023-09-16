namespace ET.Server;

public static class BenchmarkStateSyncComponentSystem
{
    [ObjectSystem]
    public class AwakeSystem : AwakeSystem<BenchmarkStateSyncClientCompoennt>
    {
        protected override void Awake(BenchmarkStateSyncClientCompoennt self)
        {
            
        }
    }
}