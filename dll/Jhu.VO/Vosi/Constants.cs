using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.Vosi
{
    public class Constants
    {
        public const string VosiAvailabilityNamespaceV1_0 = "http://www.ivoa.net/xml/VOSIAvailability/v1.0";
        public const string VosiCapabilitiesNamespaceV1_0 = "http://www.ivoa.net/xml/VOSICapabilities/v1.0";
        public const string VosiTablesNamespaceV1_0 = "http://www.ivoa.net/xml/VOSITables/v1.0";

        public const string TagCapabilities = "capabilities";
        public const string TagAvailability = "availability";
        public const string TagAvailable = "available";
        public const string TagUpSince = "upSince";
        public const string TagDownAt = "downAt";
        public const string TagBackAt = "backAt";
        public const string TagNote = "note";
        public const string TagTableSet = "tableset";
        public const string TagTable = "table";
    }
}
