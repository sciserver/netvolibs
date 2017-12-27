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
    public class Values : IValues
    {
        [XmlElement(Constants.TagMin, Form = XmlSchemaForm.Unqualified)]
        public Min Min { get; set; }

        [XmlElement(Constants.TagMax, Form = XmlSchemaForm.Unqualified)]
        public Max Max { get; set; }

        [XmlElement(Constants.TagOption, Form = XmlSchemaForm.Unqualified)]
        public Option[] OptionList { get; set; }

        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        [XmlAttribute(Constants.AttributeNull, Form = XmlSchemaForm.Unqualified)]
        public string Null { get; set; }

        [XmlAttribute(Constants.AttributeRef, Form = XmlSchemaForm.Unqualified)]
        public string Ref { get; set; }
    }
}
