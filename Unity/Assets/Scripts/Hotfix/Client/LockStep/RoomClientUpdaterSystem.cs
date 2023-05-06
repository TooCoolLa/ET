using System;
using System.IO;
using MongoDB.Bson;

namespace ET.Client
{
    [FriendOf(typeof (RoomClientUpdater))]
    public static class RoomClientUpdaterSystem
    {
        public class AwakeSystem: AwakeSystem<RoomClientUpdater>
        {
            protected override void Awake(RoomClientUpdater self)
            {
                Room room = self.GetParent<Room>();
                self.MyId = room.GetParent<Scene>().GetComponent<PlayerComponent>().MyId;
            }
        }
        
        [FriendOf(typeof (Room))]
        public class UpdateSystem: UpdateSystem<RoomClientUpdater>
        {
            protected override void Update(RoomClientUpdater self)
            {
                self.Update();
            }
        }

        private static void Update(this RoomClientUpdater self)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            long timeNow = TimeHelper.ServerFrameTime();
            Scene clientScene = room.GetParent<Scene>();
            
            if (timeNow < room.FixedTimeCounter.FrameTime(frameBuffer.PredictionFrame + 1))
            {
                return;
            }
            ++frameBuffer.PredictionFrame;
            OneFrameMessages oneFrameMessages = GetOneFrameMessages(self, frameBuffer.PredictionFrame);
            room.Update(oneFrameMessages, frameBuffer.PredictionFrame);
            

            FrameMessage frameMessage = NetServices.Instance.FetchMessage<FrameMessage>();
            frameMessage.Frame = oneFrameMessages.Frame;
            frameMessage.Input = self.Input;
            clientScene.GetComponent<SessionComponent>().Session.Send(frameMessage);
        }

        private static OneFrameMessages GetOneFrameMessages(this RoomClientUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            if (frame <= frameBuffer.RealFrame)
            {
                return frameBuffer.GetFrame(frame);
            }
            
            // predict
            return GetPredictionOneFrameMessage(self, frame);
        }

        // 获取预测一帧的消息
        private static OneFrameMessages GetPredictionOneFrameMessage(this RoomClientUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            OneFrameMessages predictionFrame = room.FrameBuffer.GetFrame(frame);
            OneFrameMessages realFrame = room.FrameBuffer.GetFrame(room.FrameBuffer.RealFrame);
            realFrame?.CopyTo(predictionFrame);
            predictionFrame.Frame = frame;
            predictionFrame.Inputs[self.MyId] = self.Input;
            
            return predictionFrame;
        }
    }
}