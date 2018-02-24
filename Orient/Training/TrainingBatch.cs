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

        public double MinimumError { get; set; }

        public double MaximumError { get; set; }
        
        public void SetResult(Volume<double> output)
        {
            var errors = new List<double>();

            for (int i = 0; i < output.Shape.TotalLength; i++)
            {
                var expected = OutputVolume.Get(i);
                var actual = output.Get(i);

                errors.Add(Math.Abs(expected - actual)); 
            }

            AverageError = errors.Average();
            MinimumError = errors.Min();
            MaximumError = errors.Max();
        }


        public void Dispose()
        {
            InputVolume?.Dispose();
            OutputVolume?.Dispose();
        }

    }

}
