﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO.TapRegExt.Common
{
    public interface IDataLimit
    {
        int Value { get; set; }
        string Unit { get; set; }
    }
}
