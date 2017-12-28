using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.VO.VoTable
{
    [TestClass]
    public class XmlReaderExtensionsTest
    {
        private XmlReader GetReader(string xml)
        {
            var settings = new XmlReaderSettings()
            {
                Async = true,
                IgnoreWhitespace = true
            };

            return XmlReader.Create(new StringReader(xml), settings);
        }

        [TestMethod]
        public async Task MoveAfterStartTest()
        {
            var xml =
@"<root>
<elem1 attrib=""value"">
    <elem2>inner text</elem2>
</elem1>
</root>";
            var r = GetReader(xml);

            r.MoveToContent();
            Assert.AreEqual("root", r.Name);

            Assert.IsFalse(await r.MoveAfterStartAsync("elem2"));
            Assert.AreEqual(XmlNodeType.Text, r.NodeType);
        }

        [TestMethod]
        public async Task MoveAfterStartEmptyTest()
        {
            var xml =
@"<root>
<elem1 attrib=""value"">
    <elem2 />
</elem1>
</root>";
            var r = GetReader(xml);

            Assert.IsTrue(await r.MoveAfterStartAsync("elem2"));
            Assert.AreEqual("elem1", r.Name);
            Assert.AreEqual(XmlNodeType.EndElement, r.NodeType);
        }

        [TestMethod]
        public async Task MoveAfterEndTest()
        {
            var xml =
@"<root>
<elem1 attrib=""value"">
    <elem2>inner text</elem2>
</elem1>
</root>";
            var r = GetReader(xml);

            r.MoveToContent();
            Assert.AreEqual("root", r.Name);

            Assert.IsFalse(await r.MoveAfterEndAsync("elem2"));
            Assert.AreEqual(XmlNodeType.EndElement, r.NodeType);
            Assert.AreEqual("elem1", r.Name);
        }

        [TestMethod]
        public async Task MoveAfterEndEmptyTest()
        {
            var xml =
@"<root>
<elem1 attrib=""value"">
    <elem2 />
</elem1>
</root>";
            var r = GetReader(xml);

            r.MoveToContent();
            Assert.AreEqual("root", r.Name);

            Assert.IsTrue(await r.MoveAfterEndAsync("elem2"));
            Assert.AreEqual(XmlNodeType.EndElement, r.NodeType);
            Assert.AreEqual("elem1", r.Name);
        }

        [TestMethod]
        public async Task MoveAfterEndInsideTest()
        {
            var xml =
@"<root>
<elem1 attrib=""value"">
    <elem2>inner text</elem2>
</elem1>
</root>";
            var r = GetReader(xml);

            r.MoveToContent();
            Assert.AreEqual("root", r.Name);

            Assert.IsFalse(await r.MoveAfterStartAsync("elem2"));
            Assert.AreEqual(XmlNodeType.Text, r.NodeType);
            Assert.IsFalse(await r.MoveAfterEndAsync("elem2"));
            Assert.AreEqual(XmlNodeType.EndElement, r.NodeType);
            Assert.AreEqual("elem1", r.Name);
        }
    }
}
