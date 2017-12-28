using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_2)]
    public class Resource
    {
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagInfo, Order = 1)]
        public List<Info> InfoList1 { get; set; } = new List<Info>();

        #region COOSYS GROUP PARAM

        [XmlElement(Constants.TagCoosys, typeof(CoordinateSystem), Order = 2)]
        [XmlElement(Constants.TagGroup, typeof(Group), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        public List<object> ItemList1_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ItemList<CoordinateSystem> CoosysList
        {
            get { return new ItemList<CoordinateSystem>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Group> GroupList
        {
            get { return new ItemList<Group>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Param> ParamList
        {
            get { return new ItemList<Param>(ItemList1_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagLink, Order = 3)]
        public List<Link> LinkList { get; set; } = new List<Link>();

        #region TABLE RESOURCE

        [XmlElement(Constants.TagTable, typeof(Table), Order = 4)]
        [XmlElement(Constants.TagResource, typeof(Resource), Order = 4)]
        public List<object> ItemList2_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ItemList<Table> TableList
        {
            get { return new ItemList<Table>(ItemList2_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Resource> ResourceList
        {
            get { return new ItemList<Resource>(ItemList2_ForXml); }
        }

        [XmlElement(Constants.TagInfo, Order = 5)]
        public List<Info> InfoList2 { get; set; } = new List<Info>();

        #endregion

        [XmlAnyElement(Order = 6)]
        public List<XmlElement> Elements { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string Utype { get; set; }

        [XmlAttribute(Constants.AttributeType)]
        public string Type { get; set; }

        [XmlAnyAttribute]
        public List<XmlAttribute> Attributes { get; set; }
    }
}
