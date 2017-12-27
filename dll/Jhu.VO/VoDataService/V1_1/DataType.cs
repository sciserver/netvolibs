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
    [XmlInclude(typeof(SimpleDataType))]
    [XmlInclude(typeof(TableDataType))]
    [XmlInclude(typeof(TapDataType))]
    [XmlInclude(typeof(TapType))]
    [XmlInclude(typeof(VOTableType))]
    public class DataType
    {
        [XmlAttribute(Constants.AttributeArraySize, Form = XmlSchemaForm.Unqualified)]
        public string ArraySize { get; set; }

        [XmlAttribute(Constants.AttributeDelim, Form = XmlSchemaForm.Unqualified)]
        public string Delim { get; set; }

        [XmlAttribute(Constants.AttributeExtendedType, Form = XmlSchemaForm.Unqualified)]
        public string ExtendedType { get; set; }

        [XmlAttribute(Constants.AttributeExtendedSchema, Form = XmlSchemaForm.Unqualified)]
        public string ExtendedSchema { get; set; }

        [XmlAnyAttribute]
        public List<XmlAttribute> Attributes { get; set; }
    }
}
