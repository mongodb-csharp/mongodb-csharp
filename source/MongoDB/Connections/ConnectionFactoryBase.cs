using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;

namespace MongoDB.Connections
{
    /// <summary>
    /// </summary>
    internal abstract class ConnectionFactoryBase : IConnectionFactory
    {
        private readonly List<MongoServerEndPoint> _servers;
        protected readonly object SyncObject = new object();
        private Func<RawConnection> _createRawConnection;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConnectionFactoryBase" /> class.
        /// </summary>
        /// <param name = "connectionString">The connection string.</param>
        protected ConnectionFactoryBase(string connectionString)
        {
            if(connectionString == null)
                throw new ArgumentNullException("connectionString");

            ConnectionString = connectionString;
            Builder = new MongoConnectionStringBuilder(connectionString);
            _servers = new List<MongoServerEndPoint>(Builder.Servers);

            _createRawConnection = () =>
            {
                lock(SyncObject)
                {
                    InvalidateReplicaSetStatus();
                    _createRawConnection = CreateRawConnectionCore;
                    return CreateRawConnectionCore();
                }
            };
        }

        /// <summary>
        /// Gets the primary end point.
        /// </summary>
        /// <value>The primary end point.</value>
        public MongoServerEndPoint PrimaryEndPoint { get; private set; }

        /// <summary>
        ///   Gets or sets the builder.
        /// </summary>
        /// <value>The builder.</value>
        protected MongoConnectionStringBuilder Builder { get; private set; }

        /// <summary>
        ///   Opens a connection.
        /// </summary>
        /// <returns></returns>
        public abstract RawConnection Open();

        /// <summary>
        ///   Closes the specified connection.
        /// </summary>
        /// <param name = "connection">The connection.</param>
        public abstract void Close(RawConnection connection);

        /// <summary>
        ///   Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }

        /// <summary>
        ///   Cleanups this instance.
        /// </summary>
        public virtual void Cleanup()
        {
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        ///   Creates the raw connection.
        /// </summary>
        /// <returns></returns>
        protected RawConnection CreateRawConnection()
        {
            return _createRawConnection();
        }

        /// <summary>
        /// Creates the raw connection core.
        /// </summary>
        /// <returns></returns>
        private RawConnection CreateRawConnectionCore()
        {
            try
            {
                return new RawConnection(PrimaryEndPoint, Builder.ConnectionTimeout);
            }
            catch(SocketException exception)
            {
                throw new MongoConnectionException("Failed to connect to server " + PrimaryEndPoint, ConnectionString, PrimaryEndPoint, exception);
            }
        }

        /// <summary>
        /// Invalidates the replica set status.
        /// </summary>
        protected void InvalidateReplicaSetStatus()
        {
            lock(SyncObject)
            {
                for(var i = 0; i < _servers.Count; i++)
                {
                    var endPoint = _servers[i];
                    RawConnection connection = null;
                    try
                    {
                        connection = new RawConnection(endPoint, Builder.ConnectionTimeout);

                        var result = connection.SendCommand("admin", new Document("ismaster", 1));

                        foreach(var replicaSetHost in ParseReplicaSetHosts(result))
                            if(!_servers.Contains(replicaSetHost))
                                _servers.Add(replicaSetHost);

                        if(true.Equals(result["ismaster"]))
                        {
                            PrimaryEndPoint = endPoint;
                            return;
                        }
                    }
                    catch(SocketException)
                    {
                        continue;
                    }
                    catch(MongoConnectionException)
                    {
                        continue;
                    }
                    finally
                    {
                        if(connection != null)
                            connection.Dispose();
                    }
                }

                if(_servers.Count <= 1)
                {
                    PrimaryEndPoint = _servers.FirstOrDefault();
                    return;
                }

                throw new MongoException("Could not found the ReplicaSet master server.");
            }
        }

        /// <summary>
        /// Parses the replica set hosts.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private static IEnumerable<MongoServerEndPoint> ParseReplicaSetHosts(Document result)
        {
            var servers = result["hosts"] as IEnumerable<string>;

            if(servers == null)
                yield break;

            foreach(var server in servers)
                yield return MongoServerEndPoint.Parse(server);
        }

        /// <summary>
        ///   Determines whether the specified connection is alive.
        /// </summary>
        /// <param name = "connection">The connection.</param>
        /// <returns>
        ///   <c>true</c> if the specified connection is alive; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsAlive(RawConnection connection)
        {
            if(connection == null)
                throw new ArgumentNullException("connection");

            if(!connection.IsConnected)
                return false;

            if(connection.IsInvalid)
                return false;

            if(Builder.ConnectionLifetime != TimeSpan.Zero)
                if(connection.CreationTime.Add(Builder.ConnectionLifetime) < DateTime.Now)
                    return false;

            return true;
        }
    }
}