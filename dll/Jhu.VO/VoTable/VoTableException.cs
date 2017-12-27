using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    [Serializable]
    public class VoTableException : Exception
    {
        public VoTableException()
        {
        }

        public VoTableException(string message)
            : base(message)
        {
        }

        public VoTableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
