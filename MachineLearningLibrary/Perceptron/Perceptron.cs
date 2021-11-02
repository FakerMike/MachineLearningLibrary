using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    public class Perceptron
    {

        public static void HW3Main(string[] args) {
            List<double[]> trainingData = DataImport.ImportExamples("Data/bank-note/train.csv");
            List<double[]> testData = DataImport.ImportExamples("Data/bank-note/test.csv");


            Perceptron perceptron = new Perceptron(trainingData, 10, 1, Mode.Regular);
            Console.WriteLine(string.Join(", ",perceptron.w));
            Console.WriteLine(perceptron.TestError(testData));

            perceptron = new Perceptron(trainingData, 10, 1, Mode.Average);
            Console.WriteLine(string.Join(", ", perceptron.a));
            Console.WriteLine(perceptron.TestError(testData));

            perceptron = new Perceptron(trainingData, 10, 1, Mode.Voted);
            Console.WriteLine(string.Join(", ", perceptron.w));
            Console.WriteLine(perceptron.TestError(testData));
            Console.WriteLine(perceptron.voters.Count);
            Console.WriteLine(string.Join(", ", perceptron.votes));


            Console.Read();
        }

        public enum Mode
        {
            Regular = 0,
            Voted,
            Average
        }

        private List<double[]> trainingData;
        private int epochs;
        public double[] w { get; private set; }
        private double r;
        public double[] a { get; private set; }
        private Mode mode;
        public List<double[]> voters { get; private set; }
        public List<int> votes { get; private set; }


        public Perceptron(List<double[]> trainingData, int epochs, double r, Mode mode)
        {
            this.trainingData = trainingData;
            this.epochs = epochs;
            w = new double[trainingData[0].Length - 1];
            a = new double[trainingData[0].Length - 1];
            this.r = r;
            this.mode = mode;
            voters = new List<double[]>();
            voters.Add(w);
            votes = new List<int>();
            votes.Add(0);

            Train();
        }

        public double TestError(List<double[]> testData)
        {
            int errorCount = 0;
                foreach (double[] x in testData)
                {
                    double prediction = Predict(x, mode);
                    if (x[x.Length - 1] == prediction)
                    {
                        continue;
                    }
                errorCount++;
                }
            return (double)errorCount / testData.Count;
        }

        private void Train()
        {  
            for (int i = 0; i < epochs; i++)
            {
                foreach (double[] x in trainingData)
                {
                    double prediction = Predict(x, Mode.Regular);
                    if (x[x.Length - 1] == prediction)
                    {
                        if (mode == Mode.Voted)
                        {
                            votes[votes.Count - 1]++;
                        }
                        if (mode == Mode.Average) a = AddWeighted(a, w, 1, 1);
                        continue;
                    }
                    w = AddWeighted(w, x, r, -prediction);
                    if (mode == Mode.Voted)
                    {
                        voters.Add(Copy(w));
                        votes.Add(1);
                        //Console.WriteLine(string.Join(", ", w));
                    }
                    if (mode == Mode.Average) a = AddWeighted(a, w, 1, 1);

                }
            }
        }

        private double Predict(double[] x, Mode predictionMode)
        {
            switch (predictionMode)
            {
                case Mode.Regular:
                    if (Dot(w, x) >= 0) return 1;
                    break;
                case Mode.Average:
                    if (Dot(a, x) >= 0) return 1;
                    break;
                case Mode.Voted:
                    int net = 0;
                    for (int i = 0; i < voters.Count; i++)
                    {
                        if (Dot(voters[i], x) >= 0) net += votes[i];
                        else net -= votes[i];
                    }
                    if (net >= 0) return 1;
                    break;
            }

            return -1;
        }

        public static double Dot(double[] w, double[] x)
        {
            // Assumes w is shorter than x (doesn't include label!)
            double result = 0;
            for (int i = 0; i < w.Length; i++) result += w[i] * x[i];
            return result;
        }

        public static double[] AddWeighted(double[] w, double[] x, double r, double y)
        {
            // Assumes w is shorter than x (doesn't include label!)
            double[] result = new double[w.Length];
            for (int i = 0; i < w.Length; i++) result[i] = w[i] + r * y * x[i];
            return result;
        }

        public static double[] Copy(double[] x)
        {
            double[] result = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                result[i] = x[i];
            }
            return result;
        }

    }
}

