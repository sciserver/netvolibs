using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    public static class Error
    {
        public static VoTableException RecursiveResourceNotSupported()
        {
            return new VoTableException(ExceptionMessage.RecursiveResourceNotSupported);
        }

        public static VoTableException UnsupportedSerialization(VoTableSerialization serialization)
        {
            return new VoTableException(String.Format(ExceptionMessage.UnsupportedSerialization, serialization));
        }

        public static VoTableException ReferencedStreamsNotSupported()
        {
            return new VoTableException(ExceptionMessage.ReferencedStreamsNotSupported);
        }

        public static VoTableException EncodingNotFound()
        {
            return new VoTableException(ExceptionMessage.EncodingNotFound);
        }

        public static VoTableException EncodingNotSupported(string encoding)
        {
            return new VoTableException(String.Format(ExceptionMessage.EncodingNotSupported, encoding));
        }

        public static VoTableException MultidimensionalStringNotSupported()
        {
            return new VoTableException(ExceptionMessage.MultidimensionalStringNotSupported);
        }

        public static VoTableException PrimitiveArraysNotSupported()
        {
            return new VoTableException(ExceptionMessage.PrimitiveArraysNotSupported);
        }

        public static VoTableException BitNotSupported()
        {
            return new VoTableException(ExceptionMessage.BitNotSupported);
        }

        public static VoTableException UnsupportedVersion(string version)
        {
            return new VoTableException(String.Format(ExceptionMessage.UnsupportedVersion, version));
        }

        public static VoTableException TableNotFound()
        {
            return new VoTableException(ExceptionMessage.TableNotFound);
        }

        public static VoTableException DataNotFound()
        {
            return new VoTableException(ExceptionMessage.DataNotFound);
        }

        public static VoTableException LinksNotSupported()
        {
            return new VoTableException(ExceptionMessage.LinksNotSupported);
        }

        public static VoTableException InvalidFormat()
        {
            return new VoTableException(ExceptionMessage.InvalidFormat);
        }

        public static VoTableException UnsupportedDataType()
        {
            return new VoTableException(ExceptionMessage.UnsupportedDataType);
        }
    }
}
