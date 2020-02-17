using System;
using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Api.Utilities
{
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Returns an instance of Environments Enum by a name used for an API environment.
        /// </summary>
        /// <param name="name">The name of the environment. Currently the accepted values are "dev", "qa", "staging", "prod", "production"</param>
        /// <returns>Environments Enum instance with specified name</returns>
        /// <exception cref="ArgumentException">Thrown when the name parameter has value that is not supported.</exception>
        public static Environments GetApiEnvironmentByName(string name)
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
