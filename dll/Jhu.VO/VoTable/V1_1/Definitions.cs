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
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    [XmlRoot(Constants.TagDefinitions, Namespace = Constants.NamespaceVoTableV1_1)]
    public class Definitions
    {
        #region COOSYS PARAM

        [XmlElement(Constants.TagCoosys, typeof(Coosys))]
        [XmlElement(Constants.TagParam, typeof(Param))]
        public List<object> ItemList_ForXml { get; set; }

        [XmlIgnore]
        public ItemList<Coosys> CoosysList
        {
            get { return new ItemList<Coosys>(ItemList_ForXml); }
        }

        [XmlIgnore]
        public ItemList<Param> ParamList
        {
            get { return new ItemList<Param>(ItemList_ForXml); }
        }

        #endregion
    }
}
