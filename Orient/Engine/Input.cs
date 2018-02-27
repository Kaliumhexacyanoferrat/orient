using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvNetSharp.Flow.Ops;
using ConvNetSharp.Volume;

#if GPU
using ConvNetSharp.Volume.GPU.Double;
#else
using ConvNetSharp.Volume.Double;
#endif

namespace Orient.Engine
{

    public class Input : IDisposable
    {

        public Bitmap Image { get; set; }

        public static Input FromFile(string file)
        {
            return new Input()
            {
                Image = (Bitmap)Bitmap.FromFile(file)
            };
        }

        public Volume<double> Volume
        {
            get
            {
                var shape = new Shape(256, 256, 3);
                var data = new double[shape.TotalLength];

                var volume = BuilderInstance.Volume.From(data, shape);            
                WriteTo(volume);

                return volume;
            }
        }
        
        public void WriteTo(Volume<double> volume, int index = 0)
        {
            for (int y = 0; y < Image.Height; y++)
            {
                for (int x = 0; x < Image.Width; x++)
                {
                    var color = Image.GetPixel(x, y);

                    volume.Set(y, x, 0, index, Normalize(color.GetBrightness()));
                    volume.Set(y, x, 1, index, Normalize(color.GetHue() / 360.0));
                    volume.Set(y, x, 2, index, Normalize(color.GetSaturation()));
                }
            }
        }

        public void Dispose()
        {
            Image.Dispose();       
        }

        private double Normalize(double input)
        {
            return (input * 2.0) - 1;
        }

    }

}
