using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoResource.V1_0
{
    [XmlInclude(typeof(VoDataService.V1_0.ParamHttp))]
    public class Interface
    {
        public AccessUrl[] AccessUrlList { get; set; }
        public SecurityMethod[] SecurityMethodList { get; set; }
        public string Version { get; set; }
        public string Role { get; set; }
    }
}
