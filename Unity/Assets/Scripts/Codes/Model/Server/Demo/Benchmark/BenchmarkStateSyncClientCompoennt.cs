namespace ET.Server;
[ComponentOf(typeof(Scene))]
public class BenchmarkStateSyncClientCompoennt : Entity,IAwake
{
    public int OnlinePlayerNums = 100;
}