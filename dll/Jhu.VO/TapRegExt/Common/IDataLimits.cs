using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt.Common
{
    public interface IDataLimits
    {
        IDataLimit Default { get; set; }
        IDataLimit Hard { get; set; }
    }
}
