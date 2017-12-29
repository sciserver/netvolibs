using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable.Common
{
    public interface IInfo
    {
        string Text { get; set; }
        string ID { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
