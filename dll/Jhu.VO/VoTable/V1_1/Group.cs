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
    [XmlRoot(Constants.TagGroup, Namespace = Constants.NamespaceVoTableV1_1)]
    public class Group : IGroup
    {
        [XmlElement(Constants.TagDescription)]
        public AnyText Description { get; set; }

        [XmlIgnore]
        IAnyText IGroup.Description
        {
            get { return Description; }
            set { Description = (AnyText)value; }
        }

        #region PARAM GROUP FIELDref PARAMref

        [XmlElement(Constants.TagParam, typeof(Param))]
        [XmlElement(Constants.TagGroup, typeof(Group))]
        [XmlElement(Constants.TagFieldRef, typeof(FieldRef))]
        [XmlElement(Constants.TagParamRef, typeof(ParamRef))]
        public List<object> ItemList_ForXml { get; set; }

        [XmlIgnore]
        public ElementList<IFieldRef> FieldRefList
        {
            get { return new ElementList<IFieldRef>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IParamRef> ParamRefList
        {
            get { return new ElementList<IParamRef>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IParam> ParamList
        {
            get { return new ElementList<IParam>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IGroup> GroupList
        {
            get { return new ElementList<IGroup>(ItemList_ForXml); }
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
