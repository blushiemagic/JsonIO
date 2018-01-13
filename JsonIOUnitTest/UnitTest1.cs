using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonIO;

namespace JsonIOUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadConstants()
        {
            Assert.AreEqual(JBool.True, JsonReader.ReadJson("true"));
            Assert.AreEqual(JBool.False, JsonReader.ReadJson("false"));
            Assert.AreEqual(JNull.Value, JsonReader.ReadJson("null"));
        }

        [TestMethod]
        public void ReadNumbers()
        {
            Assert.AreEqual(new JInt(12), JsonReader.ReadJson("12"));
            Assert.AreEqual(new JInt(12000), JsonReader.ReadJson("12e3"));
            Assert.AreEqual(new JInt(-12), JsonReader.ReadJson("-12"));
            Assert.AreEqual(new JFloatingPoint(0.12), JsonReader.ReadJson("12e-2"));
            Assert.AreEqual(new JFloatingPoint(12.345), JsonReader.ReadJson("12.345"));
            Assert.AreEqual(new JFloatingPoint(123.45), JsonReader.ReadJson("1.2345e2"));
            Assert.AreNotEqual(new JFloatingPoint(123.45), JsonReader.ReadJson("123.456"));
        }

        [TestMethod]
        public void ReadStrings()
        {
            Assert.AreEqual(new JString("abc"), JsonReader.ReadJson("\"abc\""));
            Assert.AreEqual(new JString("abc\n"), JsonReader.ReadJson("\"abc\\n\""));
            Assert.AreEqual(new JString("abcde"), JsonReader.ReadJson("\"ab\\u0063de\""));
        }

        [TestMethod]
        public void ReadEmptyStructures()
        {
            Assert.AreEqual(new JList(), JsonReader.ReadJson("[]"));
            Assert.AreEqual(new JObject(), JsonReader.ReadJson("{}"));
        }
    }
}
