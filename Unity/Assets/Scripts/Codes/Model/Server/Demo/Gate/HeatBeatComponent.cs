using System.Collections.Generic;

namespace ET.Server;
[ComponentOf(typeof(Scene))]
public class HeatBeatComponent: Entity,IAwake,IDestroy
{
    public Dictionary<long, Player> OnlinePlayers = new Dictionary<long, Player>();
}