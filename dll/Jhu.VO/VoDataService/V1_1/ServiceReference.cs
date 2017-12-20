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
    public class ServiceReference
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(VoResource.Constants.AttributeIvoID, Form = XmlSchemaForm.Unqualified)]
        public string IvoID { get; set; }
    }
}
