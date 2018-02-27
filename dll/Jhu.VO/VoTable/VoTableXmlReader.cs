using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jhu.VO.VoTable
{
    /// <summary>
    /// Provides logic to automatically add default namespace to XML when it is
    /// not provided as required by the standard. The correct namespace is necessary
    /// to properly deserialize tags into objects.
    /// </summary>
    public class VoTableXmlReader : VoXmlReader
    {
        public static new VoTableXmlReader Create(System.IO.Stream input)
        {
            var xml = XmlReader.Create(input);
            return new VoTableXmlReader(xml);
        }

        public static new VoTableXmlReader Create(System.IO.Stream input, XmlReaderSettings settings)
        {
            var xml = XmlReader.Create(input, settings);
            return new VoTableXmlReader(xml);
        }

        public static new VoTableXmlReader Create(System.IO.TextReader input)
        {
            var xml = XmlReader.Create(input);
            return new VoTableXmlReader(xml);
        }

        public static new VoTableXmlReader Create(System.IO.TextReader input, XmlReaderSettings settings)
        {
            var xml = XmlReader.Create(input, settings);
            return new VoTableXmlReader(xml);
        }

        public VoTableXmlReader(XmlReader reader)
            : base(reader)
        {
        }

        protected override void OnResolveFormat(string localName, string namespaceUri, VoXmlAttribute[] attributes)
        {
            base.OnResolveFormat(localName, namespaceUri, attributes);

            if (localName != Constants.TagVoTable)
            {
                throw Error.InvalidFormat();
            }

            if (VoTable.GetVersionFromNamespace(namespaceUri) != VoTableVersion.Unknown)
            {
                // Proper namespace is defined
                return;
            }
            else
            {
                // No namespace is defined,
                // look for the version attribute and add default namespace to reader
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i].LocalName == Constants.AttributeVersion)
                    {
                        var version = VoTable.GetVersionFromVersionString(attributes[i].Value);
                        var ns = VoTable.GetNamespaceFromVersion(version);

                        OverrideEmptyNamespace = true;
                        DefaultNamespaceUri = ns;

                        return;
                    }
                }

                throw Error.InvalidFormat();
            }
        }
    }
}
