using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orient.Trainer
{

    class Options
    {

        [Option(Default = "model.json")]
        public string Model { get; set; }
        
        [Option(Default = @"D:\Temp\Training")]
        public string TrainingSet { get; set; }

        [Option(Default = @"D:\Temp\Test")]
        public string TestSet { get; set; }

        [Option(Default = false)]
        public bool SkipGeneration { get; set; }
        
    }

}
