using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable.Common
{
    interface IVoTable
    {
        IAnyText Description { get; set; }
        IDefinitions Definitions { get; set; }
        ElementList<ICoordinateSystem> CoosysList { get; }
        ElementList<IGroup> GroupList { get; }
        ElementList<IParam> ParamList { get; }
        ElementList<IInfo> InfoList1 { get; }
        ElementList<IResource> ResourceList { get; }
        ElementList<IInfo> InfoList2 { get; }
        string ID { get; set; }
        string Version { get; set; }

        Type GetType(Type iface);
        T CreateElement<T>();
    }
}
