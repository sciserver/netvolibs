using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlRoot(Constants.TagParamRef, Namespace = Constants.VOTableNamespaceV1_2)]
    public class ParamRef : V1_1.ParamRef
    {
        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { get; set; }
    }
}
