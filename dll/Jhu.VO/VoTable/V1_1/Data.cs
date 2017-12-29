using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_1
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_1)]
    public class Data : IData
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
        public List<Info> InfoList { get; set; }

        [XmlIgnore]
        ElementList<IInfo> IData.InfoList
        {
            get { return new ElementList<IInfo>(InfoList); }
        }
    }
}
