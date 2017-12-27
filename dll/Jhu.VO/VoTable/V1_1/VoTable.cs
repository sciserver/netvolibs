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
    [XmlRoot(ElementName = Constants.TagVoTable, Namespace = Constants.NamespaceVoTableV1_1)]
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class VoTable
    {
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagDefinitions, Order = 1)]
        public Definitions Definitions { get; set; }

        #region COOSYS PARAM INFO

        [XmlElement(Constants.TagCoosys, typeof(Coosys), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        [XmlElement(Constants.TagInfo, typeof(Info), Order = 2)]
        public List<object> ItemList_ForXml { get; set; }

        [XmlIgnore]
        public ItemList<Coosys> CoosysList
        {
            get { return new ItemList<Coosys>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Group> GroupList
        {
            get { return new ItemList<Group>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Param> ParamList
        {
            get { return new ItemList<Param>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Info> InfoList1
        {
            get { return new ItemList<Info>(ItemList_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagResource, Order = 3)]
        public List<Resource> ResourceList { get; set; }

        [XmlElement(Constants.TagInfo, Order = 4)]
        public List<Info> InfoList2 { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeVersion)]
        public string Version { get; set; }
    }
}
