using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_3
{
    [XmlType(Namespace = Constants.VOTableNamespaceV1_3)]
    public class CoordinateSystem
    {
        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeEquinox, Form = XmlSchemaForm.Unqualified)]
        public string Equinox { get; set; }

        [XmlAttribute(Constants.AttributeEpoch, Form = XmlSchemaForm.Unqualified)]
        public string Epoch { get; set; }

        [XmlAttribute(Constants.AttributeSystem, Form = XmlSchemaForm.Unqualified)]
        public string System { get; set; }
    }
}
