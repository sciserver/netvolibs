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
    [XmlRoot(ElementName = Constants.TagVoTable, Namespace = Constants.NamespaceVoTableV1_1)]
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class VoTable : IVoTable
    {
        #region Constants

        private static readonly Dictionary<Type, Type> ifaceTypes = new Dictionary<Type, Type>()
        {
            { typeof(IAnyText), typeof(AnyText) },
            { typeof(ICoordinateSystem), typeof(Coosys) },
            { typeof(IData), typeof(Data) },
            { typeof(IDefinitions), typeof(Definitions) },
            { typeof(IField), typeof(Field) },
            { typeof(IFieldRef), typeof(FieldRef) },
            { typeof(IGroup), typeof(Group) },
            { typeof(IInfo), typeof(Info) },
            { typeof(ILink), typeof(Link) },
            { typeof(IParam), typeof(Param) },
            { typeof(IParamRef), typeof(ParamRef) },
            { typeof(IResource), typeof(Resource) },
            { typeof(ITable), typeof(Table) },
            { typeof(ITableData), typeof(TableData) },
            { typeof(IValues), typeof(Values) },
            { typeof(IVoTable), typeof(VoTable) },
        };

        #endregion
        #region Element properties

        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlIgnore]
        IAnyText IVoTable.Description
        {
            get { return Description; }
            set { Description = (AnyText)value; }
        }

        [XmlElement(Constants.TagDefinitions, Order = 1)]
        public Definitions Definitions { get; set; }

        [XmlIgnore]
        IDefinitions IVoTable.Definitions
        {
            get { return Definitions; }
            set { Definitions = (Definitions)value; }
        }

        #region COOSYS PARAM INFO

        [XmlElement(Constants.TagCoosys, typeof(Coosys), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        [XmlElement(Constants.TagInfo, typeof(Info), Order = 2)]
        public List<object> ItemList_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ElementList<ICoordinateSystem> CoosysList
        {
            get { return new ElementList<ICoordinateSystem>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IGroup> GroupList
        {
            get { return new ElementList<IGroup>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IParam> ParamList
        {
            get { return new ElementList<IParam>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ElementList<IInfo> InfoList1
        {
            get { return new ElementList<IInfo>(ItemList_ForXml); }
        }

        #endregion

        [XmlElement(Constants.TagResource, Order = 3)]
        public List<Resource> ResourceList { get; set; }

        [XmlIgnore]
        ElementList<IResource> IVoTable.ResourceList
        {
            get { return new ElementList<IResource>(ResourceList); }
        }
        
        [XmlIgnore]
        ElementList<IInfo> IVoTable.InfoList2 { get; } = null;

        #endregion
        #region Attribute properties

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeVersion)]
        public string Version { get; set; }

        #endregion
        #region Element generation functions

        Type IVoTable.GetType(Type iface)
        {
            return ifaceTypes[iface];
        }

        T IVoTable.CreateElement<T>()
        {
            var t = ifaceTypes[typeof(T)];
            return (T)Activator.CreateInstance(t);
        }
        
        #endregion

    }
}
