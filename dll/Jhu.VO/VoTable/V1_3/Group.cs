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
    public class Group
    {
        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagFieldRef, Form = XmlSchemaForm.Unqualified)]
        public FieldRef[] FieldRefList { get; set; }

        [XmlElement(Constants.TagParamRef, Form = XmlSchemaForm.Unqualified)]
        public ParamRef[] ParamRefList { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public Param[] ParamList { get; set; }

        [XmlElement(Constants.TagGroup, Form = XmlSchemaForm.Unqualified)]
        public Group[] GroupList { get; set; }

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
    }
}
