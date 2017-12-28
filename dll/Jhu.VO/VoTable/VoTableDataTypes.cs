using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    public static class VoTableDataTypes
    {
        public static VoTableDataType Boolean
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeBoolean,
                    Type = typeof(bool),
                    ByteSize = sizeof(Byte)
                };
            }
        }

        public static VoTableDataType Bit
        {
            get
            {
                // TODO
                return new VoTableDataType()
                {
                    Name = Constants.TypeBit,
                    Type = typeof(SharpFitsIO.Bit),
                    ByteSize = -1
                };
            }
        }

        public static VoTableDataType UnsignedByte
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeUnsignedByte,
                    Type = typeof(Byte),
                    ByteSize = sizeof(Byte)
                };
            }
        }

        public static VoTableDataType Short
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeShort,
                    Type = typeof(Int16),
                    ByteSize = sizeof(Int16)
                };
            }
        }

        public static VoTableDataType Int
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeInt,
                    Type = typeof(Int32),
                    ByteSize = sizeof(Int32)
                };
            }
        }

        public static VoTableDataType Long
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeLong,
                    Type = typeof(Int64),
                    ByteSize = sizeof(Int64)
                };
            }
        }

        public static VoTableDataType Char
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeChar,
                    Type = typeof(string),
                    ByteSize = sizeof(Byte)
                };
            }
        }

        public static VoTableDataType UnicodeChar
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeUnicodeChar,
                    Type = typeof(string),
                    ByteSize = 2 * sizeof(Byte)
                };
            }
        }

        public static VoTableDataType Float
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeFloat,
                    Type = typeof(Single),
                    ByteSize = sizeof(Single)
                };
            }
        }

        public static VoTableDataType Double
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeDouble,
                    Type = typeof(Double),
                    ByteSize = sizeof(Double)
                };
            }
        }

        public static VoTableDataType FloatComplex
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeFloatComplex,
                    Type = typeof(SharpFitsIO.SingleComplex),
                    ByteSize = 2 * sizeof(Single)
                };
            }
        }

        public static VoTableDataType DoubleComplex
        {
            get
            {
                return new VoTableDataType()
                {
                    Name = Constants.TypeDoubleComplex,
                    Type = typeof(SharpFitsIO.DoubleComplex),
                    ByteSize = 2 * sizeof(Double)
                };
            }
        }
    }
}
