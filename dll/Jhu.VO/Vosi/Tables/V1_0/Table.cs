using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.Vosi.Tables.V1_0
{
    [XmlType(Namespace = Constants.VosiTablesNamespaceV1_0)]
    public class Table : VoDataService.V1_1.Table
    {
    }
}
