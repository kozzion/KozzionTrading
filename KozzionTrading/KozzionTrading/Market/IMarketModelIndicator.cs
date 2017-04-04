﻿using KozzionCore.DataStructure.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public interface IMarketModelIndicator : IMarketModel
    {
  
        void StepSecond();
    }
}