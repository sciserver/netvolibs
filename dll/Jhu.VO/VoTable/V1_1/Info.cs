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
    [XmlRoot(Constants.TagInfo, Namespace = Constants.NamespaceVoTableV1_1)]
    public class Info : IInfo
    {
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
    }
}
