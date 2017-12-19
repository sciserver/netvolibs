using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.V1_0
{
    public class Resource
    {
        public Validation[] ValidationLevelList { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Identifier { get; set; }
        public Curation Curation { get; set; }
        public Content Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Status { get; set; }
    }
}
