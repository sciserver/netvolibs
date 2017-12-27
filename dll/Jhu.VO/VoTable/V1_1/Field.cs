using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlRoot(ElementName = Constants.TagField, Namespace = Constants.VOTableNamespaceV1_1)]
    public class Field : IField
    {
        [XmlElement(Constants.TagDescription)]
        public AnyText Description { get; set; }

        string IField.Description
        {
            get { return Description.Text; }
        }

        [XmlElement(Constants.TagValues, Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Values Values { get; set; }

        IValues IField.Values
        {
            get { return Values; }
        }

        [XmlElement(Constants.TagLink)]
        public Link Link { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeUnit)]
        public string Unit { get; set; }

        [XmlAttribute(Constants.AttributeDatatype)]
        public string Datatype { get; set; }

        [XmlAttribute(Constants.AttributePrecision)]
        public string Precision { get; set; }

        [XmlAttribute(Constants.AttributeWidth)]
        public string Width { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { get; set; }

        [XmlAttribute(Constants.AttributeArraySize)]
        public string Arraysize { get; set; }

        [XmlAttribute(Constants.AttributeType)]
        public string Type { get; set; }
    }
}
