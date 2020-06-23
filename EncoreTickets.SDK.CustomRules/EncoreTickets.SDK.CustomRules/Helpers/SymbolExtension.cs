using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.CustomRules.Enums;
using Microsoft.CodeAnalysis;

namespace EncoreTickets.SDK.CustomRules.Helpers
{
    internal static class SymbolExtension
    {
        private static readonly Dictionary<Type, ElementKind> SymbolTypeToElementKindDictionary = new Dictionary<Type, ElementKind>
        {
            { typeof(IFieldSymbol), ElementKind.Field },
            { typeof(IEventSymbol), ElementKind.Event },
        };

        private static readonly Dictionary<MethodKind, ElementKind> MethodKindToElementKindDictionary = new Dictionary<MethodKind, ElementKind>
        {
            { MethodKind.Constructor, ElementKind.Constructor },
            { MethodKind.Destructor, ElementKind.Finalizer },
        };

        private static readonly Dictionary<TypeKind, ElementKind> TypeKindToElementKindDictionary = new Dictionary<TypeKind, ElementKind>
        {
            { TypeKind.Class, ElementKind.Class },
            { TypeKind.Interface, ElementKind.Interface },
            { TypeKind.Delegate, ElementKind.Delegate },
            { TypeKind.Enum, ElementKind.Enum },
            { TypeKind.Struct, ElementKind.Struct },
        };

        private static readonly Dictionary<bool, ElementKind> IsIndexerPropertyToElementKindDictionary = new Dictionary<bool, ElementKind>
        {
            { true, ElementKind.Indexer },
            { false, ElementKind.Property },
        };

        public static ElementKind GetElementKind(this ISymbol symbol)
        {
            if (symbol is INamedTypeSymbol namedTypeSymbol)
            {
                return TypeKindToElementKindDictionary.TryGetValue(namedTypeSymbol.TypeKind, out var result)
                    ? result
                    : ElementKind.Unspecified;
            }

            if (symbol is IMethodSymbol methodSymbol)
            {
                return MethodKindToElementKindDictionary.TryGetValue(methodSymbol.MethodKind, out var result)
                    ? result
                    : ElementKind.Method;
            }

            if (symbol is IPropertySymbol propertySymbol)
            {
                return IsIndexerPropertyToElementKindDictionary[propertySymbol.IsIndexer];
            }

            return SymbolTypeToElementKindDictionary
                .FirstOrDefault(s => s.Key.IsInstanceOfType(symbol))
                .Value;
        }

        public static bool IsSubjectToOrderingRules(this ISymbol symbol)
        {
            return !symbol.IsImplicitlyDeclared &&
                   symbol.GetElementKind() != ElementKind.Unspecified &&
                   (symbol.CanBeReferencedByName ||
                    symbol.GetElementKind() == ElementKind.Constructor ||
                    symbol.GetElementKind() == ElementKind.Finalizer ||
                    symbol.GetElementKind() == ElementKind.Indexer);
        }
    }
}
