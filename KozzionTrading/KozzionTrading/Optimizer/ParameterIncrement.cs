using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class ParameterIncrement
    {
        public double Increment { get; private set; }
        public IncrementType IncrementType { get; private set; }

        public ParameterIncrement(double increment, IncrementType increment_type)
        {
            this.Increment = increment;
            this.IncrementType = increment_type;
        }


        public ParameterValue IncrementValue(ParameterValue value, int count)
        {
            for (int index = 0; index < count; index++)
            {
                value = IncrementValue(value);
            }
            return value;
        }

        public ParameterValue DecrementValue(ParameterValue value, int count)
        {
            for (int index = 0; index < count; index++)
            {
                value = DecrementValue(value);
            }
            return value;
        }

        public ParameterValue DecrementValue(ParameterValue value)
        {
            switch (IncrementType)
            {
                case IncrementType.Linear:
                    return DecrementLinear(value);
                case IncrementType.Exponential:
                    return DecrementExponential(value);
                default:
                    throw new Exception();
            }
        }

 

        public ParameterValue IncrementValue(ParameterValue value)
        {
            switch (IncrementType)
            {
                case IncrementType.Linear:
                    return IncrementLinear(value);
                case IncrementType.Exponential:
                    return IncrementExponential(value);
                default:
                    throw new Exception();
            }
        }

        private ParameterValue IncrementExponential(ParameterValue value)
        {
            switch (value.Type)
            {
                case TypeCode.Double:
                    return new ParameterValue(value.Name, TypeCode.Double, value.Float64Value * Increment);
                case TypeCode.Int32:
                    return new ParameterValue(value.Name, TypeCode.Int32, Math.Max((int)(value.Int32Value * Increment), value.Int32Value + 1));
                default:
                    throw new Exception("Unknown parameter type: " + value.Type);
            }
        }

        private ParameterValue IncrementLinear(ParameterValue value)
        {
            switch (value.Type)
            {
                case TypeCode.Double:
                    return new ParameterValue(value.Name, TypeCode.Double, value.Float64Value + Increment);
                case TypeCode.Int32:
                    return new ParameterValue(value.Name, TypeCode.Int32, Math.Max((int)(value.Int32Value + Increment), value.Int32Value + 1));
                default:
                    throw new Exception("Unknown parameter type: " + value.Type);
            }
        }

        private ParameterValue DecrementExponential(ParameterValue value)
        {
            switch (value.Type)
            {
                case TypeCode.Double:
                    return new ParameterValue(value.Name, TypeCode.Double, value.Float64Value / Increment);
                case TypeCode.Int32:
                    return new ParameterValue(value.Name, TypeCode.Int32, Math.Max((int)(value.Int32Value / Increment), value.Int32Value - 1));
                default:
                    throw new Exception("Unknown parameter type: " + value.Type);
            }
        }

        private ParameterValue DecrementLinear(ParameterValue value)
        {
            switch (value.Type)
            {
                case TypeCode.Double:
                    return new ParameterValue(value.Name, TypeCode.Double, value.Float64Value - Increment);
                case TypeCode.Int32:
                    return new ParameterValue(value.Name, TypeCode.Int32, Math.Min((int)(value.Int32Value - Increment), value.Int32Value - 1));
                default:
                    throw new Exception("Unknown parameter type: " + value.Type);
            }
        }
    }
}