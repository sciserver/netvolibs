using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class TableData : ITableData
    {
        [XmlElement(Constants.TagTR)]
        public List<Tr> TrList { get; set; }
    }
}
