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
    public class ForeignKey
    {
        [XmlElement(Constants.TagTargetTable, Form = XmlSchemaForm.Unqualified)]
        public string TargetTable { get; set; }

        [XmlElement(Constants.TagFKColumn, Form = XmlSchemaForm.Unqualified)]
        public List<FKColumn> FKColumnList { get; set; }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagUtype, Form = XmlSchemaForm.Unqualified)]
        public string UType { get; set; }
    }
}
