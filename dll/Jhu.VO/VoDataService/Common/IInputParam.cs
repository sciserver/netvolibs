using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoDataService.Common
{
    public interface IInputParam : IBaseParam
    {
        ISimpleDataType DataType { get; set; }
        string Use { get; set; }
        bool Std { get; set; }
    }
}
