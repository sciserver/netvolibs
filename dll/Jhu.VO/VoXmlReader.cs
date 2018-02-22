using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Jhu.VO
{
    /// <summary>
    /// Wraps an xml reader to support custom logic, specifically
    /// ignoring nodes with unsupported namespaces
    /// </summary>
    public class VoXmlReader : XmlReader
    {
        #region Private member variables

        private bool skipUnsupportedElements;
        private bool skipUnsupportedTypes;
        private XmlReader reader;

        #endregion
        #region Properties

        public bool SkipUnsupportedElements
        {
            get { return skipUnsupportedElements; }
            set { skipUnsupportedElements = value; }
        }

        public override XmlNodeType NodeType
        {
            get { return reader.NodeType; }
        }

        #endregion
        #region Wrapped properties not touched

        public override int AttributeCount => reader.AttributeCount;
        public override string BaseURI => reader.BaseURI;
        public override bool CanReadBinaryContent => reader.CanReadBinaryContent;
        public override bool CanReadValueChunk => reader.CanReadValueChunk;
        public override bool CanResolveEntity => reader.CanResolveEntity;
        public override int Depth => reader.Depth;
        public override bool EOF => reader.EOF;
        public override bool HasAttributes => reader.HasAttributes;
        public override bool HasValue => reader.HasValue;
        public override bool IsDefault => reader.IsDefault;
        public override bool IsEmptyElement => reader.IsEmptyElement;
        public override string LocalName => reader.LocalName;
        public override string Name => reader.Name;
        public override string NamespaceURI => reader.NamespaceURI;
        public override XmlNameTable NameTable => reader.NameTable;
        public override string Prefix => reader.Prefix;
        public override char QuoteChar => reader.QuoteChar;
        public override ReadState ReadState => reader.ReadState;
        public override IXmlSchemaInfo SchemaInfo => reader.SchemaInfo;
        public override XmlReaderSettings Settings => reader.Settings;
        public override string this[int i] => reader[i];
        public override string this[string name, string namespaceURI] => reader[name, namespaceURI];
        public override string this[string name] => reader[name];
        public override string Value => reader.Value;
        public override Type ValueType => reader.ValueType;
        public override string XmlLang => reader.XmlLang;
        public override XmlSpace XmlSpace => reader.XmlSpace;

        #endregion
        #region Constructors and initializers

        public VoXmlReader(XmlReader reader)
        {
            InitializeMembers();

            this.reader = reader;
        }

        private void InitializeMembers()
        {
            this.skipUnsupportedElements = true;
            this.skipUnsupportedTypes = true;
            this.reader = null;
        }

        #endregion

        private bool SkipUnsupportedElement()
        {
            if (!String.IsNullOrEmpty(reader.NamespaceURI) &&
                !Constants.SupportedNameSpaces.Contains(reader.NamespaceURI))
            {
                reader.Skip();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SkipUnsupportedType()
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.NamespaceURI == XmlSchema.InstanceNamespace &&
                    reader.LocalName == "type")
                {
                    // Get type namespace and look up namespace URI
                    var value = reader.Value;
                    var idx = value.IndexOf(':');

                    if (idx > 0)
                    {
                        var prefix = value.Substring(0, idx);
                        var ns = LookupNamespace(prefix);

                        if (!Constants.SupportedNameSpaces.Contains(ns))
                        {
                            // This node references an unsupported type
                            reader.Skip();
                            return true;
                        }
                    }
                }
            }

            // Wind back pointer
            reader.MoveToElement();
            return false;
        }

        public override bool Read()
        {
            var res = reader.Read();

            if (skipUnsupportedElements && reader.NodeType == XmlNodeType.Element && 
                SkipUnsupportedElement())
            {
                return !reader.EOF;
            }
            else if (skipUnsupportedTypes && reader.NodeType == XmlNodeType.Element &&
                SkipUnsupportedType())
            {
                return !reader.EOF;
            }
            else
            {
                return res;
            }
        }

        public override XmlNodeType MoveToContent()
        {
            reader.MoveToContent();

            if (skipUnsupportedElements && reader.NodeType == XmlNodeType.Element &&
                SkipUnsupportedElement())
            {
                MoveToContent();
            }
            else if (skipUnsupportedTypes && reader.NodeType == XmlNodeType.Element &&
                SkipUnsupportedType())
            {
                MoveToContent();
            }
            else
            {
            }

            return reader.NodeType;
        }

        #region Wrapped methods not touched
        
        public override string LookupNamespace(string prefix)
        {
            return reader.LookupNamespace(prefix);
        }

        public override string GetAttribute(int i)
        {
            return reader.GetAttribute(i);
        }

        public override string GetAttribute(string name)
        {
            return reader.GetAttribute(name);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return reader.GetAttribute(name, namespaceURI);
        }

        public override bool MoveToAttribute(string name)
        {
            return reader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToFirstAttribute()
        {
            return reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return reader.MoveToNextAttribute();
        }

        public override bool MoveToElement()
        {
            return reader.MoveToElement();
        }

        public override bool ReadAttributeValue()
        {
            return reader.ReadAttributeValue();
        }

        public override void ResolveEntity()
        {
            reader.ResolveEntity();
        }

        public override void Close()
        {
            reader.Close();
        }

        protected override void Dispose(bool disposing)
        {
            reader.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}
