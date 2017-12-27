using System;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.VO.VoTable
{
    public abstract class VoTableTestBase : TestClassBase
    {
        protected abstract Type VoTableType { get; }

        private string GetNamespace(Type type)
        {
            var attr = (XmlTypeAttribute)type.GetCustomAttributes(typeof(XmlTypeAttribute), false).FirstOrDefault();
            return attr.Namespace;
        }

        private string GetXsd(string ns)
        {
            // http://www.ivoa.net/xml/VOTable/v1.1
            // Schema_VoTable_v1_1

            var parts = ns.Split('/');
            var res = String.Format("Schema_{0}_{1}",
                parts[parts.Length - 2],
                parts[parts.Length - 1].Replace('.', '_'));

            return Resources.ResourceManager.GetString(res);
        }

        private XmlReader OpenReader(string xml)
        {
            // Read an XML file and validate against the xsd schema
            var type = VoTableType;
            var ns = GetNamespace(type);
            var xsd = GetXsd(ns);
            var schema = XmlSchema.Read(XmlReader.Create(new StringReader(xsd)), new ValidationEventHandler(ValidationCallBack));

            var settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.Schemas.Add(schema);

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

        protected object Deserialize(string xml)
        {
            var r = OpenReader(xml);
            var s = new XmlSerializer(VoTableType);
            s.UnknownNode += delegate (object sender, XmlNodeEventArgs e)
            {
                // Add breakpoint here to see parsing problems.
            };
            var t = s.Deserialize(r);

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

        protected T ReadElementHelper<T>(string xml, string tag)
        {
            var root = new XmlRootAttribute()
            {
                ElementName = tag,
                Namespace = Constants.NamespaceVoTableV1_3,
            };
            var s = new XmlSerializer(typeof(T), root);
            s.UnknownNode += delegate (object sender, XmlNodeEventArgs e)
            {
                throw new NotImplementedException();
            };
            var r = new XmlTextReader(new StringReader(xml))
            {
                WhitespaceHandling = WhitespaceHandling.Significant
            };

            var t = (T)s.Deserialize(r);

            Assert.IsTrue(r.EOF);

            return t;
        }
    }
}
