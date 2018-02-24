using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training
{

    public class TrainingData
    {
        
        public void Generate(string sourceDirectory, string targetDictory)
        {           
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 11
            };

            foreach (var sourceFile in Directory.EnumerateFiles(sourceDirectory))
            {
                var converter = new Converter();

                using (var sourceImage = (Bitmap)Bitmap.FromFile(sourceFile))
                {
                    foreach (var angle in new int[] { -10, -5, -3, -2, -1, 0, 1, 2, 3, 5, 10 })
                    {
                        var targetFile = new FileInfo(Path.Combine(targetDictory, angle.ToString(), Path.GetFileName(sourceFile)));

                        if (!targetFile.Directory.Exists)
                        {
                            targetFile.Directory.Create();
                        }

                        using (var rotated = RotateImage(sourceImage, angle))
                        {
                            using (var cropped = CropImage(rotated))
                            {
                                using (var input = converter.GetInput(cropped))
                                {
                                    input.Image.Save(targetFile.FullName);
                                }
                            }
                        }
                    };
                }
            };
        }

        private Bitmap RotateImage(Bitmap source, int angle)
        {
            // https://stackoverflow.com/questions/12024406/how-can-i-rotate-an-image-by-any-degree

            var rotatedImage = new Bitmap(source.Width, source.Height);

            var offset = new PointF((float)source.Width / 2, (float)source.Height / 2);

            using (var g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(offset.X, offset.Y);
                g.RotateTransform(angle);
                g.TranslateTransform(-offset.X, -offset.Y);
                g.DrawImage(source, 0, 0, source.Width, source.Height);
            }

            return rotatedImage;
        }

        private Bitmap CropImage(Bitmap source)
        {
            var croppedImage = new Bitmap((int)(source.Width * 0.75), (int)(source.Height * 0.75));

            using (var g = Graphics.FromImage(croppedImage))
            {
                g.DrawImage(source, -source.Width * 0.15f, -source.Height * 0.15f, source.Width, source.Height);
            }

            return croppedImage;
        }

    }

}
