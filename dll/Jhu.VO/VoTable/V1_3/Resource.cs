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
    public class Resource
    {
        public AnyText Description { get; set; }
        public Info[] InfoList { get; set; }
        public CoordinateSystem[] CoosysList { get; set; }
        public Group[] GroupList { get; set; }
        public Param[] ParamList { get; set; }
        public Link[] LinkList { get; set; }
        public Table[] TableList { get; set; }
        public Resource[] ResourceList { get; set; }

        [XmlAnyElement]
        public XmlElement[] Elements { get; set; }

        public string Name { get; set; }
        public string ID { get; set; }
        public string Utype { get; set; }
        public string Type { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] Attributes { get; set; }
    }
}
