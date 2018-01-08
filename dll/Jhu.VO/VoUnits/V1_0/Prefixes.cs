﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.VoUnits.V1_0
{
    public static class Prefixes
    {
        public static Prefix Deca
        {
            get
            {
                return new Prefix()
                {
                    Symbol = Constants.PrefixDeca,
                    Value = 10
                };
            }
        }
    }
}