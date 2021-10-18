using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DecisionTree
    {
        /// <summary>
        /// MAIN ENTRY POINT FOR CODE
        /// </summary>
        public static void HW1Main(string[] args)
        {
            Console.WriteLine("Problem 2.2, a - b:");
            DecisionTree tree;
            for (int i = 0; i < 3; i++) {
                Console.WriteLine("Heuristic: " + (DTHeuristic.Heuristics)i);
                for (int j=1; j<7; j++)
                {
                    tree = new DecisionTree(new DTHeuristic((DTHeuristic.Heuristics)i), false);
                    tree.SetupCar();
                    tree.CreateWithID3(j);
                    tree.LoadCarTestData();
                    double accuracy = tree.RunTest(); 
                    Console.WriteLine("Max Depth: " + j + ", Training Accuracy: " + tree.CheckTrainingData() + ", Test Accuracy: " + accuracy);
                }
            }

            Console.WriteLine("Problem 2.3a:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Heuristic: " + (DTHeuristic.Heuristics)i);
                for (int j = 1; j < 17; j++)
                {
                    tree = new DecisionTree(new DTHeuristic((DTHeuristic.Heuristics)i), false);
                    tree.SetupBank();
                    tree.CreateWithID3(j);
                    double accuracy = tree.RunTest();
                    Console.WriteLine("Max Depth: " + j + ", Training Accuracy: " + tree.CheckTrainingData() + ", Test Accuracy: " + accuracy);
                }
            }


            Console.WriteLine("Problem 2.3b:");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Heuristic: " + (DTHeuristic.Heuristics)i);
                for (int j = 1; j < 17; j++)
                {
                    tree = new DecisionTree(new DTHeuristic((DTHeuristic.Heuristics)i), false);
                    tree.SetupBank();
                    tree.ReplaceUnknowns();
                    tree.CreateWithID3(j);
                    double accuracy = tree.RunTest();
                    Console.WriteLine("Max Depth: " + j + ", Training Accuracy: " + tree.CheckTrainingData() + ", Test Accuracy: " + accuracy);
                }
            }

            Console.Read();
        }

        private DTSet trainingData;
        private DTSet testData;
        private DTHeuristic heuristic;
        private List<DTAttribute> attributes;
        private DTAttribute label;
        private DTNode root;
        private bool verbose;
        public bool[] AdaCorrect { get; private set; }

        public DecisionTree(DTHeuristic heuristic, bool verbose)
        {
            this.heuristic = heuristic;
            this.verbose = verbose;
            attributes = new List<DTAttribute>();
        }

        public int TrainingCount()
        {
            return trainingData.Examples.Count;
        }

        public void ReweightData(double[] weights)
        {
            trainingData.ReweightData(weights);
        }

        public void SetupCar() {
            string name;
            DTAttribute.DTAttributeType type;
            List<DTAttribute.DTValue> values;

            name = "buying";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("vhigh"),
                new DTAttribute.DTValue("high"),
                new DTAttribute.DTValue("med"),
                new DTAttribute.DTValue("low")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "maint";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("vhigh"),
                new DTAttribute.DTValue("high"),
                new DTAttribute.DTValue("med"),
                new DTAttribute.DTValue("low")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "doors";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("2"),
                new DTAttribute.DTValue("3"),
                new DTAttribute.DTValue("4"),
                new DTAttribute.DTValue("5more")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "persons";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("2"),
                new DTAttribute.DTValue("4"),
                new DTAttribute.DTValue("more")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "lug_boot";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("small"),
                new DTAttribute.DTValue("med"),
                new DTAttribute.DTValue("big")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "safety";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("low"),
                new DTAttribute.DTValue("med"),
                new DTAttribute.DTValue("high")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "label";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("unacc"),
                new DTAttribute.DTValue("acc"),
                new DTAttribute.DTValue("good"),
                new DTAttribute.DTValue("vgood")
            };
            label = new DTAttribute(name, type, values);

            trainingData = ImportExamplesFile("Data/car/train.csv", attributes, label);

            root = new DTNode(null, trainingData.BestLabel(label));
        }

        public void CreateWithID3(int maxLevels)
        {
            ID3Recursion(trainingData, maxLevels, root);
        }
        public void CreateWithID3()
        {
            CreateWithID3(attributes.Count + 1);
        }

        private void ID3Recursion(DTSet set, int levels, DTNode current)
        {
            double initialError;
            double max = 0;
            double informationGain;
            List<DTSet> nextSets;

            // End conditions
            // No examples - terminate
            if (set.Examples.Count == 0)
            {
                if (verbose) Console.WriteLine("Terminal: No Examples");
                current.SetToTerminal();
                return;
            }


            initialError = heuristic.SingleSetError(set, label);
            // If no error, no levels, or no attributes left, terminate
            if (initialError == 0)
            {
                if (verbose) Console.WriteLine("Terminal: No Error");
                current.SetToTerminal();
                return;
            }
            if (levels == 0)
            {
                if (verbose) Console.WriteLine("Terminal: Max Level");
                current.SetToTerminal();
                return;
            }
            if (set.AvailableAttributes.Count == 0)
            {
                if (verbose) Console.WriteLine("Terminal: No Attributes");
                current.SetToTerminal();
                return;
            }


            DTAttribute best = set.AvailableAttributes[0];
            foreach(DTAttribute attribute in set.AvailableAttributes)
            {
                informationGain = initialError - heuristic.MultiSetError(set, attribute, label);
                if (informationGain > max) {
                    max = informationGain;
                    best = attribute;
                }
            }

            current.SetAttribute(best);
            nextSets = set.Split(best);

            foreach (DTSet next in nextSets)
            {
                DTAttribute.DTValue bestLabel = current.GetLabel();
                if (next.Examples.Count > 0)
                {
                    bestLabel = next.BestLabel(label);
                }
                if (verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("Splitting at Level " + levels);
                    Console.WriteLine("Initial Error: " + initialError);
                    Console.WriteLine("Information Gain: " + max);
                    Console.WriteLine("Attribute Selected: " + best);
                    Console.WriteLine("Splitting for Value: " + next.CreationValue);
                    Console.WriteLine("Total Examples of this Value: " + next.Examples.Count);
                    Console.WriteLine("Best Label: " + bestLabel);
                }
                DTNode nextNode = current.AddNext(next.CreationValue, bestLabel);
                ID3Recursion(next, levels - 1, nextNode);
            }
            
        }

        public bool IsCorrect(DTExample test)
        {
            DTAttribute.DTValue guess = PredictValueRecursive(test, root);
            if (guess.Equals(test.Label)) return true;
            return false;
        }

        public DTAttribute.DTValue PredictValueRecursive(DTExample example, DTNode current)
        {
            if (current.IsTerminal()) return current.GetLabel();
            DTNode next = current.GetNext(example.GetAttributeValue(current.GetAttribute()));
            return PredictValueRecursive(example, next);
        }


        public void LoadCarTestData()
        {
            testData = ImportExamplesFile("Data/car/test.csv", attributes, label);
        }

        public double CheckTrainingData()
        {
            double correct = 0;
            foreach (DTExample example in trainingData.Examples)
                if (IsCorrect(example)) correct+= example.Weight;
            return correct / trainingData.GetTotalWeight();
        }


        public double RunTest()
        {
            double correct = 0;
            foreach (DTExample example in testData.Examples)
                if (IsCorrect(example)) correct += example.Weight;
            return correct / trainingData.GetTotalWeight();
        }

        public double ComputeAdaError()
        {
            AdaCorrect = new bool[trainingData.Examples.Count];
            double incorrect = 0.0;
            DTExample example;
            for (int i = 0; i < trainingData.Examples.Count; i++)
            {
                example = trainingData.Examples[i];
                if (!IsCorrect(example))
                {
                    incorrect += example.Weight;
                    AdaCorrect[i] = false;
                } else
                {
                    AdaCorrect[i] = true;
                }

            }                
            return incorrect;
        }



        public void SetupBank()
        {
            string name;
            DTAttribute.DTAttributeType type;
            List<DTAttribute.DTValue> values;

            name = "age";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "job";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("admin."),
                new DTAttribute.DTValue("unknown"),
                new DTAttribute.DTValue("unemployed"),
                new DTAttribute.DTValue("management"),
                new DTAttribute.DTValue("housemaid"),
                new DTAttribute.DTValue("entrepreneur"),
                new DTAttribute.DTValue("student"),
                new DTAttribute.DTValue("blue-collar"),
                new DTAttribute.DTValue("self-employed"),
                new DTAttribute.DTValue("retired"),
                new DTAttribute.DTValue("technician"),
                new DTAttribute.DTValue("services")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "marital status";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("married"),
                new DTAttribute.DTValue("divorced"),
                new DTAttribute.DTValue("single")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "education";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("unknown"),
                new DTAttribute.DTValue("secondary"),
                new DTAttribute.DTValue("primary"),
                new DTAttribute.DTValue("tertiary")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "default";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("yes"),
                new DTAttribute.DTValue("no")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "balance";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "housing";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("yes"),
                new DTAttribute.DTValue("no")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "loan";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("yes"),
                new DTAttribute.DTValue("no")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "contact";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("unknown"),
                new DTAttribute.DTValue("telephone"),
                new DTAttribute.DTValue("cellular")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "day";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "month";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("jan"),
                new DTAttribute.DTValue("feb"),
                new DTAttribute.DTValue("mar"),
                new DTAttribute.DTValue("apr"),
                new DTAttribute.DTValue("may"),
                new DTAttribute.DTValue("jun"),
                new DTAttribute.DTValue("jul"),
                new DTAttribute.DTValue("aug"),
                new DTAttribute.DTValue("sep"),
                new DTAttribute.DTValue("oct"),
                new DTAttribute.DTValue("nov"),
                new DTAttribute.DTValue("dec")
            };
            attributes.Add(new DTAttribute(name, type, values));

            name = "duration";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "campaign";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "pdays";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "previous";
            type = DTAttribute.DTAttributeType.INT;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("bigger"),
                new DTAttribute.DTValue("not"),
            }; ;
            attributes.Add(new DTAttribute(name, type, values));

            name = "poutcome";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("unknown"),
                new DTAttribute.DTValue("other"),
                new DTAttribute.DTValue("failure"),
                new DTAttribute.DTValue("success")
            };
            attributes.Add(new DTAttribute(name, type, values));
            
            name = "label";
            type = DTAttribute.DTAttributeType.STRING;
            values = new List<DTAttribute.DTValue>
            {
                new DTAttribute.DTValue("yes"),
                new DTAttribute.DTValue("no")
            };
            label = new DTAttribute(name, type, values);

            trainingData = ImportExamplesFile("Data/bank/train.csv", attributes, label);
            testData = ImportExamplesFile("Data/bank/test.csv", attributes, label);

            IntsToMedians();


            root = new DTNode(null, trainingData.BestLabel(label));
        }



        private void IntsToMedians()
        {
            int middle = trainingData.Examples.Count / 2;

            int[] ages = new int[trainingData.Examples.Count];
            int[] balances = new int[trainingData.Examples.Count];
            int[] days = new int[trainingData.Examples.Count];
            int[] durations = new int[trainingData.Examples.Count];
            int[] campaigns = new int[trainingData.Examples.Count];
            int[] pdayss = new int[trainingData.Examples.Count];
            int[] previouss = new int[trainingData.Examples.Count];

            for (int i = 0; i < trainingData.Examples.Count; i++)
            {
                ages[i] = trainingData.Examples[i].GetAttributeValue(attributes[0]).ValueInt;
                balances[i] = trainingData.Examples[i].GetAttributeValue(attributes[5]).ValueInt;
                days[i] = trainingData.Examples[i].GetAttributeValue(attributes[9]).ValueInt;
                durations[i] = trainingData.Examples[i].GetAttributeValue(attributes[11]).ValueInt;
                campaigns[i] = trainingData.Examples[i].GetAttributeValue(attributes[12]).ValueInt;
                pdayss[i] = trainingData.Examples[i].GetAttributeValue(attributes[13]).ValueInt;
                previouss[i] = trainingData.Examples[i].GetAttributeValue(attributes[14]).ValueInt;
            }

            Array.Sort(ages);
            Array.Sort(balances);
            Array.Sort(days);
            Array.Sort(durations);
            Array.Sort(campaigns);
            Array.Sort(pdayss);
            Array.Sort(previouss);

            for (int i = 0; i < trainingData.Examples.Count; i++)
            {
                UpdateValue(trainingData.Examples[i], 0, ages[middle]);
                UpdateValue(trainingData.Examples[i], 5, balances[middle]);
                UpdateValue(trainingData.Examples[i], 9, days[middle]);
                UpdateValue(trainingData.Examples[i], 11, durations[middle]);
                UpdateValue(trainingData.Examples[i], 12, campaigns[middle]);
                UpdateValue(trainingData.Examples[i], 13, pdayss[middle]);
                UpdateValue(trainingData.Examples[i], 14, previouss[middle]);
                UpdateValue(testData.Examples[i], 0, ages[middle]);
                UpdateValue(testData.Examples[i], 5, balances[middle]);
                UpdateValue(testData.Examples[i], 9, days[middle]);
                UpdateValue(testData.Examples[i], 11, durations[middle]);
                UpdateValue(testData.Examples[i], 12, campaigns[middle]);
                UpdateValue(testData.Examples[i], 13, pdayss[middle]);
                UpdateValue(testData.Examples[i], 14, previouss[middle]);
            }

            attributes[0].Type = DTAttribute.DTAttributeType.STRING;
            attributes[5].Type = DTAttribute.DTAttributeType.STRING;
            attributes[9].Type = DTAttribute.DTAttributeType.STRING;
            attributes[11].Type = DTAttribute.DTAttributeType.STRING;
            attributes[12].Type = DTAttribute.DTAttributeType.STRING;
            attributes[13].Type = DTAttribute.DTAttributeType.STRING;
            attributes[14].Type = DTAttribute.DTAttributeType.STRING;
        }

        private void UpdateValue(DTExample example, int index, int median)
        {
            if (example.GetAttributeValue(attributes[index]).ValueInt > median)
                example.SetAttributeValue(attributes[index], new DTAttribute.DTValue("bigger"));
            else
                example.SetAttributeValue(attributes[index], new DTAttribute.DTValue("not"));
        }


        private void ReplaceUnknowns()
        {
            string[] jobs = new string[trainingData.Examples.Count];
            string[] educations = new string[trainingData.Examples.Count];
            string[] contacts = new string[trainingData.Examples.Count];
            string[] poutcomes = new string[trainingData.Examples.Count];

            for (int i = 0; i < trainingData.Examples.Count; i++)
            {
                jobs[i] = trainingData.Examples[i].GetAttributeValue(attributes[1]).ValueString;
                educations[i] = trainingData.Examples[i].GetAttributeValue(attributes[3]).ValueString;
                contacts[i] = trainingData.Examples[i].GetAttributeValue(attributes[8]).ValueString;
                poutcomes[i] = trainingData.Examples[i].GetAttributeValue(attributes[15]).ValueString;
            }

            string newJob = BestReplacement(attributes[1], jobs);
            string newEducation = BestReplacement(attributes[3], educations);
            string newContact = BestReplacement(attributes[8], contacts);
            string newPOutcome = BestReplacement(attributes[15], poutcomes);

            foreach (DTExample example in trainingData.Examples)
            {
                if(example.GetAttributeValue(attributes[1]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[1], new DTAttribute.DTValue(newJob));
                if (example.GetAttributeValue(attributes[3]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[3], new DTAttribute.DTValue(newEducation));
                if (example.GetAttributeValue(attributes[8]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[8], new DTAttribute.DTValue(newContact));
                if (example.GetAttributeValue(attributes[15]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[15], new DTAttribute.DTValue(newPOutcome));
            }

            foreach (DTExample example in testData.Examples)
            {
                if (example.GetAttributeValue(attributes[1]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[1], new DTAttribute.DTValue(newJob));
                if (example.GetAttributeValue(attributes[3]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[3], new DTAttribute.DTValue(newEducation));
                if (example.GetAttributeValue(attributes[8]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[8], new DTAttribute.DTValue(newContact));
                if (example.GetAttributeValue(attributes[15]).ValueString == "unknown")
                    example.SetAttributeValue(attributes[15], new DTAttribute.DTValue(newPOutcome));
            }
        }

        public string BestReplacement(DTAttribute attribute, string[] strings)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            int max = 0;
            string result = "";

            foreach (DTAttribute.DTValue value in attribute.PossibleValues)
            {
                counts[value.ValueString] = 0;
            }
            foreach (string s in strings)
            {
                counts[s] += 1;
            }
            foreach (DTAttribute.DTValue value in attribute.PossibleValues)
            {
                int x = counts[value.ValueString];
                if (x > max && value.ValueString != "unknown")
                {
                    x = max;
                    result = value.ValueString;
                }
            }

            return result;
        }



















        private static DTSet ImportExamplesFile(string filename, List<DTAttribute> attributes, DTAttribute label)
        {
            DTSet result = new DTSet(attributes);

            // Code taken from https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
            using (var reader = new StreamReader(filename))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] rawValues = line.Split(',');
                    Dictionary<DTAttribute, DTAttribute.DTValue> values = new Dictionary<DTAttribute, DTAttribute.DTValue>();
                    for (int i = 0; i < attributes.Count; i++)
                    {
                        values[attributes[i]] = new DTAttribute.DTValue(attributes[i].Type, rawValues[i]);
                    }
                    DTExample example = new DTExample(1, values, new DTAttribute.DTValue(label.Type, rawValues[attributes.Count]));
                    result.Add(example);
                }
            }

            return result;
        }



        public class DTNode
        {
            private DTAttribute attribute;
            private bool terminal;
            private DTAttribute.DTValue label;
            private Dictionary<DTAttribute.DTValue, DTNode> nextNodes;
            public DTNode Parent { get; }

            public DTNode(DTNode parent, DTAttribute.DTValue bestLabel)
            {
                Parent = parent;
                label = bestLabel;
                nextNodes = new Dictionary<DTAttribute.DTValue, DTNode>();
            }

            /// <summary>
            /// Returns the next node where attribute = value
            /// </summary>
            /// <param name="attribute"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public DTNode GetNext(DTAttribute.DTValue value)
            {
                return nextNodes[value];
            }

            public DTNode AddNext(DTAttribute.DTValue value, DTAttribute.DTValue bestLabel)
            {
                nextNodes[value] = new DTNode(this, bestLabel);
                return nextNodes[value];
            }

            /// <summary>
            /// Set the attribute for this node to split on.
            /// </summary>
            /// <param name="attribute"></param>
            public void SetAttribute(DTAttribute attribute)
            {
                terminal = false;
                this.attribute = attribute;
            }

            /// <summary>
            /// Sets the node to be terminal.
            /// </summary>
            /// <param name="attribute"></param>
            public void SetToTerminal()
            {
                terminal = true;
            }

            public DTAttribute GetAttribute()
            {
                return attribute;
            }

            public bool IsTerminal()
            {
                return terminal;
            }

            public DTAttribute.DTValue GetLabel()
            {
                return label;
            }

        }
    }
}
