using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class NeuralNetworks
    {
        public static Random rand = new Random();

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public static double SigmoidPrime(double x)
        {
            return Sigmoid(x) * (1 - Sigmoid(x));
        }

        public static void HW5()
        {
            double x0, x1, x2, z01, z11, z21, z02, z12, z22, y;
            double w011, w021, w111, w121, w211, w221, w012, w022, w112, w122, w212, w222, w013, w113, w213;
            double plz11, plz21, plz12, plz22, pls11, pls21, pls12, pls22, ply;
            double plw011, plw021, plw111, plw121, plw211, plw221, plw012, plw022, plw112, plw122, plw212, plw222, plw013, plw113, plw213;

            w011 = -1;
            w021 = 1;
            w111 = -2;
            w121 = 2;
            w211 = -3;
            w221 = 3;
            w012 = -1;
            w022 = 1;
            w112 = -2;
            w122 = 2;
            w212 = -3;
            w222 = 3;
            w013 = -1;
            w113 = 2;
            w213 = -1.5;



            x0 = 1;
            x1 = 1;
            x2 = 1;
            z01 = 1;
            z11 = Sigmoid(w011 * x0 + w111 * x1 + w211 * x2);
            z21 = Sigmoid(w021 * x0 + w121 * x1 + w221 * x2);
            z02 = 1;
            z12 = Sigmoid(w012 * z01 + w112 * z11 + w212 * z21);
            z22 = Sigmoid(w022 * z01 + w122 * z11 + w222 * z21);
            y = w013 * z02 + w113 * z12 + w213 * z22;

            Console.WriteLine("Manual calculations for written homework");
            Console.WriteLine("Forward pass");
            Console.WriteLine();
            Console.WriteLine($"x0 = {x0}");
            Console.WriteLine($"x1 = {x1}");
            Console.WriteLine($"x2 = {x2}");
            Console.WriteLine();
            Console.WriteLine($"z01 = {z01}");
            Console.WriteLine($"z11 = {z11}");
            Console.WriteLine($"z21 = {z21}");
            Console.WriteLine();
            Console.WriteLine($"z02 = {z02}");
            Console.WriteLine($"z12 = {z12}");
            Console.WriteLine($"z22 = {z22}");
            Console.WriteLine();
            Console.WriteLine($"y = {y}");




            ply = y - 1;
            plw013 = ply * z02;
            plw113 = ply * z12;
            plw213 = ply * z22;

            plz12 = ply * w113;
            plz22 = ply * w213;

            pls12 = plz12 * z12 * (1 - z12);
            pls22 = plz22 * z22 * (1 - z22);

            plw012 = pls12 * z01;
            plw112 = pls12 * z11;
            plw212 = pls12 * z21;
            plw022 = pls22 * z01;
            plw122 = pls22 * z11;
            plw222 = pls22 * z21;

            plz11 = pls12 * w112 + pls22 * w122;
            plz21 = pls12 * w212 + pls22 * w222;

            pls11 = plz11 * z11 * (1 - z11);
            pls21 = plz21 * z21 * (1 - z21);

            plw011 = pls11 * x0;
            plw111 = pls11 * x1;
            plw211 = pls11 * x2;
            plw021 = pls21 * x0;
            plw121 = pls21 * x1;
            plw221 = pls21 * x2;


            Console.WriteLine("Backpropagating");
            Console.WriteLine();
            Console.WriteLine($"ply = {ply}");
            Console.WriteLine($"plw013 = {plw013}");
            Console.WriteLine($"plw113 = {plw113}");
            Console.WriteLine($"plw213 = {plw213}");
            Console.WriteLine();
            Console.WriteLine($"plz12 = {plz12}");
            Console.WriteLine($"plz22 = {plz22}");
            Console.WriteLine($"pls12 = {pls12}");
            Console.WriteLine($"pls22 = {pls22}");
            Console.WriteLine($"plw012 = {plw012}");
            Console.WriteLine($"plw112 = {plw112}");
            Console.WriteLine($"plw212 = {plw212}");
            Console.WriteLine($"plw022 = {plw022}");
            Console.WriteLine($"plw122 = {plw122}");
            Console.WriteLine($"plw222 = {plw222}");
            Console.WriteLine();
            Console.WriteLine($"plz11 = {plz11}");
            Console.WriteLine($"plz21 = {plz21}");
            Console.WriteLine($"pls11 = {pls11}");
            Console.WriteLine($"pls21 = {pls21}");
            Console.WriteLine($"plw011 = {plw011}");
            Console.WriteLine($"plw111 = {plw111}");
            Console.WriteLine($"plw211 = {plw211}");
            Console.WriteLine($"plw021 = {plw021}");
            Console.WriteLine($"plw121 = {plw121}");
            Console.WriteLine($"plw221 = {plw221}");






            DataStructure structure = new DataStructure(3, 3);
            structure.AddInputPoint(new double[] { 1, 1, 1 });
            structure.AddWeight(w011, 0, 1, 1);
            structure.AddWeight(w111, 1, 1, 1);
            structure.AddWeight(w211, 2, 1, 1);
            structure.AddWeight(w021, 0, 2, 1);
            structure.AddWeight(w121, 1, 2, 1);
            structure.AddWeight(w221, 2, 2, 1);
            structure.AddWeight(w012, 0, 1, 2);
            structure.AddWeight(w112, 1, 1, 2);
            structure.AddWeight(w212, 2, 1, 2);
            structure.AddWeight(w022, 0, 2, 2);
            structure.AddWeight(w122, 1, 2, 2);
            structure.AddWeight(w222, 2, 2, 2);
            structure.AddWeight(w013, 0, 1, 3);
            structure.AddWeight(w113, 1, 1, 3);
            structure.AddWeight(w213, 2, 1, 3);

            structure.ForwardPass();

            Console.WriteLine("Automatic");
            Console.WriteLine("Forward pass");
            Console.WriteLine();
            Console.WriteLine($"x0 = {structure.z[0,0]}");
            Console.WriteLine($"x1 = {structure.z[1, 0]}");
            Console.WriteLine($"x2 = {structure.z[2, 0]}");
            Console.WriteLine();
            Console.WriteLine($"z01 = {structure.z[0, 1]}");
            Console.WriteLine($"z11 = {structure.z[1, 1]}");
            Console.WriteLine($"z21 = {structure.z[2, 1]}");
            Console.WriteLine();
            Console.WriteLine($"z02 = {structure.z[0, 2]}");
            Console.WriteLine($"z12 = {structure.z[1, 2]}");
            Console.WriteLine($"z22 = {structure.z[2, 2]}");
            Console.WriteLine();
            Console.WriteLine($"y = {structure.y}");


            structure.Backwards(1);

            Console.WriteLine("Backpropagating");
            Console.WriteLine();
            Console.WriteLine($"ply = {structure.ply}");
            Console.WriteLine($"plw013 = {structure.plw[0, 1, 3]}");
            Console.WriteLine($"plw113 = {structure.plw[1, 1, 3]}");
            Console.WriteLine($"plw213 = {structure.plw[2, 1, 3]}");
            Console.WriteLine();
            Console.WriteLine($"plz12 = {structure.plz[1, 2]}");
            Console.WriteLine($"plz22 = {structure.plz[2, 2]}");
            Console.WriteLine($"pls12 = {structure.pls[1, 2]}");
            Console.WriteLine($"pls22 = {structure.pls[2, 2]}");
            Console.WriteLine($"plw012 = {structure.plw[0, 1, 2]}");
            Console.WriteLine($"plw112 = {structure.plw[1, 1, 2]}");
            Console.WriteLine($"plw212 = {structure.plw[2, 1, 2]}");
            Console.WriteLine($"plw022 = {structure.plw[0, 2, 2]}");
            Console.WriteLine($"plw122 = {structure.plw[1, 2, 2]}");
            Console.WriteLine($"plw222 = {structure.plw[2, 2, 2]}");
            Console.WriteLine();
            Console.WriteLine($"plz11 = {structure.plz[1, 1]}");
            Console.WriteLine($"plz21 = {structure.plz[2, 1]}");
            Console.WriteLine($"pls11 = {structure.pls[1, 1]}");
            Console.WriteLine($"pls21 = {structure.pls[2, 1]}");
            Console.WriteLine($"plw011 = {structure.plw[0, 1, 1]}");
            Console.WriteLine($"plw111 = {structure.plw[1, 1, 1]}");
            Console.WriteLine($"plw211 = {structure.plw[2, 1, 1]}");
            Console.WriteLine($"plw021 = {structure.plw[0, 2, 1]}");
            Console.WriteLine($"plw121 = {structure.plw[1, 2, 1]}");
            Console.WriteLine($"plw221 = {structure.plw[2, 2, 1]}");




            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("BankNote");
            List<double[]> trainingData = DataImport.ImportExamples("Data/bank-note/train.csv", false);
            List<double[]> testData = DataImport.ImportExamples("Data/bank-note/test.csv", false);

            Console.WriteLine("Hidden layer width 5:");
            NeuralNetwork neuralNetwork = new NeuralNetwork(5, 3, trainingData, testData);
            //neuralNetwork.Train(20, 1, 5);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 10:");
            neuralNetwork = new NeuralNetwork(10, 3, trainingData, testData);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 25:");
            neuralNetwork = new NeuralNetwork(25, 3, trainingData, testData);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 50:");
            neuralNetwork = new NeuralNetwork(50, 3, trainingData, testData);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 100:");
            neuralNetwork = new NeuralNetwork(100, 3, trainingData, testData);
            neuralNetwork.Train(10, .05, 2);


            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("BankNote Bad Load");
            neuralNetwork = new NeuralNetwork(5, 3, trainingData, testData, false);
            //neuralNetwork.Train(20, 1, 5);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 10:");
            neuralNetwork = new NeuralNetwork(10, 3, trainingData, testData, false);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 25:");
            neuralNetwork = new NeuralNetwork(25, 3, trainingData, testData, false);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 50:");
            neuralNetwork = new NeuralNetwork(50, 3, trainingData, testData, false);
            neuralNetwork.Train(10, .1, 5);

            Console.WriteLine("Hidden layer width 100:");
            neuralNetwork = new NeuralNetwork(100, 3, trainingData, testData, false);
            neuralNetwork.Train(10, .05, 2);



        }

        // Code from https://stackoverflow.com/questions/218060/random-gaussian-variables
        public static double RandomNormal()
        {
            double u1 = 1 - rand.NextDouble();
            double u2 = 1 - rand.NextDouble();
            return Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }



        public class NeuralNetwork
        {
            public DataStructure structure;

            private int width;
            private int depth;

            private List<double[]> trainingData;
            private List<double[]> testData;

            public NeuralNetwork(int width, int depth, List<double[]> trainingData, List<double[]> testData, bool smartInit = true)
            {
                structure = new DataStructure(width, depth);

                if (smartInit)
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 1; j < width; j++)
                        {
                            for (int k = 1; k < depth; k++)
                            {
                                structure.AddWeight(RandomNormal(), i, j, k);
                            }
                        }
                        structure.AddWeight(RandomNormal(), i, 1, depth);
                    }
                } else
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 1; j < width; j++)
                        {
                            for (int k = 1; k < depth; k++)
                            {
                                structure.AddWeight(0, i, j, k);
                            }
                        }
                        structure.AddWeight(0, i, 1, depth);
                    }
                }

                this.width = width;
                this.depth = depth;

                this.trainingData = trainingData;
                this.testData = testData;
            }



            // TODO:  Train from data
            public void Train(int epochs, double gamma, double d)
            {
                int updateCounter = 100;
                int updatesTotal = 0;
                Console.WriteLine($"Width,Updates,TrainingError,TestError");
                for (int t = 0; t < epochs; t++) {
                    trainingData.Shuffle();
                    double learningRate = gamma / (1 + t * gamma / d);
                    foreach (double[] sample in trainingData)
                    {
                        double[] inputs = new double[4];
                        double label = sample[4];
                        for (int i = 0; i < 4; i++)
                        {
                            inputs[i] = sample[i];
                        }
                        structure.AddInputPoint(inputs);
                        structure.ForwardPass();
                        structure.Backwards(label);
                        structure.UpdateWeights(learningRate);
                        updateCounter--;
                        if (updateCounter == 1)
                        {
                            updateCounter = 100;
                            updatesTotal++;
                            Console.WriteLine($"{width},{100 * updatesTotal},{TrainingError()},{Test()}");
                        }
                    }
                    //Console.WriteLine($"Epoch {t}: training error {TrainingError()}, test error {Test()}");
                }

            }


            public double TrainingError()
            {
                double correct = 0;
                double total = trainingData.Count;
                foreach (double[] sample in trainingData)
                {
                    double[] inputs = new double[4];
                    double label = sample[4];
                    for (int i = 0; i < 4; i++)
                    {
                        inputs[i] = sample[i];
                    }
                    structure.AddInputPoint(inputs);
                    structure.ForwardPass();
                    if (structure.y * label >= 0) correct++;
                }
                return 1 - correct / total;
            }


            public double Test()
            {
                double correct = 0;
                double total = testData.Count;
                foreach (double[] sample in testData)
                {
                    double[] inputs = new double[4];
                    double label = sample[4];
                    for (int i = 0; i < 4; i++)
                    {
                        inputs[i] = sample[i];
                    }
                    structure.AddInputPoint(inputs);
                    structure.ForwardPass();
                    if (structure.y * label >= 0) correct++;
                }
                return 1 - correct / total;
            }

        }

        public class DataStructure
        {
            // Values of nodes
            public double[,] z { get; private set; } //We're using z00 as x0
            // Derivatives of loss with respect to nodes
            public double[,] plz { get; private set; }

            // Output
            public double y { get; private set; }
            // Derivate of loss with respect to output
            public double ply { get; private set; }

            // Weights
            public double[,,] w { get; private set; }
            // Derivative of loss with respect to weights
            public double[,,] plw { get; private set; }

            // Helper - derivative of sigmoids at the nodes
            public double[,] pls { get; private set; }

            private int width;
            private int depth;


            public DataStructure(int width, int depth)
            {
                z = new double[width, depth];
                plz = new double[width, depth];
                w = new double[width, width, depth + 1];  // Has a blank layer at x,x,0.
                plw = new double[width, width, depth + 1];
                pls = new double[width, depth];
                this.width = width;
                this.depth = depth;
            }

            public void AddInputPoint(double[] x)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    z[i, 0] = x[i];
                }
            }

            public void AddWeight(double weight, int indexFrom, int indexTo, int layerTo)
            {
                w[indexFrom, indexTo, layerTo] = weight;
            }

            public void ForwardPass()
            {
                for (int j = 1; j < depth; j++)
                {
                    z[0, j] = 1;
                    for (int i = 1; i < width; i++)
                    {
                        double s = 0;
                        for (int k = 0; k < width; k++)
                        {
                            s += w[k, i, j] * z[k, j - 1];
                        }
                        z[i, j] = Sigmoid(s);
                    }
                }
                y = 0;
                for (int i = 0; i < width; i++)
                {
                    y += z[i, depth - 1] * w[i, 1, depth];
                }
            }

            public void Backwards(double ystar)
            {
                ply = y - ystar; // Loss = 1/2(y - y*)^2

                // Top layer, only one node (y)
                // for depth = 7
                // y = w[0,1,7] * z[0,6] + w[1,1,7] * z[1,6] + ...
                for (int i = 0; i < width; i++)
                {
                    plw[i, 1, depth] = ply * z[i, depth - 1];
                }




                // Next layer, all nodes only go to the output node
                // First the z's (the output of the node).
                // Again we are linear in z, so this is just the weights
                // Then the s's (the input of the node).
                // This is non-linear, but fortunately has an easy derivative (and we already know sigma of s)
                for (int i = 0; i < width; i++)
                {
                    plz[i, depth - 1] = ply * w[i, 1, depth];
                    pls[i, depth - 1] = plz[i, depth - 1] * z[i, depth - 1] * (1 - z[i, depth - 1]);
                }

                // Now the w's at this layer.  There are a lot.
                // i = from
                // j = to
                // Note there is one more from than to (nobody hits the z[0,x] node, its constant)
                for (int i = 0; i < width; i++)
                {
                    for (int j = 1; j < width; j++)
                    {
                        plw[i, j, depth - 1] = pls[j, depth - 1] * z[i, depth - 2];
                    }
                }

                // For the rest of the layers, we hit all but one node above us
                // So a lot of routes to get to us
                // i = from
                // j = to
                // k = depth
                for (int k = depth - 2; k > 0; k--)
                {

                    // First the z and s
                    for (int i = 1; i < width; i++)
                    {
                        plz[i, k] = 0;
                        for (int j = 1; j < width; j++)
                        {
                            plz[i, k] += pls[j, k + 1] * w[i, j, k + 1];
                        }
                        pls[i, k] = plz[i, k] * z[i, k] * (1 - z[i, k]);
                    }

                    // Then all the w's
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 1; j < width; j++)
                        {
                            plw[i, j, k] = pls[j, k] * z[i, k - 1];
                        }
                    }
                }


            }

            public void UpdateWeights(double gamma)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 1; j < width; j++)
                    {
                        for (int k = 1; k < depth; k++)
                        {
                            w[i, j, k] -= gamma * plw[i, j, k];
                        }
                    }
                    w[i, 1, depth] -= gamma * plw[i, 1, depth];
                }
            }
        }
    }

    static class ShuffleExtension
    {
        // Code from https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = NeuralNetworks.rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
