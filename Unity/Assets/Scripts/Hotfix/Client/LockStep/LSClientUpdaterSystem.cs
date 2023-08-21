using System;
using System.IO;

namespace ET.Client
{
    //客户端更新锁帧逻辑
    [EntitySystemOf(typeof(LSClientUpdater))]
    [FriendOf(typeof (LSClientUpdater))]
    public static partial class LSClientUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSClientUpdater self)
        {
            Room room = self.GetParent<Room>();
            self.MyId = room.Root().GetComponent<PlayerComponent>().MyId;
        }
        
        [EntitySystem]
        private static void Update(this LSClientUpdater self)
        {
            Room room = self.GetParent<Room>();
            long timeNow = TimeInfo.Instance.ServerNow();
            Scene root = room.Root();

            int i = 0;
            while (true)
            {
                //如果预测帧的下一帧时间将大于服务器当前时间
                if (timeNow < room.FixedTimeCounter.FrameTime(room.PredictionFrame + 1))
                {
                    return;
                }

                // 最多只预测5帧
                if (room.PredictionFrame - room.AuthorityFrame > 5)
                {
                    return;
                }
                
                ++room.PredictionFrame;
                OneFrameInputs oneFrameInputs = self.GetOneFrameMessages(room.PredictionFrame);
                
                room.Update(oneFrameInputs);
                room.SendHash(room.PredictionFrame);
                
                room.SpeedMultiply = ++i;

                FrameMessage frameMessage = FrameMessage.Create();
                frameMessage.Frame = room.PredictionFrame;
                frameMessage.Input = self.Input;
                root.GetComponent<ClientSenderCompnent>().Send(frameMessage);
                
                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 5)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 获取预测帧的玩家输入
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private static OneFrameInputs GetOneFrameMessages(this LSClientUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            if (frame <= room.AuthorityFrame)
            {
                return frameBuffer.FrameInputs(frame);
            }
            
            // predict
            OneFrameInputs predictionFrame = frameBuffer.FrameInputs(frame);
            //把最近的一帧权威帧的其他玩家的输入拷贝给预测帧
            frameBuffer.MoveForward(frame);
            if (frameBuffer.CheckFrame(room.AuthorityFrame))
            {
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                authorityFrame.CopyTo(predictionFrame);
            }
            //更新预测帧里的自己的输入
            predictionFrame.Inputs[self.MyId] = self.Input;
            
            return predictionFrame;
        }
    }
}