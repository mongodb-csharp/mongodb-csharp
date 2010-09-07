using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MongoDB
{
    /// <summary>
    ///   Type that holds the bson timestamp type.
    /// </summary>
    [Serializable]
    public class MongoTimestamp : IEquatable<MongoTimestamp>, IEquatable<long>, IComparable<MongoTimestamp>, IComparable<long>, IXmlSerializable
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoTimestamp" /> class.
        /// </summary>
        public MongoTimestamp()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoTimestamp" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public MongoTimestamp(long value)
        {
            Value = value;
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public long Value { get; set; }

        /// <summary>
        ///   Gets the increment.
        /// </summary>
        /// <value>The increment.</value>
        public int Increment
        {
            get { return BitConverter.ToInt32(BitConverter.GetBytes(Value), 0); }
            set
            {
                var val = BitConverter.GetBytes(Value);

                Array.Copy(BitConverter.GetBytes(value), val, 4);

                Value = BitConverter.ToInt64(val, 0);
            }
        }

        /// <summary>
        ///   Gets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public int Timestamp
        {
            get { return BitConverter.ToInt32(BitConverter.GetBytes(Value), 4); }
            set
            {
                var val = BitConverter.GetBytes(Value);

                Array.Copy(BitConverter.GetBytes(value), 0, val, 4, 4);

                Value = BitConverter.ToInt64(val, 0);
            }
        }

        /// <summary>
        ///   Compares to.
        /// </summary>
        /// <param name = "other">The other.</param>
        /// <returns></returns>
        public int CompareTo(long other)
        {
            return Value.CompareTo(other);
        }

        /// <summary>
        ///   Compares to.
        /// </summary>
        /// <param name = "other">The other.</param>
        /// <returns></returns>
        public int CompareTo(MongoTimestamp other)
        {
            return ReferenceEquals(other, null) ? 1 : Value.CompareTo(other);
        }

        /// <summary>
        ///   Equalses the specified other.
        /// </summary>
        /// <param name = "other">The other.</param>
        /// <returns></returns>
        public bool Equals(long other)
        {
            return Value.Equals(other);
        }

        /// <summary>
        ///   Equalses the specified other.
        /// </summary>
        /// <param name = "other">The other.</param>
        /// <returns></returns>
        public bool Equals(MongoTimestamp other)
        {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            return other.Value == Value;
        }

        /// <summary>
        ///   This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref = "T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        ///   An <see cref = "T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref = "M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref = "M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        ///   Generates an object from its XML representation.
        /// </summary>
        /// <param name = "reader">The <see cref = "T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Value = reader.ReadElementContentAsLong();
        }

        /// <summary>
        ///   Converts an object into its XML representation.
        /// </summary>
        /// <param name = "writer">The <see cref = "T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(Value.ToString());
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name = "obj">The <see cref = "System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref = "System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref = "T:System.NullReferenceException">
        ///   The <paramref name = "obj" /> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == typeof(MongoTimestamp) && Equals((MongoTimestamp)obj);
        }

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "a">A.</param>
        /// <param name = "b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MongoTimestamp a, MongoTimestamp b)
        {
            if(a == null || b == null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "a">A.</param>
        /// <param name = "b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MongoTimestamp a, MongoTimestamp b)
        {
            return !( a == b );
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "MongoDB.MongoTimestamp" /> to <see cref = "System.Int64" />.
        /// </summary>
        /// <param name = "timestamp">The timestamp.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator long(MongoTimestamp timestamp)
        {
            return timestamp.Value;
        }

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "System.Int64" /> to <see cref = "MongoDB.MongoTimestamp" />.
        /// </summary>
        /// <param name = "value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MongoTimestamp(long value)
        {
            return new MongoTimestamp(value);
        }
    }
}