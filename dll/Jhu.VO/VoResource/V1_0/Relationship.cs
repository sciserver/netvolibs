using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.V1_0
{
    public class Relationship
    {
        public string RelationshipType { get; set; }
        public ResourceName[] RelatedResourceList { get; set; }
    }
}
