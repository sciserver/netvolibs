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

    public class AnyText : IXmlSerializable
    {
        public string Text { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Text = reader.ReadInnerXml();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(Text);
        }
    }
}
