using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training;

namespace CommandLine
{

    class Program
    {

        static void Main(string[] args)
        {
            var trainingSet = @"D:\Temp\Training";
            var testSet = @"D:\Temp\Test";

            Console.WriteLine("Generating training data ...");

            new TrainingData().Generate(trainingSet);

            new TrainingData().Generate(testSet);

            new Trainer().Run("default.json", trainingSet, testSet);
        }

    }

}
