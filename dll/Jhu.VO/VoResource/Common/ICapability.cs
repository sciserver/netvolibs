using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.Common
{
    public interface ICapability
    {
        ElementList<IValidation> ValidationLevelList { get; }
        string Description { get; set; }
        ElementList<IInterface> InterfaceList { get; }
        string StandardID { get; set; }
    }
}
