using System.Collections.Generic;

namespace MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public static class IMongoCollectionExtensions
    {
        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateDocument">The update document.</param>
        /// <param name="queryDocument">The query document.</param>
        /// <returns>A <see cref="Document"/></returns>
        public static T FindAndModify<T>(this IMongoCollection<T> collection, Document updateDocument, Document queryDocument) where T : class
        {
            return collection.FindAndModify(updateDocument, queryDocument, new Document(), null, false, false, false);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateDocument">The update document.</param>
        /// <param name="queryDocument">The query document.</param>
        /// <param name="sortDocument">The sort document.</param>
        /// <returns>A <see cref="Document"/></returns>
        public static T FindAndModify<T>(this IMongoCollection<T> collection, Document updateDocument, Document queryDocument, Document sortDocument) where T : class
        {
            return collection.FindAndModify(updateDocument, queryDocument, sortDocument, null, false, false, false);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateDocument">The update document.</param>
        /// <param name="queryDocument">The query document.</param>
        /// <param name="sortDocument">The sort document.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns>A <see cref="Document"/></returns>
        public static T FindAndModify<T>(this IMongoCollection<T> collection, Document updateDocument, Document queryDocument, Document sortDocument, bool returnNew) where T : class
        {
            return collection.FindAndModify(updateDocument, queryDocument, sortDocument, null, returnNew, false, false);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateDocument">The update document.</param>
        /// <param name="queryDocument">The query document.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns>A <see cref="Document"/></returns>
        public static T FindAndModify<T>(this IMongoCollection<T> collection, Document updateDocument, Document queryDocument, bool returnNew) where T : class
        {
            return collection.FindAndModify(updateDocument, queryDocument, new Document(), null, returnNew, false, false);
        }

        /// <summary>
        /// Finds the and modify by example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateExample">The document example.</param>
        /// <param name="queryExample">The selector example.</param>
        /// <returns></returns>
        public static T FindAndModifyByExample<T, TExample1, TExample2>(this IMongoCollection<T> collection, TExample1 updateExample, TExample2 queryExample) where T : class
        {
            return collection.FindAndModifyByExample(updateExample, queryExample, new {}, (object)null, false, false, false);
        }

        /// <summary>
        /// Finds the and modify by example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <typeparam name="TExample3">The type of the example3.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateExample">The document example.</param>
        /// <param name="queryExample">The selector example.</param>
        /// <param name="sortExample">The sort example.</param>
        /// <returns></returns>
        public static T FindAndModifyByExample<T, TExample1, TExample2, TExample3>(this IMongoCollection<T> collection, TExample1 updateExample, TExample2 queryExample, TExample3 sortExample) where T : class
        {
            return collection.FindAndModifyByExample(updateExample, queryExample, sortExample, (object)null, false, false, false);
        }

        /// <summary>
        /// Finds the and modify by example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <typeparam name="TExample3">The type of the example3.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateExample">The document example.</param>
        /// <param name="queryExample">The selector example.</param>
        /// <param name="sortExample">The sort example.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns></returns>
        public static T FindAndModifyByExample<T, TExample1, TExample2, TExample3>(this IMongoCollection<T> collection, TExample1 updateExample, TExample2 queryExample, TExample3 sortExample, bool returnNew) where T : class
        {
            return collection.FindAndModifyByExample(updateExample, queryExample, sortExample, (object)null, returnNew, false, false);
        }

        /// <summary>
        /// Finds the and modify by example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateExample">The document example.</param>
        /// <param name="queryExample">The selector example.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns></returns>
        public static T FindAndModifyByExample<T, TExample1, TExample2>(this IMongoCollection<T> collection, TExample1 updateExample, TExample2 queryExample, bool returnNew) where T : class
        {
            return collection.FindAndModifyByExample(updateExample, queryExample, new {}, (object)null, returnNew, false, false);
        }

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        public static void Insert<T>(this IMongoCollection<T> collection, Document document) where T : class
        {
            collection.Insert(document, false);
        }

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The doc.</param>
        public static void Insert<T>(this IMongoCollection<T> collection, T document) where T : class
        {
            collection.Insert(document, false);
        }

        /// <summary>
        /// Inserts the specified example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="example">The example.</param>
        public static void InsertByExample<T, TExample>(this IMongoCollection<T> collection, TExample example) where T : class
        {
            collection.InsertByExample(example, false);
        }

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="documents">The documents.</param>
        public static void InsertMany<T>(this IMongoCollection<T> collection, IEnumerable<Document> documents) where T : class
        {
            collection.InsertMany(documents, false);
        }

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="documents">The documents.</param>
        public static void InsertMany<T>(this IMongoCollection<T> collection, IEnumerable<T> documents) where T : class
        {
            collection.InsertMany(documents, false);
        }

        /// <summary>
        /// Bulk inserts the specified documents into the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="examples">The examples.</param>
        public static void InsertManyByExample<T, TExample>(this IMongoCollection<T> collection, IEnumerable<TExample> examples) where T : class
        {
            collection.InsertManyByExample(examples, false);
        }

        /// <summary>
        /// Remove documents from the collection according to the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector.</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// </remarks>
        public static void Remove<T>(this IMongoCollection<T> collection, Document selector) where T : class
        {
            collection.Remove(selector, false);
        }

        /// <summary>
        /// Remove documents from the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// </remarks>
        public static void Remove<T>(this IMongoCollection<T> collection, T document) where T : class
        {
            collection.Remove(document, false);
        }

        /// <summary>
        /// Remove documents from the collection according to the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="example">The example.</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// </remarks>
        public static void RemoveByExample<T, TExample>(this IMongoCollection<T> collection, TExample example) where T : class
        {
            collection.RemoveByExample(example, false);
        }

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <remarks>
        /// The document will contain the _id that is saved to the database.
        /// </remarks>
        public static void Save<T>(this IMongoCollection<T> collection, Document document) where T : class
        {
            collection.Save(document, false);
        }

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        public static void Save<T>(this IMongoCollection<T> collection, T document) where T : class
        {
            collection.Save(document, false);
        }

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="example">The example.</param>
        public static void SaveByExample<T, TExample>(this IMongoCollection<T> collection, TExample example) where T : class
        {
            collection.SaveByExample(example, false);
        }

        /// <summary>
        /// Updates a document with the data in doc if found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The <see cref="Document"/> to update with</param>
        public static void Update<T>(this IMongoCollection<T> collection, T document) where T : class
        {
            collection.Update(document,  false);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Document selector) where T : class
        {
            collection.Update(document, selector, false);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Document selector, bool safemode) where T : class
        {
            collection.Update(document, selector, UpdateFlags.Default, safemode);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        public static void Update<T>(this IMongoCollection<T> collection, Document document, Document selector, UpdateFlags flags) where T : class
        {
            collection.Update(document, selector, flags, false);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public static void UpdateByExample<T, TExample1, TExample2>(this IMongoCollection<T> collection, TExample1 documentExample, TExample2 selectorExample, bool safemode) where T : class
        {
            collection.UpdateByExample(documentExample, selectorExample, UpdateFlags.Default, safemode);
        }

        /// <summary>
        /// Updates the by example.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="flags">The flags.</param>
        public static void UpdateByExample<T, TExample1, TExample2>(this IMongoCollection<T> collection, TExample1 documentExample, TExample2 selectorExample, UpdateFlags flags) where T : class
        {
            collection.UpdateByExample(documentExample, selectorExample, flags, false);
        }

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' modifier ops.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        public static void UpdateMany<T>(this IMongoCollection<T> collection, Document document, Document selector) where T : class
        {
            collection.UpdateMany(document, selector, false);
        }

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' modifier ops.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        public static void UpdateManyByExample<T, TExample1, TExample2>(this IMongoCollection<T> collection, TExample1 documentExample, TExample2 selectorExample) where T : class
        {
            collection.UpdateManyByExample(documentExample, selectorExample, false);
        }
    }
}