using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.TapRegExt.Common;

namespace Jhu.VO.TapRegExt.V1_0
{
    [XmlType(Namespace = Constants.TapRegExtNamespaceV1_0)]
    public class OutputFormat : IOutputFormat
    {
        [XmlElement(Constants.AttributeMime, Form = XmlSchemaForm.Unqualified)]
        public string Mime { get; set; }

        [XmlElement(Constants.AttributeAlias, Form = XmlSchemaForm.Unqualified)]
        public List<string> Alias { get; set; }

        [XmlAttribute(VoResource.Constants.AttributeIvoID, Form = XmlSchemaForm.Unqualified)]
        public string IvoID { get; set; }
    }
}
