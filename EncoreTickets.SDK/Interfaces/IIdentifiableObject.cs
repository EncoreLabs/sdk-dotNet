namespace EncoreTickets.SDK.Interfaces
{
    /// <summary>
    /// The interface for an identifiable IObject.
    /// </summary>
    public interface IIdentifiableObject : IObject
    {
        string id { get; }
    }
}