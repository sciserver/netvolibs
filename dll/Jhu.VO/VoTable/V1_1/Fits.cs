using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class Fits
    {
        [XmlElement(Constants.TagStream)]
        public Stream Stream { get; set; }

        [XmlAttribute(Constants.AttributeExtNum)]
        public int ExtNum { get; set; }
    }
}
