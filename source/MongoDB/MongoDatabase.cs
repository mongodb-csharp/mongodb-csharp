using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Configuration;
using MongoDB.Connections;
using MongoDB.Results;

namespace MongoDB
{
    /// <summary>
    /// </summary>
    public class MongoDatabase : IMongoDatabase
    {
        private readonly MongoConfiguration _configuration;
        private readonly Connection _connection;
        private DatabaseJavascript _javascript;
        private DatabaseMetadata _metadata;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoDatabase" /> class.
        /// </summary>
        /// <param name = "connectionString">The connection string.</param>
        public MongoDatabase(string connectionString)
        {
            if(connectionString == null)
                throw new ArgumentNullException("connectionString");

            _configuration = new MongoConfiguration {ConnectionString = connectionString};
            _connection = ConnectionFactoryFactory.GetConnection(_configuration.ConnectionString);
            Name = new MongoConnectionStringBuilder(_configuration.ConnectionString).Database;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoDatabase" /> class.
        /// </summary>
        /// <param name = "configuration">The configuration.</param>
        public MongoDatabase(MongoConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException("configuration");

            Name = new MongoConnectionStringBuilder(configuration.ConnectionString).Database;
            _configuration = configuration;
            _connection = ConnectionFactoryFactory.GetConnection(configuration.ConnectionString);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoDatabase" /> class.
        /// </summary>
        /// <param name = "configuration">The configuration.</param>
        /// <param name = "connection">The conn.</param>
        /// <param name = "name">The name.</param>
        internal MongoDatabase(MongoConfiguration configuration,
            Connection connection,
            string name)
        {
            if(configuration == null)
                throw new ArgumentNullException("configuration");
            if(connection == null)
                throw new ArgumentNullException("connection");
            if(name == null)
                throw new ArgumentNullException("name");

            Name = name;
            _configuration = configuration;
            _connection = connection;
        }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        ///   Gets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        public DatabaseMetadata Metadata
        {
            get { return _metadata ?? ( _metadata = new DatabaseMetadata(_configuration, Name, _connection) ); }
        }

        /// <summary>
        ///   Gets the javascript.
        /// </summary>
        /// <value>The javascript.</value>
        public DatabaseJavascript Javascript
        {
            get { return _javascript ?? ( _javascript = new DatabaseJavascript(this) ); }
        }

        /// <summary>
        ///   Gets the <see cref = "IUntypedCollection" /> with the specified name.
        /// </summary>
        /// <value></value>
        public IMongoCollection<Document> this[String name]
        {
            get { return GetCollection(name); }
        }

        /// <summary>
        ///   Gets the collection names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetCollectionNames()
        {
            var namespaces = this["system.namespaces"];
            var cursor = namespaces.Find(new Document());
            //Todo: Should filter built-ins
            return cursor.Documents.Select(d => (String)d["name"]);
        }

        /// <summary>
        ///   Gets the collection.
        /// </summary>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public IMongoCollection<Document> GetCollection(string name)
        {
            return new MongoCollection<Document>(_configuration, _connection, this, name);
        }

        /// <summary>
        ///   Gets the collection.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "name">The name.</param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(String name) where T : class
        {
            return new MongoCollection<T>(_configuration, _connection, this, name);
        }

        /// <summary>
        ///   Gets the collection.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            var collectionName = _configuration.SerializationFactory.GetCollectionName(typeof(T));
            return GetCollection<T>(collectionName);
        }

        /// <summary>
        ///   Gets the document that a reference is pointing to.
        /// </summary>
        /// <param name = "reference">The reference.</param>
        /// <returns></returns>
        public Document FollowReference(DBRef reference)
        {
            if(reference == null)
                throw new ArgumentNullException("reference", "cannot be null");
            var query = new Document("_id", reference.Id);
            return this[reference.CollectionName].FindOne(query);
        }

        /// <summary>
        ///   Follows the reference.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "reference">The reference.</param>
        /// <returns></returns>
        public T FollowReference<T>(DBRef reference) where T : class
        {
            if(reference == null)
                throw new ArgumentNullException("reference", "cannot be null");
            var query = new Document("_id", reference.Id);
            return GetCollection<T>(reference.CollectionName).FindOneByExample(query);
        }

        /// <summary>
        ///   Retrieves the last error.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///   Most operations do not have a return code in order to save the client from having to wait for results.
        ///   GetLastError can be called to retrieve the return code if clients want one.
        /// </remarks>
        public Document GetLastError()
        {
            return SendCommand("getlasterror");
        }

        /// <summary>
        ///   Retrieves the last error and forces the database to fsync all files before returning.
        /// </summary>
        /// <param name = "fsync">if set to <c>true</c> [fsync].</param>
        /// <returns></returns>
        /// <remarks>
        ///   Most operations do not have a return code in order to save the client from having to wait for results.
        ///   GetLastError can be called to retrieve the return code if clients want one.
        /// 
        ///   Server version 1.3+
        /// </remarks>
        public Document GetLastError(bool fsync)
        {
            return SendCommand(new Document {{"getlasterror", 1.0}, {"fsync", fsync}});
        }

        /// <summary>
        ///   Retrieves all errors since the last ResetError call.
        /// </summary>
        /// <returns></returns>
        public Document GetPreviousError()
        {
            return SendCommand("getpreverror");
        }

        /// <summary>
        ///   Gets the sister database on the same Mongo connection with the given name.
        /// </summary>
        /// <param name = "sisterDatabaseName">Name of the sister database.</param>
        /// <returns></returns>
        internal MongoDatabase GetSisterDatabase(string sisterDatabaseName)
        {
            return new MongoDatabase(_configuration, _connection, sisterDatabaseName);
        }

        /// <summary>
        ///   Resets last error.  This is good to call before a bulk operation.
        /// </summary>
        public void ResetError()
        {
            SendCommand("reseterror");
        }

        /// <summary>
        ///   Evals the specified javascript.
        /// </summary>
        /// <param name = "javascript">The javascript.</param>
        /// <returns></returns>
        public Document Eval(string javascript)
        {
            return Eval(javascript, new Document());
        }

        /// <summary>
        ///   Evals the specified javascript.
        /// </summary>
        /// <param name = "javascript">The javascript.</param>
        /// <param name = "scope">The scope.</param>
        /// <returns></returns>
        public Document Eval(string javascript, Document scope)
        {
            return Eval(new CodeWScope(javascript, scope));
        }

        /// <summary>
        ///   Evals the specified code scope.
        /// </summary>
        /// <param name = "codeScope">The code scope.</param>
        /// <returns></returns>
        public Document Eval(CodeWScope codeScope)
        {
            var cmd = new Document("$eval", codeScope);
            return SendCommand(cmd);
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <param name = "commandName">The command name.</param>
        /// <returns></returns>
        public Document SendCommand(string commandName)
        {
            return SendCommand(new Document(commandName, 1.0));
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <param name = "command">The CMD.</param>
        /// <returns></returns>
        public Document SendCommand(Document command)
        {
            return SendCommand(typeof(Document), command);
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <param name = "rootType">Type of serialization root.</param>
        /// <param name = "command">The CMD.</param>
        /// <returns></returns>
        public Document SendCommand(Type rootType, Document command)
        {
            return _connection.SendCommand(_configuration.SerializationFactory, Name, rootType, command);
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "commandName">Name of the command.</param>
        /// <returns></returns>
        public T SendCommand<T>(string commandName)
            where T : CommandResultBase
        {
            return SendCommand<T>(new Document(commandName, 1.0));
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "command">The command.</param>
        /// <returns></returns>
        public T SendCommand<T>(object command)
            where T : CommandResultBase
        {
            return _connection.SendCommand<T>(_configuration.SerializationFactory, Name, typeof(T), command);
        }

        /// <summary>
        ///   Sends the command.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "rootType">Type of serialization root.</param>
        /// <param name = "command">The command.</param>
        /// <returns></returns>
        public T SendCommand<T>(Type rootType, object command)
            where T : CommandResultBase
        {
            return _connection.SendCommand<T>(_configuration.SerializationFactory, Name, rootType, command);
        }
    }
}