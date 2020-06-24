using Microsoft.CodeAnalysis.Diagnostics;

namespace EncoreTickets.SDK.CustomRules.Rules
{
    public abstract class BaseSymbolRule : BaseRule
    {
        public abstract void AnalyzeSymbol(SymbolAnalysisContext context);
    }
}
