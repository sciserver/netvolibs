﻿using System;
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
    [XmlRoot(Constants.TagValues, Namespace = Constants.NamespaceVoTableV1_1)]
    public class Values : IValues
    {
        [XmlElement(Constants.TagMin)]
        public Min Min { get; set; }

        [XmlElement(Constants.TagMax)]
        public Max Max { get; set; }

        [XmlElement(Constants.TagOption)]
        public List<Option> OptionList { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeType)]
        public string Type { get; set; }

        [XmlAttribute(Constants.AttributeNull)]
        public string Null { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }
    }
}
