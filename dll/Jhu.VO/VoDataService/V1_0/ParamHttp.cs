using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoDataService.Common;

namespace Jhu.VO.VoDataService.V1_0
{
    [XmlType(TypeName = Constants.TypeParamHttp, Namespace = Constants.VoDataServiceNamespaceV1_0)]
    [XmlRoot(VoResource.Constants.TagInterface, Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class ParamHttp : VoResource.V1_0.Interface, IParamHttp
    {
        [XmlElement(Constants.TagQueryType, Form = XmlSchemaForm.Unqualified)]
        public string QueryType { get; set; }

        [XmlElement(Constants.TagResultType, Form = XmlSchemaForm.Unqualified)]
        public string ResultType { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public List<InputParam> ParamList_ForXml { get; set; }

        [XmlIgnore]
        public ElementList<IInputParam> ParamList
        {
            get { return new ElementList<IInputParam>(ParamList_ForXml); }
        }
    }
}
