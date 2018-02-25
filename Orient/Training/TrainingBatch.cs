using ConvNetSharp.Volume;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training
{

    public class TrainingBatch : IDisposable
    {

        public Volume<double> InputVolume { get; set; }

        public Volume<double> OutputVolume { get; set; }

        public double AverageError { get; set; }

        public double MinimumAngle { get; set; }

        public double MaximumAngle { get; set; }
        
        public void SetResult(Volume<double> output)
        {
            var errors = new List<double>();
            var angles = new List<double>();
            
            for (int i = 0; i < output.Shape.TotalLength; i++)
            {
                var expected = OutputVolume.Get(i);
                var actual = output.Get(i);

                errors.Add(Math.Abs(Math.Abs(expected) - Math.Abs(actual)));

                angles.Add(actual);
            }
            
            AverageError = errors.Average();

            MinimumAngle = angles.Min();
            MaximumAngle = angles.Max();
        }
    
        public void Dispose()
        {
            InputVolume?.Dispose();
            OutputVolume?.Dispose();
        }

    }

}
