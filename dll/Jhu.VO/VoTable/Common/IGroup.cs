using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable.Common
{
    public interface IGroup
    {
        IAnyText Description { get; set; }
        ElementList<IFieldRef> FieldRefList { get; }
        ElementList<IParamRef> ParamRefList { get; }
        ElementList<IParam> ParamList { get; }
        ElementList<IGroup> GroupList { get; }
    }
}
