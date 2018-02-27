using ConvNetSharp.Volume;
using Orient.Engine;
using System;
using System.IO;
using ConvNetSharp.Core.Training;
using NLog;

namespace Orient.Training
{

    public class NetworkTrainer
    {
        private Logger Log = LogManager.GetLogger("Trainer");

        public void Run(string model, string trainingSet, string testSet)
        {
#if GPU
            Log.Info("Enabling GPU mode ...");
            BuilderInstance<double>.Volume = new ConvNetSharp.Volume.GPU.Double.VolumeBuilder();
#endif

            Log.Info("Loading model ...");

            var network = (File.Exists(model)) ? Network.FromFile(model) : Network.CreateNew();

            var batchSize = 15;

            var testInterval = 10;
            var testSize = 10;

            var saveInterval = 50;

            var trainer = new AdamTrainer<double>(network.Net)
            {
                LearningRate = 0.001,
                Beta1 = 0.9,
                Beta2 = 0.999
            };
            /*new SgdTrainer<double>(network.Net)
            {
                LearningRate = 0.008,
                BatchSize = batchSize,
                L2Decay = 0.001,
                Momentum = 0.8
            };*/

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
                        Log.Info($"{trainingData.Epoch}\t\t{run}\t{testBatch.TotalError:0.00}\t{testBatch.MinimumAngle:0.00}\t{testBatch.MaximumAngle:0.00}\t{testBatch.MaxError:0.00}\t{trainer.Loss:0.00000}");
                    }
                }
            }
            while (!Console.KeyAvailable);

            Save(network, model);
        }

        private void WriteHeader()
        {
            Log.Info("");
            Log.Info("Epoch #\tRun #\tErr\tMin\tMax\tMaxErr\tLoss");
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
