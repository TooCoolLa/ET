namespace ET.Server
{
    public static class AOISeeCheckHelper
    {
        /// <summary>
        /// Demo里面的IsCanSee默认返回TRUE
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsCanSee(AOIEntity a, AOIEntity b)
        {
            return true;
        }
    }
}