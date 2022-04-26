//using System;
//using GlitchFinder.Contracts;
//using GlitchFinder.Managers;

//namespace GlitchFinder
//{
//    public class GlitchFinderService : IGlitchFinderService
//    {
//        private GlitchFinderService() { }
//        private static readonly Lazy<IGlitchFinderService> Lazy = new(() => new GlitchFinderService());
//        public static IGlitchFinderService Service => Lazy.Value;

//        public bool Run(CommandLineArgumentManager command)
//        {
//            bool isEqual;
            
//            switch (command.Command)
//            {
//                case Command.Compare:
//                    isEqual = mgr.Compare(command.Arguments[0]);
//                    Output(isEqual
//                        ? "No glitches found"
//                        : "There are glitches in the matrix.");
//                    return isEqual;
//                case Command.Baseline:
//                    isEqual =  mgr.SetBaseline(command.Arguments[0]);
//                    if (isEqual)
//                        Output("Baseline set.");
//                    return isEqual;
//                case Command.RegressionTest:
//                    isEqual = mgr.RegressionTest(command.Arguments[0]);
//                    Output(isEqual
//                        ? "No glitches found"
//                        : "There are glitches in the matrix.");
//                    return isEqual;
//                case Command.NewComparison:
//                    mgr.NewComparison(command.Arguments[0]);
//                    return true;
//                case Command.NewRegressionTest:
//                    mgr.NewRegressionTest(command.Arguments[0]);
//                    return true;
//                default:
//                    throw new NotImplementedException();
//            }
//        }
//        private void Output(string output)
//        {
//            Console.WriteLine(output);
//        }
//    }
//}