using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoResource.V1_0
{
    [XmlType(Namespace = Constants.VoResourceNamespaceV1_0)]
    public class Contact
    {
        [XmlElement(Constants.TagName, Form = XmlSchemaForm.Unqualified)]
        public ResourceName Name { get; set; }

        [XmlElement(Constants.TagAddress, Form = XmlSchemaForm.Unqualified)]
        public string Address { get; set; }

        [XmlElement(Constants.TagEmail, Form = XmlSchemaForm.Unqualified)]
        public string Email { get; set; }

        [XmlElement(Constants.TagTelephone, Form = XmlSchemaForm.Unqualified)]
        public string Telephone { get; set; }
    }
}
