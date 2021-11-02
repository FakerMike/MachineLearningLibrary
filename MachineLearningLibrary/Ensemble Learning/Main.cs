using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class EnsembleLearningProgram
    {
        public static void HW2Main(string[] args)
        {
            Console.WriteLine("Running AdaBoost with 500 trees...");
            AdaBoost booster = new AdaBoost(500);
            Console.WriteLine("Running Bagging with 500 trees...");
            Bagging bagger = new Bagging(500);
            Console.WriteLine("Running Random Forest with 500 trees, 2 choices per branch...");
            RandomForest forest2 = new RandomForest(500, 2);
            Console.WriteLine("Running Random Forest with 500 trees, 4 choices per branch...");
            RandomForest forest4 = new RandomForest(500, 4);
            Console.WriteLine("Running Random Forest with 500 trees, 6 choices per branch...");
            RandomForest forest6 = new RandomForest(500, 6);
            Console.Read();
            Console.Read();
            Console.Read();
        }
    }
}
