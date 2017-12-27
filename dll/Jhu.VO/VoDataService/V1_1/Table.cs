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
    [XmlInclude(typeof(Vosi.Tables.V1_0.Table))]
    public class Table
    {
        [XmlElement(Constants.TagName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(Constants.TagTitle, Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagUtype, Form = XmlSchemaForm.Unqualified)]
        public string UType { get; set; }

        [XmlElement(Constants.TagColumn, Form = XmlSchemaForm.Unqualified)]
        public List<TableParam> ColumnList { get; set; }

        [XmlElement(Constants.TagForeignKey, Form = XmlSchemaForm.Unqualified)]
        public List<ForeignKey> ForeignKeyList { get; set; }

        [XmlAttribute(Constants.AttributeIsMimeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        [XmlAnyAttribute]
        public List<XmlAttribute> Attributes { get; set; }
    }
}
