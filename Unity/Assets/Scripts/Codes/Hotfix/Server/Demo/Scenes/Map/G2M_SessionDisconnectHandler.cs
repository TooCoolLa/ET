

using System.Collections.Generic;

namespace ET.Server
{
	[ActorMessageHandler(SceneType.Map)]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(Unit unit, G2M_SessionDisconnect message)
		{
			M2C_RemoveUnits removeUnits = new M2C_RemoveUnits()
			{
				Units = new List<long>()
			};
			removeUnits.Units.Add(unit.Id);
			MessageHelper.Broadcast(unit,removeUnits);
			UnitComponent unitComponent = unit.DomainScene().GetComponent<UnitComponent>();
			unitComponent.Remove(unit.Id);
			unit.Dispose();
			await ETTask.CompletedTask;
		}
	}
}