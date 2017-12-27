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

namespace Jhu.VO.VoTable.V1_1
{
    [TestClass]
    public class VoTableTest : VoTableTestBase
    {
        protected override Type VoTableType
        {
            get { return typeof(VoTable); }
        }

        [TestMethod]
        public void VoTableDeserializeMultiresourceTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_multiresource.xml"));
            Deserialize(xml);
        }

        // TODO: add tests for existing services: vizier, gaia, etc.
    }
}
