using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class Bagging
    {
        private List<DecisionTree> trees;

        public Bagging(int maxT)
        {
            double iTrainingError, iTestError, aTrainingError, aTestError;
            int count;

            trees = new List<DecisionTree>();

            DecisionTree tree = new DecisionTree(new DTHeuristic(DTHeuristic.Heuristics.GINI), false);
            tree.SetupBank();

            count = tree.TrainingCount();

            tree.BagTrainingData(count);
            tree.CreateWithID3();
            Console.WriteLine("n,ITrainingError,ITestError,ATrainingAccuracy,ATestAccuracy");

            for (int n = 1; n < maxT; n++)
            {
                trees.Add(tree);
                iTrainingError = 1 - tree.CheckTrainingData();
                iTestError = 1 - tree.RunTest();
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

                //Console.WriteLine(tempWeight);

                tree = new DecisionTree(new DTHeuristic(DTHeuristic.Heuristics.ENTROPY), false);
                tree.SetupBank();
                tree.BagTrainingData(count);
                tree.CreateWithID3();

            }

            trees.Add(tree);
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
                for (int i = 0; i < trees.Count; i++)
                {
                    newValue = trees[i].PredictValueRecursive(example, trees[i].root);
                    if (valueCounts.ContainsKey(newValue))
                    {
                        valueCounts[newValue] = valueCounts[newValue] + 1;
                    }
                    else
                    {
                        valueCounts[newValue] = 1;
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
