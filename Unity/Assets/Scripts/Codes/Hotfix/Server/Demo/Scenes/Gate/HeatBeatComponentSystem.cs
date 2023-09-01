using System.Linq;

namespace ET.Server;
[FriendOf(typeof(HeatBeatComponent))]
public static class HeatBeatComponentSystem
{
    public static void Add(this HeatBeatComponent self, Player player)
    {
        self.OnlinePlayers.Add(player.Id, player);
    }

    public static Player Get(this HeatBeatComponent self, long id)
    {
        self.OnlinePlayers.TryGetValue(id, out Player gamer);
        return gamer;
    }

    public static void Remove(this HeatBeatComponent self, long id)
    {
        self.OnlinePlayers.Remove(id);
    }

    public static Player[] GetAll(this HeatBeatComponent self)
    {
        return self.OnlinePlayers.Values.ToArray();
    }
    
    public static async ETTask TimeoutRemoveUnit(HeatBeatComponent self,long playerid,ETCancellationToken etCancellationToken)
    {
        await TimerComponent.Instance.WaitAsync(10 * 1000, cancellationToken: etCancellationToken);
        
    }
}