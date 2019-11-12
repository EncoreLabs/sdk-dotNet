using System;
using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Utilities
{
    public static class EnvironmentExtension
    {
        /// <summary>
        /// Returns an Environments Enum instance by name.
        /// </summary>
        /// <param name="name">The name of the environment. Currently the accepted values are "dev", "qa", "staging", "prod", "production"</param>
        /// <returns>Environments Enum instance with specified name</returns>
        /// <exception cref="ArgumentException">Thrown when the name parameter has value that is not supported.</exception>
        public static Environments EnvironmentFromName(string name)
        {
            switch (name.ToLower())
            {
                case "dev":
                    return Environments.Sandbox;
                case "qa":
                    return Environments.QA;
                case "staging":
                    return Environments.Staging;
                case "production":
                case "prod":
                    return Environments.Production;
                default:
                    throw new ArgumentException($"Environment {name} is not defined.");
            } 
        }
    }
}
