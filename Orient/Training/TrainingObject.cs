using Orient.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orient.Training
{

    public class TrainingObject
    {

        public string File { get; private set; }

        public double Angle { get; private set; }
        
        public TrainingObject(string file, double angle)
        {
            File = file;
            Angle = angle;
        }

        public Output GetOutput()
        {
            return new Output(Angle);
        }

        public Input LoadInput()
        {
            return Input.FromFile(File);
        }

    }

}
