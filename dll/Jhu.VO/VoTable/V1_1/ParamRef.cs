using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlRoot(Constants.TagParamRef, Namespace = Constants.VOTableNamespaceV1_1)]
    public class ParamRef
    {
        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }
    }
}
