using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_3
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_3)]
    public class Resource : IResource
    {
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlIgnore]
        IAnyText IResource.Description
        {
            get { return Description; }
            set { Description = (AnyText)value; }
        }

        [XmlElement(Constants.TagInfo, Order = 1)]
        public List<Info> InfoList1 { get; set; } = new List<Info>();

        [XmlIgnore]
        ElementList<IInfo> IResource.InfoList1
        {
            get { return new ElementList<IInfo>(InfoList1); }
        }

        #region COOSYS GROUP PARAM

        [XmlElement(Constants.TagCoosys, typeof(CoordinateSystem), Order = 2)]
        [XmlElement(Constants.TagGroup, typeof(Group), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        public List<object> ItemList1_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ElementList<ICoordinateSystem> CoosysList
        {
            get { return new ElementList<ICoordinateSystem>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IGroup> GroupList
        {
            get { return new ElementList<IGroup>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IParam> ParamList
        {
            get { return new ElementList<IParam>(ItemList1_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagLink, Order = 3)]
        public List<Link> LinkList { get; set; } = new List<Link>();

        [XmlIgnore]
        ElementList<ILink> IResource.LinkList
        {
            get { return new ElementList<ILink>(LinkList); }
        }

        #region TABLE RESOURCE

        [XmlElement(Constants.TagTable, typeof(Table), Order = 4)]
        [XmlElement(Constants.TagResource, typeof(Resource), Order = 4)]
        public List<object> ItemList2_ForXml { get; set; }

        [XmlIgnore]
        public ElementList<Table> TableList
        {
            get { return new ElementList<Table>(ItemList2_ForXml); }
        }

        [XmlIgnore]
        public ElementList<Resource> ResourceList
        {
            get { return new ElementList<Resource>(ItemList2_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagInfo, Order = 5)]
        public List<Info> InfoList2 { get; set; } = new List<Info>();

        [XmlIgnore]
        ElementList<IInfo> IResource.InfoList2
        {
            get { return new ElementList<IInfo>(InfoList2); }
        }

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
