﻿using System;
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
    public class Fits : ITableData
    {
        [XmlElement(Constants.TagStream)]
        public Stream Stream { get; set; }

        [XmlAttribute(Constants.AttributeExtNum)]
        public int ExtNum { get; set; }
    }
}
