using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class ParameterSet
    {

        public Dictionary<string, ParameterValue> ParameterValuesMap;
        public IReadOnlyList<ParameterValue> ParameterValues;

        public ParameterSet(IList<ParameterValue> parameter_values)
        {
            ParameterValues = new List<ParameterValue>(parameter_values);
            ParameterValuesMap = new Dictionary<string, ParameterValue>();
            foreach (ParameterValue parameter_value in ParameterValues)
            {
                ParameterValuesMap.Add(parameter_value.Name, parameter_value);
            }
        }

        public ParameterValue this[string key]
        {
            get
            {
                return ParameterValuesMap[key];
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ParameterSet {");
            foreach (ParameterValue value in ParameterValues)
            {
                builder.Append("  ");
                builder.AppendLine(value.ToString());
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        public override int GetHashCode()
        {
            int hash_code = 0;
            foreach (ParameterValue value in ParameterValues)
            {
                hash_code += value.GetHashCode();
            }
            return hash_code;
        }

        public override bool Equals(object other)
        {
            if (other is ParameterSet)
            {
                ParameterSet other_typed = (ParameterSet)other;
                if (ParameterValues.Count != other_typed.ParameterValues.Count)
                {
                    return false;
                }
                foreach (ParameterValue value in ParameterValues)
                {
                    if (!other_typed.ParameterValuesMap.ContainsKey(value.Name))
                    {
                        return false;
                    }

                    if (!other_typed.ParameterValuesMap[value.Name].Equals(value))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;

        }

    }
}