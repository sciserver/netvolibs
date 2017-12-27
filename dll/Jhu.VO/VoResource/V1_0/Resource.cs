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
    [XmlInclude(typeof(Organisation))]
    [XmlInclude(typeof(Service))]
    [XmlInclude(typeof(VoDataService.V1_0.DataService))]
    [XmlInclude(typeof(VoDataService.V1_0.TableService))]
    [XmlInclude(typeof(VoDataService.V1_0.CatalogService))]
    public class Resource
    {
        [XmlElement(Constants.TagValidationLevel, Form = XmlSchemaForm.Unqualified)]
        public List<Validation> ValidationLevelList { get; set; }

        [XmlElement(Constants.TagTitle, Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [XmlElement(Constants.TagShortName, Form = XmlSchemaForm.Unqualified)]
        public string ShortName { get; set; }

        [XmlElement(Constants.TagIdentifier, Form = XmlSchemaForm.Unqualified)]
        public string Identifier { get; set; }

        [XmlElement(Constants.TagCuration, Form = XmlSchemaForm.Unqualified)]
        public Curation Curation { get; set; }

        [XmlElement(Constants.TagContent, Form = XmlSchemaForm.Unqualified)]
        public Content Content { get; set; }

        [XmlAttribute(Constants.AttributeCreated, Form = XmlSchemaForm.Unqualified)]
        public DateTime Created { get; set; }

        [XmlAttribute(Constants.AttributeUpdated, Form = XmlSchemaForm.Unqualified)]
        public DateTime Updated { get; set; }

        [XmlAttribute(Constants.AttributeStatus, Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }
    }
}
