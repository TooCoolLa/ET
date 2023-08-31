using Unity.Mathematics;
namespace ET.Client
{
    /// <summary>
    /// Horizontal And Verticle输入记录
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class MoveByInput : Entity,IUpdate,IAwake
    {
        public float2 Input;
        public float moveSpeed, rotSpeed;
    }
}