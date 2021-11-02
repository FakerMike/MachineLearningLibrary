using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DTExample
    {
        public bool training { get; }
        public Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues { get; private set; }
        public double Weight { get; private set; }
        public DTAttribute.DTValue Label { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DTExample(double weight, Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues)
        {
            this.attributeValues = attributeValues;
            Weight = weight;
            training = false;
        }
        public DTExample(double weight, Dictionary<DTAttribute, DTAttribute.DTValue> attributeValues, DTAttribute.DTValue label)
        {
            this.attributeValues = attributeValues;
            Weight = weight;
            training = true;
            Label = label;
        }

        public DTExample(double newWeight, DTExample example)
        {
            attributeValues = example.attributeValues;
            Weight = newWeight;
            training = example.training;
            Label = example.Label;
        }

        public void SetWeight(double weight)
        {
            Weight = weight;
        }

        public bool IsTraining() { return training; }


        public DTAttribute.DTValue GetAttributeValue(DTAttribute attribute)
        {
            return attributeValues[attribute];
        }

        public void SetAttributeValue(DTAttribute attribute, DTAttribute.DTValue value)
        {
            attributeValues[attribute] = value;
        }

    }
}
