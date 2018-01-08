using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits.V1_0
{
    public class Unit : UnitElement
    {
        public Prefix Prefix { get; set; }
        public BaseUnit Base { get; set; }
        public int Exponent { get; set; }
    }
}
