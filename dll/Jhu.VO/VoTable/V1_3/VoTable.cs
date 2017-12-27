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
    [XmlRoot(ElementName = Constants.TagVOTable, Namespace = Constants.VOTableNamespaceV1_3)]
    [XmlType(Namespace = Constants.VOTableNamespaceV1_3)]
    public class VoTable
    {
        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagDefinitions, Form = XmlSchemaForm.Unqualified)]
        public Definitions Definitions { get; set; }

        [XmlElement(Constants.TagCoosys, Form = XmlSchemaForm.Unqualified)]
        public CoordinateSystem[] CoosysList { get; set; }

        [XmlElement(Constants.TagGroup, Form = XmlSchemaForm.Unqualified)]
        public Group[] GroupList { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public Param[] ParamList { get; set; }

        [XmlElement(Constants.TagInfo, Form = XmlSchemaForm.Unqualified)]
        public Info[] InfoList { get; set; }

        [XmlElement(Constants.TagResource, Form = XmlSchemaForm.Unqualified)]
        public Resource[] ResourceList { get; set; }

        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeVersion, Form = XmlSchemaForm.Unqualified)]
        public string Version { get; set; }
    }
}
