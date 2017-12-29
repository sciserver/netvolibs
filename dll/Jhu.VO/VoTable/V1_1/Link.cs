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
    [XmlRoot(Constants.TagLink, Namespace = Constants.NamespaceVoTableV1_1)]
    public class Link : ILink
    {
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeContentRole)]
        public string ContentRole { get; set; }

        [XmlAttribute(Constants.AttributeContentType)]
        public string ContentType { get; set; }

        [XmlAttribute(Constants.AttributeTitle)]
        public string Title { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeHref)]
        public string Href { get; set; }

        [XmlAttribute(Constants.AttributeGref)]
        public string Gref { get; set; }

        [XmlAttribute(Constants.AttributeAction)]
        public string Action { get; set; }
    }
}
