using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_2)]
    [XmlRoot(Constants.TagParam, Namespace = Constants.NamespaceVoTableV1_2)]
    public class Param : Field, IField
    {
        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
    }
}
