using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoTable
{
    public interface IField
    {
        string Description { get;}

        IValues Values { get;}

        string ID { get; }

        string Unit { get; }

        string Datatype { get; }

        string Precision { get; }

        string Width { get; }

        string Name { get; }

        string Ucd { get; }

        string UType { get; }

        string Arraysize { get; }
    }
}
