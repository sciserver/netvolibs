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
    [XmlInclude(typeof(CatalogService))]
    public class DataService : VoResource.V1_0.Service
    {
        [XmlElement(VoResource.Constants.TagFacility, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] FacilityList { get; set; }

        [XmlElement(VoResource.Constants.TagInstrument, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] InstrumentList { get; set; }

        [XmlElement(Constants.TagCoverage, Form = XmlSchemaForm.Unqualified)]
        public Coverage Coverage { get; set; }
    }
}
