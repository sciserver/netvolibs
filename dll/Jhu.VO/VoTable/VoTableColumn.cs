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
        private string format;

        private VoTableResource resource;
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

        internal string Format
        {
            get { return format; }
        }

        public VoTableDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        #endregion
        #region Constructors and initializers

        internal VoTableColumn(VoTableResource resource)
        {
            InitializeMembers();
            this.resource = resource;
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
            this.format = "{0}";

            this.dataType = null;
            this.resource = null;
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
            this.format = old.format;

            this.dataType = old.dataType;
            this.resource = old.resource;
        }

        public object Clone()
        {
            return new VoTableColumn(this);
        }

        #endregion

        public static VoTableColumn Create(VoTableResource resource, string id, string name, VoTableDataType dataType)
        {
            var column = new VoTableColumn(resource)
            {
                ID = id,
                Name = name,
                DataType = dataType
            };

            return column;
        }

        internal static VoTableColumn FromField(VoTableResource resource, IField field)
        {
            var column = new VoTableColumn(resource)
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

            column.DataType = VoTableDataType.FromField(field);

            return column;
        }

        internal void ToField(IField field)
        {
            field.ID = String.IsNullOrWhiteSpace(id) ? null : id;
            field.Name = String.IsNullOrWhiteSpace(name) ? null : name;
            field.Ucd = String.IsNullOrWhiteSpace(ucd) ? null : ucd;
            field.UType = String.IsNullOrWhiteSpace(utype) ? null : utype;
            field.Unit = String.IsNullOrWhiteSpace(unit) ? null : unit;
            field.Precision = String.IsNullOrWhiteSpace(precision) ? null : precision;
            field.Width = String.IsNullOrWhiteSpace(width) ? null : width;

            field.Datatype = dataType.Name;
            field.Arraysize = dataType.FormatArraySize();

            if (!String.IsNullOrWhiteSpace(description))
            {
                field.Description = resource.votable.votable.CreateElement<IAnyText>();
                field.Description.Text = description;
            }
            else
            {
                field.Description = null;
            }
        }
    }
}
