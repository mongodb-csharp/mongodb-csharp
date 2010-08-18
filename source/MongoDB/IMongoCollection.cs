using System;
using System.Collections.Generic;
using System.Collections;

namespace MongoDB
{
    internal interface IMongoCollection
    {
        long Count();

        long Count(object selector);

        ICursor FindAll();

        ICursor Find(object spec);

        object FindAndModify(object document, object selector, object sort, bool returnNew);

        void Insert(object document, bool safemode);

        void InsertMany(IEnumerable documents, bool safemode);

        MapReduce MapReduce();

        void Remove(object selector, bool safemode);

        void Save(object document, bool safemode);

        void Update(object document, object selector, UpdateFlags flags, bool safemode);
    }

    /// <summary>
    ///   A collection is a storage unit for a group of <see cref = "Document" />s. 
    ///   The documents do not all have to contain the same schema but for 
    ///   efficiency they should all be similar.
    /// </summary>
    /// <remarks>
    ///   Safemode checks the database for any errors that may have occurred during 
    ///   the insert such as a duplicate key constraint violation.
    /// </remarks>
    public interface IMongoCollection<T>
        where T : class
    {
        /// <summary>
        ///   Gets the database.
        /// </summary>
        /// <value>The database.</value>
        IMongoDatabase Database { get; }

        /// <summary>
        ///   Name of the collection.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   String value of the database name.
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        ///   Full name of the collection databasename . collectionname
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///   Metadata about the collection such as indexes.
        /// </summary>
        CollectionMetadata Metadata { get; }

        /// <summary>
        /// Finds and returns the first document in a selector query.
        /// </summary>
        /// <param name="javascriptWhere">The where.</param>
        /// <returns>
        /// A <see cref="Document"/> from the collection.
        /// </returns>
        T FindOne(string javascriptWhere);

        /// <summary>
        /// Finds and returns the first document in a selector query.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        T FindOne(Document selector);

        /// <summary>
        /// Finds and returns the first document in a selector query.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// A <see cref="Document"/> from the collection.
        /// </returns>
        T FindOneByExample<TExample>(TExample selector);

        /// <summary>
        ///   Returns a cursor that contains all of the documents in the collection.
        /// </summary>
        /// <remarks>
        ///   Cursors load documents from the database in batches instead of all at once.
        /// </remarks>
        ICursor<T> FindAll();

        /// <summary>
        /// Uses the $where operator to query the collection.  The value of the where is Javascript that will
        /// produce a true for the documents that match the criteria.
        /// </summary>
        /// <param name="javascriptWhere">Javascript</param>
        /// <returns></returns>
        ICursor<T> Find(string javascriptWhere);

        /// <summary>
        /// Queries the collection using the query selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        ICursor<T> Find(Document selector);

        /// <summary>
        /// Queries the collection using the query selector.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        ICursor<T> FindByExample<TExample>(TExample selector);

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="sort"><see cref="Document"/> containing the names of columns to sort on with the values being the
        /// <see cref="IndexOrder"/></param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns>A <see cref="Document"/></returns>
        T FindAndModify(Document document, Document selector, Document sort, bool returnNew);

        /// <summary>
        /// Finds the and modify by example.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <typeparam name="TExample3">The type of the example3.</typeparam>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="sortExample">The sort example.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns></returns>
        T FindAndModifyByExample<TExample1, TExample2, TExample3>(TExample1 documentExample, TExample2 selectorExample, TExample3 sortExample, bool returnNew);

        /// <summary>
        ///   Entrypoint into executing a map/reduce query against the collection.
        /// </summary>
        /// <returns></returns>
        MapReduce MapReduce();

        ///<summary>
        ///  Count all items in the collection.
        ///</summary>
        long Count();

        /// <summary>
        /// Counts the specified selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        long Count(Document selector);

        /// <summary>
        /// Count all items in a collection that match the query selector.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <remarks>
        /// It will return 0 if the collection doesn't exist yet.
        /// </remarks>
        long CountByExample<TExample>(TExample selector);

        /// <summary>
        ///   Inserts the Document into the collection.
        /// </summary>
        void Insert(Document document, bool safemode);

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void Insert(T document, bool safemode);

        /// <summary>
        /// Inserts the by example.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void InsertByExample<TExample>(TExample example, bool safemode);

        /// <summary>
        /// Inserts the specified documents in one batch.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void InsertMany(IEnumerable<Document> documents, bool safemode);

        /// <summary>
        /// Inserts the specified documents in one batch.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void InsertMany(IEnumerable<T> documents, bool safemode);

        /// <summary>
        /// Inserts the specified documents in one batch.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="examples">The examples.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void InsertManyByExample<TExample>(IEnumerable<TExample> examples, bool safemode);

        /// <summary>
        ///   Remove documents from the collection according to the selector.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        ///   An empty document will match all documents in the collection and effectively truncate it.
        ///   See the safemode description in the class description
        /// </remarks>
        void Remove(Document selector, bool safemode);

        /// <summary>
        /// Remove documents from the collection.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// See the safemode description in the class description
        /// </remarks>
        void Remove(T document, bool safemode);

        /// <summary>
        ///   Remove documents from the collection according to the selector.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void RemoveByExample<TExample>(TExample example, bool safemode);

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// The document will contain the _id that is saved to the database.
        /// </remarks>
        void Save(Document document, bool safemode);

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void Save(T document, bool safemode);

        /// <summary>
        /// Inserts or updates a document in the database.  If the document does not contain an _id one will be
        /// generated and an upsert sent.  Otherwise the document matching the _id of the document will be updated.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void SaveByExample<TExample>(TExample example, bool safemode);

        /// <summary>
        /// Updates a document with the data in doc if found.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void Update(T document, bool safemode);

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void Update(Document document, Document selector, UpdateFlags flags, bool safemode);

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void UpdateByExample<TExample1, TExample2>(TExample1 document, TExample2 selector, UpdateFlags flags, bool safemode);

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' modifier ops.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// See the safemode description in the class description
        /// </remarks>
        void UpdateAll(Document document, Document selector, bool safemode);

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' modifier ops.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="safeMode">if set to <c>true</c> [safe mode].</param>
        void UpdateAllByExample<TExample1, TExample2>(TExample1 documentExample, TExample2 selectorExample, bool safeMode);
    }
}