using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.Vosi.Availability.Common
{
    public interface IAvailability
    {
        bool Available { get; set; }
        DateTime? UpSince { get; set; }
        DateTime? DownAt { get; set; }
        DateTime? BackAt { get; set; }
        string Note { get; set; }
    }
}
