using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DTAttribute
    {
        public enum DTAttributeType
        {
            STRING = 0,
            INT,
            FLOAT
        }

        public DTAttributeType Type { get; set; }
        public string Name { get; }
        public List<DTValue> PossibleValues { get; }

        public DTAttribute(string name, DTAttributeType type, IEnumerable<DTValue> values)
        {
            Name = name;
            Type = type;
            PossibleValues = new List<DTValue>(values);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(DTAttribute))
            {
                DTAttribute att = (DTAttribute)obj;
                if (att.Name == Name)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            string result = "{" + Name + ": ";
            foreach (DTValue value in PossibleValues)
            {
                result += value + ", ";
            }
            result += "}";
            return result;
        }

        public class DTValue
        {
            public DTAttributeType Type { get; }

            public string ValueString { get; }
            public int ValueInt { get; }
            public float ValueFloat { get; }

            // Constructors
            public DTValue(string value)
            {
                Type = DTAttributeType.STRING;
                ValueString = value;
            }
            public DTValue(int value)
            {
                Type = DTAttributeType.INT;
                ValueInt = value;
            }
            public DTValue(float value)
            {
                Type = DTAttributeType.FLOAT;
                ValueFloat = value;
            }
            public DTValue(DTAttributeType type, string toParse)
            {
                Type = type;
                switch (type) {
                    case DTAttributeType.FLOAT:
                        ValueFloat = float.Parse(toParse);
                        break;
                    case DTAttributeType.INT:
                        ValueInt = int.Parse(toParse);
                        break;
                    case DTAttributeType.STRING:
                        ValueString = toParse;
                        break;
                }
            }


            public override int GetHashCode()
            {
                if (Type == DTAttributeType.FLOAT)
                    return ValueFloat.GetHashCode();
                if (Type == DTAttributeType.INT)
                    return ValueInt.GetHashCode();
                if (Type == DTAttributeType.STRING)
                    return ValueString.GetHashCode();

                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == GetType())
                {
                    DTValue att = (DTValue)obj;
                    if (att.Type == Type)
                    {
                        if (Type == DTAttributeType.FLOAT)
                            if (att.ValueFloat == ValueFloat)
                                return true;
                        if (Type == DTAttributeType.INT)
                            if (att.ValueInt == ValueInt)
                                return true;
                        if (Type == DTAttributeType.STRING)
                            if (att.ValueString == ValueString)
                                return true;
                    }
                }
                return false;
            }

            public override string ToString()
            {
                if (Type == DTAttributeType.FLOAT)
                    return ValueFloat.ToString();
                if (Type == DTAttributeType.INT)
                    return ValueInt.ToString();
                if (Type == DTAttributeType.STRING)
                    return ValueString.ToString();

                return base.ToString();
            }
        }
    }
}
