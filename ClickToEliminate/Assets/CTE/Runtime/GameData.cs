using Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public static IBlock[,] Blocks;
        public static BlockType[,] BlockTypes;

        /* func */
        public static void LoadSaving()
        {
            string content = PlayerPrefs.GetString(nameof(PlayerRecord));
            if (string.IsNullOrEmpty(content))
                PlayerRecord = new PlayerRecord();
            else
                PlayerRecord = JsonConvert.DeserializeObject<PlayerRecord>(content);
            PlayerRecord.TryFixRecord();
        }
        public static void SaveOut()
        {
            string content = JsonConvert.SerializeObject(PlayerRecord);
            PlayerPrefs.SetString(nameof(PlayerRecord), content);
        }
    }
}