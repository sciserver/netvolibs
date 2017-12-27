using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class Resource
    {
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        #region COOSYS GROUP PARAM

        [XmlElement(Constants.TagInfo, typeof(Info), Order = 1)]
        [XmlElement(Constants.TagCoosys, typeof(Coosys), Order = 1)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 1)]
        public List<object> ItemList1_ForXml { get; set; }

        [XmlIgnore]
        public ItemList<Info> InfoList
        {
            get { return new ItemList<Info>(ItemList1_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Coosys> CoosysList
        {
            get { return new ItemList<Coosys>(ItemList1_ForXml); }
        }
        
        [XmlIgnore]
        public ItemList<Param> ParamList
        {
            get { return new ItemList<Param>(ItemList1_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagLink, Order = 2)]
        public List<Link> LinkList { get; set; }

        [XmlElement(Constants.TagTable, Order = 3)]
        public List<Table> TableList { get; set; }

        [XmlElement(Constants.TagResource, Order = 4)]
        public List<Table> ResourceList { get; set; }
        
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
