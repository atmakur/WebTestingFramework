using Atmakur.Testing.Core.Extensions;
using System;
using System.IO;

namespace Atmakur.Testing.Core.Utilities
{
    public static class TestResultsHelper
    {
        private static string TestResultsFile = string.Empty;
        private static object _locker = null;
        static TestResultsHelper()
        {
            if (_locker.IsNull())
            {
                _locker = new object();
                TestResultsFile = Path.Combine(AppConfig.AssemblyDirectory, "TestResults_" + DateTime.Now.ToNameString() + ".csv");
            }
        }

        public static void GenerateOutputResults(TestCaseResult testCaseResult)
        {
            if (AppConfig.OutputResultsToConsole || AppConfig.OutputResultsToFile)
            {
                lock (_locker)
                {
                    int index = 0;
                    foreach (var results in testCaseResult)
                    {
                        if (AppConfig.OutputResultsToFile)
                        {
                            var outputMsg = index > 0 ? "," + results.Item2.ToString() : results.Item2.ToString();
                            File.AppendAllText(TestResultsFile, outputMsg);
                        }
                        if (AppConfig.OutputResultsToConsole)
                            Console.WriteLine("{0}: {1}", results.Item1, results.Item2);
                        index++;
                    }
                    if (AppConfig.OutputResultsToFile)
                        File.AppendAllText(TestResultsFile, Environment.NewLine);
                }
            }
        }
    }
}
