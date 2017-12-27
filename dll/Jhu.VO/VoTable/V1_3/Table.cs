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
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlElement(Constants.TagInfo, Order = 1)]
        public List<Info> InfoList1 { get; set; }

        #region FIELD PARAM GROUP

        [XmlElement(Constants.TagField, typeof(Field), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        [XmlElement(Constants.TagGroup, typeof(Group), Order = 2)]
        public List<object> ItemList_ForXml { get; set; }

        [XmlIgnore]
        public ItemList<Field> FieldList
        {
            get { return new ItemList<Field>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Param> ParamList
        {
            get { return new ItemList<Param>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Group> GroupList
        {
            get { return new ItemList<Group>(ItemList_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagLink, Order = 3)]
        public List<Link> LinkList { get; set; }

        [XmlElement(Constants.TagData, Order = 4)]
        public Data Data { get; set; }

        [XmlElement(Constants.TagInfo, Order = 5)]
        public List<Info> InfoList2 { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { get; set; }

        [XmlAttribute(Constants.AttributeNRows)]
        public int NRows { get; set; }
    }
}
