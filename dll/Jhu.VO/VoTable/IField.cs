using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    public interface IField
    {
        string Description { get; set; }

        IValues Values { get; set; }

        string ID { get; set; }

        string Unit { get; set; }

        string Datatype { get; set; }

        string Precision { get; set; }

        string Width { get; set; }

        string Name { get; set; }

        string Ucd { get; set; }

        string UType { get; set; }

        string Arraysize { get; set; }
    }
}
