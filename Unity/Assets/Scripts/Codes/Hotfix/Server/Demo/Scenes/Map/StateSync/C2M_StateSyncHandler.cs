using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class C2M_StateSyncHandler: AMActorLocationRpcHandler<Unit, C2M_StateSync, M2C_Snapshot>
    {
        protected override async ETTask Run(Unit unit, C2M_StateSync request, M2C_Snapshot response)
        {
            unit.Position = request.MyState.Position;
            unit.Rotation = request.MyState.Rotation;
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            numericComponent.Set(NumericType.MoveSpeed,request.MyState.moveSpeed);
            numericComponent.Set(NumericType.InputX,request.MyState.Input.x);
            numericComponent.Set(NumericType.InputY,request.MyState.Input.y);
            AOIEntity aoiEntity = unit.GetComponent<AOIEntity>();
            response.MyState = request.MyState;
            response.OtherUnits = new Dictionary<long, StateInfo>();
            var seeplayer = aoiEntity.GetSeePlayers();
            if (seeplayer != null && seeplayer.Count > 0)
                foreach (var kv in seeplayer)
                {
                    //剔除自己
                    if (kv.Key == unit.Id)
                        continue;
                    var player = kv.Value;
                    NumericComponent otherNumc = player.Unit.GetComponent<NumericComponent>();
                    float2 input = new float2(otherNumc.GetAsFloat(NumericType.InputX), otherNumc.GetAsFloat(NumericType.InputY));
                    response.OtherUnits[player.Unit.Id] = new StateInfo()
                    {
                        UnitID = player.Unit.Id,
                        Position = player.Unit.Position,
                        Rotation = player.Unit.Rotation,
                        unitDesc = new UnitDesc() { ConfigID = player.Unit.ConfigId },
                        moveSpeed = otherNumc.GetAsFloat(NumericType.MoveSpeed),
                        Input = input
                    };
                }
            await ETTask.CompletedTask;
        }
    }
}