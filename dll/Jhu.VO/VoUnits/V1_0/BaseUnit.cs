using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits.V1_0
{
    public class BaseUnit
    {
        public string Symbol { get; set; }
        public PrefixType SupportedPrefix { get; set; }
        public bool Recognized { get; set; }
        public bool Deprecated { get; set; }
    }
}
