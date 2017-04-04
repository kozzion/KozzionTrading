using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class ParameterValue
    {
        public string Name { get; private set; }
        public TypeCode Type { get; private set; }
        public double Float64Value { get; private set; }
        public int Int32Value { get; private set; }

        public ParameterValue(string name, TypeCode type, int value)
        {
            this.Name = name;
            this.Type = type;

            switch (this.Type)
            {
                case TypeCode.Int32:
                    this.Int32Value = value;
                    break;
                case TypeCode.Double:
                    this.Float64Value = value;
                    break;
                default:
                    throw new Exception("Unknown type");
            }           
        }


        public ParameterValue(string name, TypeCode type, double value)
        {
            this.Name = name;
            this.Type = type;

            switch (this.Type)
            {
                case TypeCode.Int32:
                    throw new Exception("Incorret type");
                case TypeCode.Double:
                    this.Float64Value = value;
                    break;
                default:
                    throw new Exception("Unknown type");
            }
        }

        public override string ToString()
        {
            switch (this.Type)
            {
                case TypeCode.Int32:
                    return Name + " Int32: " + this.Int32Value;
                case TypeCode.Double:
                    return Name + " Float64: " + this.Float64Value;
                default:
                    throw new Exception("Unknown type");
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() + this.Type.GetHashCode() + this.Float64Value.GetHashCode() + this.Int32Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is ParameterValue)
            {
                ParameterValue other_typed = (ParameterValue)other;
                if (!other_typed.Name.Equals(this.Name))
                {
                    return false;
                }
                if (!other_typed.Type.Equals(this.Type))
                {
                    return false;
                }
                if (!other_typed.Float64Value.Equals(this.Float64Value))
                {
                    return false;
                }
                if (!other_typed.Int32Value.Equals(this.Int32Value))
                {
                    return false;
                }
                return true;
            }
            return false;
          
        }
    }
}
