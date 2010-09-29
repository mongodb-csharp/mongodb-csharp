using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;

namespace MongoDB.Protocol
{
    /// <summary>
    /// The OP_REPLY message is sent by the database in response to an CONTRIB:OP_QUERY  or CONTRIB:OP_GET_MORE  message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// struct {
    ///     MsgHeader header;                 // standard message header
    ///     int32     responseFlag;           // normally zero, non-zero on query failure
    ///     int64     cursorID;               // id of the cursor created for this query response
    ///     int32     startingFrom;           // indicates where in the cursor this reply is starting
    ///     int32     numberReturned;         // number of documents in the reply
    ///     BSON[]    documents;              // documents
    /// }
    /// </remarks>
    internal class ReplyMessage<T> : MessageBase where T : class
    {
        private readonly BsonReaderSettings _readerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyMessage&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="readerSettings">The reader settings.</param>
        public ReplyMessage(BsonReaderSettings readerSettings){
            if(readerSettings == null)
                throw new ArgumentNullException("readerSettings");
            _readerSettings = readerSettings;
        }

        /// <summary>
        /// normally zero, non-zero on query failure     
        /// </summary>
        /// <value>The response flag.</value>
        public ResponseFlags ResponseFlag { get; set; }

        /// <summary>
        /// id of the cursor created for this query response 
        /// </summary>
        /// <value>The cursor id.</value>
        public long CursorId { get; set; }

        /// <summary>
        /// indicates where in the cursor this reply is starting     
        /// </summary>
        /// <value>The starting from.</value>
        public int StartingFrom { get; set; }

        /// <summary>
        /// number of documents in the reply       
        /// </summary>
        /// <value>The number returned.</value>
        public int NumberReturned { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public T[] Documents { get; set; }

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Read(Stream stream){
            stream = new BufferedStream(stream, 256);
            var reader = new BinaryReader(stream);
            Header = ReadHeader(reader);
            ResponseFlag = (ResponseFlags)reader.ReadInt32();
            CursorId = reader.ReadInt64();
            StartingFrom = reader.ReadInt32();
            NumberReturned = reader.ReadInt32();

            var breader = new BsonReader(stream, _readerSettings);
            var documents = new List<T>();
            
            for(var num = 0; num < NumberReturned; num++)
                documents.Add((T)breader.ReadObject());
            
            Documents = documents.ToArray();
        }

        /// <summary>
        /// Reads the header.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected MessageHeader ReadHeader(BinaryReader reader){
            var header = new MessageHeader(OpCode.Reply){
                MessageLength = reader.ReadInt32(),
                RequestId = reader.ReadInt32(),
                ResponseTo = reader.ReadInt32()
            };
            
            var opCode = reader.ReadInt32();
            
            if((OpCode)opCode != OpCode.Reply)
                throw new InvalidDataException("Should have been a reply but wasn't");
            
            return header;
        }
    }
}