using System;
using Database;
using Newtonsoft.Json;

namespace CTE
{
    public class SectionData : IDataItem<int>
    {
        /* field */
        public int SectionIndex { get; set; }
        public int[] LevelIndex { get; set; }

        /* inter */
        [JsonIgnore]
        public int PrimaryKey => SectionIndex;
        [JsonIgnore]
        public int LevelCount => LevelIndex.Length;

        /* func */
        public bool DataItemIsEqual(IDataItem<object> dataItem)
        {
            if (dataItem is SectionData data
                && SectionIndex == data.SectionIndex)
                return true;
            else
                return false;
        }
    }
}