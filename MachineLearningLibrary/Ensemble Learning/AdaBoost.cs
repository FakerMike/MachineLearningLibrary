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

        
        public AdaBoost(int maxT)
        {
            double[] weights;
            double error, iTrainingError, iTestError, aTrainingError, aTestError;
            double adaWeight = 0.0;
            double tempWeight;
            int count;

            trees = new List<DecisionTree>();
            AdaWeights = new List<double>();

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

            Console.WriteLine("n,ITrainingError,ITestError,ATrainingAccuracy,ATestAccuracy");


            for (int n = 1; n < maxT; n++)
            {
                error = tree.ComputeAdaError();
                adaWeight = 0.5 * Math.Log((1-error)/error);
                trees.Add(tree);
                AdaWeights.Add(adaWeight);
                iTrainingError = 1-tree.CheckTrainingData();
                iTestError = 1-tree.RunTest();
                aTrainingError = ComputeTrainingError(tree.trainingData);
                aTestError = ComputeTrainingError(tree.testData);
                Console.WriteLine($"{n},{iTrainingError},{iTestError},{aTrainingError},{aTestError}");
                

                tree.DisposeData();

                //Console.WriteLine($"Tree {n} complete, error {error}, weight {adaWeight}");
                /*if (n % 25 == 0)
                {
                    Console.WriteLine($"Tree {n} complete");
                }*/


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

                //Console.WriteLine(tempWeight);

                for (int i = 0; i < count; i++)
                {
                    weights[i] = weights[i] / tempWeight;
                }

                tree = new DecisionTree(new DTHeuristic(DTHeuristic.Heuristics.ENTROPY), false);
                tree.SetupBank();
                tree.ReweightData(weights);
                tree.CreateWithID3(2);

            }

            error = tree.ComputeAdaError();
            adaWeight = 0.5 * Math.Log((1 - error) / error);
            trees.Add(tree);
            AdaWeights.Add(adaWeight);
            iTrainingError = 1 - tree.CheckTrainingData();
            iTestError = 1 - tree.RunTest();
            aTrainingError = ComputeTrainingError(tree.trainingData);
            aTestError = ComputeTrainingError(tree.testData);
            Console.WriteLine($"{maxT},{iTrainingError},{iTestError},{aTrainingError},{aTestError}");

        }

        public double ComputeTrainingError(DTSet trainingSet)
        {
            Dictionary<DTAttribute.DTValue, double> valueCounts;
            DTAttribute.DTValue newValue, guessValue = null;
            double max;

            int correct = 0;
            foreach (DTExample example in trainingSet.Examples)
            {
                max = 0;
                valueCounts = new Dictionary<DTAttribute.DTValue, double>();
                for (int i = 0; i<trees.Count; i++)
                {
                    newValue = trees[i].PredictValueRecursive(example, trees[i].root);
                    if (valueCounts.ContainsKey(newValue))
                    {
                        valueCounts[newValue] = valueCounts[newValue] + AdaWeights[i];
                    } else
                    {
                        valueCounts[newValue] = AdaWeights[i];
                    }
                }
                foreach (DTAttribute.DTValue key in valueCounts.Keys)
                {
                    if (valueCounts[key] > max)
                    {
                        max = valueCounts[key];
                        guessValue = key;
                    }
                }
                if (guessValue.Equals(example.Label)) correct++;
            }
            return (double)correct / trainingSet.Examples.Count;
        }



    }
}
