using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Jhu.VO.VoTable
{
    public class VoTableDataType : ICloneable
    {
        #region Private member variables

        [NonSerialized]
        private string name;

        [NonSerialized]
        private Type type;

        [NonSerialized]
        private int[] size;

        [NonSerialized]
        private bool isArray;

        [NonSerialized]
        private bool isVariableSize;

        [NonSerialized]
        private bool isUnboundSize;

        [NonSerialized]
        private int byteSize;

        private bool isNullable;
        private object nullValue;
        private string xType;

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets the name of the data type.
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        /// <summary>
        /// Gets the corresponding .Net type
        /// </summary>
        [IgnoreDataMember]
        public Type Type
        {
            get { return type; }
            internal set { type = value; }
        }

        [DataMember]
        public int[] Size
        {
            get { return size; }
            set { size = value; }
        }

        [DataMember]
        public bool IsArray
        {
            get { return isArray; }
            set { isArray = value; }
        }

        [DataMember]
        public bool IsVariableSize
        {
            get { return isVariableSize; }
            set { isVariableSize = value; }
        }

        [DataMember]
        public bool IsUnboundSize
        {
            get { return isUnboundSize; }
            set { isUnboundSize = value; }
        }

        [IgnoreDataMember]
        public bool IsUnicode
        {
            get { return type == typeof(string) && byteSize == 2; }
        }

        [IgnoreDataMember]
        public bool IsFixedLength
        {
            get
            {
                return !isVariableSize && !isUnboundSize;
            }
        }

        [IgnoreDataMember]
        public bool HasLength
        {
            get
            {
                return size != null && size.Length > 0;
            }
        }

        [IgnoreDataMember]
        public int Length
        {
            get
            {
                // TODO: review this when implementing arrays
                // It handles varchar and varbinary now.

                if (isUnboundSize || size == null || size.Length == 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    int l = 1;

                    for (int i = 0; i < size.Length; i++)
                    {
                        l *= size[i];
                    }

                    return l;
                }
            }
        }

        [DataMember]
        public int ByteSize
        {
            get { return byteSize; }
            internal set { byteSize = value; }
        }

        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        public object NullValue
        {
            get { return nullValue; }
            set { nullValue = value; }
        }

        public string XType
        {
            get { return xType; }
            set { xType = value; }
        }

        #endregion
        #region Constructors and initializers

        internal VoTableDataType()
        {
            InitializeMembers();
        }

        internal VoTableDataType(VoTableDataType old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.name = null;
            this.type = null;
            this.size = null;
            this.isArray = false;
            this.isVariableSize = false;
            this.isUnboundSize = false;
            this.isNullable = false;
            this.nullValue = null;
            this.xType = null;
        }

        private void CopyMembers(VoTableDataType old)
        {
            this.name = old.name;
            this.type = old.type;
            this.size = old.size;
            this.isArray = old.isArray;
            this.isVariableSize = old.isVariableSize;
            this.isUnboundSize = old.isUnboundSize;
            this.isNullable = old.isNullable;
            this.nullValue = old.nullValue;
            this.xType = old.xType;
        }

        public object Clone()
        {
            return new VoTableDataType(this);
        }

        #endregion
        #region Static factory functions

        public static VoTableDataType Create(Type type)
        {
            return Create(type, null, false, false);
        }

        public static VoTableDataType Create(Type type, int[] size, bool isVariableSize, bool isUnboundSize)
        {
            VoTableDataType dt;

            if (type == typeof(Boolean))
            {
                dt = VoTableDataTypes.Boolean;
            }
            else if (type == typeof(Byte))
            {
                dt = VoTableDataTypes.UnsignedByte;
            }
            else if (type == typeof(Int16))
            {
                dt = VoTableDataTypes.Short;
            }
            else if (type == typeof(Int32))
            {
                dt = VoTableDataTypes.Int;
            }
            else if (type == typeof(Int64))
            {
                dt = VoTableDataTypes.Long;
            }
            else if (type == typeof(Char) || type == typeof(String))
            {
                dt = VoTableDataTypes.UnicodeChar;
            }
            else if (type == typeof(Single))
            {
                dt = VoTableDataTypes.Float;
            }
            else if (type == typeof(Double))
            {
                dt = VoTableDataTypes.Double;
            }
            else if (type == typeof(SharpFitsIO.SingleComplex))
            {
                dt = VoTableDataTypes.FloatComplex;
            }
            else if (type == typeof(SharpFitsIO.DoubleComplex))
            {
                dt = VoTableDataTypes.DoubleComplex;
            }
            else
            {
                throw  Error.UnsupportedDataType();
            }

            dt.size = size;
            dt.isArray = size != null || isVariableSize || isUnboundSize;
            dt.isVariableSize = isVariableSize;
            dt.isUnboundSize = isUnboundSize;

            return dt;
        }

        internal static VoTableDataType Create(IField field)
        {
            var dt = new VoTableDataType();

            switch (field.Datatype.ToLowerInvariant())
            {
                case Constants.TypeBoolean:
                    dt = VoTableDataTypes.Boolean;
                    break;
                case Constants.TypeBit:
                    dt = VoTableDataTypes.Bit;
                    break;
                case Constants.TypeUnsignedByte:
                    dt = VoTableDataTypes.UnsignedByte;
                    break;
                case Constants.TypeShort:
                    dt = VoTableDataTypes.Short;
                    break;
                case Constants.TypeInt:
                    dt = VoTableDataTypes.Int;
                    break;
                case Constants.TypeLong:
                    dt = VoTableDataTypes.Long;
                    break;
                case Constants.TypeChar:
                    dt = VoTableDataTypes.Char;
                    break;
                case Constants.TypeUnicodeChar:
                    dt = VoTableDataTypes.UnicodeChar;
                    break;
                case Constants.TypeFloat:
                    dt = VoTableDataTypes.Float;
                    break;
                case Constants.TypeDouble:
                    dt = VoTableDataTypes.Double;
                    break;
                case Constants.TypeFloatComplex:
                    dt = VoTableDataTypes.FloatComplex;
                    break;
                case Constants.TypeDoubleComplex:
                    dt = VoTableDataTypes.DoubleComplex;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (field is V1_2.Field)
            {
                dt.XType = ((V1_2.Field)field).Xtype;
            }

            var nullvalue = field.Values?.Null;
            if (nullvalue != null)
            {
                dt.isNullable = true;
                dt.nullValue = nullvalue;
            }

            GetArraySize(field.Arraysize, out dt.size, out dt.isArray, out dt.isVariableSize, out dt.isUnboundSize);

            return dt;
        }

        private static void GetArraySize(string arraysize, out int[] size, out bool isArray, out bool isVariableSize, out bool isUnboundSize)
        {
            // example: <FIELD ID= "values" datatype="int" arraysize="100*"/>
            // example: <FIELD ID= "values" datatype="int" arraysize="100,*"/>
            // example: <FIELD ID= "values" datatype="int" arraysize="100,10*"/>

            if (String.IsNullOrEmpty(arraysize))
            {
                size = new int[0];
                isArray = false;
                isVariableSize = false;
                isUnboundSize = false;
            }
            else
            {
                isArray = true;

                var parts = arraysize.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var lastpart = parts[parts.Length - 1];

                if (lastpart == "*")
                {
                    // "10,20,*"
                    isVariableSize = false;
                    isUnboundSize = true;
                    size = new int[parts.Length - 1];
                }
                else if (lastpart.EndsWith("*"))
                {
                    // "10,20*"
                    isVariableSize = true;
                    isUnboundSize = false;
                    size = new int[parts.Length];
                }
                else
                {
                    // "10,20"
                    isVariableSize = false;
                    isUnboundSize = false;
                    size = new int[parts.Length];
                }

                for (int i = 0; i < size.Length; i++)
                {
                    size[i] = Int32.Parse(parts[i].TrimEnd('*'));
                }
            }
        }

        #endregion
    }
}
