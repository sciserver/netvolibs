using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_3
{
    [XmlType(Namespace = Constants.VOTableNamespaceV1_3)]
    public class Data
    {
        [XmlElement(Constants.TagTableData, Form = XmlSchemaForm.Unqualified)]
        public TableData TableData { get; set; }

        [XmlElement(Constants.TagBinary, Form = XmlSchemaForm.Unqualified)]
        public Binary Binary { get; set; }

        [XmlElement(Constants.TagBinary2, Form = XmlSchemaForm.Unqualified)]
        public Binary2 Binary2 { get; set; }

        [XmlElement(Constants.TagFits, Form = XmlSchemaForm.Unqualified)]
        public Fits Fits { get; set; }

        [XmlElement(Constants.TagInfo, Form = XmlSchemaForm.Unqualified)]
        public Info[] InfoList { get; set; }
    }
}
