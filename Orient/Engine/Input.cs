using System;
using System.Collections.Generic;
using System.Drawing;
using ConvNetSharp.Volume.Double;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvNetSharp.Flow.Ops;
using ConvNetSharp.Volume;

namespace Engine
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
                var shape = new Shape(256, 256);
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
                    volume.Set(x, y, 0, index, Image.GetPixel(x, y).R / 255.0);
                }
            }
        }

        public void Dispose()
        {
            Image.Dispose();       
        }

    }

}
