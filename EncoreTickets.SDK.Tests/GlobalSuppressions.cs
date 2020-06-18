// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "No documentation required for unit testing projects.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Unit tests need underscores for readability.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Test case sources can be located in the same file as the testing class.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions", Justification = "Regions are useful in test classes and test case sources.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Multi-line parameters are useful in test case sources.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:Static elements should appear before instance elements", Justification = "Not applicable for test case sources and their respective test classes.", Scope = "namespaceanddescendants", Target = "EncoreTickets.SDK.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:'public' members should come before 'private' members", Justification = "Elements should be ordered by access.", Scope = "type", Target = "~T:EncoreTickets.SDK.Tests.UnitTests.Pricing.PricingServiceApiTestSource")]