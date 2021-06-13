using Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CTE
{
    public class MapData : IDataItem<int>
    {
        /* field */
        public int LevelIndex { get; set; }
        public BlockType[,] Blocks;

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
    }
}