using System;
using MongoDB.Util;
using NUnit.Framework;

namespace MongoDB.UnitTests.Util
{
    [TestFixture]
    public class OidGeneratorTests
    {
        [Test]
        public void Generate_CalledFirstTime_IncrementShouldStartWith1()
        {
            var oid = new OidGenerator().Generate();

            var hex = BitConverter.ToString(oid.ToByteArray()).Replace("-", "");
            Assert.IsTrue(hex.EndsWith("000001"), "Increment didn't start with 1.");
        }

        [Test]
        public void Generate_CalledSecondTime_NextIncrementShouldHaveBeen2()
        {
            var generator = new OidGenerator();
            generator.Generate();//skip first

            var oid = generator.Generate();
            var hex = BitConverter.ToString(oid.ToByteArray()).Replace("-", "");
            Assert.IsTrue(hex.EndsWith("000002"), "Next increment should have been 2");
        }

        [Test]
        public void Generate_GenerateOid_CretedIsThisMonthAndYear()
        {
            var oid = new OidGenerator().Generate();

            var now = DateTime.UtcNow;

            var created = oid.Created;
            Assert.AreEqual(now.Year, created.Year);
            Assert.AreEqual(now.Month, created.Month);

            Console.Out.WriteLine(created);
        }
    }
}