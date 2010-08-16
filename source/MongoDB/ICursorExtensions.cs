using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB
{
    public static class ICursorExtensions
    {
        /// <summary>
        ///   Sorts the specified field.
        /// </summary>
        /// <param name = "field">The field.</param>
        /// <returns></returns>
        public static ICursor<T> Sort<T>(this ICursor<T> cursor, string field) where T : class
        {
            return cursor.Sort(field, IndexOrder.Ascending);
        }

        /// <summary>
        ///   Sorts the specified field.
        /// </summary>
        /// <param name = "field">The field.</param>
        /// <param name = "order">The order.</param>
        /// <returns></returns>
        public static ICursor<T> Sort<T>(this ICursor<T> cursor, string field, IndexOrder order) where T : class
        {
            return cursor.Sort(new Document(field, order));
        }
    }
}
