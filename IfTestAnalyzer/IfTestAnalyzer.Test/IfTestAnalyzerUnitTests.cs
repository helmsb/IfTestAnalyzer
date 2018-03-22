using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using IfTestAnalyzer;

namespace IfTestAnalyzer.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
using System;

namespace IfTestCurlyBraceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if(1==1)
            Console.WriteLine('Without Curly');

            if (1==1)
            {
                Console.WriteLine('With Curly');
            }
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "IfTestAnalyzer",
                Message = "You need curly braces around your if test",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 13)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;

namespace IfTestCurlyBraceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if(1==1)
            {
                Console.WriteLine('Without Curly');
            }

            if (1==1)
            {
                Console.WriteLine('With Curly');
            }
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new IfTestAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new IfTestAnalyzerAnalyzer();
        }
    }
}
