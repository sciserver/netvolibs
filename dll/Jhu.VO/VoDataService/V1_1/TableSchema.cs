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
    public class TableSchema
    {
        [XmlElement(Constants.TagName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(Constants.TagTitle, Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagUtype, Form = XmlSchemaForm.Unqualified)]
        public string Utype { get; set; }

        [XmlElement(Constants.TagTable, Form = XmlSchemaForm.Unqualified)]
        public Table[] TableList { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] Attributes { get; set; }
    }
}
