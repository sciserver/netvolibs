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
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable
{
    /// <summary>
    /// Implements functionality responsible for reading and writing a single
    /// resource block within a VOTable.
    /// </summary>
    [Serializable]
    public class VoTableResource : ICloneable
    {
        private static readonly System.Globalization.CultureInfo invariantCulture = System.Globalization.CultureInfo.InvariantCulture;

        private delegate object TextColumnReader(VoTableColumn column, string text);
        private delegate string TextColumnWriter(VoTableColumn column, object value);
        private delegate object BinaryColumnReader(VoTableColumn column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter);
        private delegate void BinaryColumnWriter(VoTableColumn column, byte[] buffer, SharpFitsIO.BitConverterBase bitConverter, object value);

        // Support one table per resource, no recursive resources
        [NonSerialized]
        private IResource resource;

        [NonSerialized]
        private ITable table;

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
        private bool tableEndReached;
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
            this.tableEndReached = false;
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
            this.tableEndReached = old.tableEndReached;
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

        public static VoTableResource Create(VoTable votable, VoTableSerialization serialization)
        {
            var resource = new VoTableResource(votable);
            resource.InitializeVersion(votable.Version);
            resource.serialization = serialization;
            return resource;
        }

        #endregion
        #region Version support

        private void InitializeVersion(VoTableVersion version)
        {
            // TODO: move this to somewhere else (maybe create function?)
            // Initialize internal objects
            switch (version)
            {
                case VoTableVersion.V1_1:
                    resource = new V1_1.Resource();
                    table = new V1_1.Table();
                    break;
                case VoTableVersion.V1_2:
                    resource = new V1_2.Resource();
                    table = new V1_2.Table();
                    break;
                case VoTableVersion.V1_3:
                    resource = new V1_3.Resource();
                    table = new V1_3.Table();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
        #region Column functions

        public void RegisterTypeMapping(VoTableDataTypeMapping mapping)
        {
            // TODO implement mapping in reader functions

            if (dataTypeMappings == null)
            {
                dataTypeMappings = new Dictionary<Type, VoTableDataTypeMapping>();
            }

            dataTypeMappings[mapping.From] = mapping;
        }

        public void CreateColumns(IList<VoTableColumn> columns)
        {
            this.columns = new List<VoTableColumn>(columns);

            for (int i = 0; i < columns.Count; i++)
            {
                var field = table.CreateField();
                columns[i].ToField(field);
                table.FieldList.Add(field);
            }
        }

        public void CreateColumns(Type structType)
        {
            throw new NotImplementedException();
        }

        public void CreateColumns(DbDataReader dr)
        {
            var schema = dr.GetSchemaTable();
            var columns = new VoTableColumn[schema.Rows.Count];

            for (int i = 0; i < columns.Length; i++)
            {
                var row = schema.Rows[i];

                var name = (string)row[SchemaTableColumn.ColumnName];
                var type = (Type)row[SchemaTableColumn.DataType];
                var length = (int)row[SchemaTableColumn.ColumnSize];
                var nullable = (bool)row[SchemaTableColumn.AllowDBNull];
                bool isVariableSize = false;
                bool isUnboundSize = false;
                int[] size = new int[] { 1 };

                // If data type is array or string then use length
                if (type.IsArray)
                {
                    // TODO: implement array support
                    throw new NotImplementedException();
                }
                else if (type == typeof(string))
                {
                    if (length < 1)
                    {
                        // This is a max column
                        isUnboundSize = true;
                    }

                    // TODO: add support for fixed length strings
                    isVariableSize = true;
                    size = new int[] { length };
                }

                // Create type and columns
                VoTableDataType votableType;
                if (dataTypeMappings != null && dataTypeMappings.ContainsKey(type))
                {
                    votableType = dataTypeMappings[type].MapType(size, isVariableSize, isUnboundSize);
                }
                else
                {
                    votableType = VoTableDataType.Create(type, size, isVariableSize, isUnboundSize);
                }

                columns[i] = VoTableColumn.Create("col" + i, name, votableType);
            }

            CreateColumns(columns);
        }

        #endregion
        #region Reader functions

        public async Task ReadHeaderAsync()
        {
            await ReadResourceElementAsync();
            await ReadTableElementAsync();
            await ReadDataElementAsync();

            if (!tableEndReached)
            {
                CreateTextColumnReaders();
                ParseMagicNulls();

                if (serialization == VoTableSerialization.Binary ||
                    serialization == VoTableSerialization.Binary2)
                {
                    CreateBinaryColumnReaders();
                }
            }

            // Reader is positioned on the first TR tag or the STREAM tag now
        }

        private Task ReadResourceElementAsync()
        {
            InitializeVersion(File.Version);

            // The reader is now positioned on the RESOURCE tag   
            File.XmlReader.ReadStartElement(Constants.TagResource);

            // TODO: read attributes

            // Read until the the TABLE tag is found and call the specific reader function
            // which will then read the table header and stop at the DATA tag.
            // If no table tag found we will bump into the closing </RESOURCE> tag anyway,
            // so throw and exception there
            // Also, throw exception on embeded RESOURCE tags.

            while (!File.XmlReader.IsStartElement(Constants.TagTable))
            {
                // Stop criterium: </RESOURCE>
                if (File.XmlReader.IsEndElement(Constants.TagResource))
                {
                    throw Error.TableNotFound();
                }

                switch (File.XmlReader.Name)
                {
                    case Constants.TagDescription:
                        resource.Description = File.ReadElement<IAnyText>(Constants.TagDescription, File.Namespace);
                        break;
                    case Constants.TagInfo:
                        resource.InfoList1.Add(File.ReadElement<IInfo>());
                        break;
                    case Constants.TagCoosys:
                        resource.CoosysList.Add(File.ReadElement<ICoordinateSystem>());
                        break;
                    case Constants.TagGroup:
                        switch (File.Version)
                        {
                            case VoTableVersion.V1_1:
                                break;
                            case VoTableVersion.V1_2:
                            case VoTableVersion.V1_3:
                                resource.GroupList.Add(File.ReadElement<IGroup>());
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case Constants.TagParam:
                        resource.ParamList.Add(File.ReadElement<IParam>());
                        break;
                    case Constants.TagLink:
                        resource.LinkList.Add(File.ReadElement<ILink>());
                        break;
                    case Constants.TagResource:
                        throw Error.RecursiveResourceNotSupported();
                    default:
                        throw Error.InvalidFormat();
                }
            }

            return Task.CompletedTask;
        }

        private Task ReadTableElementAsync()
        {
            // While processing the above tags, collect info on columns
            columns = new List<VoTableColumn>();

            File.XmlReader.ReadStartElement(Constants.TagTable);

            while (!File.XmlReader.IsStartElement(Constants.TagData))
            {
                // Stop criterium: </TABLE>
                if (File.XmlReader.IsEndElement(Constants.TagTable))
                {
                    throw Error.DataNotFound();
                }

                IField field = null;

                switch (File.XmlReader.Name)
                {
                    case Constants.TagDescription:
                        table.Description = File.ReadElement<IAnyText>(Constants.TagDescription, File.Namespace);
                        break;
                    case Constants.TagInfo:
                        switch (File.Version)
                        {
                            case VoTableVersion.V1_1:
                                break;
                            case VoTableVersion.V1_2:
                            case VoTableVersion.V1_3:
                                table.InfoList1.Add(File.ReadElement<IInfo>());
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case Constants.TagField:
                        field = File.ReadElement<IField>();
                        table.FieldList.Add(field);
                        break;
                    case Constants.TagParam:
                        table.ParamList.Add(File.ReadElement<IParam>());
                        break;
                    case Constants.TagGroup:
                        table.GroupList.Add(File.ReadElement<IGroup>());
                        break;
                    case Constants.TagLink:
                        table.LinkList.Add(File.ReadElement<ILink>());
                        throw Error.LinksNotSupported();
                    default:
                        throw Error.InvalidFormat();
                }

                if (field != null)
                {
                    var c = VoTableColumn.FromField(field);
                    columns.Add(c);
                }
            }

            // The reader is now positioned on a DATA tag
            return Task.CompletedTask;
        }

        private async Task ReadDataElementAsync()
        {
            await File.XmlReader.MoveAfterStartAsync(Constants.TagData);

            if (File.XmlReader.IsStartElement(Constants.TagTableData))
            {
                serialization = VoTableSerialization.TableData;
                await ReadTableDataElementAsync();
            }
            else if (File.XmlReader.IsStartElement(Constants.TagBinary))
            {
                serialization = VoTableSerialization.Binary;
                await ReadBinaryElementAsync();
            }
            else if (File.XmlReader.IsStartElement(Constants.TagBinary2))
            {
                serialization = VoTableSerialization.Binary2;
                await ReadBinary2ElementAsync();
            }
            else if (File.XmlReader.IsStartElement(Constants.TagFits))
            {
                // TODO: implement FITS serialization
                throw Error.UnsupportedSerialization(VoTableSerialization.Fits);
            }
            else
            {
                throw Error.UnsupportedSerialization(VoTableSerialization.Unknown);
            }
        }

        private async Task ReadTableDataElementAsync()
        {
            // Position reader on the very first TR tag
            // subsequent processing will be done when OnReadNextRow is called
            // by the framework

            // If the TABLEDATA is empty then we're on the DATA tag,
            // otherwise it's a TR tag

            // All subsequent tags will be read row-by-row

            var empty = await File.XmlReader.MoveAfterStartAsync(Constants.TagTableData);
            tableEndReached = empty;
        }

        private async Task ReadBinaryElementAsync()
        {
            await File.XmlReader.MoveAfterStartAsync(Constants.TagBinary);
            await ReadStreamElementAsync();
        }

        private async Task ReadBinary2ElementAsync()
        {
            await File.XmlReader.MoveAfterStartAsync(Constants.TagBinary2);
            await ReadStreamElementAsync();
        }

        private async Task ReadStreamElementAsync()
        {
            // The reader is now positioned on a STREAM element

            if (File.XmlReader.IsEmptyElement)
            {
                tableEndReached = true;
            }
            else
            {
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

                await File.XmlReader.MoveAfterStartAsync(Constants.TagStream);
                // Now the reader is positioned on a base64 encoded binary stream

                // TODO: estimate size of stride buffer
                binaryBuffer = new byte[0x10000];
            }
        }

        /// <summary>
        /// Completes reading of a table and stops on the last tag.
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure to read all possible data
        /// rows that the client didn't consume.
        /// </remarks>
        public async Task ReadToFinishAsync()
        {
            // This can be called by the framework anywhere within a RESOURCE tag

            // Make sure that binary mode components are destroyed
            binaryBuffer = null;

            // Make sure that the reader is position right after the
            // closing TABLEDATA/BINARY/BINARY2/FITS tag. There might be INFO tags
            // that will be read in the footer

            // If might happen that we are already outside the aforementioned
            // tags, if the table is read completely or was empty.

            if (!tableEndReached)
            {

                await File.XmlReader.MoveAfterEndAsync(
                    Constants.TagTableData, Constants.TagBinary,
                    Constants.TagBinary2, Constants.TagFits);
            }

            // Now the reader is positioned after the TABLEDATA/BINARY/BINARY2/FITS tags
            // Footer reader will consume the rest of the tags.
        }

        /// <summary>
        /// Completes reading of a resource by reading its closing tag.
        /// </summary>
        public async Task ReadFooterAsync()
        {
            // Make sure the ending RESOURCE tag is read and the reader
            // is positioned at the next tag

            // The TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.

            while (!File.XmlReader.IsEndElement(Constants.TagData))
            {
                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        // no additional INFO tags
                        break;
                    case VoTableVersion.V1_2:
                    case VoTableVersion.V1_3:
                        table.Data.InfoList.Add(File.ReadElement<IInfo>());
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (!File.XmlReader.IsEndElement(Constants.TagTable))
            {
                switch (File.Version)
                {
                    case VoTableVersion.V1_1:
                        // no additional INFO tags
                        break;
                    case VoTableVersion.V1_2:
                    case VoTableVersion.V1_3:
                        table.InfoList2.Add(File.ReadElement<IInfo>());
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (!File.XmlReader.IsEndElement(Constants.TagResource))
            {
                switch (File.XmlReader.Name)
                {
                    case Constants.TagInfo:
                        switch (File.Version)
                        {
                            case VoTableVersion.V1_1:
                                break;
                            case VoTableVersion.V1_2:
                            case VoTableVersion.V1_3:
                                resource.InfoList2.Add(File.ReadElement<IInfo>());
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        await File.XmlReader.MoveAfterEndAsync(file.XmlReader.Name);
                        break;
                }
            }

            File.XmlReader.ReadEndElement();
        }

        #endregion
        #region Row reader functions

        private void ParseMagicNulls()
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].DataType.NullValue != null)
                {
                    columns[i].DataType.NullValue = textColumnReaders[i](columns[i], (string)columns[i].DataType.NullValue);
                }
            }
        }

        public Task<bool> ReadNextRowAsync(object[] values, int startIndex)
        {
            // TODO: take ID from attribute of TR tag?
            // TODO: use encoding from TD tag?
            // According to the docs, these are just dummy attributes

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
            if (tableEndReached)
            {
                return false;
            }
            else if (VoTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTR) != 0)
            {
                await File.XmlReader.MoveAfterEndAsync(Constants.TagTableData);
                tableEndReached = true;
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

                            // TODO: add array support here. If arraysize is set and not a string,
                            // split text and use parser on parts.

                            // TODO: implement type mapping

                            var text = await File.XmlReader.ReadContentAsStringAsync();
                            values[startIndex + q] = textColumnReaders[q](columns[q], text);

                            // Magic nulls
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
                            // Standard null
                            values[startIndex + q] = null;
                            await File.XmlReader.ReadAsync();
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
            if (tableEndReached)
            {
                return false;
            }
            else
            {
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

                                values[startIndex + i] = binaryColumnReaders[i](column, binaryBuffer, l, File.BitConverter);
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
                    tableEndReached = true;
                    return false;
                }
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

                if (type == typeof(SharpFitsIO.Bit))
                {
                    textColumnReaders[i] = delegate (VoTableColumn column, string text)
                    {
                        if (text == "1" || text == "t" || text == "T")
                        {
                            return new SharpFitsIO.Bit(true);
                        }
                        else if (text == "0" || text == "f" || text == "F")
                        {
                            return new SharpFitsIO.Bit(false);
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
                else if (type == typeof(Boolean))
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
            CreateTextColumnWriters();
            WriteMagicNulls();

            if (serialization == VoTableSerialization.Binary ||
                serialization == VoTableSerialization.Binary2)
            {
                CreateBinaryColumnWriters();
            }

            await WriteResourceElementAsync();
            await WriteTableElementAsync();
            await WriteDataElementAsync();
        }

        private async Task WriteResourceElementAsync()
        {
            await File.XmlWriter.WriteStartElementAsync("", Constants.TagResource, File.Namespace);

            await File.WriteAttributeAsync(Constants.AttributeName, resource.Name);
            await File.WriteAttributeAsync(Constants.AttributeID, resource.ID);
            await File.WriteAttributeAsync(Constants.AttributeUType, resource.Utype);
            await File.WriteAttributeAsync(Constants.AttributeType, resource.Type);
            await File.WriteAttributesAsync(resource.Attributes);

            File.WriteElement(resource.Description, Constants.TagDescription, File.Namespace);
            File.WriteElements(resource.InfoList1);
            File.WriteElements(resource.CoosysList);
            File.WriteElements(resource.GroupList);
            File.WriteElements(resource.ParamList);
            File.WriteElements(resource.LinkList);
        }

        private async Task WriteTableElementAsync()
        {
            await File.XmlWriter.WriteStartElementAsync("", Constants.TagTable, File.Namespace);

            await File.WriteAttributeAsync(Constants.AttributeID, table.ID);
            await File.WriteAttributeAsync(Constants.AttributeName, table.Name);

            File.WriteElement(table.Description, Constants.TagDescription, File.Namespace);
            File.WriteElements(table.InfoList1);
            File.WriteElements(table.ParamList);
            File.WriteElements(table.GroupList);
            File.WriteElements(table.FieldList);
            File.WriteElements(table.LinkList);
        }

        private async Task WriteDataElementAsync()
        {
            await File.XmlWriter.WriteStartElementAsync("", Constants.TagData, File.Namespace);

            switch (serialization)
            {
                case VoTableSerialization.TableData:
                    await File.XmlWriter.WriteStartElementAsync("", Constants.TagTableData, File.Namespace);
                    break;
                case VoTableSerialization.Binary:
                    await File.XmlWriter.WriteStartElementAsync("", Constants.TagBinary, File.Namespace);
                    break;
                case VoTableSerialization.Binary2:
                    await File.XmlWriter.WriteStartElementAsync("", Constants.TagBinary2, File.Namespace);
                    break;
                case VoTableSerialization.Fits:
                default:
                    throw new NotImplementedException();
            }

            switch (serialization)
            {
                case VoTableSerialization.TableData:
                    break;
                case VoTableSerialization.Binary:
                case VoTableSerialization.Binary2:
                case VoTableSerialization.Fits:
                    await WriteStreamElementAsync();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task WriteStreamElementAsync()
        {
            await File.XmlWriter.WriteStartElementAsync("", Constants.TagStream, File.Namespace);
            await File.WriteAttributeAsync(Constants.AttributeEncoding, VoTableEncoding.Base64.ToString().ToLowerInvariant());

            // TODO: open stream here
        }
        
        public async Task WriteNextRowAsync(params object[] values)
        {
            switch (serialization)
            {
                case VoTableSerialization.TableData:
                    await WriteNextRowToTableAsync(values);
                    break;
                case VoTableSerialization.Binary:
                case VoTableSerialization.Binary2:
                    await WriteNextRowToStreamAsync(values);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task WriteFooterAsync()
        {
            // Stream
            switch (serialization)
            {
                case VoTableSerialization.TableData:
                    break;
                case VoTableSerialization.Binary:
                case VoTableSerialization.Binary2:
                case VoTableSerialization.Fits:
                    await File.XmlWriter.WriteEndElementAsync();
                    break;
                default:
                    throw new NotImplementedException();
            }

            // DataTable/Binary/Binary2/Fits
            await File.XmlWriter.WriteEndElementAsync();

            // Data
            File.WriteElements(table.Data.InfoList);
            await File.XmlWriter.WriteEndElementAsync();

            // Table
            File.WriteElements(table.InfoList2);
            await File.XmlWriter.WriteEndElementAsync();

            // Resource
            File.WriteElements(resource.InfoList2);
            await File.XmlWriter.WriteEndElementAsync();
        }
        


        public async Task WriteFromDataReaderAsync(DbDataReader dr)
        {
            // Create columns now if they haven't been created
            if (columns.Count == 0)
            {
                CreateColumns(dr);
            }

            await WriteHeaderAsync();

            var values = new object[dr.FieldCount];
            while (await dr.ReadAsync())
            {
                dr.GetValues(values);
                await WriteNextRowAsync(values);
            }

            await WriteFooterAsync();
        }

        #endregion
        #region Row writer functions

        private void WriteMagicNulls()
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].DataType.NullValue != null)
                {
                    columns[i].DataType.NullValue = textColumnWriters[i](columns[i], columns[i].DataType.NullValue);
                }
            }
        }

        private async Task WriteNextRowToTableAsync(params object[] values)
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTR, File.Namespace);

            for (int i = 0; i < columns.Count; i++)
            {
                await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTD, File.Namespace);

                // TODO: Do not use format here, or use standard votable formatting
                if (values[i] == null || values[i] == DBNull.Value)
                {
                    if (columns[i].DataType.NullValue != null)
                    {
                        await File.XmlWriter.WriteStringAsync((string)columns[i].DataType.NullValue);
                    }
                }
                else
                {
                    var text = textColumnWriters[i](Columns[i], values[i]);
                    await File.XmlWriter.WriteStringAsync(text);
                }

                // </TD>
                await File.XmlWriter.WriteEndElementAsync();
            }

            // </TR>
            await File.XmlWriter.WriteEndElementAsync();
        }

        private async Task WriteNextRowToStreamAsync(params object[] values)
        {
            // TODO
            throw new NotImplementedException();
        }

        private void CreateTextColumnWriters()
        {
            textColumnWriters = new TextColumnWriter[Columns.Count];

            for (int i = 0; i < textColumnWriters.Length; i++)
            {
                var datatype = Columns[i].DataType;
                var type = datatype.Type;

                // TODO: how to deal with bit arrays?
                // TODO: how to deal with arrays in general?

                if (type == typeof(SharpFitsIO.Bit))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        var v = (SharpFitsIO.Bit)value;
                        return v.Value ? "1" : "0";
                    };
                }
                else if (type == typeof(Boolean))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        var v = (Boolean)value;
                        return v ? "T" : "F";
                    };
                }
                else if (type == typeof(Byte) ||
                         type == typeof(Int16) ||
                         type == typeof(Int32) ||
                         type == typeof(Int64) ||
                         type == typeof(Single) ||
                         type == typeof(Double))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        return String.Format(invariantCulture, column.Format, value);
                    };
                }
                else if (type == typeof(SharpFitsIO.SingleComplex))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        var v = (SharpFitsIO.SingleComplex)value;
                        return v.ToString(columns[i].Format, invariantCulture);
                    };
                }
                else if (type == typeof(SharpFitsIO.DoubleComplex))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        var v = (SharpFitsIO.DoubleComplex)value;
                        return v.ToString(columns[i].Format, invariantCulture);
                    };
                }
                else if (type == typeof(string))
                {
                    textColumnWriters[i] = delegate (VoTableColumn column, object value)
                    {
                        // TODO: what if text is encoded?
                        // what if fixed length?
                        return (string)value;
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void CreateBinaryColumnWriters()
        {
            binaryColumnWriters = new BinaryColumnWriter[Columns.Count];

            /*
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
            */
        }

        #endregion
    }
}