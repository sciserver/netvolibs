using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits
{
    public static class Constants
    {
        public const string UnitUnknown = "unknown";
        public const string UnitUnknown2 = "UNKNOWN";
        public const string UnitDimensionless = "";

        public const string PrefixDeca = "da";
        public const string PrefixHecto = "h";
        public const string PrefixKilo = "k";
        public const string PrefixMega = "M";
        public const string PrefixGiga = "G";
        public const string PrefixTera = "T";
        public const string PrefixPeta = "P";
        public const string PrefixExa = "E";
        public const string PrefixZetta = "Z";
        public const string PrefixYotta = "Y";

        public const string PrefixDeci = "d";
        public const string PrefixCenti = "c";
        public const string PrefixMilli = "m";
        public const string PrefixMicro = "u";
        public const string PrefixNano = "n";
        public const string PrefixPico = "p";
        public const string PrefixFemto = "f";
        public const string PrefixAtto = "a";
        public const string PrefixZepto = "z";
        public const string PrefixYocto = "y";

        public const string PrefixKibi = "Ki";
        public const string PrefixMebi = "Mi";
        public const string PrefixGibi = "Gi";
        public const string PrefixTebi = "Ti";
        public const string PrefixPebi = "Pi";
        public const string PrefixExbi = "Ei";
        public const string PrefixZebi = "Zi";
        public const string PrefixYobi = "Yi";

        // Base units
        public const string UnitMetre = "m";
        public const string UnitSecond = "s";
        public const string UnitAmpere = "A";
        public const string UnitKelvin = "K";
        public const string UnitMole = "mol";
        public const string UnitCandela = "cd";
        public const string UnitGram = "g";
        public const string UnitRadian = "rad";
        public const string UnitSteradian = "sr";
        public const string UnitHertz = "Hz";
        public const string UnitNewton = "N";
        public const string UnitPascal = "Pa";
        public const string UnitJoule = "J";
        public const string UnitWatt = "W";
        public const string UnitCoulomb = "C";
        public const string UnitVolt = "V";
        public const string UnitSiemens = "S";
        public const string UnitFarad = "F";
        public const string UnitWeber = "Wb";
        public const string UnitTesla = "T";
        public const string UnitHenry = "H";
        public const string UnitLumen = "lm";
        public const string UnitLux = "lx";
        public const string UnitOhm = "Ohm";

        // Known units
        // public const string UnitPercent = "%";  // Not supported
        public const string UnitAdu = "adu";
        public const string UnitAngstrom = "Angstrom";
        public const string UnitAngstrom2 = "angstrom";
        public const string UnitAngstrom3 = "AA";       // Not VO standard
        public const string UnitArcmin = "arcmin";
        public const string UnitArcsec = "arcsec";
        public const string UnitAU = "AU";
        public const string UnitAU2 = "au";
        public const string UnitBesselianYear = "Ba";
        public const string UnitBarn = "barn";
        public const string UnitBeam = "beam";
        public const string UnitBin = "bin";
        public const string UnitBit = "bit";
        public const string UnitByte = "byte";
        public const string UnitByte2 = "B";
        public const string UnitChannel = "chan";
        public const string UnitCount = "count";
        public const string UnitCount2 = "ct";
        public const string UnitDay = "d";
        public const string UnitDecibel = "dB";
        public const string UnitDebye = "D";
        public const string UnitDegree = "deg";
        public const string UnitErg = "erg";
        public const string UnitElectronVolt = "eV";
        public const string UnitGauss = "G";
        public const string UnitHour = "h";
        public const string UnitJansky = "Jy";
        public const string UnitLightYear = "lyr";
        public const string UnitMagnitudes = "mag";
        public const string UnitMilliArcsec = "mas";
        public const string UnitMinute = "min";
        public const string UnitParsec = "pc";
        public const string UnitPhoton = "photon";
        public const string UnitPhoton2 = "ph";
        public const string UnitPixel = "pixel";
        public const string UnitPixel2 = "pix";
        public const string UnitRayleigh = "R";
        public const string UnitRydberg = "Ry";
        public const string UnitSun = "Sun";
        public const string UnitSolarLuminosity = "solLum";
        public const string UnitSolarMass = "solMass";
        public const string UnitSolarRadius = "solRad";
        public const string UnitTropicalYear = "ta";
        public const string UnitAmu = "u";      // Atomic mass unit
        public const string UnitVoxel = "voxel";
        public const string UnitJulianYear = "yr";
        public const string UnitJulianYear2 = "a";

        public const string FunctionLog10 = "log";
        public const string FunctionLog = "ln";
        public const string FunctionExp = "exp";
        public const string FunctionSqrt = "sqrt";

        public const string OpMultiply = ".";
        public const string OpDivide = "/";
        public const string OpPower = "**";
    }
}
