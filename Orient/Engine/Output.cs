using ConvNetSharp.Volume;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orient.Engine
{

    public class Output
    {

        public double Angle { get; private set; }
                
        public Output(double angle)
        {
            Angle = angle;
        }

        public void WriteTo(Volume<double> volume, int index = 0)
        {
            volume.Set(index, Angle);
        }

    }

}
