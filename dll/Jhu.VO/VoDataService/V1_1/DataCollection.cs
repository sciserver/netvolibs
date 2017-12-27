using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_1
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_1)]
    public class DataCollection
    {
        [XmlElement(VoResource.Constants.TagFacility, Form = XmlSchemaForm.Unqualified)]
        public List<VoResource.V1_0.ResourceName> FacilityList { get; set; }

        [XmlElement(VoResource.Constants.TagInstrument, Form = XmlSchemaForm.Unqualified)]
        public List<VoResource.V1_0.ResourceName> InstrumentList { get; set; }

        [XmlElement(VoResource.Constants.TagRights, Form = XmlSchemaForm.Unqualified)]
        public List<string> RightsList { get; set; }

        [XmlElement(Constants.TagFormat, Form = XmlSchemaForm.Unqualified)]
        public List<Format> FormatList { get; set; }

        [XmlElement(Constants.TagCoverage, Form = XmlSchemaForm.Unqualified)]
        public Coverage Coverage { get; set; }

        [XmlElement(Constants.TagTableSet, Form = XmlSchemaForm.Unqualified)]
        public TableSet TableSet { get; set; }

        [XmlElement(VoResource.Constants.TagAccessUrl, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.AccessUrl AccessUrl { get; set; }
    }
}
