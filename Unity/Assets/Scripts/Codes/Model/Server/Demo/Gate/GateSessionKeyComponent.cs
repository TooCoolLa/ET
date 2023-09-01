using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 登陆token获取方法
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class GateSessionKeyComponent : Entity, IAwake
    {
        public readonly Dictionary<long, string> sessionKey = new Dictionary<long, string>();
    }
}