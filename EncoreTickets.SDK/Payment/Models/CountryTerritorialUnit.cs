namespace EncoreTickets.SDK.Payment.Models
{
    /// <summary>
    /// Internal territorial unit of a country (province / state / etc)
    /// </summary>
    public class CountryTerritorialUnit
    {
        /// <summary>
        /// Gets or sets unit abbreviation.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets unit name.
        /// </summary>
        public string Name { get; set; }
    }
}