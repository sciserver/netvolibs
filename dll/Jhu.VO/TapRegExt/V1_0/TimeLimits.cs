using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.TapRegExt.Common;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(Namespace = Constants.TapRegExtNamespaceV1_0)]
    public class TimeLimits : ITimeLimits
    {
        [XmlElement(Constants.TagDefault, Form = XmlSchemaForm.Unqualified)]
        public int? Default { get; set; }

        [XmlElement(Constants.TagHard, Form = XmlSchemaForm.Unqualified)]
        public int? Hard { get; set; }
    }
}
