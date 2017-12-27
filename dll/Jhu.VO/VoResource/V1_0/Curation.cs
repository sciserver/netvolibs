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
    public class Curation
    {
        [XmlElement(Constants.TagPublisher, Form = XmlSchemaForm.Unqualified)]
        public ResourceName Publisher { get; set; }

        [XmlElement(Constants.TagCreator, Form = XmlSchemaForm.Unqualified)]
        public List<Creator> CreatorList { get; set; }

        [XmlElement(Constants.TagContributor, Form = XmlSchemaForm.Unqualified)]
        public List<ResourceName> ContributorList { get; set; }

        [XmlElement(Constants.TagDate, Form = XmlSchemaForm.Unqualified)]
        public List<Date> DateList { get; set; }

        [XmlElement(Constants.TagVersion, Form = XmlSchemaForm.Unqualified)]
        public string Version { get; set; }

        [XmlElement(Constants.TagContact, Form = XmlSchemaForm.Unqualified)]
        public List<Contact> ContactList { get; set; }
    }
}
