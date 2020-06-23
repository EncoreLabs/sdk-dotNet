using Microsoft.CodeAnalysis;

namespace EncoreTickets.SDK.CustomRules.Rules
{
    public abstract class BaseRule
    {
        public abstract DiagnosticDescriptor Rule { get; }
    }
}
