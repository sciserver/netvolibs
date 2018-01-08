using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits.V1_0
{
    public class Function
    {
        public string Symbol { get; set; }
        public bool Recognized { get; set; }
        public Func<Double, Double> Delegate { get; set; }
    }
}
