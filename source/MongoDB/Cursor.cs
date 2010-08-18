using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Configuration.Mapping;
using MongoDB.Connections;
using MongoDB.Protocol;
using MongoDB.Serialization;
using MongoDB.Util;

namespace MongoDB
{
    /// <summary>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class Cursor<T> : ICursor<T>, ICursor
        where T : class
    {
        private readonly Connection _connection;
        private readonly string _databaseName;
        private readonly IMappingStore _mappingStore;
        private readonly ISerializationFactory _serializationFactory;
        private readonly Document _specOpts = new Document();
        private object _fields;
        private bool _keepCursor;
        private int _limit;
        private QueryOptions _options;
        private ReplyMessage<T> _reply;
        private int _skip;
        private object _spec;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Cursor&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "serializationFactory">The serialization factory.</param>
        /// <param name = "mappingStore">The mapping store.</param>
        /// <param name = "connection">The conn.</param>
        /// <param name = "databaseName">Name of the database.</param>
        /// <param name = "collectionName">Name of the collection.</param>
        internal Cursor(ISerializationFactory serializationFactory,
            IMappingStore mappingStore,
            Connection connection,
            string databaseName,
            string collectionName)
        {
            //Todo: add public constrcutor for users to call
            IsModifiable = true;
            _connection = connection;
            _databaseName = databaseName;
            FullCollectionName = databaseName + "." + collectionName;
            _serializationFactory = serializationFactory;
            _mappingStore = mappingStore;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Cursor&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "serializationFactory">The serialization factory.</param>
        /// <param name = "mappingStore">The mapping store.</param>
        /// <param name = "connection">The conn.</param>
        /// <param name = "databaseName">Name of the database.</param>
        /// <param name = "collectionName">Name of the collection.</param>
        /// <param name = "spec">The spec.</param>
        /// <param name = "limit">The limit.</param>
        /// <param name = "skip">The skip.</param>
        /// <param name = "fields">The fields.</param>
        internal Cursor(ISerializationFactory serializationFactory,
            IMappingStore mappingStore,
            Connection connection,
            string databaseName,
            string collectionName,
            object spec,
            int limit,
            int skip,
            object fields)
            : this(serializationFactory, mappingStore, connection, databaseName, collectionName)
        {
            //Todo: add public constrcutor for users to call
            if(spec == null)
                spec = new Document();
            _spec = spec;
            _limit = limit;
            _skip = skip;
            _fields = fields;
        }

        /// <summary>
        ///   Gets or sets the full name of the collection.
        /// </summary>
        /// <value>The full name of the collection.</value>
        public string FullCollectionName { get; private set; }

        /// <summary>
        ///   Gets the cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        public int CursorPosition
        {
            get
            {
                if(_reply == null)
                    return 0;

                return _reply.StartingFrom + _reply.NumberReturned;
            }
        }

        void ICursor.Spec(object spec)
        {
            TryModify();
            _spec = spec;
        }

        void ICursor.Hint(object index)
        {
            TryModify();
            AddOrRemoveSpecOpt("$hint", index);
        }

        void ICursor.Limit(int limit)
        {
            TryModify();
            _limit = limit;
        }

        void ICursor.Skip(int skip)
        {
            TryModify();
            _skip = skip;
        }

        void ICursor.Fields(object fields)
        {
            TryModify();
            _fields = fields;
        }

        void ICursor.Sort(object fields)
        {
            TryModify();
            AddOrRemoveSpecOpt("$orderby", fields);
        }

        void ICursor.KeepCursor(bool value)
        {
            _keepCursor = value;
        }

        void ICursor.Snapshot()
        {
            TryModify();
            AddOrRemoveSpecOpt("$snapshot", true);
        }

        IEnumerable ICursor.Documents
        {
            get
            {
                do
                {
                    _reply = RetrieveData<T>();

                    if(_reply == null)
                        throw new InvalidOperationException("Expecting reply but get null");

                    foreach(var document in _reply.Documents)
                        yield return document;
                }
                while(Id > 0 && _limit < CursorPosition);

                if(!_keepCursor)
                    Dispose(true);
            }
        }

        void ICursor.Options(QueryOptions options)
        {
            TryModify();
            _options = options;
        }

        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public long Id { get; private set; }

        /// <summary>
        ///   Specs the specified spec.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <returns></returns>
        public ICursor<T> Spec(Document spec)
        {
            ( (ICursor)this ).Spec(spec);
            return this;
        }

        /// <summary>
        ///   Specs the by example.
        /// </summary>
        /// <typeparam name = "TExample">The type of the example.</typeparam>
        /// <param name = "spec">The spec.</param>
        /// <returns></returns>
        public ICursor<T> SpecByExample<TExample>(TExample spec)
        {
            return Spec(ObjectToDocumentConverter.Convert(spec));
        }

        /// <summary>
        ///   Limits the specified limit.
        /// </summary>
        /// <param name = "limit">The limit.</param>
        /// <returns></returns>
        public ICursor<T> Limit(int limit)
        {
            ( (ICursor)this ).Limit(limit);
            return this;
        }

        /// <summary>
        ///   Skips the specified skip.
        /// </summary>
        /// <param name = "skip">The skip.</param>
        /// <returns></returns>
        public ICursor<T> Skip(int skip)
        {
            ( (ICursor)this ).Skip(skip);
            return this;
        }

        /// <summary>
        ///   Fieldses the specified fields.
        /// </summary>
        /// <param name = "fields">The fields.</param>
        /// <returns></returns>
        public ICursor<T> Fields(Document fields)
        {
            ( (ICursor)this ).Fields(fields);
            return this;
        }

        /// <summary>
        ///   Fieldses the by example.
        /// </summary>
        /// <typeparam name = "TExample">The type of the example.</typeparam>
        /// <param name = "fields">The fields.</param>
        /// <returns></returns>
        public ICursor<T> FieldsByExample<TExample>(TExample fields)
        {
            return Fields(ObjectToDocumentConverter.Convert(fields));
        }

        /// <summary>
        ///   Sorts the specified fields.
        /// </summary>
        /// <param name = "fields">The fields.</param>
        /// <returns></returns>
        public ICursor<T> Sort(Document fields)
        {
            ( (ICursor)this ).Sort(fields);
            return this;
        }

        /// <summary>
        ///   Sorts the by example.
        /// </summary>
        /// <param name = "fields">The fields.</param>
        /// <returns></returns>
        public ICursor<T> SortByExample<TExample>(TExample fields)
        {
            return Sort(ObjectToDocumentConverter.Convert(fields));
        }

        /// <summary>
        ///   Hints the specified index.
        /// </summary>
        /// <param name = "index">The index.</param>
        /// <returns></returns>
        public ICursor<T> Hint(Document index)
        {
            ( (ICursor)this ).Hint(index);
            return this;
        }

        /// <summary>
        ///   Hints the specified index.
        /// </summary>
        /// <typeparam name = "TExample">The type of the example.</typeparam>
        /// <param name = "example">The example.</param>
        /// <returns></returns>
        public ICursor<T> HintByExample<TExample>(TExample example)
        {
            return Hint(ObjectToDocumentConverter.Convert(example));
        }

        /// <summary>
        ///   Keeps the cursor open.
        /// </summary>
        /// <param name = "value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        /// <remarks>
        ///   By default cursors are closed automaticly after documents 
        ///   are Enumerated.
        /// </remarks>
        public ICursor<T> KeepCursor(bool value)
        {
            ( (ICursor)this ).KeepCursor(value);
            return this;
        }

        /// <summary>
        ///   Snapshots the specified index.
        /// </summary>
        public ICursor<T> Snapshot()
        {
            ( (ICursor)this ).Snapshot();
            return this;
        }

        /// <summary>
        ///   Explains this instance.
        /// </summary>
        /// <returns></returns>
        public Document Explain()
        {
            TryModify();
            _specOpts["$explain"] = true;

            var explainResult = RetrieveData<Document>();
            try
            {
                var explain = explainResult.Documents.FirstOrDefault();

                if(explain == null)
                    throw new InvalidOperationException("Explain failed. No documents where returned.");

                return explain;
            }
            finally
            {
                if(explainResult.CursorId > 0)
                    KillCursor(explainResult.CursorId);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this <see cref = "Cursor&lt;T&gt;" /> is modifiable.
        /// </summary>
        /// <value><c>true</c> if modifiable; otherwise, <c>false</c>.</value>
        public bool IsModifiable { get; private set; }

        /// <summary>
        ///   Gets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public IEnumerable<T> Documents
        {
            get { return ( (ICursor)this ).Documents.OfType<T>(); }
        }

        /// <summary>
        ///   Optionses the specified options.
        /// </summary>
        /// <param name = "options">The options.</param>
        /// <returns></returns>
        public ICursor<T> Options(QueryOptions options)
        {
            ( (ICursor)this ).Options(options);
            return this;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Releases unmanaged resources and performs other cleanup operations before the
        ///   <see cref = "Cursor&lt;T&gt;" /> is reclaimed by garbage collection.
        /// </summary>
        ~Cursor()
        {
            Dispose(false);
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ( (ICursor)this ).Dispose();
        }

        /// <summary>
        ///   Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name = "disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if(Id == 0 || !_connection.IsConnected) //All server side resources disposed of.
                return;

            KillCursor(Id);
        }

        /// <summary>
        ///   Kills the cursor.
        /// </summary>
        private void KillCursor(long cursorId)
        {
            var killCursorsMessage = new KillCursorsMessage(cursorId);

            try
            {
                _connection.SendMessage(killCursorsMessage, _databaseName);
                Id = 0;
            }
            catch(IOException exception)
            {
                throw new MongoConnectionException("Could not read data, communication failure", _connection, exception);
            }
        }

        /// <summary>
        ///   Retrieves the data.
        /// </summary>
        /// <typeparam name = "TReply">The type of the reply.</typeparam>
        /// <returns></returns>
        private ReplyMessage<TReply> RetrieveData<TReply>() where TReply : class
        {
            IsModifiable = false;

            IRequestMessage message;

            if(Id <= 0)
            {
                var writerSettings = _serializationFactory.GetBsonWriterSettings(typeof(T));

                message = new QueryMessage(writerSettings)
                {
                    FullCollectionName = FullCollectionName,
                    Query = BuildSpec(),
                    NumberToReturn = _limit,
                    NumberToSkip = _skip,
                    Options = _options,
                    ReturnFieldSelector = ConvertFieldSelectorToDocument(_fields)
                };
            }
            else
                message = new GetMoreMessage(FullCollectionName, Id, _limit);

            var readerSettings = _serializationFactory.GetBsonReaderSettings(typeof(T));

            try
            {
                var reply = _connection.SendTwoWayMessage<TReply>(message, readerSettings, _databaseName);

                Id = reply.CursorId;

                return reply;
            }
            catch(IOException exception)
            {
                throw new MongoConnectionException("Could not read data, communication failure", _connection, exception);
            }
        }

        /// <summary>
        ///   Tries the modify.
        /// </summary>
        private void TryModify()
        {
            if(!IsModifiable)
                throw new InvalidOperationException("Cannot modify a cursor that has already returned documents.");
        }

        /// <summary>
        ///   Adds the or remove spec opt.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "doc">The doc.</param>
        private void AddOrRemoveSpecOpt(string key, object doc)
        {
            if(doc == null)
                _specOpts.Remove(key);
            else
                _specOpts[key] = doc;
        }

        /// <summary>
        ///   Builds the spec.
        /// </summary>
        /// <returns></returns>
        private object BuildSpec()
        {
            if(_specOpts.Count == 0)
                return _spec;

            var document = new Document();
            _specOpts.CopyTo(document);
            document["$query"] = _spec;
            return document;
        }

        private Document ConvertFieldSelectorToDocument(object document)
        {
            if(document == null)
                return null;

            if(!( document is Document ) && typeof(T).IsAssignableFrom(document.GetType()))
                throw new NotSupportedException("An entity type is not supported in field selection. Use either a document or an anonymous type.");

            var doc = ObjectToDocumentConverter.Convert(document);
            var classMap = _mappingStore.GetClassMap(typeof(T));
            if(doc.Count > 0 && ( classMap.IsPolymorphic || classMap.IsSubClass ))
                doc[classMap.DiscriminatorAlias] = true;

            return doc.Count == 0 ? null : doc;
        }
    }
}