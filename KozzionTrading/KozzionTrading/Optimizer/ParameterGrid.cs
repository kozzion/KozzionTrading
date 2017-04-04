using KozzionCore.Tools;
using KozzionTrading.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class ParameterSpaceGrid
    {
        public List<ParameterAxis> AxesList { get; private set; }
        public Dictionary<string, int> ParameterNameToAxisIndex { get; private set; }
        public int Size { get; private set; }

        public ParameterSpaceGrid(IList<ParameterAxis> axes)
        {
            AxesList = new List<ParameterAxis>(axes);
            ParameterNameToAxisIndex = new Dictionary<string, int>();
            foreach (ParameterAxis axis in axes)
            {
                if (ParameterNameToAxisIndex.ContainsKey(axis.Name))
                {
                    throw new Exception();
                }
            }

            Size = 0;
            foreach (ParameterAxis axis in AxesList)
            {
                Size += axis.Size;
            }
        }

        public ParameterSpaceGrid(ParameterSet defaults, double[] increments, int[] decrement_count, int[] increment_count)
        {
            AxesList = new List<ParameterAxis>();
            ParameterNameToAxisIndex = new Dictionary<string, int>();
            for (int parameter_index = 0; parameter_index < defaults.ParameterValues.Count; parameter_index++)
            {
                ParameterValue default_value = defaults.ParameterValues[parameter_index];
                ParameterIncrement increment = new ParameterIncrement(increments[parameter_index], IncrementType.Linear);
                ParameterValue min_value = increment.DecrementValue(default_value, decrement_count[parameter_index]);

                AxesList.Add(new ParameterAxis(
                      min_value,
                      increment,
                      decrement_count[parameter_index] + increment_count[parameter_index] + 1,
                      decrement_count[parameter_index]
                    ));
                ParameterNameToAxisIndex[default_value.Name] = parameter_index;
            }

            Size = 1;
            foreach (ParameterAxis axis in AxesList)
            {
                Size *= axis.Size;
            }
        }

        public bool Contains(int[] location)
        {
            if (location.Length != AxesList.Count)
            {
                throw new Exception("Incorrect location size");
            }

            for (int index = 0; index < location.Length; index++)
            {
                if ((location[index] < 0) || (AxesList[index].Size <= location[index]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Contains(ParameterSet parameter_set)
        {
            foreach (ParameterValue value in parameter_set.ParameterValues)
            {
                if (!ParameterNameToAxisIndex.ContainsKey(value.Name))
                {
                    return false;
                }

                if (AxesList[ParameterNameToAxisIndex[value.Name]].Type != value.Type)
                {
                    return false;
                }

                if (!AxesList[ParameterNameToAxisIndex[value.Name]].Contains(value))
                {
                    return false;
                }
            }
            return true;
        }

        //public ParemeterGrid(IList<Tuple<ParameterValue, 2>> axes)
        //{
        //}
        public int[] GetLocation(ParameterSet parameter_set)
        {
            int[] location = new int[AxesList.Count];
            for (int axes_index = 0; axes_index < AxesList.Count; axes_index++)
            {
                ParameterAxis axis = AxesList[axes_index];
                if (!parameter_set.ParameterValuesMap.ContainsKey(axis.Name))
                {
                    throw new Exception("ParameterSet does not contain: " + axis.Name);
                }
                location[axes_index] = axis.GetIndex(parameter_set.ParameterValuesMap[axis.Name]);
            }
            return location;
        }


        public ParameterSet GetParameterSet(int[] location)
        {
            if (!Contains(location))
            {
                throw new Exception("ParameterSet does not contain location");
            }
            List<ParameterValue> values = new List<ParameterValue>();
            for (int axes_index = 0; axes_index < AxesList.Count; axes_index++)
            {
                values.Add(AxesList[axes_index].GetValue(location[axes_index]));
            }
            return new ParameterSet(values);
        }

        public List<ParameterSet> GetNeighborsArea(ParameterSet parameter_set)
        {
            int[] source_location = GetLocation(parameter_set);
            List<int[]> locations = new List<int[]>();
            for (int axes_index = 0; axes_index < AxesList.Count; axes_index++)
            {
                int[] new_location_0 = ToolsCollection.Copy(source_location);
                int[] new_location_1 = ToolsCollection.Copy(source_location);
                new_location_0[axes_index]++;
                new_location_1[axes_index]--;

                if (Contains(new_location_0))
                {
                    locations.Add(new_location_0);
                }

                if (Contains(new_location_1))
                {
                    locations.Add(new_location_1);
                }
            }
            List<ParameterSet> parameter_sets = new List<ParameterSet>();
            foreach (int[] location in locations)
            {
                parameter_sets.Add(GetParameterSet(location));
            }  
            return parameter_sets;
        }

        public List<ParameterSet> GetNeighborsAxis(ParameterSet parameter_set, string parameter_name, int min_offset, int max_offset)
        {
            List<ParameterSet> neighbors = new List<ParameterSet>();

            return neighbors;
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ParameterSpaceGrid");
            foreach (ParameterAxis Axis in AxesList)
            {
                builder.AppendLine(Axis.ToString());
            }
            return builder.ToString();
        }
    }
}
