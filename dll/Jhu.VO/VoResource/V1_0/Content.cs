using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.V1_0
{
    public class Content
    {
        public string[] SubjectList { get; set; }
        public string Description { get; set; }
        public Source Source { get; set; }
        public string ReferenceUrl { get; set; }
        public string[] TypeList { get; set; }
        public string[] ContentLevelList { get; set; }
        public Relationship[] RelationshipList { get; set; }
    }
}
