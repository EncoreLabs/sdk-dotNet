using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EncoreTickets.SDK.CustomRules.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EncoreTickets.SDK.CustomRules
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CustomRulesAnalyzer : DiagnosticAnalyzer
    {
        private static readonly IEnumerable<(BaseSymbolRule Rule, SymbolKind SymbolKind)> SymbolRules = new List<(BaseSymbolRule, SymbolKind)>
        {
            (new ElementOrderingRule(), SymbolKind.NamedType)
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => CreateSupportedDiagnostics();

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            foreach (var rule in SymbolRules)
            {
                context.RegisterSymbolAction(rule.Rule.AnalyzeSymbol, rule.SymbolKind);
            }
        }

        private static ImmutableArray<DiagnosticDescriptor> CreateSupportedDiagnostics()
        {
            var symbolRules = SymbolRules.Select(r => r.Rule.Rule);
            return ImmutableArray.Create(symbolRules.ToArray());
        }
    }
}
