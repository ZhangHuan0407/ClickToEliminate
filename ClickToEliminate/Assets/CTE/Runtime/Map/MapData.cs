using Database;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace CTE
{
    public class MapData : IDataItem<int>
    {
        /* field */
        public int LevelIndex { get; set; }
        public BlockType[,] Blocks;
        public int MaxClickTime = 1;
        public Vector2Decimal CellSize;
        public Vector2Decimal LeftBottomCorner;

        /* inter */
        [JsonIgnore]
        public int PrimaryKey => LevelIndex;

        /* func */
        public bool DataItemIsEqual(IDataItem<object> dataItem)
        {
            if (dataItem is MapData data && LevelIndex == data.LevelIndex)
                return true;
            else
                return false;
        }

        public Vector2 GetBlockAnchoredPosition(int indexX, int indexY) =>
            new Vector2(
                (float)((indexX - 1) * CellSize.x + LeftBottomCorner.x),
                (float)((indexY - 1) * CellSize.y + LeftBottomCorner.y));
    }
}