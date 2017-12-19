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
    [XmlInclude(typeof(VoDataService.V1_0.DataService))]
    [XmlInclude(typeof(VoDataService.V1_0.TableService))]
    [XmlInclude(typeof(VoDataService.V1_0.CatalogService))]
    public class Service
    {
        public string[] RightsList { get; set; }
        public Capability[] CapabilityList { get; set; }
    }
}
