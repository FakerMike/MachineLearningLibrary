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

        /// <summary>
        /// Constructor
        /// </summary>
        public DTSet(IEnumerable<DTAttribute> attributes)
        {
            AvailableAttributes = new List<DTAttribute>(attributes);
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
                if (remainingAttribute != attribute)
                {
                    newAttributes.Add(remainingAttribute);
                }
            }

            foreach(DTAttribute.DTValue value in attribute.PossibleValues)
            {
                newSets[value] = new DTSet(newAttributes);
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
    }
}
