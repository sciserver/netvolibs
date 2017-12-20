﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(Namespace = Constants.TapRegExtNamespace)]
    public class LanguageFeatureList
    {
        [XmlElement(Constants.TagFeature, Form = XmlSchemaForm.Unqualified)]
        public LanguageFeature[] FeatureList { get; set; }

        [XmlAttribute(Constants.AttributeType, Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
    }
}
