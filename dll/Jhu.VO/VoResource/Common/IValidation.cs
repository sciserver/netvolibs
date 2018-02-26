using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoResource.Common
{
    public interface IValidation
    {
        int Value { get; set; }
        string ValidatedBy { get; set; }
    }
}
