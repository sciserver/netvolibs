using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Threading.Tasks;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable
{
    /// <summary>
    /// Implements functionality to read and write VOTables.
    /// </summary>
    [Serializable]
    public class VoTable : SharpFitsIO.BinaryDataFile, IDisposable, ICloneable
    {
        public static readonly StringComparer Comparer = StringComparer.InvariantCulture;

        [NonSerialized]
        private XmlReader xmlReader;

        [NonSerialized]
        private bool ownsXmlReader;

        [NonSerialized]
        private XmlWriter xmlWriter;

        [NonSerialized]
        private bool ownsXmlWriter;

        [NonSerialized]
        private VoTableVersion version;

        [NonSerialized]
        private IVoTable votable;

        [NonSerialized]
        private string @namespace;

        [NonSerialized]
        private List<VoTableResource> resources;

        [NonSerialized]
        private int resourceCounter;

        public XmlReader XmlReader
        {
            get { return xmlReader; }
        }

        public XmlWriter XmlWriter
        {
            get { return xmlWriter; }
        }

        [IgnoreDataMember]
        public VoTableVersion Version
        {
            get { return version; }
        }

        [IgnoreDataMember]
        public string Namespace
        {
            get { return @namespace; }
        }

        [IgnoreDataMember]
        protected List<VoTableResource> Resources
        {
            get { return resources; }
        }

        [IgnoreDataMember]
        public override bool IsClosed
        {
            get { return base.IsClosed && xmlReader == null && xmlWriter == null; }
        }

        #region Constructors and initializers

        /// <summary>
        /// Initializes a VOTable object without opening any underlying stream.
        /// </summary>
        public VoTable()
        {
            InitializeMembers(new StreamingContext());
        }

        public VoTable(VoTable old)
        {
            CopyMembers(old);
        }

        public VoTable(string path, FileAccess fileAccess, VoTableVersion version)
            : base(path, fileAccess, SharpFitsIO.Endianness.BigEndian)
        {
            InitializeMembers(new StreamingContext());
            this.version = version;
            Open();
        }

        public VoTable(Stream stream, FileAccess fileAccess, VoTableVersion version)
            : base(stream, fileAccess, SharpFitsIO.Endianness.BigEndian)
        {
            InitializeMembers(new StreamingContext());
            this.version = version;
            Open();
        }
        
        /// <summary>
        /// Initializes a VOTable object by re-using an already open xml reader.
        /// </summary>
        /// <param name="xmlReader"></param>
        public VoTable(XmlReader xmlReader)
        {
            InitializeMembers(new StreamingContext());

            this.FileAccess = FileAccess.Read;
            this.xmlReader = xmlReader;
        }

        /// <summary>
        /// Initializes a VOTable object by re-using an already open xml writer.
        /// </summary>
        /// <param name="xmlWriter"></param>
        public VoTable(XmlWriter xmlWriter)
        {
            InitializeMembers(new StreamingContext());

            this.FileAccess = FileAccess.Write;
            this.xmlWriter = xmlWriter;
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.xmlReader = null;
            this.ownsXmlReader = false;
            this.xmlWriter = null;
            this.ownsXmlWriter = false;

            this.version = VoTableVersion.Unknown;

            this.resources = new List<VoTableResource>();
            this.resourceCounter = -1;
        }

        private void CopyMembers(VoTable old)
        {
            this.xmlReader = null;
            this.ownsXmlReader = false;
            this.xmlWriter = null;
            this.ownsXmlWriter = false;

            this.version = old.version;

            // Deep copy resources
            this.resources = new List<VoTableResource>();
            foreach (var r in old.resources)
            {
                this.resources.Add((VoTableResource)r.Clone());
            }
            this.resourceCounter = old.resourceCounter;
        }

        public object Clone()
        {
            return new VoTable(this);
        }

        #endregion
        #region Stream open/close

        protected override void EnsureNotOpen()
        {
            base.EnsureNotOpen();

            if (ownsXmlReader && xmlReader != null ||
                ownsXmlReader && xmlWriter != null)
            {
                throw new InvalidOperationException();
            }
        }

        protected override void OpenForRead()
        {
            base.OpenForRead();

            if (xmlReader == null)
            {
                OpenOwnXmlReader();
            }
        }

        protected override void OpenForWrite()
        {
            base.OpenForWrite();

            InitializeVersion();

            if (xmlWriter == null)
            {
                OpenOwnXmlWriter();
            }
        }

        private void OpenOwnXmlReader()
        {
            // TODO: add validation
            // but how to do it if version is not known?

            var settings = new XmlReaderSettings()
            {
                Async = true,
                IgnoreComments = true,
                IgnoreWhitespace = true,

                // TODO: add schemas
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessSchemaLocation
            };
            xmlReader = XmlReader.Create(WrappedStream, settings);
            ownsXmlReader = true;
        }

        private void OpenOwnXmlWriter()
        {
            var settings = new XmlWriterSettings()
            {
                Async = true,
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "  ",
            };
            xmlWriter = XmlWriter.Create(WrappedStream, settings);
            ownsXmlWriter = true;
        }

        public override void Close()
        {
            if (ownsXmlReader && xmlReader != null)
            {
                xmlReader.Close();
                xmlReader.Dispose();
                xmlReader = null;
                ownsXmlReader = false;
            }

            if (ownsXmlWriter && xmlWriter != null)
            {
                xmlWriter.Flush();
                xmlWriter.Close();
                xmlWriter.Dispose();
                xmlWriter = null;
                ownsXmlWriter = false;
            }

            base.Close();
        }

        #endregion
        #region XML utility functions

        private XmlSerializer CreateSerializer(Type type, string elementName, string @namespace)
        {
            XmlRootAttribute root = null;

            if (elementName != null || @namespace != null)
            {
                root = new XmlRootAttribute()
                {
                    ElementName = elementName,
                    Namespace = @namespace,
                    IsNullable = false
                };
            }

            var s = new XmlSerializer(type, root);
            return s;
        }

        public T ReadElement<T>()
        {
            return ReadElement<T>(null, null);
        }

        public T ReadElement<T>(string elementName, string @namespace)
        {
            var t = votable.GetType(typeof(T));
            var s = CreateSerializer(t, elementName, @namespace);
            return (T)s.Deserialize(XmlReader);
        }

        public void WriteElement<T>(T value)
        {
            WriteElement<T>(value, null, null);
        }

        public void WriteElement<T>(T value, string elementName, string @namespace)
        {
            if (value != null)
            {
                var s = CreateSerializer(typeof(T), elementName, @namespace);
                s.Serialize(XmlWriter, value);
            }
        }

        public void WriteElements(IEnumerable values)
        {
            foreach (var i in values)
            {
                var s = CreateSerializer(i.GetType(), null, null);
                s.Serialize(XmlWriter, i);
            }
        }

        public async Task WriteAttributeAsync(string name, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                await XmlWriter.WriteAttributeStringAsync("", name, null, value);
            }
        }

        public async Task WriteAttributesAsync(IEnumerable<XmlAttribute> attributes)
        {
            if (attributes != null)
            {
                foreach (var a in attributes)
                {
                    await XmlWriter.WriteAttributeStringAsync("", a.Name, null, a.Value);
                }
            }
        }

        #endregion
        #region Version support

        private void InitializeVersion()
        {
            // TODO: move this to somewhere else (maybe create function?)
            // Initialize internal objects
            switch (version)
            {
                case VoTableVersion.V1_1:
                    votable = new V1_1.VoTable();
                    @namespace = Constants.NamespaceVoTableV1_1;
                    break;
                case VoTableVersion.V1_2:
                    votable = new V1_2.VoTable();
                    @namespace = Constants.NamespaceVoTableV1_2;
                    break;
                case VoTableVersion.V1_3:
                    votable = new V1_3.VoTable();
                    @namespace = Constants.NamespaceVoTableV1_3;
                    break;
                default:
                    throw Error.UnsupportedVersion(version.ToString());
            }
        }

        #endregion
        #region VOTable reader implementation

        public async Task ReadHeaderAsync()
        {
            // Skip initial declarations
            await XmlReader.MoveToContentAsync();

            switch (XmlReader.NamespaceURI)
            {
                case Constants.NamespaceVoTableV1_1:
                    version = VoTableVersion.V1_1;
                    break;
                case Constants.NamespaceVoTableV1_2:
                    version = VoTableVersion.V1_2;
                    break;
                case Constants.NamespaceVoTableV1_3:
                    version = VoTableVersion.V1_3;
                    break;
                default:
                    throw Error.UnsupportedVersion(XmlReader.NamespaceURI);
            }

            InitializeVersion();

            // Reader now should be positioned on the VOTABLE tag
            // Read attributes

            votable.ID = XmlReader.GetAttribute(Constants.AttributeID);
            votable.Version = XmlReader.GetAttribute(Constants.AttributeVersion);

            await XmlReader.MoveAfterStartAsync(Constants.TagVoTable);

            // Read all tags inside VOTABLE but stop at any RESOURCE tag
            // because they are handled outside of this function
            while (!XmlReader.IsStartElement(Constants.TagResource))
            {
                switch (XmlReader.Name)
                {
                    case Constants.TagDescription:
                        votable.Description = ReadElement<IAnyText>(Constants.TagDescription, @namespace);
                        break;
                    case Constants.TagDefinitions:
                        votable.Definitions = ReadElement<IDefinitions>();
                        break;
                    case Constants.TagCoosys:
                        votable.CoosysList.Add(ReadElement<ICoordinateSystem>());
                        break;
                    case Constants.TagGroup:
                        switch (version)
                        {
                            case VoTableVersion.V1_1:
                                break;
                            case VoTableVersion.V1_2:
                            case VoTableVersion.V1_3:
                                votable.GroupList.Add(ReadElement<IGroup>());
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case Constants.TagParam:
                        votable.ParamList.Add(ReadElement<IParam>());
                        break;
                    case Constants.TagInfo:
                        votable.InfoList1.Add(ReadElement<IInfo>());
                        break;
                    default:
                        throw Error.InvalidFormat();
                }
            }

            // TODO:
            // * INFO -- also parse TAP query status

            // Reader is positioned on the first RESOURCE tag now
            // Header is read completely, now wait for framework to call OnReadNextBlock
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        public VoTableResource ReadNextResource()
        {
            // Check if the current tag is a RESOURCE. If so, create a new VOTableResource object
            // and return it. Subsequent handling of tags until the closing RESOURCE tag will
            // be done inside the VOTableResource class

            if (XmlReader.IsStartElement(Constants.TagResource))
            {
                return new VoTableResource(this);
            }
            else
            {
                return null;
            }
        }

        public Task ReadFooterAsync()
        {
            // The TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.
            
            while (!XmlReader.IsEndElement(Constants.TagVoTable))
            {
                switch (version)
                {
                    case VoTableVersion.V1_1:
                        // no additional info tags
                        break;
                    case VoTableVersion.V1_2:
                    case VoTableVersion.V1_3:
                        votable.InfoList2.Add(ReadElement<IInfo>());
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            
            XmlReader.ReadEndElement();

            return Task.CompletedTask;
        }

        #endregion
        #region VOTable writer implementation
        
        public async Task WriteHeaderAsync()
        {
            await XmlWriter.WriteStartDocumentAsync();

            string ver = null;

            switch (version)
            {
                case VoTableVersion.V1_1:
                    ver = Constants.VersionV1_1;
                    break;
                case VoTableVersion.V1_2:
                    ver = Constants.VersionV1_2;
                    break;
                case VoTableVersion.V1_3:
                    ver = Constants.VersionV1_3;
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            await XmlWriter.WriteStartElementAsync(null, Constants.TagVoTable, @namespace);
            await XmlWriter.WriteAttributeStringAsync("xmlns", "xsd", null, Constants.NamespaceXsd);
            await XmlWriter.WriteAttributeStringAsync("xmlns", "xsi", null, Constants.NamespaceXsi);
            await XmlWriter.WriteAttributeStringAsync("xmlns", "xlink", null, Constants.NamespaceXlink);

            await WriteAttributeAsync(Constants.AttributeID, "votable_1");
            await WriteAttributeAsync(Constants.AttributeVersion, ver);

            WriteElement(votable.Description, Constants.TagDescription, @namespace);
            WriteElement(votable.Definitions);
            WriteElements(votable.CoosysList);
            WriteElements(votable.GroupList);
            WriteElements(votable.ParamList);
            WriteElements(votable.InfoList1);
        }

        public async Task WriteFooterAsync()
        {
            await XmlWriter.WriteEndElementAsync();
        }

        #endregion
    }
}
