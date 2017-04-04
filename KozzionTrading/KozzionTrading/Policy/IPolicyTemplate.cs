using KozzionTrading.Optimizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Policy
{
    public interface IPolicyTemplate
    {
        ParameterSet DefaultParameters { get;}
        string Title { get; }
        IPolicy Instance(ParameterSet parameter_set);
        IPolicy Instance();
    }
}
