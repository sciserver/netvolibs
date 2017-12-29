﻿using System;
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
    public class Table : ITable
    {
        [XmlElement(Constants.TagDescription, Order = 0)]
        public AnyText Description { get; set; }

        [XmlIgnore]
        IAnyText ITable.Description
        {
            get { return Description; }
            set { Description = (AnyText)value; }
        }

        [XmlElement(Constants.TagInfo, Order = 1)]
        public List<Info> InfoList1 { get; set; } = new List<Info>();

        [XmlIgnore]
        ElementList<IInfo> ITable.InfoList1
        {
            get { return new ElementList<IInfo>(InfoList1); }
        }

        #region FIELD PARAM GROUP

        [XmlElement(Constants.TagField, typeof(Field), Order = 2)]
        [XmlElement(Constants.TagParam, typeof(Param), Order = 2)]
        [XmlElement(Constants.TagGroup, typeof(Group), Order = 2)]
        public List<object> ItemList_ForXml { get; set; } = new List<object>();

        [XmlIgnore]
        public ElementList<IField> FieldList
        {
            get { return new ElementList<IField>(ItemList_ForXml); }
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

        [XmlElement(Constants.TagLink, Order = 3)]
        public List<Link> LinkList { get; set; } = new List<Link>();

        [XmlIgnore]
        ElementList<ILink> ITable.LinkList
        {
            get { return new ElementList<ILink>(LinkList); }
        }

        [XmlElement(Constants.TagData, Order = 4)]
        public Data Data { get; set; } = new Data();

        [XmlIgnore]
        IData ITable.Data
        {
            get { return Data; }
        }

        [XmlElement(Constants.TagInfo, Order = 5)]
        public List<Info> InfoList2 { get; set; } = new List<Info>();

        [XmlIgnore]
        ElementList<IInfo> ITable.InfoList2
        {
            get { return new ElementList<IInfo>(InfoList2); }
        }

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
