namespace ET.Server;
[ComponentOf(typeof(BenchmarkStateSyncClientCompoennt))]
public class BenchmarkPlayerClientComponent : Entity,IAwake<Scene,string>
{
    public Scene microClientScene;
    
}