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
    public class Table
    {
        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagInfo, Form = XmlSchemaForm.Unqualified)]
        public Info[] InfoList { get; set; }

        [XmlElement(Constants.TagField, Form = XmlSchemaForm.Unqualified)]
        public Field[] FieldList { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public Param[] ParamList { get; set; }

        [XmlElement(Constants.TagGroup, Form = XmlSchemaForm.Unqualified)]
        public Group[] GroupList { get; set; }

        [XmlElement(Constants.TagLink, Form = XmlSchemaForm.Unqualified)]
        public Link[] LinkList { get; set; }

        [XmlElement(Constants.TagData, Form = XmlSchemaForm.Unqualified)]
        public Data Data { get; set; }

        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeRef, Form = XmlSchemaForm.Unqualified)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd, Form = XmlSchemaForm.Unqualified)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType, Form = XmlSchemaForm.Unqualified)]
        public string UType { get; set; }

        [XmlAttribute(Constants.AttributeNRows, Form = XmlSchemaForm.Unqualified)]
        public int NRows { get; set; }
    }
}
