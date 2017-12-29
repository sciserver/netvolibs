using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    public abstract class VoTableDataTypeMapping
    {
        public abstract Type From { get; }

        public abstract VoTableDataType MapType(int[] size, bool isVariableSize, bool isUnboundSize);
        public abstract object MapValue(object value);
    }
}
