using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Core.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class Network
    {
        
        public Net<double> Net { get; private set; }

        public static Network CreateNew()
        {
            var net = new Net<double>();

            net.AddLayer(new InputLayer(256, 256, 3));

            // ToDo: Pad ConvLayers?

            net.AddLayer(new ConvLayer(32, 32, 64) { Stride = 24 });
            net.AddLayer(new ReluLayer());
            net.AddLayer(new PoolLayer(10, 10) { Stride = 10 });
            
            net.AddLayer(new FullyConnLayer(256));
            net.AddLayer(new ReluLayer());

            net.AddLayer(new FullyConnLayer(64));
            //net.AddLayer(new ReluLayer());

            /*net.AddLayer(new FullyConnLayer(256));
            net.AddLayer(new ReluLayer());        

            net.AddLayer(new FullyConnLayer(256));
            net.AddLayer(new ReluLayer());*/

            /*net.AddLayer(new FullyConnLayer(8));
            net.AddLayer(new ReluLayer());*/

            net.AddLayer(new FullyConnLayer(1));

            net.AddLayer(new RegressionLayer());
            
            return new Network(net);
        }

        public static Network FromFile(string fileName)
        {
            return new Network(SerializationExtensions.FromJson<double>(File.ReadAllText(fileName)));
        }

        private Network(Net<double> net)
        {
            Net = net;
        }

        public void Save(string fileName)
        {
            File.WriteAllText(fileName, Net.ToJson());
        }

    }

}
