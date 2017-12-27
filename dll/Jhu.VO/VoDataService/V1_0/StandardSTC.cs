using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_0
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class StandardStc : VoResource.V1_0.Resource
    {
        // TODO StcResourceProfile ???
        [XmlElement(Stc.Constants.TagStcResourceProfile, Form = XmlSchemaForm.Unqualified)]
        public List<Stc.V1_30.astroSTCDescriptionType> StcResourceProfileList { get; set; }
    }
}
