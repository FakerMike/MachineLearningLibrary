using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace MachineLearningLibrary
{
    class DataImport
    {






        public static List<double[]> ImportExamples(string filename, bool enrich = true)
        {
            List<double[]> result = new List<double[]>();

            // Code taken from https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] rawValues = line.Split(',');
                    if (enrich) { 
                    double[] parsedValues = new double[rawValues.Length + 1];
                    parsedValues[0] = 1;  // Enrich
                    for (int i = 0; i < rawValues.Length; i++)
                    {
                        parsedValues[i + 1] = double.Parse(rawValues[i]);
                    }
                    if (parsedValues[rawValues.Length] > 0) parsedValues[rawValues.Length] = 1;
                    else parsedValues[rawValues.Length] = -1;
                    result.Add(parsedValues);
                    } else
                    {
                        double[] parsedValues = new double[rawValues.Length];
                        for (int i = 0; i < rawValues.Length; i++)
                        {
                            parsedValues[i] = double.Parse(rawValues[i]);
                        }
                        if (parsedValues[rawValues.Length - 1] > 0) parsedValues[rawValues.Length - 1] = 1;
                        else parsedValues[rawValues.Length - 1] = -1;
                        result.Add(parsedValues);
                    }
                }
            }

            return result;
        }

    }



}
