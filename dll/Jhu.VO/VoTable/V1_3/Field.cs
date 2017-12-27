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
    [XmlInclude(typeof(Param))]
    public class Field : IField
    {
        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public AnyText Description { get; set; }

        string IField.Description
        {
            get { return Description.Text; }
            set { Description.Text = value; }
        }

        [XmlElement(Constants.TagValues, Form = XmlSchemaForm.Unqualified)]
        public Values Values { get; set; }

        IValues IField.Values
        {
            get { return Values; }
            set { Values = (V1_3.Values)value; }
        }

        [XmlElement(Constants.TagLink, Form = XmlSchemaForm.Unqualified)]
        public Link[] LinkList { get; set; }

        [XmlAttribute(Constants.AttributeID, Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeUnit, Form = XmlSchemaForm.Unqualified)]
        public string Unit { get; set; }

        [XmlAttribute(Constants.AttributeDatatype, Form = XmlSchemaForm.Unqualified)]
        public string Datatype { get; set; }

        [XmlAttribute(Constants.AttributePrecision, Form = XmlSchemaForm.Unqualified)]
        public string Precision { get; set; }

        [XmlAttribute(Constants.AttributeWidth, Form = XmlSchemaForm.Unqualified)]
        public string Width { get; set; }

        [XmlAttribute(Constants.AttributeXType, Form = XmlSchemaForm.Unqualified)]
        public string Xtype { get; set; }

        [XmlAttribute(Constants.AttributeRef, Form = XmlSchemaForm.Unqualified)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeName, Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeUcd, Form = XmlSchemaForm.Unqualified)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType, Form = XmlSchemaForm.Unqualified)]
        public string UType { get; set; }

        [XmlAttribute(Constants.AttributeArraySize, Form = XmlSchemaForm.Unqualified)]
        public string Arraysize { get; set; }

        [XmlAttribute(Constants.AttributeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
    }
}
