using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class AdaBoost
    {
        private List<DecisionTree> trees;
        private List<double> AdaWeights;
        private bool verbose;
        private int maxT;
        
        public AdaBoost(int maxT)
        {
            double[] weights;
            double error;
            double adaWeight = 0.0;
            double tempWeight;
            int count;

            trees = new List<DecisionTree>();
            AdaWeights = new List<double>();

            verbose = true;
            DecisionTree tree = new DecisionTree(new DTHeuristic(DTHeuristic.Heuristics.GINI), false);
            tree.SetupBank();

            count = tree.TrainingCount();
            weights = new double[count];
            for (int i = 0; i < count; i++)
            {
                weights[i] = 1.0 / count;
            }
            tree.ReweightData(weights);
            tree.CreateWithID3(2);



            for (int n = 1; n < maxT; n++)
            {
                error = tree.ComputeAdaError();
                adaWeight = 0.5 * Math.Log((1-error)/error);
                trees.Add(tree);
                AdaWeights.Add(adaWeight);
                Console.WriteLine($"Tree {n} complete, error {error}, weight {adaWeight}");

                /*
                int correctCount = 0;
                for (int i = 0; i < count; i++)
                {
                    if (tree.AdaCorrect[i]) correctCount++;
                }

                Console.WriteLine(correctCount);
                */

                tempWeight = 0.0;
                for (int i = 0; i < count; i++)
                {
                    if (tree.AdaCorrect[i])
                        weights[i] = weights[i] * Math.Exp(-adaWeight);
                    else
                        weights[i] = weights[i] * Math.Exp(adaWeight);
                    tempWeight += weights[i];
                }

                Console.WriteLine(tempWeight);

                for (int i = 0; i < count; i++)
                {
                    weights[i] = weights[i] / tempWeight;
                }

                tree = new DecisionTree(new DTHeuristic(DTHeuristic.Heuristics.ENTROPY), false);
                tree.SetupBank();
                tree.ReweightData(weights);
                tree.CreateWithID3(2);
            }

            trees.Add(tree);
            AdaWeights.Add(adaWeight);
        }




    }
}
