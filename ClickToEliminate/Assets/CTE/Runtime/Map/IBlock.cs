using System;

namespace CTE
{
    public interface IBlock
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
        /// 临近砖块破碎一次
        /// </summary>
        /// <param name="blockType">起始破碎砖块类型</param>
        void NearlyBreakOnce(BlockType blockType);
        void PlayBreakAnimation();
    }
}