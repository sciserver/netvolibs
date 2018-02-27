using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO
{
    public class VoXmlAttribute
    {
        public string LocalName { get; internal set; }
        public string NamespaceURI { get; internal set; }
        public string Value { get; internal set; }
    }
}
