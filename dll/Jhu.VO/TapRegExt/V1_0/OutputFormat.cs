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
    public class OutputFormat
    {
        [XmlElement(Constants.AttributeMime, Form = XmlSchemaForm.Unqualified)]
        public string Mime { get; set; }

        [XmlElement(Constants.AttributeAlias, Form = XmlSchemaForm.Unqualified)]
        public string[] Alias { get; set; }

        [XmlAttribute(VoResource.Constants.AttributeIvoID, Form = XmlSchemaForm.Unqualified)]
        public string IvoID { get; set; }
    }
}
