using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class C2M_StateSyncHandler: AMActorLocationRpcHandler<Unit, C2M_StateSync, M2C_Snapshot>
    {
        protected override async ETTask Run(Unit unit, C2M_StateSync request, M2C_Snapshot response)
        {
            unit.Position = request.MyState.Position;
            unit.Rotation = request.MyState.Rotation;
            AOIEntity aoiEntity = unit.GetComponent<AOIEntity>();
            response.MyState = request.MyState;
            List<StateInfo> otherUnits = new List<StateInfo>();
            var seeplayer = aoiEntity.GetSeePlayers();
            if (seeplayer != null && seeplayer.Count > 0)
                foreach (var kv in seeplayer)
                {
                    if(kv.Key == unit.Id)
                        continue;
                    var player = kv.Value;
                    otherUnits.Add(new StateInfo()
                    {
                        UnitID = player.Unit.Id,
                        Position = player.Unit.Position,
                        Rotation = player.Unit.Rotation,
                        unitDesc = new UnitDesc() { ConfigID = player.Unit.ConfigId }
                    });
                }

            response.OtherUnits = otherUnits;
            await ETTask.CompletedTask;
        }
    }
}