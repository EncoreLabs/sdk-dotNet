using System.Globalization;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Returns the specified string converted to title case.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string source)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(source.ToLowerInvariant());
        }

        /// <summary>
        /// Returns user-friendly block name.
        /// </summary>
        /// <param name="blockName">Name of the block.</param>
        /// <param name="restrictedView">Set to true if the 'restricted view' label is needed.</param>
        /// <returns></returns>
        public static string ToUserFriendlyBlockName(this string blockName, bool restrictedView)
        {
            const string restrictedViewLabel = " (Restricted View)";
            return $"{blockName.ToTitleCase()}{(restrictedView ? restrictedViewLabel : "")}";
        }
    }
}
