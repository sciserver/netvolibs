using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_2)]
    [XmlRoot(Constants.TagCoosys, Namespace = Constants.NamespaceVoTableV1_2)]
    public class CoordinateSystem : ICoordinateSystem
    {
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeEquinox)]
        public string Equinox { get; set; }

        [XmlAttribute(Constants.AttributeEpoch)]
        public string Epoch { get; set; }

        [XmlAttribute(Constants.AttributeSystem)]
        public string System { get; set; }
    }
}
