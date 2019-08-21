namespace EncoreTickets.SDK.Interfaces
{
    /// <summary>
    /// Progress interface.
    /// </summary>
    public interface IProgressCallback
    {
        /// <summary>
        /// Starts the specified method type.
        /// </summary>
        void Begin();

        /// <summary>
        /// Progresses the updated.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="methodType">Type of the method.</param>
        /// <param name="recordProcessed">The record processed.</param>
        /// <param name="recordsRemaining">The records remaining.</param>
        /// <param name="cancel">if set to <c>true</c> [cancel].</param>
        void ProgressUpdated(object[] records, ListMethodType methodType, int recordProcessed, int recordsRemaining, out bool cancel);

        /// <summary>
        /// Completes the specified total records.
        /// </summary>
        /// <param name="totalRecords">The total records.</param>
        void Complete(int totalRecords);
    }
}
