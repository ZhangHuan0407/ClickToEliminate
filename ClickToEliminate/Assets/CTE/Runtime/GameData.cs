using Database;
using System;
using System.Collections.Generic;

namespace CTE
{
    public static class GameData
    {
        /* field */
        public static Table<int, SectionData> SectionConfig;
        public static Table<int, MapData> MapConfig;

        public static PlayerRecord PlayerRecord;

        /// <summary>
        /// 当前选择的关卡，地图数据
        /// </summary>
        public static MapData Map;
        /// <summary>
        /// 场景当前的砖块状态
        /// </summary>
        public static Block[,] Blocks;

        /* func */

    }
}