using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;

namespace EncoreTickets.SDK.Api.Utilities
{
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Returns an Environments Enum instance by name.
        /// </summary>
        /// <param name="name">The name of the environment. Currently the accepted values are "dev", "qa", "staging", "prod", "production"</param>
        /// <returns>Environments Enum instance with specified name</returns>
        /// <exception cref="ArgumentException">Thrown when the name parameter has value that is not supported.</exception>
        public static Environments EnvironmentFromName(string name)
        {
            try
            {
                return EnumExtension.GetEnumFromString<Environments>(name);
            }
            catch (Exception)
            {
                switch (name.ToLower())
                {
                    case "dev":
                        return Environments.Sandbox;
                    case "prod":
                        return Environments.Production;
                    default:
                        throw new ArgumentException($"Environment {name} is not defined.");
                }
            }
        }
    }
}
