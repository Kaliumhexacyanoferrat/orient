using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training;

namespace CommandLine
{

    class Program
    {

        static void Main(string[] args)
        {
            new TrainingData().Generate(@"D:\Temp\Training\Source", @"D:\Temp\Training\Input");

            new Trainer().Run();
        }

    }

}
