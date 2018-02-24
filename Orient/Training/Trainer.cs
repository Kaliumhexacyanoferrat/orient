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

        public void Run()
        {
            Console.WriteLine("Loading model ...");

            var network = Network.FromFile("default.json");

            var batchSize = 20;
            var testSize = 10;
            var saveInterval = 25;

            var trainer = new SgdTrainer<double>(network.Net)
            {
                LearningRate = 0.01,
                BatchSize = batchSize,
                L2Decay = 0.001,
                Momentum = 0.7
            };

            var trainingData = TrainingSet.FromDirectory(@"D:\Temp\Training\Input", batchSize);
            var testData = TrainingSet.FromDirectory(@"D:\Temp\Test\Input", testSize);

            Console.WriteLine("Training ...");

            var run = 0;

            do
            {
                if (++run % saveInterval == 0)
                {
                    Console.WriteLine("Saving model ...");
                    network.Save("default.json");
                }

                // train
                using (var batch = trainingData.GetBatch())
                {
                    trainer.Train(batch.InputVolume, batch.OutputVolume);
                }

                // and test
                using (var testBatch = testData.GetBatch())
                {
                    var result = network.Net.Forward(testBatch.InputVolume);

                    testBatch.SetResult(result);

                    // evaluate results
                    Console.WriteLine($"Epoch #{trainingData.Epoch} - Run #{run} - Avg: {testBatch.AverageError:0.00}, Min: {testBatch.MinimumError:0.00}, Max: {testBatch.MaximumError:0.00}, Loss: {trainer.Loss}");
                }
            }
            while (!Console.KeyAvailable);

            Console.WriteLine("Saving model ...");
            network.Save("default.json");
        }

    }

}
