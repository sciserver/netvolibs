using System;
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
    [XmlRoot(Constants.TagMax, Namespace = Constants.NamespaceVoTableV1_3)]
    public class Max : IMax
    {
        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeInclusive)]
        public string Inclusive { get; set; }
    }
}
