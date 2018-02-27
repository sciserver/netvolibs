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
    /// ignoring nodes with unsupported namespaces and providing
    /// default namespace support;
    /// </summary>
    /// <remarks>
    /// All names must be wrapped with NameTable.Add() when used
    /// with XmlSerializer
    /// </remarks>
    public class VoXmlReader : XmlReader
    {
        #region Private member variables

        private HashSet<string> supportedNamespaces;
        private string defaultNamespaceUri;

        private bool overrideEmptyNamespace;
        private bool skipUnsupportedElements;
        private bool skipUnsupportedTypes;

        private bool isFormatResolved;

        private XmlReader reader;

        #endregion
        #region Properties

        public bool OverrideEmptyNamespace
        {
            get { return overrideEmptyNamespace; }
            set { overrideEmptyNamespace = value; }
        }

        public string DefaultNamespaceUri
        {
            get { return defaultNamespaceUri; }
            set { defaultNamespaceUri = NameTable.Add(value); }
        }

        public bool SkipUnsupportedElements
        {
            get { return skipUnsupportedElements; }
            set { skipUnsupportedElements = value; }
        }

        public bool SkipUnsupportedTypes
        {
            get { return skipUnsupportedTypes; }
            set { skipUnsupportedTypes = value; }
        }

        public override XmlNodeType NodeType
        {
            get { return reader.NodeType; }
        }

        public override string NamespaceURI
        {
            get
            {
                if (overrideEmptyNamespace && defaultNamespaceUri != null &&
                    reader.NodeType == XmlNodeType.Element &&
                    String.IsNullOrEmpty(reader.NamespaceURI))
                {
                    // http://www.ivoa.net/xml/VOTable/v1.2
                    return defaultNamespaceUri;
                }
                else
                {
                    // http://www.ivoa.net/xml/VOTable/v1.2
                    return reader.NamespaceURI;
                }
            }
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

        public static new VoXmlReader Create(System.IO.Stream input)
        {
            var xml = XmlReader.Create(input);
            return new VoXmlReader(xml);
        }

        public static new VoXmlReader Create(System.IO.Stream input, XmlReaderSettings settings)
        {
            var xml = XmlReader.Create(input, settings);
            return new VoXmlReader(xml);
        }

        public static new VoXmlReader Create(System.IO.TextReader input)
        {
            var xml = XmlReader.Create(input);
            return new VoXmlReader(xml);
        }

        public static new VoXmlReader Create(System.IO.TextReader input, XmlReaderSettings settings)
        {
            var xml = XmlReader.Create(input, settings);
            return new VoXmlReader(xml);
        }

        public VoXmlReader(XmlReader reader)
        {
            InitializeMembers();

            this.reader = reader;
        }

        private void InitializeMembers()
        {
            this.supportedNamespaces = new HashSet<string>(Constants.SupportedNamespaces);
            this.defaultNamespaceUri = null;

            this.overrideEmptyNamespace = false;
            this.skipUnsupportedElements = true;
            this.skipUnsupportedTypes = true;

            this.isFormatResolved = false;

            this.reader = null;
        }

        #endregion

        private void ResolveFormat()
        {
            // Read the very first element, figure out format then rewind
            reader.MoveToContent();

            var tagLocalName = reader.LocalName;
            var tagNamespaceUri = reader.NamespaceURI;
            var attributes = new VoXmlAttribute[reader.AttributeCount];

            int q = 0;
            while (reader.MoveToNextAttribute())
            {
                attributes[q] = new VoXmlAttribute()
                {
                    LocalName = reader.LocalName,
                    NamespaceURI = reader.NamespaceURI,
                    Value = reader.Value
                };
                q++;
            }

            OnResolveFormat(tagLocalName, tagNamespaceUri, attributes);

            reader.MoveToElement();

            isFormatResolved = true;
        }

        protected virtual void OnResolveFormat(string localName, string namespaceUri, VoXmlAttribute[] attributes)
        {
        }

        private bool SkipUnsupportedElement()
        {
            if (!String.IsNullOrEmpty(reader.NamespaceURI) &&
                !supportedNamespaces.Contains(reader.NamespaceURI))
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

                        if (!supportedNamespaces.Contains(ns))
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

            if (!isFormatResolved && reader.NodeType == XmlNodeType.Element)
            {
                ResolveFormat();
            }

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

        public async override Task<bool> ReadAsync()
        {
            var res = await reader.ReadAsync();

            if (!isFormatResolved && reader.NodeType == XmlNodeType.Element)
            {
                ResolveFormat();
            }

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
            if (!isFormatResolved)
            {
                ResolveFormat();
            }

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

        public override async Task<XmlNodeType> MoveToContentAsync()
        {
            if (!isFormatResolved)
            {
                ResolveFormat();
            }

            await reader.MoveToContentAsync();

            if (skipUnsupportedElements && reader.NodeType == XmlNodeType.Element &&
                SkipUnsupportedElement())
            {
                await MoveToContentAsync();
            }
            else if (skipUnsupportedTypes && reader.NodeType == XmlNodeType.Element &&
                SkipUnsupportedType())
            {
                await MoveToContentAsync();
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

        public override Task<string> GetValueAsync()
        {
            return reader.GetValueAsync();
        }
        
        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
        {
            return reader.ReadContentAsBase64(buffer, index, count);
        }

        public override Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
        {
            return reader.ReadContentAsBase64Async(buffer, index, count);
        }

    }
}
