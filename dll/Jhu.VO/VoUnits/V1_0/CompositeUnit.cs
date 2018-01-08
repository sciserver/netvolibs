using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits.V1_0
{
    public class CompositeUnit
    {
        public double Factor { get; set; }

        public List<UnitElement> Elements { get; set; }
    }
}
