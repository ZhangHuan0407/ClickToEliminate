namespace Database
{
    /// <summary>
    /// Json 数据最外层应当为表，而非数组。数据元素以数组形式排列。
    /// 提供统一的数组转表格式
    /// </summary>
    public partial class Table<TKey, TData> where TData : class, IDataItem<TKey>, new()
    {
        internal class JsonTable<_TData> where _TData : class, IDataItem<TKey>, new()
        {
            internal _TData[] DataSet;
        }
    }
}