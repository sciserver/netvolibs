using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable.V1_3
{
    [XmlType(Namespace = Constants.NamespaceVoTableV1_3)]
    public class Data : IData
    {
        #region TABLEDATA BINARY BINARY2 FITS

        [XmlElement(Constants.TagTableData, typeof(TableData))]
        [XmlElement(Constants.TagBinary, typeof(Binary))]
        [XmlElement(Constants.TagBinary2, typeof(Binary2))]
        [XmlElement(Constants.TagFits, typeof(Fits))]
        public object Item_ForXml { get; set; }

        [XmlIgnore]
        public ITableData TableData
        {
            get { return Item_ForXml as TableData; }
            set { Item_ForXml = value; }
        }

        [XmlIgnore]
        public ITableData Binary
        {
            get { return Item_ForXml as Binary; }
            set { Item_ForXml = value; }
        }

        [XmlIgnore]
        public ITableData Binary2
        {
            get { return Item_ForXml as Binary2; }
            set { Item_ForXml = value; }
        }

        [XmlIgnore]
        public ITableData Fits
        {
            get { return Item_ForXml as Fits; }
            set { Item_ForXml = value; }
        }

        #endregion

        [XmlElement(Constants.TagInfo)]
        public List<Info> InfoList { get; set; } = new List<Info>();

        [XmlIgnore]
        ElementList<IInfo> IData.InfoList
        {
            get { return new ElementList<IInfo>(InfoList); }
        }
    }
}
