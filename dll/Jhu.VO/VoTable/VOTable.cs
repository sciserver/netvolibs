using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
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

            var settings = new XmlReaderSettings()
            {
                Async = true,
                IgnoreComments = true,
                IgnoreWhitespace = true,
                // TODO: add schemas
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

            // TODO: debug code, delete
            s.UnknownNode += delegate (object sender, XmlNodeEventArgs e)
            {
                throw new NotImplementedException();
            };

            return (T)s.Deserialize(XmlReader);
        }

        #endregion
        #region VOTable reader implementation

        public void ReadHeader()
        {
            ReadVOTableElement();
        }

        private void ReadVOTableElement()
        {
            // Skip initial declarations
            XmlReader.MoveToContent();

            switch (XmlReader.NamespaceURI)
            {
                case Constants.VOTableNamespaceV1_1:
                    version = VoTableVersion.V1_1;
                    break;
                case Constants.VOTableNamespaceV1_2:
                    version = VoTableVersion.V1_2;
                    break;
                case Constants.VOTableNamespaceV1_3:
                    version = VoTableVersion.V1_3;
                    break;
                default:
                    throw Error.UnsupportedVersion(XmlReader.NamespaceURI);
            }

            // Reader now should be positioned on the VOTABLE tag
            // Read attributes

            var id = XmlReader.GetAttribute(Constants.AttributeID);
            var ver = XmlReader.GetAttribute(Constants.AttributeVersion);

            // Finish reading tag and move to next content
            XmlReader.ReadStartElement(Constants.TagVOTable);
            XmlReader.MoveToContent();

            // Read all tags inside VOTABLE but stop at any RESOURCE tag
            // because they are handled outside of this function
            while (XmlReader.NodeType == XmlNodeType.Element &&
                   Comparer.Compare(XmlReader.Name, Constants.TagResource) != 0)
            {
                switch (version)
                {
                    case VoTableVersion.V1_1:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                var d = Deserialize<V1_1.AnyText>();
                                break;
                            case Constants.TagDefinitions:
                            case Constants.TagCoosys:
                            case Constants.TagGroup:
                            case Constants.TagParam:
                            case Constants.TagInfo:
                                // TODO: implement deserializets,
                                // now just skip the tag
                                XmlReader.Skip();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VoTableVersion.V1_2:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                var d = Deserialize<V1_2.AnyText>();
                                break;
                            case Constants.TagDefinitions:
                            case Constants.TagCoosys:
                            case Constants.TagGroup:
                            case Constants.TagParam:
                            case Constants.TagInfo:
                                // TODO: implement deserializets,
                                // now just skip the tag
                                XmlReader.Skip();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VoTableVersion.V1_3:
                        switch (XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                var d = Deserialize<V1_3.AnyText>();
                                break;
                            case Constants.TagDefinitions:
                            case Constants.TagCoosys:
                            case Constants.TagGroup:
                            case Constants.TagParam:
                            case Constants.TagInfo:
                                // TODO: implement deserializets,
                                // now just skip the tag
                                XmlReader.Skip();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                XmlReader.MoveToContent();
            }

            // TODO: implement parsers for header info including these tags:
            // * DESCRIPTION
            // * DEFINITIONS -- just ignore because it's deprecated
            // * COOSYS -- just ignore because it's deprecated
            // * GROUP
            // * PARAM
            // * INFO -- also parse TAP query status

            // Reader is positioned on the first RESOURCE tag now
            // Header is read completely, now wait for framework to call OnReadNextBlock

            // TODO: make sure XSD validation fails when no RESOURCE tag found
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

            if (XmlReader.NodeType == XmlNodeType.Element &&
                VoTable.Comparer.Compare(XmlReader.Name, Constants.TagResource) == 0)
            {
                return new VoTableResource(this);
            }
            else
            {
                return null;
            }
        }

        public void ReadFooter()
        {
            // The TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.

            while (XmlReader.NodeType != XmlNodeType.EndElement ||
                   VoTable.Comparer.Compare(XmlReader.Name, Constants.TagVOTable) != 0)
            {
                switch (version)
                {
                    case VoTableVersion.V1_1:
                        Deserialize<V1_1.Info>();
                        break;
                    case VoTableVersion.V1_2:
                        Deserialize<V1_2.Info>();
                        break;
                    case VoTableVersion.V1_3:
                        Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            XmlReader.ReadEndElement();
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
