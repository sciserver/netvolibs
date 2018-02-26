using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.Common
{
    public interface IInterface
    {
        ElementList<IAccessUrl> AccessUrlList { get; }
        ElementList<ISecurityMethod> SecurityMethodList { get; }
        string Version { get; set; }
        string Role { get; set; }
    }
}
