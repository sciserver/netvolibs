using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable.Common
{
    public interface IParam : IField
    {
        string Value { get; set; }
    }
}
