using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt.Common
{
    public interface IOutputFormat
    {
        string Mime { get; set; }
        List<string> Alias { get; }
        string IvoID { get; set; }
    }
}
