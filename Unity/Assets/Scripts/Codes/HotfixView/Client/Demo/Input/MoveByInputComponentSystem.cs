﻿using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(MoveByInput))]
    public static class MoveByInputComponentSystem
    {
        [ObjectSystem]
        public class MoveByInputAwakeSystem : AwakeSystem<MoveByInput>
        {
            protected override void Awake(MoveByInput self)
            {
                self.moveSpeed = 5f;
                self.rotSpeed = 20f;
            }
        }
        [ObjectSystem]
        public class MoveByInputUpdateSystem : UpdateSystem<MoveByInput>
        {
            protected override void Update(MoveByInput self)
            {
                self.Input = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                //self.
                var myUnit = UnitHelper.GetMyUnitFromClientScene(self.DomainScene());
                if(myUnit != null)
                {
                    myUnit.TargetRotation = Camera.main.transform.rotation;

                    //myUnit.Position += math.mul(myUnit.TargetRotation,  new float3(self.Input.x * self.moveSpeed, 0, self.Input.y * self.moveSpeed) * Time.fixedDeltaTime);
                    myUnit.Position += new float3(self.Input.x * self.moveSpeed, 0, self.Input.y * self.moveSpeed) * Time.fixedDeltaTime;
                }
            }
        }

        public static float2 GetInput(this MoveByInput self) => self.Input;
    }
}