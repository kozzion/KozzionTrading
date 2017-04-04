using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class ParameterAxis
    {
        public int Size { get { return ValueList.Count; } }
        public string Name { get { return MinValue.Name; } }
        public TypeCode Type { get { return MinValue.Type; } }

        public ParameterValue MinValue { get; private set; }
        public ParameterValue DefaultValue { get; private set; }
        public IReadOnlyList<ParameterValue> ValueList { get; private set; }
        public Dictionary<ParameterValue, int> ValuesToIndexes { get; private set; }


        public ParameterAxis(ParameterValue min_value, ParameterIncrement increment, int axis_count, int default_index)
        {
            this.MinValue = min_value;
            ValuesToIndexes = new Dictionary<ParameterValue, int>();

            List<ParameterValue> temp_list = new List<ParameterValue>();

            temp_list.Add(min_value);
            ValuesToIndexes.Add(min_value, 0);
            for (int index = 1; index < axis_count; index++)
            {
                temp_list.Add(increment.IncrementValue(temp_list.Last()));
                ValuesToIndexes.Add(temp_list.Last(), index);
            }
            this.ValueList = temp_list;
            this.DefaultValue = ValueList[default_index];
        }

        public bool Contains(ParameterValue parameter_value)
        {
            return ValuesToIndexes.ContainsKey(parameter_value);
        }

        public int GetIndex(ParameterValue parameter_value)
        {
            return ValuesToIndexes[parameter_value];
        }

        public ParameterValue GetValue(int index)
        {
            return ValueList[index];
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ParameterAxis");
            foreach (ParameterValue value in ValueList)
            {
                builder.AppendLine(value.ToString());
            }
            return builder.ToString();
        }
    }
}
