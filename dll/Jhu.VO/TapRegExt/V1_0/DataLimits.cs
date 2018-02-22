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
    [XmlType(Namespace = Constants.TapRegExtNamespaceV1_0)]
    public class DataLimits
    {
        [XmlElement(Constants.TagDefault, Form = XmlSchemaForm.Unqualified)]
        public DataLimit Default { get; set; }

        [XmlElement(Constants.TagHard, Form = XmlSchemaForm.Unqualified)]
        public DataLimit Hard { get; set; }
    }
}
