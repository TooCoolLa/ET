using System;

namespace ET.Client
{
    public static class GameObjectComponentSystem
    {
        [ObjectSystem]
        public class AwakeSystem : AwakeSystem<GameObjectComponent>
        {
            protected override void Awake(GameObjectComponent self)
            {
                self.LerpRatio = 20f;
            }
        }
        [ObjectSystem]
        public class DestroySystem: DestroySystem<GameObjectComponent>
        {
            protected override void Destroy(GameObjectComponent self)
            {
                UnityEngine.Object.Destroy(self.GameObject);
            }
        }
        
    }
}