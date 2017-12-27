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
    [XmlType(Namespace = Constants.NamespaceVoTableV1_3)]
    [XmlRoot(Constants.TagGroup, Namespace = Constants.NamespaceVoTableV1_3)]
    public class Group
    {
        [XmlElement(Constants.TagDescription)]
        public AnyText Description { get; set; }

        #region PARAM GROUP FIELDref PARAMref

        [XmlElement(Constants.TagParam, typeof(Param))]
        [XmlElement(Constants.TagGroup, typeof(Group))]
        [XmlElement(Constants.TagFieldRef, typeof(FieldRef))]
        [XmlElement(Constants.TagParamRef, typeof(ParamRef))]
        public List<object> ItemList_ForXml { get; set; }

        [XmlIgnore]
        public ItemList<FieldRef> FieldRefList
        {
            get { return new ItemList<FieldRef>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<ParamRef> ParamRefList
        {
            get { return new ItemList<ParamRef>(ItemList_ForXml); }
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
    }
}
