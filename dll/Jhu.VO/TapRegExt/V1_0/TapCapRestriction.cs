using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoResource.V1_0;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlRoot(VoResource.Constants.TagCapability, Namespace = Constants.TapRegExtNamespace)]
    public class TapCapRestriction : Capability
    {
        
    }
}
