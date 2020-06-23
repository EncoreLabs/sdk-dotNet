using System.Threading.Tasks;
using EncoreTickets.SDK.CustomRules.Enums;
using EncoreTickets.SDK.CustomRules.Rules;
using NUnit.Framework;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.NUnit.AnalyzerVerifier<
    EncoreTickets.SDK.CustomRules.CustomRulesAnalyzer
>;

namespace EncoreTickets.SDK.CustomRules.Test
{
    [TestFixture]
    public class CustomRulesTests
    {
        [Test]
        public async Task ElementOrderingRule_IfRuleIsNotBroken_ReportsNoDiagnostics()
        {
            var testCode = @"
    namespace ConsoleApplication
    {
        class TypeName
        {   
            public enum Enum {}

            public interface IInterface {}

            public struct Struct {}

            public class Class {}

            public string field;

            public string Property { get; set; }

            public string this[int index] => ""Indexer"";

            public delegate string Delegate();

            public event Delegate Event;

            public TypeName() {}

            public void Method() {}

            ~TypeName() {}
        }
    }";

            await Verify.VerifyAnalyzerAsync(testCode);
        }

        [Test]
        public async Task ElementOrderingRule_ReportsAllDiagnostics()
        {
            var testCode = @"
    namespace ConsoleApplication
    {
        class TypeName
        {   
            ~TypeName() {}

            public void Method() {}

            public TypeName() {}

            public event Delegate Event;

            public delegate string Delegate();

            public string this[int index] => ""Indexer"";
            
            public string Property { get; set; }
            
            public string field;

            public class Class {}

            public struct Struct {}

            public interface IInterface {}

            public enum Enum {}
        }
    }";

            var expectedDiagnostics = new[]
            {
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(8, 25).WithArguments(ElementKind.Method, ElementKind.Finalizer),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(10, 20).WithArguments(ElementKind.Constructor, ElementKind.Method),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(12, 35).WithArguments(ElementKind.Event, ElementKind.Constructor),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(14, 36).WithArguments(ElementKind.Delegate, ElementKind.Event),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(16, 27).WithArguments(ElementKind.Indexer, ElementKind.Delegate),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(18, 27).WithArguments(ElementKind.Property, ElementKind.Indexer),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(20, 27).WithArguments(ElementKind.Field, ElementKind.Property),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(22, 26).WithArguments(ElementKind.Class, ElementKind.Field),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(24, 27).WithArguments(ElementKind.Struct, ElementKind.Class),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(26, 30).WithArguments(ElementKind.Interface, ElementKind.Struct),
                Verify.Diagnostic(ElementOrderingRule.DiagnosticId).WithLocation(28, 25).WithArguments(ElementKind.Enum, ElementKind.Interface),
            };
            await Verify.VerifyAnalyzerAsync(testCode, expectedDiagnostics);
        }
    }
}
