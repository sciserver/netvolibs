using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.V1_0
{
    public class Curation
    {
        public ResourceName Publisher { get; set; }
        public Creator[] CreatorList { get; set; }
        public ResourceName[] ContributorList { get; set; }
        public Date[] DateList { get; set; }
        public string Version { get; set; }
        public Contact[] ContactList { get; set; }
    }
}
