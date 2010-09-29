using System;

namespace MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum RemoveFlags
    {
        /// <summary>
        /// No flag set
        /// </summary>
        None = 0,
        /// <summary>
        /// If set, the database will remove only the first matching document 
        /// in the collection. Otherwise all matching documents will be removed. 
        /// </summary>
        SingleRemove = 1
    }
}