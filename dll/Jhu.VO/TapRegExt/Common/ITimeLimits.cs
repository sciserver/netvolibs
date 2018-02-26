using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt.Common
{
    public interface ITimeLimits
    {
        int? Default { get; set; }
        int? Hard { get; set; }
    }
}
