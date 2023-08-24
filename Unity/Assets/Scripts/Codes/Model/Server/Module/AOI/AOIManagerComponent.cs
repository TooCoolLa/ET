namespace ET.Server
{
    /// <summary>
    /// AOI管理器组件（场景级别）
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class AOIManagerComponent: Entity, IAwake
    {
        public const int CellSize = 10 * 1000;
    }
}