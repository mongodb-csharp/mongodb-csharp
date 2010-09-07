using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Configuration;
using MongoDB.Connections;
using MongoDB.Protocol;
using MongoDB.Results;
using MongoDB.Util;
using System.Collections;

namespace MongoDB
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoCollection<T> : IMongoCollection<T>, IUntypedCollection where T : class
    {
        private readonly MongoConfiguration _configuration;
        private readonly Connection _connection;
        private MongoDatabase _database;
        private CollectionMetadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="collectionName">The name.</param>
        internal MongoCollection(MongoConfiguration configuration, Connection connection, string databaseName, string collectionName)
        {
            //Todo: add public constructors for users to call
            Name = collectionName;
            DatabaseName = databaseName;
            _configuration = configuration;
            _connection = connection;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public IMongoDatabase Database
        {
            get { return _database ?? (_database = new MongoDatabase(_configuration, _connection, DatabaseName)); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets the full name including database name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return DatabaseName + "." + Name; }
        }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        public CollectionMetadata Metadata
        {
            get { return _metadata ?? (_metadata = new CollectionMetadata(_configuration, DatabaseName, Name, _connection)); }
        }

        /// <summary>
        /// Finds and returns the first document in a selector query.
        /// </summary>
        /// <param name="javascriptWhere">The where.</param>
        /// <returns>
        /// A <see cref="Document"/> from the collection.
        /// </returns>
        public T FindOne(string javascriptWhere)
        {
            var spec = new Document { { "$where", new Code(javascriptWhere) } };
            using (var cursor = Find(spec).Limit(-1))
                return cursor.Documents.FirstOrDefault();
        }

        /// <summary>
        /// Finds and returns the first document in a query.
        /// </summary>
        /// <param name="selector">The spec.</param>
        /// <returns></returns>
        public T FindOne(Document selector)
        {
            using (var cursor = Find(selector).Limit(1))
                return cursor.Documents.FirstOrDefault();
        }

        /// <summary>
        /// Finds and returns the first document in a query.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// A <see cref="Document"/> from the collection.
        /// </returns>
        public T FindOneByExample<TExample>(TExample selector)
        {
            return FindOne(ObjectToDocumentConverter.Convert(selector));
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public ICursor<T> FindAll()
        {
            return (ICursor<T>)((IUntypedCollection)this).FindAll();
        }

        /// <summary>
        /// Finds the specified where.
        /// </summary>
        /// <param name="javascriptWhere">The where.</param>
        /// <returns></returns>
        public ICursor<T> Find(string javascriptWhere)
        {
            var spec = new Document { { "$where", new Code(javascriptWhere) } };
            return Find(spec);
        }

        /// <summary>
        /// Queries the collection using the query selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public ICursor<T> Find(Document selector)
        {
            return (ICursor<T>)((IUntypedCollection)this).Find(selector);
        }

        /// <summary>
        /// Finds the specified document.
        /// </summary>
        /// <param name="selector">The spec.</param>
        /// <returns></returns>
        public ICursor<T> FindByExample<TExample>(TExample selector)
        {
            return Find(ObjectToDocumentConverter.Convert(selector));
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <param name="updateDocument">The update document.</param>
        /// <param name="selectorDocument">The selector document.</param>
        /// <param name="sortDocument">The sort document.</param>
        /// <param name="fieldsDocument">The fields document.</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <param name="upsert">if set to <c>true</c> [upsert].</param>
        /// <returns></returns>
        public T FindAndModify(Document updateDocument, Document selectorDocument, Document sortDocument, Document fieldsDocument, bool remove, bool returnNew, bool upsert)
        {
            return (T)((IUntypedCollection)this).FindAndModify(updateDocument, selectorDocument, sortDocument, fieldsDocument, remove, returnNew, upsert);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <typeparam name="TExample3">The type of the example3.</typeparam>
        /// <typeparam name="TExample4">The type of the example4.</typeparam>
        /// <param name="updateExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="sortExample">The sort example.</param>
        /// <param name="fieldsExample">The fields example.</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        /// <param name="upsert">if set to <c>true</c> [upsert].</param>
        /// <returns></returns>
        public T FindAndModifyByExample<TExample1, TExample2, TExample3, TExample4>(TExample1 updateExample, TExample2 selectorExample, TExample3 sortExample, TExample4 fieldsExample, bool remove, bool returnNew, bool upsert)
        {
            return (T)((IUntypedCollection)this).FindAndModify(
                ObjectToDocumentConverter.Convert(updateExample), 
                ObjectToDocumentConverter.Convert(selectorExample), 
                ObjectToDocumentConverter.Convert(sortExample),
                ObjectToDocumentConverter.Convert(fieldsExample),
                remove,
                returnNew,
                upsert);
        }

        /// <summary>
        /// Entrypoint into executing a map/reduce query against the collection.
        /// </summary>
        /// <returns>A <see cref="MapReduce"/></returns>
        public MapReduce MapReduce()
        {
            return ((IUntypedCollection)this).MapReduce();
        }

        ///<summary>
        ///  Count all items in the collection.
        ///</summary>
        public long Count()
        {
            return Count(new Document());
        }

        /// <summary>
        /// Count all items in a collection that match the query spec.
        /// </summary>
        /// <param name="selector">The spec.</param>
        /// <returns></returns>
        /// <remarks>
        /// It will return 0 if the collection doesn't exist yet.
        /// </remarks>
        public long Count(Document selector)
        {
            return ((IUntypedCollection)this).Count(selector);
        }

        /// <summary>
        /// Counts the by example.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public long CountByExample<TExample>(TExample selector)
        {
            return Count(ObjectToDocumentConverter.Convert(selector));
        }

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <param name="document">The doc.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Insert(Document document, bool safemode)
        {
            InsertMany(new[] { document }, safemode);
        }

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Insert(T document, bool safemode)
        {
            InsertMany(new[] { document }, safemode);
        }

        /// <summary>
        /// Inserts the by example.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void InsertByExample<TExample>(TExample example, bool safemode)
        {
            InsertManyByExample(new[] { example }, safemode);
        }

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void InsertMany(IEnumerable<Document> documents, bool safemode)
        {
            ((IUntypedCollection)this).InsertMany(documents, safemode);
        }

        /// <summary>
        /// Inserts the specified documents.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void InsertMany(IEnumerable<T> documents, bool safemode)
        {
            ((IUntypedCollection)this).InsertMany(documents, safemode);
        }

        /// <summary>
        /// Inserts all.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="examples">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void InsertManyByExample<TExample>(IEnumerable<TExample> examples, bool safemode)
        {
            InsertMany(examples.Select(x => ObjectToDocumentConverter.Convert(x)).ToArray(), safemode);
        }

        /// <summary>
        /// Remove documents from the collection according to the selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// See the safemode description in the class description
        /// </remarks>
        public void Remove(Document selector, bool safemode)
        {
            ((IUntypedCollection)this).Remove(selector, safemode);
        }

        /// <summary>
        /// Remove documents from the collection.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// See the safemode description in the class description
        /// </remarks>
        public void Remove(T document, bool safemode)
        {
            ((IUntypedCollection)this).Remove(document, safemode);
        }

        /// <summary>
        /// Remove documents from the collection according to the selector.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        /// <remarks>
        /// An empty document will match all documents in the collection and effectively truncate it.
        /// </remarks>
        public void RemoveByExample<TExample>(TExample example, bool safemode)
        {
            Remove(ObjectToDocumentConverter.Convert(example), false);
        }

        /// <summary>
        /// Saves the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Save(Document document, bool safemode)
        {
            ((IUntypedCollection)this).Save(document, safemode);
        }

        /// <summary>
        /// Saves the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Save(T document, bool safemode)
        {
            ((IUntypedCollection)this).Save(document, safemode);
        }

        /// <summary>
        /// Saves the specified document.
        /// </summary>
        /// <typeparam name="TExample">The type of the example.</typeparam>
        /// <param name="example">The example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void SaveByExample<TExample>(TExample example, bool safemode)
        {
            ((IUntypedCollection)this).Save(ObjectToDocumentConverter.Convert(example), safemode);
        }

        /// <summary>
        /// Updates a document with the data in doc if found.
        /// </summary>
        /// <param name="document">The <see cref="Document"/> to update with</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Update(T document, bool safemode)
        {
            var descriptor = _configuration.SerializationFactory.GetObjectDescriptor(typeof(T));

            var value = descriptor.GetPropertyValue(document, "_id");

            if(value==null)
                return;
            
            ((IUntypedCollection)this).Update(document, new Document("_id", value), UpdateFlags.Upsert, safemode);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <param name="document">The <see cref="Document"/> to update with</param>
        /// <param name="selector">The query spec to find the document to update.</param>
        /// <param name="flags"><see cref="UpdateFlags"/></param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void Update(Document document, Document selector, UpdateFlags flags, bool safemode)
        {
            ((IUntypedCollection)this).Update(document, selector, flags, safemode);
        }

        /// <summary>
        /// Updates a document with the data in doc as found by the selector.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="flags"><see cref="UpdateFlags"/></param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void UpdateByExample<TExample1, TExample2>(TExample1 documentExample, TExample2 selectorExample, UpdateFlags flags, bool safemode)
        {
            ((IUntypedCollection)this).Update(ObjectToDocumentConverter.Convert(documentExample), ObjectToDocumentConverter.Convert(selectorExample), flags, safemode);
        }

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' ops.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void UpdateMany(Document document, Document selector, bool safemode)
        {
            Update((Document)EnsureUpdateDocument(document), selector, UpdateFlags.MultiUpdate, safemode);
        }

        /// <summary>
        /// Runs a multiple update query against the database.  It will wrap any
        /// doc with $set if the passed in doc doesn't contain any '$' ops.
        /// </summary>
        /// <typeparam name="TExample1">The type of the example1.</typeparam>
        /// <typeparam name="TExample2">The type of the example2.</typeparam>
        /// <param name="documentExample">The document example.</param>
        /// <param name="selectorExample">The selector example.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        public void UpdateManyByExample<TExample1, TExample2>(TExample1 documentExample, TExample2 selectorExample, bool safemode)
        {
            UpdateMany(ObjectToDocumentConverter.Convert(documentExample), ObjectToDocumentConverter.Convert(selectorExample), safemode);
        }

        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        long IUntypedCollection.Count()
        {
            return Count();
        }

        /// <summary>
        /// Counts the specified selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        long IUntypedCollection.Count(object selector)
        {
            try
            {
                var response = Database.SendCommand(typeof(T), new Document("count", Name).Add("query", selector));
                return Convert.ToInt64((double)response["n"]);
            }
            catch (MongoCommandException)
            {
                //FIXME This is an exception condition when the namespace is missing. 
                //-1 might be better here but the console returns 0.
                return 0;
            }
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        IUntypedCursor IUntypedCollection.FindAll()
        {
            var spec = new Document();
            return ((IUntypedCollection)this).Find(spec);
        }

        /// <summary>
        /// Finds the specified selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        IUntypedCursor IUntypedCollection.Find(object selector)
        {
            var cursor = (IUntypedCursor)new Cursor<T>(_configuration.SerializationFactory, _configuration.MappingStore, _connection, DatabaseName, Name);
            cursor.Spec(selector);
            return cursor;
        }

        /// <summary>
        /// Finds the and modify.
        /// </summary>
        /// <param name="update">Update document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <param name="upsert">if set to <c>true</c> [upsert].</param>
        /// <returns></returns>
        object IUntypedCollection.FindAndModify(object update, object selector, object sort, object fields, bool remove, bool returnNew, bool upsert)
        {
            try
            {
                var command = new Document
                {
                    {"findandmodify", Name},
                    {"query", selector},
                    {"update", EnsureUpdateDocument(update)},
                    {"remove", remove},
                    {"new", returnNew},
                    {"upsert", upsert}
                };

                if(sort != null)
                    command.Add("sort", sort);
                if(fields != null)
                    command.Add("fields", fields);

                var response = _connection.SendCommand<FindAndModifyResult<T>>(_configuration.SerializationFactory,
                    DatabaseName,
                    typeof(T),
                    command);

                return response.Value;
            }
            catch (MongoCommandException)
            {
                // This is when there is no document to operate on
                return null;
            }
        }

        /// <summary>
        /// Inserts the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void IUntypedCollection.Insert(object document, bool safemode)
        {
            ((IUntypedCollection)this).InsertMany(new[] { document }, safemode);
        }

        /// <summary>
        /// Inserts the many.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void IUntypedCollection.InsertMany(IEnumerable documents, bool safemode)
        {
            var rootType = typeof(T);
            var writerSettings = _configuration.SerializationFactory.GetBsonWriterSettings(rootType);

            var insertMessage = new InsertMessage(writerSettings)
            {
                FullCollectionName = FullName
            };

            var descriptor = _configuration.SerializationFactory.GetObjectDescriptor(rootType);
            var insertDocument = new ArrayList();

            foreach (var document in documents)
            {
                var id = descriptor.GetPropertyValue(document, "_id");

                if (id == null)
                    descriptor.SetPropertyValue(document, "_id", descriptor.GenerateId(document));

                insertDocument.Add(document);
            }

            insertMessage.Documents = insertDocument.ToArray();

            try
            {
                _connection.SendMessage(insertMessage, DatabaseName);
                CheckLastError(safemode);
            }
            catch (IOException exception)
            {
                throw new MongoConnectionException("Could not insert document, communication failure", _connection, exception);
            }
        }

        /// <summary>
        /// Maps the reduce.
        /// </summary>
        /// <returns></returns>
        MapReduce IUntypedCollection.MapReduce()
        {
            return new MapReduce(Database, Name, typeof(T));
        }

        /// <summary>
        /// Removes the specified selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void IUntypedCollection.Remove(object selector, bool safemode)
        {
            var writerSettings = _configuration.SerializationFactory.GetBsonWriterSettings(typeof(T));

            try
            {
                _connection.SendMessage(new DeleteMessage(writerSettings)
                {
                    FullCollectionName = FullName,
                    Selector = selector
                }, DatabaseName);
                
                CheckLastError(safemode);
            }
            catch (IOException exception)
            {
                throw new MongoConnectionException("Could not remove document, communication failure", _connection, exception);
            }
        }

        /// <summary>
        /// Saves the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void IUntypedCollection.Save(object document, bool safemode)
        {
            //Try to generate a selector using _id for an existing document.
            //otherwise just set the upsert flag to 1 to insert and send onward.

            var descriptor = _configuration.SerializationFactory.GetObjectDescriptor(typeof(T));

            var value = descriptor.GetPropertyValue(document, "_id");

            if (value == null)
            {
                //Likely a new document
                descriptor.SetPropertyValue(document, "_id", descriptor.GenerateId(value));

                ((IUntypedCollection)this).Insert(document, safemode);
            }
            else
                ((IUntypedCollection)this).Update(document, new Document("_id", value), UpdateFlags.Upsert, safemode);
        }

        /// <summary>
        /// Updates the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        void IUntypedCollection.Update(object document, object selector, UpdateFlags flags, bool safemode)
        {            
            var writerSettings = _configuration.SerializationFactory.GetBsonWriterSettings(typeof(T));

            try
            {
                _connection.SendMessage(new UpdateMessage(writerSettings)
                {
                    FullCollectionName = FullName,
                    Selector = selector,
                    Document = document,
                    Flags = (int)flags
                }, DatabaseName);
                
                CheckLastError(safemode);
            }
            catch (IOException exception)
            {
                throw new MongoConnectionException("Could not update document, communication failure", _connection, exception);
            }
        }

        /// <summary>
        /// Checks the last error.
        /// </summary>
        /// <param name="safemode">if set to <c>true</c> [safemode].</param>
        private void CheckLastError(bool safemode)
        {
            if(!safemode)
                return;

            var lastError = Database.GetLastError();

            if(ErrorTranslator.IsError(lastError))
                throw ErrorTranslator.Translate(lastError);
        }

        /// <summary>
        /// Ensures the update document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        private object EnsureUpdateDocument(object document)
        {
            var descriptor = _configuration.SerializationFactory.GetObjectDescriptor(typeof(T));

            var foundOp = descriptor.GetMongoPropertyNames(document)
                .Any(name => name.IndexOf('$') == 0);

            return foundOp == false ? new Document("$set", document) : document;
        }
    }
}
