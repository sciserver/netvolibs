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
    public class VoTableElementsTest : VoTableTestBase
    {
        protected override Type VoTableType
        {
            get { return typeof(VoTable); }
        }

        #region Element deserialization tests

        [TestMethod]
        public void DeserializeDescriptionTest()
        {
            var xml = @"<DESCRIPTION xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"">This is just simple text or can contain <b>embeded HTML</b></DESCRIPTION>";
            ReadElementHelper<V1_3.AnyText>(xml, Constants.TagDescription);
        }


        [TestMethod]
        public void DeserializeCoosysTest()
        {
            var xml = @"<COOSYS xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" ID='coosys_FK5' system='eq_FK5' equinox='2000.0'/>";
            var e = ReadElementHelper<V1_3.CoordinateSystem>(xml, Constants.TagCoosys);

            Assert.AreEqual("coosys_FK5", e.ID);
            Assert.AreEqual("eq_FK5", e.System);
            Assert.AreEqual("2000.0", e.Equinox);
            // Need check
        }

        [TestMethod]
        public void DeserializeDefinitionsTest()
        {
            var xml = @"<DEFINITIONS xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" />";
            var e = ReadElementHelper<V1_3.Definitions>(xml, Constants.TagDefinitions);

            // TODO
        }


        [TestMethod]
        public void DeserializeDescriptionTest2()
        {
            var xml = @"<DESCRIPTION xmlns=""http://www.ivoa.net/xml/VOTable/v1.3""> Velocities and Distance estimations </DESCRIPTION>";
            var e = ReadElementHelper<V1_3.AnyText>(xml, Constants.TagDescription);

            Assert.AreEqual(" Velocities and Distance estimations ", e.Text);
        }

        [TestMethod]
        public void DeserializeFieldTest()
        {
            var xml =
@"<FIELD xmlns=""http://www.ivoa.net/xml/VOTable/v1.3""
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
            var e = ReadElementHelper<V1_3.Field>(xml, Constants.TagField);

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
        public void DeserializeFieldRefTest()
        {
            var xml = @"<FIELDref xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" ref= ""field_2"" ucd = ""em.phot.mag"" utype = ""any.thing2"" />";
            var e = ReadElementHelper<V1_3.FieldRef>(xml, Constants.TagFieldRef);

            Assert.AreEqual("field_2", e.Ref);
            Assert.AreEqual("em.phot.mag", e.Ucd);
            Assert.AreEqual("any.thing2", e.UType);
        }

        [TestMethod]
        public void DeserializeGroupTest()
        {
            var xml = @"
<GROUP xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" ID=""link1"" name=""group_name"" ref=""???"" ucd=""em.phot"" utype=""???"">
    <DESCRIPTION>this is a text</DESCRIPTION>
    <PARAM datatype=""char"" arraysize=""*"" ucd=""pos.frame"" name=""cooframe""
        utype=""stc:AstroCoords.coord_system_id"" value=""UTC-ICRS-TOPO"" />
    <FIELDref ref=""col1""/>
    <FIELDref ref=""col2""/>
</GROUP>";
            var e = ReadElementHelper<V1_3.Group>(xml, Constants.TagGroup);

            Assert.AreEqual("link1", e.ID);
            Assert.AreEqual("group_name", e.Name);
            Assert.AreEqual("???", e.Ref);
            Assert.AreEqual("em.phot", e.Ucd);
            Assert.AreEqual("???", e.UType);

            Assert.AreEqual(3, e.ItemList_ForXml.Count);
            Assert.AreEqual(1, e.ParamList.Count);
            Assert.AreEqual(2, e.FieldRefList.Count);
        }

        [TestMethod]
        public void DeserializeInfoTest()
        {
            var xml = @"<INFO xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" name=""info_tag"" value=""0"">inner text</INFO>";
            var e = ReadElementHelper<V1_3.Info>(xml, Constants.TagInfo);

            Assert.AreEqual("info_tag", e.Name);
            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("inner text", e.Text);
        }

        [TestMethod]
        public void DeserializeLinkTest()
        {
            var xml = @"<LINK xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" ID=""link1"" content-role=""location"" content-type=""text/plain"" title=""Another representation"" value=""whatever value"" href=""http://voservices.net/skyquery"" gref=""???"" action=""http://voservices.net/skyquery""/> ";
            var e = ReadElementHelper<V1_3.Link>(xml, Constants.TagLink);

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
        public void DeserializeMaxTest()
        {
            var xml = @"<MAX xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" value=""0.32"" inclusive=""yes"" />";
            var e = ReadElementHelper<V1_3.Max>(xml, Constants.TagMax);

            Assert.AreEqual("0.32", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }

        [TestMethod]
        public void DeserializeMinTest()
        {
            var xml = @"<MIN xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" value=""0"" inclusive=""yes"" />";
            var e = ReadElementHelper<V1_3.Min>(xml, Constants.TagMin);

            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }


        [TestMethod]
        public void DeserializeOptionTest()
        {
            var xml = @"<OPTION xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" name=""na"" value=""va"" />";
            var e = ReadElementHelper<V1_3.Option>(xml, Constants.TagOption);

            Assert.AreEqual("na", e.Name);
            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void DeserializeParamDeserializeTest()
        {
            var xml = @"<PARAM xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" value=""va""/>";
            var e = ReadElementHelper<V1_3.Param>(xml, Constants.TagParam);

            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void DeserializeParamRefTest()
        {
            var xml = @"<PARAMref xmlns=""http://www.ivoa.net/xml/VOTable/v1.3"" ref=""re"" ucd=""uc"" utype=""ut""/>";
            var e = ReadElementHelper<V1_3.ParamRef>(xml, Constants.TagParamRef);

            Assert.AreEqual("re", e.Ref);
            Assert.AreEqual("uc", e.Ucd);
            Assert.AreEqual("ut", e.UType);
        }

        #endregion
    }
}
