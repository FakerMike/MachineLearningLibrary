using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DTSet
    {
        // The attributes still available to this set.
        public List<DTAttribute> AvailableAttributes { get; }

        // The examples in this set.
        public List<DTExample> Examples { get; }

        // If created by a split, save the value used to create it
        public DTAttribute.DTValue CreationValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public DTSet(IEnumerable<DTAttribute> attributes)
        {
            AvailableAttributes = new List<DTAttribute>(attributes);
            Examples = new List<DTExample>();
        }
        public DTSet(IEnumerable<DTAttribute> attributes, DTAttribute.DTValue value)
        {
            AvailableAttributes = new List<DTAttribute>(attributes);
            CreationValue = value;
            Examples = new List<DTExample>();
        }

        public void Add(DTExample example)
        {
            Examples.Add(example);
        }

        public List<DTSet> Split(DTAttribute attribute)
        {
            Dictionary<DTAttribute.DTValue, DTSet> newSets = new Dictionary<DTAttribute.DTValue, DTSet>();
            List<DTAttribute> newAttributes = new List<DTAttribute>();
            DTSet currentSet;
            List<DTSet> result = new List<DTSet>();
            
            foreach (DTAttribute remainingAttribute in AvailableAttributes)
            {
                if (!remainingAttribute.Equals(attribute))
                {
                    newAttributes.Add(remainingAttribute);
                }
            }

            foreach(DTAttribute.DTValue value in attribute.PossibleValues)
            {
                newSets[value] = new DTSet(newAttributes, value);
            }

            foreach (DTExample example in Examples)
            {
                currentSet = newSets[example.GetAttributeValue(attribute)];
                currentSet.Add(example);
            }

            foreach (DTSet completeSet in newSets.Values)
            {
                result.Add(completeSet);
            }

            return result;
        }

        public DTAttribute.DTValue BestLabel(DTAttribute labelSettings)
        {
            Dictionary<DTAttribute.DTValue, int> counts = new Dictionary<DTAttribute.DTValue, int>();
            int max = 0;
            DTAttribute.DTValue result = null;

            foreach(DTAttribute.DTValue value in labelSettings.PossibleValues)
            {
                counts[value] = 0;
            }
            foreach(DTExample example in Examples)
            {
                counts[example.Label] += 1;
            }
            foreach (DTAttribute.DTValue value in labelSettings.PossibleValues)
            {
                int x = counts[value];
                if (x > max)
                {
                    x = max;
                    result = value;
                }
            }

            return result;
        }
    }
}
