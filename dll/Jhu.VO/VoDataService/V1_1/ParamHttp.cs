using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_1
{
    [XmlType(TypeName = Constants.TypeParamHttp, Namespace = Constants.VoDataServiceNamespaceV1_1)]
    [XmlRoot(VoResource.Constants.TagInterface, Namespace = Constants.VoDataServiceNamespaceV1_1)]
    public class ParamHttp : VoResource.V1_0.Interface
    {
        [XmlElement(Constants.TagQueryType, Form = XmlSchemaForm.Unqualified)]
        public string QueryType { get; set; }

        [XmlElement(Constants.TagResultType, Form = XmlSchemaForm.Unqualified)]
        public string ResultType { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public List<InputParam> ParamList { get; set; }

        [XmlElement(Constants.TagTestQuery, Form = XmlSchemaForm.Unqualified)]
        public List<string> TestQueryList { get; set; }
    }
}
