using ET.Client;

namespace ET.Server;
[MessageHandler(SceneType.BenchmarkClient)]
public class M2C_BenchCreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
{
    protected override async ETTask Run(Session session, M2C_CreateMyUnit message)
    {
        var scene = session.DomainScene();
        UnitComponent unitComponent = scene.AddComponent<UnitComponent>();
        Unit unit = Client.UnitFactory.Create(scene, message.Unit);
        unitComponent.Add(unit);
        scene.AddComponent<StateSyncComponent>();
        await ETTask.CompletedTask;
    }
}