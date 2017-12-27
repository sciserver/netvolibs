﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlRoot(Constants.TagParam, Namespace = Constants.VOTableNamespaceV1_2)]
    public class Param :V1_1.Param
    {
        /*
        // extension Field

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
        */
        [XmlAttribute(Constants.AttributeXType)]
        public string Xtype { get; set; }
    }
}
