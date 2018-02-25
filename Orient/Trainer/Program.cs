using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orient.Training;
using NLog.Config;
using NLog.Targets;
using NLog;

namespace Orient.Trainer
{

    class Program
    {

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);

            var parsed = result as Parsed<Options>;

            if (parsed != null)
            {
                SetupLogging();

                var options = parsed.Value;

                var logger = LogManager.GetLogger("Trainer");
                logger.Info("Generating training data ...");

                if (!options.SkipGeneration)
                {
                    var data = new TrainingData();

                    data.Generate(options.TrainingSet);
                    data.Generate(options.TestSet);
                }

                var trainer = new NetworkTrainer();
                trainer.Run(options.Model, options.TrainingSet, options.TestSet);
            }
            else
            {
                Console.WriteLine(HelpText.AutoBuild(result));
            }
        }

        private static void SetupLogging()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.Layout = @"[${logger}] ${message}";

            config.AddTarget("console", consoleTarget);

            var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }

    }

}
