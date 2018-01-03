using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable.Common
{
    public interface ITable
    {
        IAnyText Description { get; set; }
        ElementList<IInfo> InfoList1 { get; }
        ElementList<IField> FieldList { get; }
        ElementList<IParam> ParamList { get; }
        ElementList<IGroup> GroupList { get; }
        ElementList<ILink> LinkList { get; }
        ElementList<IInfo> InfoList2 { get; }
        IData Data { get; }
        string ID { get; set; }
        string Name { get; set; }
    }
}
