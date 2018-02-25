using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training
{

    public class TrainingSet
    {
        private int _Current;

        private List<TrainingObject> _TrainingObjects;

        public int Epoch { get; private set; }

        public int BatchSize { get; private set; }

        public static TrainingSet FromDirectory(string path, int batchSize)
        {
            var entries = new List<TrainingObject>();

            foreach (var file in Directory.EnumerateFiles(path, "*.JPG", SearchOption.AllDirectories))
            {
                var info = new FileInfo(file);
                entries.Add(new TrainingObject(file, Double.Parse(info.Directory.Name)));
            };

            entries.Shuffle();

            return new TrainingSet()
            {
                Epoch = 1,
                BatchSize = batchSize,
                _TrainingObjects = entries
            };
        }

        public TrainingBatch GetBatch()
        {
            var inputShape = new Shape(256, 256, 3, BatchSize);
            var inputData = new double[inputShape.TotalLength];

            var outputShape = new Shape(1, 1, 1, BatchSize);
            var outputData = new double[outputShape.TotalLength];

            var inputVolume = BuilderInstance.Volume.From(inputData, inputShape);
            var outputVolume = BuilderInstance.Volume.From(outputData, outputShape);

            for (int i = 0; i < BatchSize; i++)
            {
                var obj = _TrainingObjects[_Current];

                using (var input = obj.LoadInput())
                {
                    input.WriteTo(inputVolume, i);
                }

                obj.GetOutput().WriteTo(outputVolume, i);

                if (++_Current >= _TrainingObjects.Count)
                {
                    _TrainingObjects.Shuffle();
                    _Current = 0;

                    Epoch++;                
                }
            }

            return new TrainingBatch()
            {
                InputVolume = inputVolume,
                OutputVolume = outputVolume
            };
        }

    }

}
