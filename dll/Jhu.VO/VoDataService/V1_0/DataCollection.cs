﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.VO.VoDataService.V1_0
{
    [XmlType(Namespace = Constants.VoDataServiceNamespaceV1_0)]
    public class DataCollection
    {
        [XmlElement(Constants.TagFacility, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] FacilityList { get; set; }

        [XmlElement(Constants.TagInstrument, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.ResourceName[] InstrumentList { get; set; }

        [XmlElement(Constants.TagRights, Form = XmlSchemaForm.Unqualified)]
        public string[] RightsList { get; set; }

        [XmlElement(Constants.TagFormat, Form = XmlSchemaForm.Unqualified)]
        public Format[] FormatList { get; set; }

        [XmlElement(Constants.TagCoverage, Form = XmlSchemaForm.Unqualified)]
        public Coverage Coverage { get; set; }

        [XmlElement(Constants.TagCatalog, Form = XmlSchemaForm.Unqualified)]
        public Catalog[] CatalogList { get; set; }

        [XmlElement(VoResource.Constants.TagAccessUrl, Form = XmlSchemaForm.Unqualified)]
        public VoResource.V1_0.AccessUrl AccessUrl { get; set; }
    }
}
