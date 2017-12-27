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
    public class VoTableTest : TestClassBase
    {
        #region XSD-based validation tests

        protected XmlReader OpenReader(string xml)
        {
            // Read an XML file and validate against the xsd schema
            var schema = XmlReader.Create(new StringReader(Resources.Schema_VoTable_v1_3));

            var settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.Schemas.Add(Constants.VOTableNamespaceV1_3, schema);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            return reader;
        }

        protected void Validate(string xml)
        {
            var reader = OpenReader(xml);
            
            // Parse the whole file. 
            while (reader.Read()) ;
        }

        protected VoTable Deserialize(string xml)
        {
            var r = OpenReader(xml);
            var s = new XmlSerializer(typeof(VoTable));
            s.UnknownNode += delegate (object sender, XmlNodeEventArgs e)
            {
                throw new NotImplementedException();
            };
            var t = (VoTable)s.Deserialize(r);

            Assert.IsTrue(r.EOF);

            return t;
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            }
            else
            {
                throw args.Exception;
            }
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
            var xml = File.ReadAllText(GetTestFilePath(@"test\files\votable\votable_everything.xml"));
            Deserialize(xml);
        }

        // TODO: implement tests for votables from various sources

        #endregion
    }
}
