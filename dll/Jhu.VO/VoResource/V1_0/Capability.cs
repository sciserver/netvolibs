using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoResource.Common;

namespace Jhu.VO.VoResource.V1_0
{
    [XmlType(Namespace = Constants.VoResourceNamespaceV1_0)]
    [XmlInclude(typeof(TapRegExt.V1_0.TapCapRestriction))]
    [XmlInclude(typeof(TapRegExt.V1_0.TableAccess))]
    public class Capability : ICapability
    {
        [XmlElement(Constants.TagValidationLevel, Form = XmlSchemaForm.Unqualified)]
        public List<Validation> ValidationLevelList { get; set; }

        [XmlIgnore]
        ElementList<IValidation> ICapability.ValidationLevelList
        {
            get { return new ElementList<IValidation>(ValidationLevelList); }
        }

        [XmlElement(Constants.TagDescription, Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }

        [XmlElement(Constants.TagInterface, Form = XmlSchemaForm.Unqualified)]
        public List<Interface> InterfaceList { get; set; }

        [XmlIgnore]
        ElementList<IInterface> ICapability.InterfaceList
        {
            get { return new ElementList<IInterface>(InterfaceList); }
        }

        [XmlAttribute(Constants.AttributeStandardID)]
        public string StandardID { get; set; }
    }
}
