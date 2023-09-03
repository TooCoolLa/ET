using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (ApplySnapshotDataComponent))]
    public static class AppleSnapshotDataComponentSystem
    {
        [ObjectSystem]
        public class AppleSnapshotDataComponentAwakeSystem: AwakeSystem<ApplySnapshotDataComponent>
        {
            protected override void Awake(ApplySnapshotDataComponent self)
            {
                self.StateBuff = new Dictionary<long, StateInfo>();
                
                self.moveSpeed = 5f;
            }
        }

        [ObjectSystem]
        public class AppleSnapshotDataComponentUpdateSystem: UpdateSystem<ApplySnapshotDataComponent>
        {
            protected override void Update(ApplySnapshotDataComponent self)
            {
                var snapshort = self.Snapshot;
                UnitComponent unitComponent = self.DomainScene().CurrentScene().GetComponent<UnitComponent>();
                if (unitComponent != null && snapshort != null && snapshort.OtherUnits != null && snapshort.OtherUnits.Count > 0)
                {
                    if (self.AuthFrameCount + self.FrameInterval >= Time.frameCount)
                    { 
                        float ratio = (float)(Time.frameCount - self.AuthFrameCount) / self.FrameInterval;
                        foreach (var kv in snapshort.OtherUnits)
                        {
                            var stateInfo = kv.Value;
                            if (stateInfo.UnitID == self.myUnitID)
                                continue;
                            Unit unit = unitComponent.Get(stateInfo.UnitID);
                            if (unit != null)
                            {
                                if (self.StateBuff != null && self.StateBuff.TryGetValue(unit.Id, out var oldStateInfo))
                                {
                                    unit.Position = math.lerp(oldStateInfo.Position, stateInfo.Position, ratio);
                                    unit.Rotation = math.slerp(oldStateInfo.Rotation, stateInfo.Rotation, ratio);
                                }
                                else
                                {
                                    unit.Position = stateInfo.Position;
                                    unit.Rotation = stateInfo.Rotation;
                                }
                            }
                            else
                            {
                                unit = UnitFactory.Create(self.DomainScene(), stateInfo);
                            }
                        }
                        Logger.Instance.Debug($"插值到最新状态{ratio}");
                        self.inPredict = false;
                    }
                    //根据输入执行一下预测,预测最多不超过10帧
                    // else
                    // {
                    //     foreach (var kv in snapshort.OtherUnits)
                    //     {
                    //         var stateInfo = kv.Value;
                    //         if (stateInfo.UnitID == self.myUnitID)
                    //             continue;
                    //         Unit unit = unitComponent.Get(stateInfo.UnitID);
                    //         if (unit == null)
                    //         {
                    //             unit = UnitFactory.Create(self.DomainScene(), stateInfo);
                    //         }
                    //         if(unit != null)
                    //         {
                    //             unit.Position += new float3(stateInfo.Input.x * self.moveSpeed, 0, stateInfo.Input.y * self.moveSpeed) *
                    //                     Time.fixedDeltaTime;
                    //             stateInfo.CopyUnit2StateInfo(unit);
                    //             self.StateBuff[unit.Id] = stateInfo;
                    //         }
                    //     }
                    //     Logger.Instance.Debug($"执行预测");
                    //     self.inPredict = true;
                    //     
                    // }
                }
            }

        }
            public static void CopyUnit2StateInfo(this StateInfo self, Unit unit)
            {
                self.Position = unit.Position;
                self.Rotation = unit.Rotation;
            }

        public static void Clear(this StateInfo info)
        {
            info.Position = float3.zero;
            info.Rotation = quaternion.identity;
        }
        public static void SetSnapshotData(this ApplySnapshotDataComponent self, M2C_Snapshot snapshot,long myUnitID)
        {
            //self.LastFrameSnapshot = self.Snapshot;
            self.Snapshot = snapshot;
            self.myUnitID = myUnitID;
            self.FrameInterval = Time.frameCount - self.AuthFrameCount;
            self.AuthFrameCount = Time.frameCount;
            //self.StartFrameCount = self.AuthFrameCount;
            Logger.Instance.Debug($"<color=green>当前帧间隔为{self.FrameInterval}</color>");
        }

        public static StateInfo GetStateInfoFromBuffer(this StateInfo[] buffer, int startFrame, int nowFrameCount)
        {
            return buffer[(Mathf.Max(0,nowFrameCount - startFrame)) % buffer.Length];
        }
        public static void SetStateInfoFromBuffer(this StateInfo[] buffer, StateInfo frame,int startFrame, int nowFrameCount)
        {
             buffer[Mathf.Max(0,nowFrameCount - startFrame) % buffer.Length] = frame;
        }
    }
}