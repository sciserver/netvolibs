using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoTable.V1_2
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_2)]
    public class Data
    {
        #region TABLEDATA BINARY BINARY2 FITS

        [XmlElement(Constants.TagTableData, typeof(TableData))]
        [XmlElement(Constants.TagBinary, typeof(Binary))]
        [XmlElement(Constants.TagFits, typeof(Fits))]
        public object Item_ForXml { get; set; }

        [XmlIgnore]
        public TableData TableData
        {
            get { return Item_ForXml as TableData; }
            set { Item_ForXml = value; }
        }

        [XmlIgnore]
        public Binary Binary
        {
            get { return Item_ForXml as Binary; }
            set { Item_ForXml = value; }
        }
        
        [XmlIgnore]
        public Fits Fits
        {
            get { return Item_ForXml as Fits; }
            set { Item_ForXml = value; }
        }

        #endregion

        [XmlElement(Constants.TagInfo)]
        public List<Info> InfoList { get; set; } = new List<Info>();
    }
}
