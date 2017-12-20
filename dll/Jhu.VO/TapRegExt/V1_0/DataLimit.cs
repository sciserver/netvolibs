using System;
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
    public class DataLimit
    {
        [XmlText]
        public int Value { get; set; }

        [XmlAttribute(Constants.AttributeUnit, Form = XmlSchemaForm.Unqualified)]
        public string Unit { get; set; }
    }
}
