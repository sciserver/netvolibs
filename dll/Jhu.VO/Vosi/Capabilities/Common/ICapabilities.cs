using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.Vosi.Capabilities.Common
{
    public interface ICapabilities
    {
        ElementList<VoResource.Common.ICapability> CapabilityList { get; }
    }
}
