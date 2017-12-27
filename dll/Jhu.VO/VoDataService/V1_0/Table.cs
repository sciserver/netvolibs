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
    public class Table
    {
        [XmlElement(Constants.TagName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagColumn, Form = XmlSchemaForm.Unqualified)]
        public List<TableParam> ColumnList { get; set; }

        [XmlAttribute(VoResource.Constants.AttributeRole, Form = XmlSchemaForm.Unqualified)]
        public string Role { get; set; }
    }
}
