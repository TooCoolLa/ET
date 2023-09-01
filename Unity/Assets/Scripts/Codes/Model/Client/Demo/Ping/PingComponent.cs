namespace ET.Client
{
    [ComponentOf(typeof(Session))]
    public class PingComponent: Entity, IAwake, IDestroy,IUpdate
    {
        public long Ping; //延迟值
    }
}