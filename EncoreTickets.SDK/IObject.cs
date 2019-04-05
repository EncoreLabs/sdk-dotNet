using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK
{
    /// <summary>
    /// An identifiable IObject
    /// </summary>
    public interface IIdentifiableObject : IObject
    {
        string id { get; }
    }

    /// <summary>
    /// Marker interface
    /// </summary>
    public interface IObject
    {
    }
}
