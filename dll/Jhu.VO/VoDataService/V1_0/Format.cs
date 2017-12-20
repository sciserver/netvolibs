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
    public class Format
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeIsMimeType, Form = XmlSchemaForm.Unqualified)]
        public bool IsMimeType { get; set; }
    }
}
