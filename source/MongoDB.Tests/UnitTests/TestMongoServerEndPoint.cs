using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using NUnit.Framework;

namespace MongoDB.UnitTests
{
    [TestFixture]
    public class TestMongoServerEndPoint
    {
        [Test]
        public void TryParse_NullHost_ReturnsFalse()
        {
            MongoServerEndPoint endPoint;
            var result = MongoServerEndPoint.TryParse(null, out endPoint);

            Assert.IsNull(endPoint);
            Assert.IsFalse(result);
        }

        [Test]
        public void TryParse_OnlyHost_CanBeParsed()
        {
            MongoServerEndPoint endPoint;
            var result = MongoServerEndPoint.TryParse("testhost", out endPoint);

            Assert.IsTrue(result);
            Assert.IsNotNull(endPoint);
            Assert.AreEqual("testhost",endPoint.Host);
            Assert.AreEqual(MongoServerEndPoint.DefaultPort,endPoint.Port);
        }

        [Test]
        public void TryParse_HostAndPort_CanBeParsed()
        {
            MongoServerEndPoint endPoint;
            var result = MongoServerEndPoint.TryParse("testhost:100", out endPoint);

            Assert.IsTrue(result);
            Assert.IsNotNull(endPoint);
            Assert.AreEqual("testhost", endPoint.Host);
            Assert.AreEqual(100, endPoint.Port);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_NullHost_Throws()
        {
            MongoServerEndPoint.Parse(null);
        }

        [Test]
        public void Parse_OnlyHost_CanBeParsed()
        {
            var endPoint = MongoServerEndPoint.Parse("testhost");

            Assert.IsNotNull(endPoint);
            Assert.AreEqual("testhost", endPoint.Host);
            Assert.AreEqual(MongoServerEndPoint.DefaultPort, endPoint.Port);
        }

        [Test]
        public void Parse_HostAndPort_CanBeParsed()
        {
            var endPoint = MongoServerEndPoint.Parse("testhost:100");

            Assert.IsNotNull(endPoint);
            Assert.AreEqual("testhost", endPoint.Host);
            Assert.AreEqual(100, endPoint.Port);
        }

        [Test]
        public void CanBeBinarySerialized()
        {
            var source = new MongoServerEndPoint("myserver", 12345);
            var formatter = new BinaryFormatter();

            var mem = new MemoryStream();
            formatter.Serialize(mem, source);
            mem.Position = 0;

            var dest = (MongoServerEndPoint)formatter.Deserialize(mem);

            Assert.AreEqual(source, dest);
        }

        [Test]
        public void CanBeXmlSerialized()
        {
            var source = new MongoServerEndPoint("myserver", 12345);
            var serializer = new XmlSerializer(typeof(MongoServerEndPoint));

            var writer = new StringWriter();
            serializer.Serialize(writer, source);
            var dest = (MongoServerEndPoint)serializer.Deserialize(new StringReader(writer.ToString()));

            Assert.AreEqual(source, dest);
        }
    }
}