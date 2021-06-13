using System;

namespace CTE
{
    public interface Block
    {
        /* field */
        BlockType BlockType { get; }
        int MapX { get; set; }
        int MapY { get; set; }
        bool WillDestroy { get; set; }

        /* func */
        /// <summary>
        /// 界面待摧毁检测
        /// </summary>
        void BreakCheck();
        /// <summary>
        /// 砖块破碎一次
        /// </summary>
        void BreakOnce();
    }
}