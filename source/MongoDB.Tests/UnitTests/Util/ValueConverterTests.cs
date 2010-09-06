using System;
using MongoDB.Util;
using NUnit.Framework;

namespace MongoDB.UnitTests.Util
{
    [TestFixture]
    public class ValueConverterTests
    {
        [Test]
        public void Convert_LongWithTypeTimespan_IsConvertedToTimespan()
        {
            var source = TimeSpan.FromSeconds(100);

            var dest = ValueConverter.Convert(source.Ticks, typeof(TimeSpan));

            Assert.AreEqual(source,dest);
        }

        [Test]
        public void Convert_BinaryWithTypeByteArray_IsConvertedToByteArray()
        {
            var source = new Binary(new byte[] {1, 2, 3, 4});

            var dest = ValueConverter.Convert(source, typeof(byte[])) as byte[];

            Assert.IsNotNull(dest);
            Assert.AreEqual(4, dest.Length);
            Assert.AreEqual(1, dest[0]);
            Assert.AreEqual(2, dest[1]);
            Assert.AreEqual(3, dest[2]);
            Assert.AreEqual(4, dest[3]);
        }

        [Test]
        public void Convert_IntWithTypeDouble_SimpleTypeConversionConvertsIntToDouble()
        {
            const int source = 1000;

            var dest = ValueConverter.Convert(source, typeof(double));

            Assert.IsInstanceOfType(typeof(double), dest);
            Assert.AreEqual(source, dest);
        }

        [Test]
        public void Convert_IntWithTypeNullableInt_ConverstIntToNulableInt()
        {
            const int source = 1000;

            var dest = (int?)ValueConverter.Convert(source, typeof(int?));

            Assert.IsNotNull(dest);
            Assert.AreEqual(source,dest);
        }

        enum TestEnum
        {
            Value1=1,
            Value2=2
        }

        [Test]
        public void Convert_StringWithTypeEnum_ConvertToEnumType()
        {
            const string source = "Value2";

            var dest = (TestEnum)ValueConverter.Convert(source, typeof(TestEnum));

            Assert.IsNotNull(dest);
            Assert.AreEqual(TestEnum.Value2, dest);
        }

        [Test]
        public void Convert_IntWithTypeEnum_ConvertToEnumType()
        {
            const int source = 2;

            var dest = (TestEnum)ValueConverter.Convert(source, typeof(TestEnum));

            Assert.IsNotNull(dest);
            Assert.AreEqual(TestEnum.Value2, dest);
        }

        [Test]
        public void Convert_DoubleWithTypeEnum_ConvertToEnumType()
        {
            const double source = 2.0d;

            var dest = (TestEnum)ValueConverter.Convert(source, typeof(TestEnum));

            Assert.IsNotNull(dest);
            Assert.AreEqual(TestEnum.Value2, dest);
        }
    }
}