using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.VO.VoTable.V1_3
{
    [TestClass]
    public class VoTableTest : VoTableTestBase
    {
        protected override Type VoTableType
        {
            get { return typeof(VoTable); }
        }

        [TestMethod]
        public void ValidationTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_everything.xml"));
            Validate(xml);
        }

        [TestMethod]
        public void DeserializationTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_v1.3.xml"));
            Deserialize(xml);
        }

        [TestMethod]
        public void DeserializeEverythingTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_everything.xml"));
            Deserialize(xml);
        }

        [TestMethod]
        public void VoTableDeserializeMultiresourceTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_multiresource.xml"));
            Deserialize(xml);
        }

        [TestMethod]
        public void VoTableDeserializeBinaryTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_binary.xml"));
            Deserialize(xml);
        }

        [TestMethod]
        public void VoTableDeserializeBinary2Test()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_binary2.xml"));
            Deserialize(xml);
        }
        
        [TestMethod]
        public void VoTableDeserializeFitsTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_fits.xml"));
            Deserialize(xml);
        }
        
        // TODO: add tests for existing services: vizier, gaia, etc.
    }
}
