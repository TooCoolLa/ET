namespace ET
{
    public class FixedTimeCounter
    {
        private long startTime;
        private int startFrame;
        public int Interval { get; private set; }

        public FixedTimeCounter(long startTime, int startFrame, int interval)
        {
            this.startTime = startTime;
            this.startFrame = startFrame;
            this.Interval = interval;
        }
        /// <summary>
        /// 重新计算开始帧与帧间隔
        /// </summary>
        /// <param name="interval">间隔</param>
        /// <param name="frame">帧</param>
        public void ChangeInterval(int interval, int frame)
        {
            this.startTime += (frame - this.startFrame) * this.Interval;
            this.startFrame = frame;
            this.Interval = interval;
        }
        /// <summary>
        /// 目标帧的时间
        /// </summary>
        /// <param name="frame">目标帧</param>
        /// <returns>毫秒数</returns>
        public long FrameTime(int frame)
        {
            return this.startTime + (frame - this.startFrame) * this.Interval;
        }
        /// <summary>
        /// 重设开始时间与开始帧
        /// </summary>
        /// <param name="time"></param>
        /// <param name="frame"></param>
        public void Reset(long time, int frame)
        {
            this.startTime = time;
            this.startFrame = frame;
        }
    }
}