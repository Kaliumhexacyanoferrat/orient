using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers;
using ConvNetSharp.Volume.Double;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Core.Training.Double;
using ConvNetSharp.Volume;
using Orient.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ConvNetSharp.Core.Fluent;
using ConvNetSharp.Core.Training;
using NLog;

namespace Orient.Training
{

    public class NetworkTrainer
    {
        private Logger Log = LogManager.GetLogger("Trainer");

        public void Run(string model, string trainingSet, string testSet)
        {
            Log.Info("Loading model ...");
            
            var network = (File.Exists(model)) ? Network.FromFile(model) : Network.CreateNew();

            var batchSize = 10;

            var testInterval = 1;
            var testSize = 10;

            var saveInterval = 100;

            var trainer = new SgdTrainer<double>(network.Net)
            {
                LearningRate = 0.001,
                BatchSize = batchSize,
                // L2Decay = 0.001,
                Momentum = 0.5
            };

            var trainingData = TrainingSet.FromDirectory(Path.Combine(trainingSet, "rotated"), batchSize);
            var testData = TrainingSet.FromDirectory(Path.Combine(testSet, "rotated"), testSize);

            Log.Info("Training ...");

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
                        Log.Info($"{trainingData.Epoch}\t{run}\t{testBatch.TotalError:0.00}\t{testBatch.MinimumAngle:0.00}\t{testBatch.MaximumAngle:0.00}\t{trainer.Loss:0.00000}");
                    }
                }
            }
            while (!Console.KeyAvailable);

            Save(network, model);
        }

        private void WriteHeader()
        {
            Log.Info("");
            Log.Info("Epoch #\tRun #\tErr\tMin\tMax\tLoss");
            Log.Info("");
        }

        private void Save(Network network, string modelFile)
        {
            Log.Info("Saving model ...");
            network.Save(modelFile);

            WriteHeader();
        }

    }

}
