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

namespace Training
{

    public class Trainer
    {

        public void Run()
        {
            var input = Input.FromFile(@"D:\Temp\Training\Input\5\IMG_1272.JPG");

            var volume = BuilderInstance.Volume.Build(input.Storage, input.Shape);

            var network = FluentNet<double>.Create(256, 256, 1)
                .Conv(32, 32, 8).Stride(1).Pad(2)
                .Relu()
                .Pool(8, 8).Stride(2)
                .Conv(4, 4, 16).Stride(1).Pad(2)
                .Relu()
                .Pool(2, 2).Stride(3)
                .FullyConn(11)
                .Softmax(11)
                .Build();

            var probBefore = network.Forward(volume);
            Console.WriteLine(probBefore.Get(9));

            var temp = new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0  };

            var trainer = new SgdTrainer(network) { LearningRate = 0.01, L2Decay = 0.001 };
            trainer.Train(volume, BuilderInstance.Volume.From(temp, new Shape(1, 1, 11, 1)));

            var probAfter = network.Forward(volume);
            Console.WriteLine(probAfter.Get(9));
        }

    }

}
