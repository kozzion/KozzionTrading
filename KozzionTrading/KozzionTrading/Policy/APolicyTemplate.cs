using System;
using KozzionTrading.Market;
using KozzionTrading.Optimizer;

namespace KozzionTrading.Policy
{
    public abstract class APolicyTemplate : IPolicyTemplate
    {
        public string Title { get; private set; }

        public ParameterSet DefaultParameters { get; private set; }

        protected APolicyTemplate(string title, ParameterSet default_parameters)
        {
            Title = title;
            DefaultParameters = default_parameters;
        }

        public IPolicy Instance()
        {
            return Instance(DefaultParameters);
        }


        public abstract IPolicy Instance(ParameterSet parameter_set);
    }
}