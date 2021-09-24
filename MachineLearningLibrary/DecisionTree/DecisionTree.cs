using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DecisionTree
    {



        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            DTSet examples = ImportExamplesFile("filename");
            DTSettings settings = new DTSettings();

            DecisionTree tree = CreateWithID3(settings, examples);


            Console.Read();
        }


        public static DecisionTree CreateWithID3(DTSettings settings, DTSet examples)
        {
            //TODO: this
            return new DecisionTree();
        }


        private static DTSet ImportExamplesFile(string filename)
        {
            //TODO:  File import
            return null;
        }



        public class DecisionNode
        {
            private DTAttribute attribute;
            private bool terminal;
            private DTLabel label;
            private Dictionary<DTAttribute.DTValue, DecisionNode> nextNodes;

            public DecisionNode()
            {
                nextNodes = new Dictionary<DTAttribute.DTValue, DecisionNode>();
            }

            /// <summary>
            /// Returns the next node where attribute = value
            /// </summary>
            /// <param name="attribute"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public DecisionNode GetNext(DTAttribute.DTValue value)
            {
                return nextNodes[value];
            }

            public void AddNext(DTAttribute.DTValue value)
            {
                nextNodes[value] = new DecisionNode();
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

            public DTLabel GetLabel()
            {
                return label;
            }

        }
    }
}
