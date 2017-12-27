using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_0
{

    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class Catalog
    {
        [XmlElement(Constants.TagTable, Form = XmlSchemaForm.Unqualified)]
        public List<Table> TableList { get; set; }
    }
}
