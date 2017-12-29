﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Jhu.VO.VoTable
{
    public class Constants
    {
        public const string FileExtensionVOTable = ".votable";
        public const string MimeTypeVOTable = "application/x-votable+xml";

        // Name spaces
        public const string NamespaceXsi = "http://www.w3.org/2001/XMLSchema-instance";
        public const string NamespaceXlink = "http://www.w3.org/1999/xlink";
        public const string NamespaceXsd = "http://www.w3.org/2001/XMLSchema";
        public const string NamespaceVoTableV1_1 = "";
        public const string NamespaceVoTableV1_2 = "http://www.ivoa.net/xml/VOTable/v1.2";
        public const string NamespaceVoTableV1_3 = "http://www.ivoa.net/xml/VOTable/v1.3";

        public const string VersionV1_1 = "1.1";
        public const string VersionV1_2 = "1.2";
        public const string VersionV1_3 = "1.3";

        public const string TagVoTable = "VOTABLE";
        public const string TagDescription = "DESCRIPTION";
        public const string TagDefinitions = "DEFINITIONS";
        public const string TagCoosys = "COOSYS";
        public const string TagGroup = "GROUP";
        public const string TagParam = "PARAM";
        public const string TagInfo = "INFO";
        public const string TagValues = "VALUES";
        public const string TagMin = "MIN";
        public const string TagMax = "MAX";
        public const string TagResource = "RESOURCE";
        public const string TagTable = "TABLE";
        public const string TagField = "FIELD";
        public const string TagTableData = "TABLEDATA";
        public const string TagBinary = "BINARY";
        public const string TagBinary2 = "BINARY2";
        public const string TagFits = "FITS";
        public const string TagStream = "STREAM";
        public const string TagData = "DATA";
        public const string TagTR = "TR";
        public const string TagTD = "TD";
        public const string TagLink = "LINK";
        public const string TagOption = "OPTION";
        public const string TagFieldRef = "FIELDref";
        public const string TagParamRef = "PARAMref";

        public const string AttributeID = "ID";
        public const string AttributeVersion = "version";
        public const string AttributeName = "name";
        public const string AttributeValue = "value";
        public const string AttributeDescription = "description";
        public const string AttributeXType = "xtype";
        public const string AttributeUType = "utype";
        public const string AttributeRef = "ref";
        public const string AttributeDataType = "datatype";
        public const string AttributeUcd = "ucd";
        public const string AttributeUnit = "unit";
        public const string AttributePrecision = "precision";
        public const string AttributeWidth = "width";
        public const string AttributeArraySize = "arraysize";
        public const string AttributeInclusive = "inclusive";
        public const string AttributeContentRole = "content-role";
        public const string AttributeContentType = "content-type";
        public const string AttributeTitle = "title";
        public const string AttributeHref = "href";
        public const string AttributeGref = "gref";
        public const string AttributeAction = "action";
        public const string AttributeEncoding = "encoding";
        public const string AttributeType = "type";
        public const string AttributeDatatype = "datatype";
        public const string AttributeNull = "null";
        public const string AttributeSystem = "system";
        public const string AttributeEpoch = "epoch";
        public const string AttributeEquinox = "equinox";
        public const string AttributeExtNum = "extnum";
        public const string AttributeActuate = "actuate";
        public const string AttributeExpires = "expires";
        public const string AttributeRights = "rights";
        public const string AttributeNRows = "nrows";

        public const string TypeBoolean = "boolean";
        public const string TypeBit = "bit";
        public const string TypeUnsignedByte = "unsignedbyte";
        public const string TypeShort = "short";
        public const string TypeInt = "int";
        public const string TypeLong = "long";
        public const string TypeChar = "char";
        public const string TypeUnicodeChar = "unicodechar";
        public const string TypeFloat = "float";
        public const string TypeDouble = "double";
        public const string TypeFloatComplex = "floatcomplex";
        public const string TypeDoubleComplex = "doublecomplex";
    }

}
