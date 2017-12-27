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
    public class TableParam : BaseParam
    {
        [XmlElement(Constants.TagDataType, Form = XmlSchemaForm.Unqualified)]
        public TableDataType DataType { get; set; }

        [XmlElement(Constants.TagFlag, Form = XmlSchemaForm.Unqualified)]
        public List<string> FlagList { get; set; }

        [XmlAttribute(VoResource.Constants.AttributeStd, Form = XmlSchemaForm.Unqualified)]
        public bool Std { get; set; }
    }
}
