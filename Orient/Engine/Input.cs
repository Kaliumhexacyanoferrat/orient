using System;
using System.Collections.Generic;
using System.Drawing;
using ConvNetSharp.Volume;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class Input : IDisposable
    {
        private VolumeStorage<double> _Storage;

        public Bitmap Image { get; set; }

        public Shape Shape
        {
            get { return new Shape(256, 256); }
        }

        public VolumeStorage<double> Storage
        {
            get
            {
                if (_Storage == null)
                {
                    var storage = new NcwhVolumeStorage<double>(Shape);

                    for (int y = 0; y < Image.Height; y++)
                    {
                        for (int x = 0; x < Image.Width; x++)
                        {
                            storage.Set(x, y, Image.GetPixel(x, y).R / 255.0);
                        }
                    }

                    _Storage = storage;
                }

                return _Storage;
            }
        }

        public void Dispose()
        {
            Image.Dispose();
        }

        public static Input FromFile(string file)
        {
            return new Input()
            {
                Image = (Bitmap)Bitmap.FromFile(file)
            };
        }

    }

}
