using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(PingComponent))]
    public static partial class PingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PingComponent self)
        {
            self.PingAsync().Coroutine();
        }
        
        [EntitySystem]
        private static void Destroy(this PingComponent self)
        {
            self.Ping = default;
        }
        
        private static async ETTask PingAsync(this PingComponent self)
        {
            Session session = self.GetParent<Session>();
            long instanceId = self.InstanceId;
            Fiber fiber = self.Fiber();
            
            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                long time1 = TimeInfo.Instance.ClientNow();
                try
                {
                    C2G_Ping c2GPing = C2G_Ping.Create(true);
                    G2C_Ping response = await session.Call(c2GPing) as G2C_Ping;

                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }

                    long time2 = TimeInfo.Instance.ClientNow();
                    self.Ping = time2 - time1;
                    //此时此刻，服务器的时间等于消息中的时间加上消息传递所用的时间
                    TimeInfo.Instance.ServerMinusClientTime = response.Time + (time2 - time1) / 2 - time2;
                    
                    await fiber.TimerComponent.WaitAsync(2000);
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Info($"ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"ping error: \n{e}");
                }
            }
        }
    }
}