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
    [XmlRoot(Constants.TagOption, Namespace = Constants.NamespaceVoTableV1_3)]
    public class Option : IOption
    {
        [XmlElement(Constants.TagOption)]
        public List<Option> OptionList { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
    }
}
