using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_0
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class ParamHttp : VoResource.V1_0.Interface
    {
        [XmlElement(Constants.TagQueryType, Form = XmlSchemaForm.Unqualified)]
        public string QueryType { get; set; }

        [XmlElement(Constants.TagResultType, Form = XmlSchemaForm.Unqualified)]
        public string ResultType { get; set; }

        [XmlElement(Constants.TagParam, Form = XmlSchemaForm.Unqualified)]
        public InputParam[] ParamList { get; set; }
    }
}
