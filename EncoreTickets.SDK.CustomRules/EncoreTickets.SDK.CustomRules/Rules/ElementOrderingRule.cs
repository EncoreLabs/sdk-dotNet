using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.CustomRules.Enums;
using EncoreTickets.SDK.CustomRules.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EncoreTickets.SDK.CustomRules.Rules
{
    public class ElementOrderingRule : BaseSymbolRule
    {
        public const string DiagnosticId = "Custom_SA1201";
        private const string Category = "Ordering";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ElementOrderingAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ElementOrderingAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ElementOrderingAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        public override DiagnosticDescriptor Rule { get; } = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private List<ElementKind> ElementKindOrder { get; } = new List<ElementKind>
        {
            ElementKind.Enum,
            ElementKind.Interface,
            ElementKind.Struct,
            ElementKind.Class,
            ElementKind.Field,
            ElementKind.Property,
            ElementKind.Indexer,
            ElementKind.Delegate,
            ElementKind.Event,
            ElementKind.Constructor,
            ElementKind.Finalizer,
            ElementKind.Method,
        };

        public override void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var members = namedTypeSymbol.GetMembers().Where(s => s.IsSubjectToOrderingRules()).ToArray();
            for (var i = 1; i < members.Length; i++)
            {
                var currentElementKind = members[i].GetElementKind();
                var previousElementKind = members[i - 1].GetElementKind();
                if (ElementKindOrder.Contains(currentElementKind) &&
                    ElementKindOrder.Contains(previousElementKind) &&
                    ElementKindOrder.IndexOf(currentElementKind) < ElementKindOrder.IndexOf(previousElementKind))
                {
                    var diagnostic = Diagnostic.Create(Rule, members[i].Locations[0], currentElementKind.ToString(), previousElementKind.ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
