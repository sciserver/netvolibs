﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_1
{
    [XmlType(TypeName = Constants.TypeTapDataType, Namespace = Constants.VoDataServiceNamespaceV1_1)]
    [XmlInclude(typeof(TapType))]
    public class TapDataType : TableDataType
    {
        [XmlAttribute(Constants.AttributeSize, Form = XmlSchemaForm.Unqualified)]
        public uint Size { get; set; }
    }
}
