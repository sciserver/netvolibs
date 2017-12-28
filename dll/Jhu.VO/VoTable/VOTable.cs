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
        private V1_1.VoTable votable_v1_1;

        [NonSerialized]
        private V1_2.VoTable votable_v1_2;

        [NonSerialized]
        private V1_3.VoTable votable_v1_3;

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
            get { return XmlWriter; }
        }

        [IgnoreDataMember]
        public VoTableVersion Version
        {
            get { return version; }
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

        public VoTable(string path, FileAccess fileAccess, SharpFitsIO.Endianness endianness)
            :base(path, fileAccess, endianness)
        {
            InitializeMembers(new StreamingContext());
            Open();
        }

        public VoTable(string path, FileAccess fileAccess)
            : this(path, fileAccess, SharpFitsIO.Endianness.BigEndian)
        {
            // Overload
        }

        public VoTable(Stream stream, FileAccess fileAccess, SharpFitsIO.Endianness endianness)
            :base(stream, fileAccess, endianness)
        {
            InitializeMembers(new StreamingContext());
            Open();
        }

        public VoTable(Stream stream, FileAccess fileAccess)
            : this(stream, fileAccess, SharpFitsIO.Endianness.BigEndian)
        {
            // Overload
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
            ownsXmlWriter = true;
            xmlWriter = XmlWriter.Create(WrappedStream);
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

        public T Deserialize<T>()
        {
            return Deserialize<T>(null, null);
        }

        public T Deserialize<T>(string elementName, string @namespace)
        {
            XmlRootAttribute root = null;

            if (elementName != null || @namespace != null)
            {
                root = new XmlRootAttribute()
                {
                    ElementName = elementName,
                    Namespace = @namespace
                };
            }

            var s = new XmlSerializer(typeof(T), root);
            return (T)s.Deserialize(XmlReader);
        }

        #endregion
        #region VOTable reader implementation

        public Task ReadHeaderAsync()
        {
            return ReadVOTableElementAsync();
        }

        private async Task ReadVOTableElementAsync()
        {
            // Skip initial declarations
            await XmlReader.MoveToContentAsync();

            switch (XmlReader.NamespaceURI)
            {
                case Constants.NamespaceVoTableV1_1:
                    version = VoTableVersion.V1_1;
                    votable_v1_1 = new V1_1.VoTable();
                    break;
                case Constants.NamespaceVoTableV1_2:
                    version = VoTableVersion.V1_2;
                    votable_v1_2 = new V1_2.VoTable();
                    break;
                case Constants.NamespaceVoTableV1_3:
                    version = VoTableVersion.V1_3;
                    votable_v1_3 = new V1_3.VoTable();
                    break;
                default:
                    throw Error.UnsupportedVersion(XmlReader.NamespaceURI);
            }

            // Reader now should be positioned on the VOTABLE tag
            // Read attributes

            var id = XmlReader.GetAttribute(Constants.AttributeID);
            var ver = XmlReader.GetAttribute(Constants.AttributeVersion);

            await XmlReader.MoveAfterStartAsync(Constants.TagVoTable);

            // Read all tags inside VOTABLE but stop at any RESOURCE tag
            // because they are handled outside of this function
            while (!XmlReader.IsStartElement(Constants.TagResource))
            {
                switch (version)
                {
                    case VoTableVersion.V1_1:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                votable_v1_1.Description = Deserialize<V1_1.AnyText>(Constants.TagDescription, Constants.NamespaceVoTableV1_1);
                                break;
                            case Constants.TagDefinitions:
                                votable_v1_1.Definitions = Deserialize<V1_1.Definitions>();
                                break;
                            case Constants.TagCoosys:
                                votable_v1_1.CoosysList.Add(Deserialize<V1_1.Coosys>());
                                break;
                            case Constants.TagParam:
                                votable_v1_1.ParamList.Add(Deserialize<V1_1.Param>());
                                break;
                            case Constants.TagInfo:
                                votable_v1_1.InfoList1.Add(Deserialize<V1_1.Info>());
                                break;
                            default:
                                throw Error.InvalidFormat();
                        }
                        break;

                    case VoTableVersion.V1_2:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                votable_v1_2.Description = Deserialize<V1_2.AnyText>(Constants.TagDescription, Constants.NamespaceVoTableV1_2);
                                break;
                            case Constants.TagDefinitions:
                                votable_v1_2.Definitions = Deserialize<V1_2.Definitions>();
                                break;
                            case Constants.TagCoosys:
                                votable_v1_2.CoosysList.Add(Deserialize<V1_2.CoordinateSystem>());
                                break;
                            case Constants.TagGroup:
                                votable_v1_2.GroupList.Add(Deserialize<V1_2.Group>());
                                break;
                            case Constants.TagParam:
                                votable_v1_2.ParamList.Add(Deserialize<V1_2.Param>());
                                break;
                            case Constants.TagInfo:
                                votable_v1_2.InfoList1.Add(Deserialize<V1_2.Info>());
                                break;
                            default:
                                throw Error.InvalidFormat();
                        }
                        break;

                    case VoTableVersion.V1_3:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                votable_v1_3.Description = Deserialize<V1_3.AnyText>(Constants.TagDescription, Constants.NamespaceVoTableV1_3);
                                break;
                            case Constants.TagDefinitions:
                                votable_v1_3.Definitions = Deserialize<V1_3.Definitions>();
                                break;
                            case Constants.TagCoosys:
                                votable_v1_3.CoosysList.Add(Deserialize<V1_3.CoordinateSystem>());
                                break;
                            case Constants.TagGroup:
                                votable_v1_3.GroupList.Add(Deserialize<V1_3.Group>());
                                break;
                            case Constants.TagParam:
                                votable_v1_3.ParamList.Add(Deserialize<V1_3.Param>());
                                break;
                            case Constants.TagInfo:
                                votable_v1_3.InfoList1.Add(Deserialize<V1_3.Info>());
                                break;
                            default:
                                throw Error.InvalidFormat();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
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
                        votable_v1_2.InfoList2.Add(Deserialize<V1_2.Info>());
                        break;
                    case VoTableVersion.V1_3:
                        votable_v1_3.InfoList2.Add(Deserialize<V1_3.Info>());
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            XmlReader.ReadEndElement();

            return Task.CompletedTask;
        }

        #endregion


























#if false

        /// <summary>
        /// Writes the file header but stops before the block header.
        /// </summary>
        /// <remarks>
        /// Writes from VOTABLE until the RESOURCE tag.
        /// </remarks>
        private void WriteVOTableElement()
        {
            await XmlWriter.WriteStartElementAsync(null, Constants.TagVOTable, null);
            await XmlWriter.WriteAttributeStringAsync(null, Constants.AttributeVersion, null, Constants.VOTableVersion);

            // *** TODO: how to add these?
            //XmlWriter.WriteAttributeString("xmlns", Constants.VOTableXsi);
            //XmlWriter.WriteAttributeString("xmlns:xsi", Constants.VOTableXsi);
            //XmlWriter.WriteAttributeString("xmlns:stc", Constants.StcNs);
        }

        // Write end element
        await XmlWriter.WriteEndElementAsync();
        

        protected override void OnWriteHeader()
        {
            StartVoTable();

            for (int i = 0; i < Columns.Count; i++)
            {
                outputWriter.WriteStartElement(VOTableKeywords.Field);
                outputWriter.WriteAttributeString(VOTableKeywords.Name, Columns[i].Name.Replace(separator, '_'));
                outputWriter.WriteAttributeString(VOTableKeywords.DataType, Columns[i].DataType.Name.ToString());
                outputWriter.WriteEndElement();               
            }

            StartVOTableData();
        }

        private void StartVoTable() {
            
            outputWriter.WriteStartElement(VOTableKeywords.VoTable);
            outputWriter.WriteStartElement(VOTableKeywords.Resource);
            outputWriter.WriteStartElement(VOTableKeywords.Table);
        }

        private void StartVOTableData()
        {
            outputWriter.WriteStartElement(VOTableKeywords.Data);
            outputWriter.WriteStartElement(VOTableKeywords.TableData);
        }

        protected override void OnWrite(object[] values)
        {           
            outputWriter.WriteStartElement(VOTableKeywords.TR);
            for (int i = 0; i < Columns.Count; i++)
            {
                outputWriter.WriteElementString(VOTableKeywords.TD, ColumnFormatters[i](values[i], Columns[i].Format));   
            }
            outputWriter.WriteEndElement();
            
        }
        protected override void OnWriteFooter()
        {
            outputWriter.WriteEndElement();
            outputWriter.WriteEndElement();
            outputWriter.Flush();
            outputWriter.Close();            
        }
       
        public override bool IsClosed {
            get { return false; }
        }

        protected FormatterDelegate[] ColumnFormatters
        {
            get { return columnFormatters; }
        }

        public ParserDelegate[] ColumnParsers
        {
            get { return columnParsers; }
        }        

        private bool GetVOTableData(out string[] parts)
        {
            int cnt = 0;
            parts = new string[Columns.Count];
            try
            {
                inputReader.ReadToFollowing(VOTableKeywords.TD);
                do
                {
                    parts[cnt] = inputReader.ReadString();
                    cnt++;
                } while (inputReader.ReadToNextSibling(VOTableKeywords.TD));

                inputReader.ReadToNextSibling(VOTableKeywords.TR);
                return true;

            }catch(Exception exp){
                return false;
            }
        }

        /*
        public void Open(XmlTextWriter output, Encoding en, CultureInfo culture) {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Write;
            this.outputWriter = output;            
        }
        
        public void Open(XmlReader input)
        {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Read;
            this.inputReader = input;            
        }

        public void EnsureNotOpen() {
            if (this.inputReader != null) {
                throw new InvalidOperationException();
            }
        }
         * */
#endif
    }
}
