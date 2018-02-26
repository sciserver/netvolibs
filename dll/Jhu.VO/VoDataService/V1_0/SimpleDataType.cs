using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoDataService.Common;

namespace Jhu.VO.VoDataService.V1_0
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class SimpleDataType : ISimpleDataType
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeArraySize, Form = XmlSchemaForm.Unqualified)]
        public string ArraySize { get; set; }
    }
}
