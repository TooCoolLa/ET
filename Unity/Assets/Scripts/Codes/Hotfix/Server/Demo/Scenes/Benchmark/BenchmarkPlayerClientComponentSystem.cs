using System;
using ET.Client;

namespace ET.Server;

public static class BenchmarkPlayerClientComponentSystem
{
    [ObjectSystem]
    public class AwakeSystem : AwakeSystem<BenchmarkPlayerClientComponent,Scene,string>
    {
        protected override async void Awake(BenchmarkPlayerClientComponent self, Scene a,string account)
        {
            self.microClientScene = a;
            await ET.Client.LoginHelper.Login(a, account, String.Empty);
            await Client.EnterMapHelper.EnterMapAsync(a);
            a.AddComponent<StateSyncComponent>();
        }
    }
}