using Checker_v._3._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Helpers
{
    public abstract class TestsEngine
    {
        public static TestsEngine ObtainEngine(ProgrammingLanguage programmingLanguage)
        {
            return ObtainEngine(programmingLanguage.Name);
        }

        public static TestsEngine ObtainEngine(string programmingLanguage)
        {
            switch (programmingLanguage)
            {
                case "JavaScript":
                    return new JavaScriptTestsEngine();
                default: 
                    return null;
            }
        }

        public abstract bool RunTest(string studentSolutionFilePath, string teacherTestFilePath);
    }
}
