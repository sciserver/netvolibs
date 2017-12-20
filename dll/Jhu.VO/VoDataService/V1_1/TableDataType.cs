using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_1
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_1)]
    [XmlInclude(typeof(TapDataType))]
    [XmlInclude(typeof(TapType))]
    [XmlInclude(typeof(VOTableType))]
    public class TableDataType : DataType
    {
    }
}
