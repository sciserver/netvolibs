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

namespace Jhu.VO.VoTable
{
    [TestClass]
    public class VoTableElementsTest : TestClassBase
    {
        protected void ValidateVOTable(string xml)
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

            // TODO: add the XSD
            //Resources.Schema_VoTable_v1_3

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            // Parse the file. 
            while (reader.Read()) ;

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
            var xml = File.ReadAllText(GetTestFilePath(@"modules\skyquery\test\files\votable_everything.xml"));
            ValidateVOTable(xml);
        }

        [TestMethod]
        public void DeserializerTest()
        {
            // The streaming reader cannot deserialize the entire
            // xml document into memory but the serializer can work
            // from a position and read a single tag (and its contents)

            var xml = @"<DESCRIPTION>This is just simple text or can contain <b>embeded HTML</b></DESCRIPTION>";

            var r = XmlReader.Create(new StringReader(xml));
            var s = new XmlSerializer(typeof(V1_3.AnyText));
            s.Deserialize(r);

            r.ReadEndElement();

            Assert.IsTrue(r.EOF);
        }

        private T ReadElementHelper<T>(string xml)
        {
            var s = new XmlSerializer(typeof(T));
            var r = new XmlTextReader(new StringReader(xml));
            r.Namespaces = false;

            return (T)s.Deserialize(r);
        }


        [TestMethod]
        public void ReadCoosysElementTest()
        {
            var xml = "<COOSYS ID='coosys_FK5' system='eq_FK5' equinox='2000.0'/>";
            var e = ReadElementHelper<V1_3.CoordinateSystem>(xml);

            Assert.AreEqual("coosys_FK5", e.ID);
            Assert.AreEqual("eq_FK5", e.System);
            Assert.AreEqual("2000.0", e.Equinox);
            // Need check
        }

        [TestMethod]
        public void ReadDefinitionsElementTest()
        {
            var xml = "";
            var e = ReadElementHelper<V1_3.Definitions>(xml);

            // TODO
        }


        [TestMethod]
        public void ReadDescriptionElementTest()
        {
            var xml = "<DESCRIPTION> Velocities and Distance estimations </DESCRIPTION>";
            var e = ReadElementHelper<V1_3.AnyText>(xml);

            Assert.AreEqual(" Velocities and Distance estimations ", e.Text);
        }

        [TestMethod]
        public void ReadFieldElementTest()
        {
            var xml =
@"<FIELD 
    ID=""ID"" 
    unit=""un"" 
    datatype=""dt"" 
    precision=""pr"" 
    width=""wi"" 
    xtype=""xt"" 
    ref=""re"" 
    name=""na"" 
    ucd=""uc"" 
    utype=""ut"" 
    arraysize=""as"" 
    type=""ty"">
    <VALUES null=""null"" />
</FIELD>";
            var e = ReadElementHelper<V1_3.Field>(xml);

            Assert.AreEqual("ID", e.ID);
            Assert.AreEqual("un", e.Unit);
            Assert.AreEqual("dt", e.Datatype);
            Assert.AreEqual("pr", e.Precision);
            Assert.AreEqual("wi", e.Width);
            Assert.AreEqual("xt", e.Xtype);
            Assert.AreEqual("re", e.Ref);
            Assert.AreEqual("na", e.Name);
            Assert.AreEqual("uc", e.Ucd);
            Assert.AreEqual("ut", e.UType);
            Assert.AreEqual("as", e.Arraysize);
            Assert.AreEqual("ty", e.Type);
            Assert.IsNotNull(e.Values);
        }

        [TestMethod]
        public void ReadFieldRefElementTest()
        {
            var xml = "<FIELDref ref= \"field_2\" ucd = \"em.phot.mag\" utype = \"any.thing2\" />";
            var e = ReadElementHelper<V1_3.FieldRef>(xml);

            Assert.AreEqual("field_2", e.Ref);
            Assert.AreEqual("em.phot.mag", e.Ucd);
            Assert.AreEqual("any.thing2", e.UType);
        }

        [TestMethod]
        public void ReadGroupElementTest()
        {
            var xml = " <GROUP ID=\"link1\" name=\"group_name\" ref=\"???\" ucd=\"em.phot\" utype=\"???\"></GROUP> ";
            var e = ReadElementHelper<V1_3.Group>(xml);

            Assert.AreEqual("link1", e.ID);
            Assert.AreEqual("group_name", e.Name);
            Assert.AreEqual("???", e.Ref);
            Assert.AreEqual("em.phot", e.Ucd);
            Assert.AreEqual("???", e.UType);
        }

        [TestMethod]
        public void ReadInfoElementTest()
        {
            var xml = "<INFO name=\"info_tag\" value=\"0\">inner text</INFO>";
            var e = ReadElementHelper<V1_3.Info>(xml);

            Assert.AreEqual("info_tag", e.Name);
            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("inner text", e.Text);
        }

        [TestMethod]
        public void ReadLinkElementTest()
        {
            var xml = "<LINK ID=\"link1\" content-role=\"location\" content-type=\"text/plain\" title=\"Another representation\" value=\"whatever value\" href=\"http://voservices.net/skyquery\" gref=\"???\" action=\"http://voservices.net/skyquery\"/> ";
            var e = ReadElementHelper<V1_3.Link>(xml);

            Assert.AreEqual("link1", e.ID);
            Assert.AreEqual("location", e.ContentRole);
            Assert.AreEqual("text/plain", e.ContentType);
            Assert.AreEqual("Another representation", e.Title);
            Assert.AreEqual("whatever value", e.Value);
            Assert.AreEqual("http://voservices.net/skyquery", e.Href);
            Assert.AreEqual("???", e.Gref);
            Assert.AreEqual("http://voservices.net/skyquery", e.Action);
        }

        [TestMethod]
        public void ReadMaxElementTest()
        {
            var xml = "<MAX value=\"0.32\" inclusive=\"yes\" />";
            var e = ReadElementHelper<V1_3.Max>(xml);

            Assert.AreEqual("0.32", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }

        [TestMethod]
        public void ReadMinElementTest()
        {
            var xml = "<MIN value=\"0\" inclusive=\"yes\" />";
            var e = ReadElementHelper<V1_3.Min>(xml);

            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }


        [TestMethod]
        public void ReadOptionElementTest()
        {
            var xml = "<OPTION name=\"na\" value=\"va\" />";
            var e = ReadElementHelper<V1_3.Option>(xml);

            Assert.AreEqual("na", e.Name);
            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void ReadParamElementTest()
        {
            var xml = "<PARAM value=\"va\"/>";
            var e = ReadElementHelper<V1_3.Param>(xml);

            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void ReadParamRefElementTest()
        {
            var xml = "<PARAMref ref=\"re\" ucd=\"uc\" utype=\"ut\"/>";
            var e = ReadElementHelper<V1_3.ParamRef>(xml);

            Assert.AreEqual("re", e.Ref);
            Assert.AreEqual("uc", e.Ucd);
            Assert.AreEqual("ut", e.UType);
        }
    }
}
