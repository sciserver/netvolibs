using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(TypeName = Constants.TypeTapCapRestriction, Namespace = Constants.TapRegExtNamespaceV1_0)]
    [XmlRoot(VoResource.Constants.TagCapability, Namespace = Constants.TapRegExtNamespaceV1_0)]
    public abstract class TapCapRestriction : VoResource.V1_0.Capability
    {
        
    }
}
