using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoDataService.Common
{
    public interface IParamHttp : Jhu.VO.VoResource.Common.IInterface
    {
        string QueryType { get; set; }
        string ResultType { get; set; }
        ElementList<IInputParam> ParamList { get; }
    }
}
