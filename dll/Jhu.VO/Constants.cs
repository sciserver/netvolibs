using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Jhu.VO
{
    public class Constants
    {
        public static readonly HashSet<string> SupportedNamespaces = new HashSet<string>()
        {
            "http://www.w3.org/1999/xlink",
            XmlSchema.InstanceNamespace,
            Stc.Constants.StcNamespaceV1_30,
            TapRegExt.Constants.TapRegExtNamespaceV1_0,
            VoDataService.Constants.VoDataServiceNamespaceV1_0,
            VoDataService.Constants.VoDataServiceNamespaceV1_1,
            VoResource.Constants.VoResourceNamespaceV1_0,
            Vosi.Constants.VosiAvailabilityNamespaceV1_0,
            Vosi.Constants.VosiCapabilitiesNamespaceV1_0,
            Vosi.Constants.VosiTablesNamespaceV1_0,
            VoTable.Constants.NamespaceVoTableV1_1,
            VoTable.Constants.NamespaceVoTableV1_2,
            VoTable.Constants.NamespaceVoTableV1_3,
        };

        public const string StandardIDTap = "ivo://ivoa.net/std/TAP";
        public const string StandardIDVosiTables = "ivo://ivoa.net/std/VOSI#tables";
        public const string StandardIDVosiCapabilities = "ivo://ivoa.net/std/VOSI#capabilities";
        public const string StandardIDVosiAvailability = "ivo://ivoa.net/std/VOSI#availability";
        public const string StanraddIDDaliExamples = "ivo://ivoa.net/std/DALI#examples";
    }
}
