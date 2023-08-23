using System.Net;

namespace ET.Server
{
    public struct NetServerComponentOnRead
    {
        public Session Session;
        public object Message;
    }
    /// <summary>
    /// 与客户端直联的需要挂载这个组件
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class NetServerComponent: Entity, IAwake<IPEndPoint>, IDestroy
    {
        public int ServiceId;
    }
}