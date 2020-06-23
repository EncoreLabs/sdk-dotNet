using Microsoft.CodeAnalysis;

namespace EncoreTickets.SDK.CustomRules.Rules
{
    internal abstract class BaseRule
    {
        public abstract DiagnosticDescriptor Rule { get; }
    }
}
