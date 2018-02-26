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
    public class DataLimits : IDataLimits
    {
        [XmlElement(Constants.TagDefault, Form = XmlSchemaForm.Unqualified)]
        public DataLimit Default { get; set; }

        [XmlIgnore]
        IDataLimit IDataLimits.Default
        {
            get { return Default; }
            set { Default = (DataLimit)value; }
        }

        [XmlElement(Constants.TagHard, Form = XmlSchemaForm.Unqualified)]
        public DataLimit Hard { get; set; }

        [XmlIgnore]
        IDataLimit IDataLimits.Hard
        {
            get { return Hard; }
            set { Hard = (DataLimit)value; }
        }
    }
}
