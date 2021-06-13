using System;
using System.Collections.Generic;
using UnityEngine;

namespace CTE
{
    public class PlayerRecord
    {
        /* const */

        /* field */
        /// <summary>
        /// 账户金币数
        /// </summary>
        public long Coins;
        /// <summary>
        /// 关卡完成记录
        /// </summary>
        public bool[] LevelDone;

        /* ctor */
        public PlayerRecord()
        {
        }

        /* func */
        public void TryFixRecord()
        {
            int levelCount = GameData.MapConfig.Count;

            if (Coins < 0L || Coins > int.MaxValue * 100L)
                 Coins = 0L;

            LevelDone = LevelDone ?? new bool[levelCount];
            if (LevelDone.Length < levelCount)
            {
                bool[] fixLevelDone = new bool[levelCount];
                for (int index = 0; index < LevelDone.Length; index++)
                    fixLevelDone[index] = LevelDone[index];
            }
        }

        public bool IsDone(int levelIndex)
        {
            if (levelIndex < 0 
                || levelIndex > LevelDone.Length - 1)
            {
                Debug.LogError($"{nameof(levelIndex)} : {levelIndex} is out of range.");
                return false;
            }
            return LevelDone[levelIndex];
        }
    }
}