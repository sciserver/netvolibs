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
    public class Coverage
    {
        // TODO: this is an element reference, how does this translated to c#?
        [XmlElement(Stc.Constants.TagStcResourceProfile, Form = XmlSchemaForm.Unqualified)]
        public Stc.V1_30.astroSTCDescriptionType StcResourceProfile { get; set; }

        [XmlElement(Constants.TagFootprint, Form = XmlSchemaForm.Unqualified)]
        public ServiceReference Footprint { get; set; }

        [XmlElement(Constants.TagWaveband, Form = XmlSchemaForm.Unqualified)]
        public List<string> WavebandList { get; set; }
    }
}
