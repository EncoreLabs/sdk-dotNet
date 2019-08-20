using System;

namespace EncoreTickets.SDK.Api.Context
{
    /// <summary>
    /// Api Error Event Args
    /// </summary>
    public class ApiErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the friendly message to use.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public ApiErrorEventArgs(Exception exception, string message)
        {
            Exception = exception;
            Message = message;
        }
    }
}