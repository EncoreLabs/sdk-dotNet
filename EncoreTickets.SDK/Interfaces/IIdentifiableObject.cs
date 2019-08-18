namespace EncoreTickets.SDK.Interfaces
{
    /// <summary>
    /// An identifiable IObject
    /// </summary>
    public interface IIdentifiableObject : IObject
    {
        string id { get; }
    }
}