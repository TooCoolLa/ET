namespace ET
{
    /// <summary>
    /// None = 0;Location = 1; ActorLocationSender = 2; Mailbox = 3; UnitId = 4;DB = 5;Resources = 6;ResourcesLoader = 7;Max = 100;
    /// </summary>
    public static class CoroutineLockType
    {
        public const int None = 0;
        public const int Location = 1;                  // location进程上使用
        public const int ActorLocationSender = 2;       // ActorLocationSender中队列消息 
        public const int Mailbox = 3;                   // Mailbox中队列
        public const int UnitId = 4;                    // Map服务器上线下线时使用
        public const int DB = 5;                            
        public const int Resources = 6;
        public const int ResourcesLoader = 7;

        public const int Max = 100; // 这个必须最大
    }
}