using System.Threading.Tasks;
using NUnit.Framework;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.NUnit.CodeFixVerifier<
    EncoreTickets.SDK.CustomRules.CustomRulesAnalyzer, 
    EncoreTickets.SDK.CustomRules.CustomRulesCodeFixProvider
>;

namespace EncoreTickets.SDK.CustomRules.Test
{
    [TestFixture]
    public class CustomRulesUnitTests
    {
        //No diagnostics expected to show up
        [Test]
        public async Task TestMethod1()
        {
            var test = @"";
            await Verify.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [Test]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = Verify.Diagnostic(CustomRulesAnalyzer.DiagnosticId).WithLocation(11, 15).WithArguments("TypeName");
            await Verify.VerifyCodeFixAsync(test, new[] { expected }, fixtest);
        }
    }
}
