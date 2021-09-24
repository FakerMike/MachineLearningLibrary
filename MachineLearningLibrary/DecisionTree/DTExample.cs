using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DTExample
    {
        private bool training;
        private Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues;
        public float Weight { get; }
        public DTLabel Label { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DTExample(float weight, Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues)
        {
            this.attributeValues = attributeValues;
            Weight = weight;
            training = false;
        }
        public DTExample(float weight, Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues, DTLabel label)
        {
            this.attributeValues = attributeValues;
            Weight = weight;
            training = true;
            Label = label;
        }


        public bool IsTraining() { return training; }


        public DTAttribute.DTValue GetAttributeValue(DTAttribute attribute)
        {
            return attributeValues[attribute];
        }

    }
}
