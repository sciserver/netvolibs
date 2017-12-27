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
    public class Definitions
    {
        [XmlElement(Constants.TagCoosys, Form = XmlSchemaForm.Unqualified)]
        public CoordinateSystem[] CoosysList { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public Param[] ParamList { get; set; }
    }
}
