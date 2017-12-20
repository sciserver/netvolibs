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
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Constants.VosiTablesNamespaceV1_0)]
    [XmlRoot(Constants.TagTableSet, Namespace = Constants.VosiTablesNamespaceV1_0, IsNullable = false)]
    public class TableSet : VoDataService.V1_1.TableSet
    {
    }
}
