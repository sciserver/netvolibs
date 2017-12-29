using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.VO.VoTable.Common;

namespace Jhu.VO.VoTable
{
    public class VoTableColumn : ICloneable
    {
        #region Private variables

        private string id;
        private string name;
        private string description;
        private string ucd;
        private string utype;
        private string unit;
        private string precision;
        private string width;

        private VoTableDataType dataType;

        #endregion
        #region Properties

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Ucd
        {
            get { return ucd; }
            set { ucd = value; }
        }

        public string UType
        {
            get { return utype; }
            set { utype = value; }
        }

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public string Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        public string Width
        {
            get { return width; }
            set { width = value; }
        }

        public VoTableDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        #endregion
        #region Constructors and initializers

        internal VoTableColumn()
        {
            InitializeMembers();
        }

        internal VoTableColumn(VoTableColumn old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = null;
            this.name = null;
            this.description = null;
            this.ucd = null;
            this.utype = null;
            this.unit = null;
            this.precision = null;
            this.width = null;
            this.dataType = null;
        }

        private void CopyMembers(VoTableColumn old)
        {
            this.id = old.id;
            this.name = old.name;
            this.description = old.description;
            this.ucd = old.ucd;
            this.utype = old.utype;
            this.unit = old.unit;
            this.precision = old.precision;
            this.width = old.width;
            this.dataType = old.dataType;
        }

        public object Clone()
        {
            return new VoTableColumn(this);
        }

        #endregion

        public static VoTableColumn Create(string id, string name, VoTableDataType dataType)
        {
            var column = new VoTableColumn()
            {
                ID = id,
                Name = name,
                DataType = dataType
            };

            return column;
        }

        internal static VoTableColumn Create(IField field)
        {
            var column = new VoTableColumn()
            {
                ID = field.ID,
                Name = field.Name,
                Description = field.Description?.Text,
                Ucd = field.Ucd,
                UType = field.UType,
                Unit = field.Unit,
                Precision = field.Precision,
                Width = field.Width,
            };

            column.DataType = VoTableDataType.Create(field);

            return column;
        }

        internal V1_1.Field ToField()
        {
            throw new NotImplementedException();
        }
    }
}
