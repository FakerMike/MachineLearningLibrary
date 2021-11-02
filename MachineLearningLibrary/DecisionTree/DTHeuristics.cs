using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningLibrary
{
    class DTHeuristic
    {
        public enum Heuristics
        {
            ENTROPY = 0,
            MAJORITY_ERROR,
            GINI
        }
        private delegate double PurityCalculation(double[] p, double num);
        private PurityCalculation purity;


        public DTHeuristic(Heuristics heuristic)
        {
            if (heuristic == Heuristics.ENTROPY) purity = EntropyCalculation;
            if (heuristic == Heuristics.GINI) purity = GiniCalculation;
            if (heuristic == Heuristics.MAJORITY_ERROR) purity = MECalculation;
        }

        public double MultiSetError(DTSet set, DTAttribute attribute, DTAttribute labelSettings)
        {
            double num = set.GetTotalWeight();
            List<DTSet> sets = set.Split(attribute);
            double entropy = 0;

            foreach (DTSet newSet in sets)
            {
                entropy += newSet.GetTotalWeight() / num * SingleSetError(newSet, labelSettings);
            }

            return entropy;
        }

        public double SingleSetError(DTSet set, DTAttribute labelSettings)
        {
            double[] p = new double[labelSettings.PossibleValues.Count];
            double num = set.GetTotalWeight();
            double entropy;

            Dictionary<DTAttribute.DTValue, int> index = new Dictionary<DTAttribute.DTValue, int>();

            int temp = 0;
            foreach(DTAttribute.DTValue value in labelSettings.PossibleValues)
            {
                index[value] = temp++;
            }

            foreach(DTExample example in set.Examples)
            {
                p[index[example.Label]] += example.Weight;
            }

            entropy = purity(p, num);

            return entropy;
        }

        private static double EntropyCalculation(double[] p, double num)
        {
            double result = 0;
            double x;



            for (int i = 0; i < p.Length; i++)
            {
                x = p[i] / num;
                if (x > 0) result -= x * Math.Log(x, 2);
            }

            return result;
        }

        private static double GiniCalculation(double[] p, double num)
        {
            double result = 1;
            double x;

            for (int i = 0; i < p.Length; i++)
            {
                x = p[i] / num;
                result -= x * x;
            }

            return result;
        }

        private static double MECalculation(double[] p, double num)
        {
            return (num - p.Max())/num;
        }


    }
}
