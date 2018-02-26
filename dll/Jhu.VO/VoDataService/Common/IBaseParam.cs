using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jhu.VO.VoDataService.Common
{
    public interface IBaseParam
    {
        string Name { get; set; }
        string Description { get; set; }
        string Unit { get; set; }
        string Ucd { get; set; }
    }
}
