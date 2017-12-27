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
    [XmlInclude(typeof(Vosi.Tables.V1_0.TableSet))]
    public class TableSet
    {
        [XmlElement(Constants.TagSchema, Form = XmlSchemaForm.Unqualified)]
        public List<TableSchema> SchemaList { get; set; }

        [XmlAnyAttribute]
        public List<XmlAttribute> Attributes { get; set; }
    }
}
