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
    public class Option
    {
        [XmlElement(Constants.TagOption, Form = XmlSchemaForm.Unqualified)]
        public Option[] OptionList { get; set; }

        [XmlAttribute(Constants.AttributeName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue, Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }
    }
}
