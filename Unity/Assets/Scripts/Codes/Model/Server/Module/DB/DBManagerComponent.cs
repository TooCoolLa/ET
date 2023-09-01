namespace ET.Server
{
    [ComponentOf()]
    public class DBManagerComponent: Entity, IAwake, IDestroy
    {
        [StaticField]
        public static DBManagerComponent Instance;
        
        public DBComponent[] DBComponents = new DBComponent[IdGenerater.MaxZone];
    }
}