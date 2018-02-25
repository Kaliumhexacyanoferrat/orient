using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers;
using ConvNetSharp.Volume.Double;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Core.Training.Double;
using ConvNetSharp.Volume;
using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ConvNetSharp.Core.Fluent;
using ConvNetSharp.Core.Training;

namespace Training
{

    public class Trainer
    {

        public void Run(string model, string trainingSet, string testSet)
        {
            Console.WriteLine("Loading model ...");
            
            var network = (File.Exists(model)) ? Network.FromFile(model) : Network.CreateNew();

            var batchSize = 30;

            var testInterval = 1;
            var testSize = 10;

            var saveInterval = 100;

            var trainer = new SgdTrainer<double>(network.Net)
            {
                LearningRate = 0.0025,
                BatchSize = batchSize,
                L2Decay = 0.001,
                Momentum = 0.5
            };

            var trainingData = TrainingSet.FromDirectory(Path.Combine(trainingSet, "rotated"), batchSize);
            var testData = TrainingSet.FromDirectory(Path.Combine(testSet, "rotated"), testSize);

            Console.WriteLine("Training ...");

            WriteHeader();

            var run = 0;

            do
            {
                if (++run % saveInterval == 0)
                {
                    Save(network, model);
                }

                // train
                using (var batch = trainingData.GetBatch())
                {
                    trainer.Train(batch.InputVolume, batch.OutputVolume);
                }

                if (run % testInterval == 0)
                {
                    using (var testBatch = testData.GetBatch())
                    {
                        // and test
                        var result = network.Net.Forward(testBatch.InputVolume);

                        testBatch.SetResult(result);

                        // evaluate results
                        Console.WriteLine($"{trainingData.Epoch}\t{run}\t{testBatch.AverageError:0.00}\t{testBatch.MinimumAngle:0.00}\t{testBatch.MaximumAngle:0.00}\t{trainer.Loss:0.00000}");
                    }
                }
            }
            while (!Console.KeyAvailable);

            Save(network, model);
        }

        private void WriteHeader()
        {
            Console.WriteLine("");
            Console.WriteLine("Epoch #\tRun #\tErr\tMin\tMax\tLoss");
            Console.WriteLine("");
        }

        private void Save(Network network, string modelFile)
        {
            Console.WriteLine("Saving model ...");
            network.Save(modelFile);

            WriteHeader();
        }

    }

}
