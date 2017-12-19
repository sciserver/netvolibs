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
    public class TableService : VoResource.V1_0.Service
    {
        [XmlElement(Constants.TagFacility, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] FacilityList { get; set; }

        [XmlElement(Constants.TagInstrument, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] InstrumentList { get; set; }

        [XmlElement(Constants.TagTable, Form = XmlSchemaForm.Unqualified)]
        public Table[] TableList { get; set; }

    }
}
