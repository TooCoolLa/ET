using Unity.Mathematics;

namespace ET
{
	[ObjectSystem]
	public class UnitComponentAwakeSystem : AwakeSystem<UnitComponent>
	{
		protected override void Awake(UnitComponent self)
		{
			self.Random = Random.CreateFromIndex(1000);
		}
	}
	
	[ObjectSystem]
	public class UnitComponentDestroySystem : DestroySystem<UnitComponent>
	{
		protected override void Destroy(UnitComponent self)
		{
		}
	}
	[FriendOf(typeof(UnitComponent))]
	public static class UnitComponentSystem
	{
		public static void Add(this UnitComponent self, Unit unit)
		{
		}

		public static Unit Get(this UnitComponent self, long id)
		{
			Unit unit = self.GetChild<Unit>(id);
			return unit;
		}

		public static Random GetRandom(this UnitComponent self)
		{

			return self.Random;
		}
		public static void Remove(this UnitComponent self, long id)
		{
			Unit unit = self.GetChild<Unit>(id);
			unit?.Dispose();
		}
	}
}