using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
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
        
        #region COOSYS GROUP PARAM

        [XmlElement(Constants.TagInfo, typeof(Info), Order = 1)]
        [XmlElement(Constants.TagCoosys, typeof(Coosys), Order = 1)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 1)]
        public List<object> ItemList1_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ElementList<IInfo> InfoList1
        {
            get { return new ElementList<IInfo>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ElementList<ICoordinateSystem> CoosysList
        {
            get { return new ElementList<ICoordinateSystem>(ItemList1_ForXml); }
        }
        
        [XmlIgnore]
        public ElementList<IParam> ParamList
        {
            get { return new ElementList<IParam>(ItemList1_ForXml); }
        }

        #endregion

        [XmlIgnore]
        public ElementList<IGroup> GroupList
        {
            get { return null; }
        }

        [XmlElement(Constants.TagLink, Order = 2)]
        public List<Link> LinkList { get; set; } = new List<Link>();

        [XmlIgnore]
        ElementList<ILink> IResource.LinkList
        {
            get { return new ElementList<ILink>(LinkList); }
        }

        [XmlElement(Constants.TagTable, Order = 3)]
        public List<Table> TableList { get; set; } = new List<Table>();

        [XmlElement(Constants.TagResource, Order = 4)]
        public List<Table> ResourceList { get; set; }

        [XmlIgnore]
        public ElementList<IInfo> InfoList2
        {
            get { return null; }
        }

        [XmlAnyElement(Order = 5)]
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
