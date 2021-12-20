using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Helpers
{
    public class JavaScriptTestsEngine : TestsEngine
    {
        public override bool RunTest(string studentSolutionFilePath, string teacherTestFilePath)
        {
            var JSEngine = new Engine();
            try
            {
                var studentScript = GetScript(studentSolutionFilePath);
                var teacherScript = GetScript(teacherTestFilePath);
                string script = studentScript + "\n" + teacherScript;
            
                var testFunction = JSEngine.Execute(script).GetValue("Test");
                var testResult = testFunction.Invoke().AsBoolean();
                return testResult;
            }
            catch (Jint.Runtime.JavaScriptException Ex)
            {
                //Тут можно будет выводить ошибку
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetScript(string filePath)
        {
            using (var sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
