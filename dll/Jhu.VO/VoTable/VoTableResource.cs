using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data.Common;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Jhu.VO.VoTable
{
    /// <summary>
    /// Implements functionality responsible for reading and writing a single
    /// resource block within a VOTable.
    /// </summary>
    [Serializable]
    public class VoTableResource : ICloneable
    {
        private static readonly char[] whitespace = new[] { ' ', '\t', '\r', '\n' };
        private static readonly System.Globalization.CultureInfo invariantCulture = System.Globalization.CultureInfo.InvariantCulture;

        private delegate object TextColumnReader(VoTableColumn column, string text);
        private delegate void TextColumnWriter(VoTableColumn column, XmlReader writer, object value);
        private delegate object BinaryColumnReader(VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter);
        private delegate void BinaryColumnWriter(VoTableColumn column, byte[] buffer, SharpFitsIO.BitConverterBase bitConverter, object value);

        [NonSerialized]
        private Dictionary<Type, VoTableDataTypeMapping> dataTypeMappings;

        /// <summary>
        /// Collection of table columns
        /// </summary>
        [NonSerialized]
        private List<VoTableColumn> columns;

        [NonSerialized]
        protected VoTable file;

        private VoTableSerialization serialization;
        private byte[] binaryBuffer;
        private TextColumnReader[] textColumnReaders;
        private TextColumnWriter[] textColumnWriters;
        private BinaryColumnReader[] binaryColumnReaders;
        private BinaryColumnWriter[] binaryColumnWriters;

        #region Properties

        [IgnoreDataMember]
        public ReadOnlyCollection<VoTableColumn> Columns
        {
            get
            {
                return new ReadOnlyCollection<VoTableColumn>(columns);
            }
        }

        /// <summary>
        /// Gets the objects wrapping the whole VOTABLE file.
        /// </summary>
        private VoTable File
        {
            get { return file; }
        }

        #endregion
        #region Constructors and initializers

        /// <summary>
        /// Initializes a VOTable resource block object.
        /// </summary>
        /// <param name="file"></param>
        public VoTableResource(VoTable file)
        {
            InitializeMembers();

            this.file = file;
        }

        public VoTableResource(VoTableResource old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.file = null;
            this.serialization = VoTableSerialization.Unknown;
            this.binaryBuffer = null;
            this.textColumnReaders = null;
            this.textColumnWriters = null;
            this.binaryColumnReaders = null;
            this.binaryColumnWriters = null;
        }

        private void CopyMembers(VoTableResource old)
        {
            this.file = old.file;
            this.serialization = old.serialization;
            this.binaryBuffer = null;
            this.textColumnReaders = null;
            this.textColumnWriters = null;
            this.binaryColumnReaders = null;
            this.binaryColumnWriters = null;
        }

        public object Clone()
        {
            return new VoTableResource(this);
        }

        #endregion
        #region Column functions

        public void RegisterTypeMapping(VoTableDataTypeMapping mapping)
        {
            if (dataTypeMappings == null)
            {
                dataTypeMappings = new Dictionary<Type, VoTableDataTypeMapping>();
            }

            dataTypeMappings[mapping.From] = mapping;
        }
        
        public void CreateColumns(VoTableColumn[] columns)
        {
            this.columns = new List<VoTableColumn>(columns);
        }

        public void CreateColumns(Type structType)
        {
            throw new NotImplementedException();
        }

        public void CreateColumns(DbDataReader dr)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Reader functions

        public void ReadHeader()
        {
            ReadResourceElement();
        }

        private void ReadResourceElement()
        {
            // The reader is now positioned on the RESOURCE tag   
            File.XmlReader.ReadStartElement(Constants.TagResource);

            // Read until the the TABLE tag is found and call the specific reader function
            // which will then read the table header and stop at the DATA tag.
            // If no table tag found we will bump into the closing </RESOURCE> tag anyway,
            // so throw and exception there
            // Also, throw exception on embeded RESOURCE tags.

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
                VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) != 0)
            {
                // Stop criterium: </RESOURCE>
                if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                    VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagResource) == 0)
                {
                    throw Error.TableNotFound();
                }

                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_1.AnyText>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_1.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_1.Coosys>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_1.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_1.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case VoTableVersion.V1_2:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_2.AnyText>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_2.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_2.Coosys>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_2.Group>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_2.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_2.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VoTableVersion.V1_3:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_3.AnyText>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_3.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_3.CoordinateSystem>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_3.Group>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_3.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_3.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                File.XmlReader.MoveToContent();
            }

            ReadTableElement();
            ReadDataElement();
            // Reader is positioned on the first TR tag or the STREAM tag now
        }

        private void ReadTableElement()
        {
            // While processing the above tags, collect info on columns
            columns = new List<VoTableColumn>();

            File.XmlReader.ReadStartElement(Constants.TagTable);

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                // Stop criterium: </TABLE>
                if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                    VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) == 0)
                {
                    throw Error.DataNotFound();
                }

                IField field = null;

                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_1.AnyText>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_1.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_1.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_1.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_1.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VoTableVersion.V1_2:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_2.AnyText>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_2.Info>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_2.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_2.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_2.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_2.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VoTableVersion.V1_3:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_3.AnyText>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_3.Info>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_3.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_3.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_3.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_3.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                }

                if (field != null)
                {
                    var c = VoTableColumn.Create(field);
                    columns.Add(c);
                }

                File.XmlReader.MoveToContent();
            }

            // The reader is now positioned on a DATA tag
        }

        private void ReadDataElement()
        {
            File.XmlReader.ReadStartElement(Constants.TagData);
            File.XmlReader.MoveToContent();

            if (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) == 0)
            {
                serialization = VoTableSerialization.TableData;
                ReadTableDataElement();
            }
            else if (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary) == 0)
            {
                serialization = VoTableSerialization.Binary;
                ReadBinaryElement();
            }
            else if (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary2) == 0)
            {
                serialization = VoTableSerialization.Binary2;
                ReadBinary2Element();
            }
            else if (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagFits) == 0)
            {
                throw Error.UnsupportedSerialization(VoTableSerialization.Fits);
            }
        }

        private void ReadTableDataElement()
        {
            // TODO: position reader on the very first TR tag
            // subsequent processing will be done when OnReadNextRow is called
            // by the framework

            File.XmlReader.ReadStartElement(Constants.TagTableData);

            // All subsequent tags will be read row-by-row

            CreateTextColumnReaders();
            ParseMagicNull();
        }

        private void ReadBinaryElement()
        {
            File.XmlReader.ReadStartElement(Constants.TagBinary);
            File.XmlReader.MoveToContent();
            ReadStreamElement();
        }

        private void ReadBinary2Element()
        {
            File.XmlReader.ReadStartElement(Constants.TagBinary2);
            File.XmlReader.MoveToContent();
            ReadStreamElement();
        }

        private void ReadStreamElement()
        {
            // The reader is now positioned on a STREAM element
            if (File.XmlReader.GetAttribute(Constants.AttributeHref) != null)
            {
                throw Error.ReferencedStreamsNotSupported();
            }

            // Figure out content encoding
            var encattr = File.XmlReader.GetAttribute(Constants.AttributeEncoding);

            if (encattr == null)
            {
                throw Error.EncodingNotFound();
            }

            if (!Enum.TryParse(encattr, true, out VoTableEncoding encoding) ||
                encoding != VoTableEncoding.Base64)
            {
                throw Error.EncodingNotSupported(encattr);
            }

            File.XmlReader.ReadStartElement(Constants.TagStream);
            // Now the reader is positioned on a base64 encoded binary stream

            CreateTextColumnReaders();
            ParseMagicNull();
            CreateBinaryColumnReaders();

            // TODO: estimate size of stride buffer
            binaryBuffer = new byte[0x10000];
        }

        private void ParseMagicNull()
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].DataType.NullValue != null)
                {
                    columns[i].DataType.NullValue = textColumnReaders[i](columns[i], (string)columns[i].DataType.NullValue);
                }
            }
        }

        /// <summary>
        /// Completes reading of a table and stops on the last tag.
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure to read all possible data
        /// rows that the client didn't consume.
        /// </remarks>
        public void ReadToFinish()
        {
            // This can be called by the framework anywhere within a RESOURCE tag

            // Make sure that binary mode components are destroyed
            binaryBuffer = null;

            // Make sure that the reader is position right after the
            // closing TABLEDATA/BINARY/BINARY2/FITS tag. There might be INFO tags
            // that will be read in the footer
            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   File.XmlReader.NodeType == XmlNodeType.EndElement &&
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) != 0 &&
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary) != 0 &&
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary2) != 0 &&
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagFits) != 0)
            {
                File.XmlReader.Read();
            }

            File.XmlReader.ReadEndElement();

            // Now the reader is positioned after the TABLEDATA/BINARY/BINARY2/FITS tags
            // Footer reader will consume the rest of the tags.
        }

        /// <summary>
        /// Completes reading of a resource by reading its closing tag.
        /// </summary>
        public void ReadFooter()
        {
            // Make sure the ending RESOURCE tag is read and the reader
            // is positioned at the next tag

            // The TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VoTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VoTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) != 0)
            {
                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VoTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VoTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagResource) != 0)
            {
                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VoTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VoTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // TODO: in reality, the RESOURCE tag could contain additional special tags
            }

            File.XmlReader.ReadEndElement();
        }

        #endregion
        #region Row reader functions

        public Task<bool> ReadNextRowAsync(object[] values, int startIndex)
        {
            // TODO: take ID from attribute of TR tag
            // TODO: use encoding from TD tag

            if (serialization == VoTableSerialization.TableData)
            {

                return ReadNextRowFromTableAsync(values, startIndex);
            }
            else
            {
                return ReadNextRowFromStreamAsync(values, startIndex);
            }
        }
        
        private async Task<bool> ReadNextRowFromTableAsync(object[] values, int startIndex)
        {
            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) == 0 ||
                 VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) == 0))
            {
                // End of table
                return false;
            }
            else
            {
                // Consume TR tag
                File.XmlReader.ReadStartElement(Constants.TagTR);

                // Read the TD tags
                var q = 0;
                while (true)
                {
                    if (File.XmlReader.NodeType == XmlNodeType.Element &&
                        VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTD) == 0)
                    {

                        if (!File.XmlReader.IsEmptyElement)
                        {
                            File.XmlReader.ReadStartElement(Constants.TagTD);

                            var text = await File.XmlReader.ReadContentAsStringAsync();
                            values[startIndex + q] = textColumnReaders[q](columns[q], text);

                            if (columns[q].DataType.NullValue != null &&
                                columns[q].DataType.NullValue.Equals(values[startIndex + q]))
                            {
                                values[startIndex + q] = null;
                            }

                            // Consume closing tag
                            File.XmlReader.ReadEndElement();
                        }
                        else
                        {
                            values[startIndex + q] = null;
                            File.XmlReader.Read();
                        }
                    }
                    else if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                        VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTR) == 0)
                    {
                        // End of a row found
                        File.XmlReader.ReadEndElement();

                        break;
                    }
                    else
                    {
                        throw Error.InvalidFormat();
                    }
                    q++;
                }

                return true;
            }
        }

        private async Task<bool> ReadNextRowFromStreamAsync(object[] values, int startIndex)
        {
            // TODO: implement arrays

            try
            {
                if (serialization == VoTableSerialization.Binary2)
                {
                    int prefixlen = (Columns.Count + 7) / 8;

                    var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, prefixlen);

                    if (prefixlen != s)
                    {
                        return false;
                    }

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        int bb = i / 8;
                        int bi = i - bb * 8;

                        if (((binaryBuffer[bb] << bi) & 0x80) != 0)
                        {
                            values[startIndex + i] = DBNull.Value;
                        }
                        else
                        {
                            values[startIndex + i] = null;
                        }
                    }
                }

                for (int i = 0; i < Columns.Count; i++)
                {
                    if (values[startIndex + i] != DBNull.Value)
                    {
                        var column = Columns[i];

                        if (column.DataType.IsFixedLength)
                        {
                            var l = column.DataType.ByteSize;

                            if (column.DataType.HasLength)
                            {
                                l *= column.DataType.Length;
                            }

                            var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            values[startIndex +i] = binaryColumnReaders[i](column, binaryBuffer, l, File.BitConverter);
                        }
                        else
                        {
                            var l = 4;
                            var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            var length = File.BitConverter.ToInt32(binaryBuffer, 0);
                            l = column.DataType.ByteSize * length;

                            // If stride buffer is not enough, increase
                            if (l > binaryBuffer.Length)
                            {
                                binaryBuffer = new byte[l]; // TODO: round up to 64k blocks?
                            }

                            s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            values[startIndex + i] = binaryColumnReaders[i](column, binaryBuffer, length, File.BitConverter);
                        }
                    }
                }

                return true;
            }
            catch (EndOfStreamException)
            {
                binaryBuffer = null;
                return false;
            }
        }

        private void CreateTextColumnReaders()
        {
            textColumnReaders = new TextColumnReader[Columns.Count];

            for (int i = 0; i < textColumnReaders.Length; i++)
            {
                var datatype = Columns[i].DataType;
                var type = datatype.Type;

                // TODO: how to deal with bit arrays?
                // TODO: how to deal with arrays in general?

                if (type == typeof(Boolean))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        if (text == "1" || text == "t" || text == "T")
                        {
                            return true;
                        }
                        else if (text == "0" || text == "f" || text == "F")
                        {
                            return false;
                        }
                        else if (text == "" || text == " " || text == "?")
                        {
                            return null;
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    };
                }
                else if (type == typeof(Byte))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Byte.Parse(text);
                    };
                }
                else if (type == typeof(Int16))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Int16.Parse(text);
                    };
                }
                else if (type == typeof(Int32))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Int32.Parse(text);
                    };
                }
                else if (type == typeof(Int64))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Int64.Parse(text);
                    };
                }
                else if (type == typeof(Single))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Single.Parse(text, invariantCulture);
                    };
                }
                else if (type == typeof(Double))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return Double.Parse(text, invariantCulture);
                    };
                }
                else if (type == typeof(SharpFitsIO.SingleComplex))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return SharpFitsIO.SingleComplex.Parse(text, invariantCulture);
                    };
                }
                else if (type == typeof(SharpFitsIO.DoubleComplex))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        return SharpFitsIO.DoubleComplex.Parse(text, invariantCulture);
                    };
                }
                else if (type == typeof(string))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        // TODO: what if text is encoded?
                        return text;
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void CreateBinaryColumnReaders()
        {
            binaryColumnReaders = new BinaryColumnReader[Columns.Count];

            for (int i = 0; i < binaryColumnReaders.Length; i++)
            {
                var datatype = Columns[i].DataType;
                var type = datatype.Type;

                // TODO: how to deal with bit arrays?
                // TODO: how to deal with arrays in general?

                if (type == typeof(Boolean))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return buffer[0] != 0;
                    };
                }
                else if (type == typeof(Byte))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return buffer[0];
                    };
                }
                else if (type == typeof(Int16))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt16(buffer, 0);
                    };
                }
                else if (type == typeof(Int32))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt32(buffer, 0);
                    };
                }
                else if (type == typeof(Int64))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt64(buffer, 0);
                    };
                }
                else if (type == typeof(Single))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToSingle(buffer, 0);
                    };
                }
                else if (type == typeof(Double))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToDouble(buffer, 0);
                    };
                }
                else if (type == typeof(SharpFitsIO.SingleComplex))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToSingleComplex(buffer, 0);
                    };
                }
                else if (type == typeof(SharpFitsIO.DoubleComplex))
                {
                    binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToDoubleComplex(buffer, 0);
                    };
                }
                else if (type == typeof(string))
                {
                    if (datatype.IsUnicode)
                    {
                        // Unicode
                        binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                        {
                            return Encoding.Unicode.GetChars(buffer, 0, 2 * length);
                        };
                    }
                    else
                    {
                        // ASCII
                        // Fixed length
                        binaryColumnReaders[i] = delegate (VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                        {
                            return Encoding.ASCII.GetChars(buffer, 0, length);
                        };
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        #endregion
        #region Writer functions

        /// <summary>
        /// Writes the resource header into the stream.
        /// </summary>
        public async Task WriteHeaderAsync()
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagResource, null);
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTable, null);

            // Write columns
            for (int i = 0; i < Columns.Count; i++)
            {
                await WriteColumnAsync(Columns[i]);
            }

            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagData, null);
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTableData, null);
        }

        private async Task WriteColumnAsync(VoTableColumn column)
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagField, null);

            await File.XmlWriter.WriteAttributeStringAsync(null, Constants.AttributeName, null, column.Name);
            // *** TODO: write other column properties

            await File.XmlWriter.WriteEndElementAsync();
        }


        public async Task WriteNextRowAsync(params object[] values)
        {
            /*
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTR, null);

            for (int i = 0; i < Columns.Count; i++)
            {
                // TODO: Do not use format here, or use standard votable formatting
                if (values[i] == DBNull.Value)
                {
                    // TODO: how to handle nulls in VOTable?
                    // Leave field blank
                }
                else
                {
                    await File.XmlWriter.WriteElementStringAsync(null, Constants.TagTD, null, ColumnFormatters[i](values[i], "{0}"));
                }
            }

            await File.XmlWriter.WriteEndElementAsync();
            */

            throw new NotImplementedException();
        }

        public async Task WriteFromDataReaderAsync(DbDataReader dr)
        {
            throw new NotImplementedException();
        }

        public async Task WriteFooterAsync()
        {
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
        }

        #endregion
        #region Read delegate generator functions

        private int ReadString(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, out object value)
        {
            throw new NotImplementedException();
        }

        private int ReadScalar(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, out object value)
        {
            throw new NotImplementedException();
        }

        private int ReadArray(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, out object value)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Write delegate generator functions

        private int WriteString(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, object value)
        {
            throw new NotImplementedException();
        }

        private int WriteScalar(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, object value)
        {
            throw new NotImplementedException();
        }

        private int WriteArray(SharpFitsIO.BitConverterBase converter, VoTableColumn col, byte[] bytes, int startIndex, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}