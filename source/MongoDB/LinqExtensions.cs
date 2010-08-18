using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Linq;

namespace MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Counts the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static int Count<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> selector) where T : class
        {
            return collection.Linq().Count(selector);
        }

        /// <summary>
        /// Removes the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        public static void Remove<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> selector) where T : class
        {
            collection.Remove(GetQuery(collection, selector));
        }

        /// <summary>
        /// Finds the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static ICursor<T> Find<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> selector) where T : class
        {
            return collection.Find(GetQuery(collection, selector));
        }

        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static T FindOne<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> selector) where T : class
        {
            return collection.FindOneByExample(GetQuery(collection, selector));
        }

        /// <summary>
        /// Linqs the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IQueryable<T> Linq<T>(this IMongoCollection<T> collection) where T : class
        {
            return new MongoQuery<T>(new MongoQueryProvider((IUntypedCollection)collection));
        }

        /// <summary>
        /// Linqs the selectorified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IQueryable<Document> Linq(this IMongoCollection<Document> collection)
        {
            return new MongoQuery<Document>(new MongoQueryProvider((IUntypedCollection)collection));
        }

        /// <summary>
        /// Updates the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector) where T : class
        {
            collection.Update(document, GetQuery(collection, selector));
        }

        /// <summary>
        /// Updates the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="safeMode">if set to <c>true</c> [safe mode].</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector, bool safeMode) where T : class
        {
            collection.Update(document, GetQuery(collection, selector), safeMode);
        }

        /// <summary>
        /// Updates the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector, UpdateFlags flags) where T : class
        {
            collection.Update(document, GetQuery(collection, selector), flags);
        }

        /// <summary>
        /// Updates the selectorified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="safeMode">if set to <c>true</c> [safe mode].</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector, UpdateFlags flags, bool safeMode) where T : class
        {
            collection.Update(document, GetQuery(collection, selector), flags, safeMode);
        }

        /// <summary>
        /// Updates all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        public static void UpdateAll<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector) where T : class
        {
            collection.UpdateMany(document, GetQuery(collection, selector));
        }

        /// <summary>
        /// Updates all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="safeMode">if set to <c>true</c> [safe mode].</param>
        public static void UpdateAll<T>(this IMongoCollection<T> collection, Document document, Expression<Func<T, bool>> selector, bool safeMode) where T : class
        {
            collection.UpdateMany(document, GetQuery(collection, selector), safeMode);
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        private static Document GetQuery<T>(IMongoCollection<T> collection, Expression<Func<T, bool>> selector) where T : class
        {
            var query = new MongoQuery<T>(new MongoQueryProvider((IUntypedCollection)collection))
                .Where(selector);
            var queryObject = ((IMongoQueryable)query).GetQueryObject();
            return queryObject.Query;
        }
    }
}