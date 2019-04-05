using System;

namespace EncoreTickets.SDK
{
    /// <summary>
    /// Api Error Event Args
    /// </summary>
    public class ApiErrorEventArgs : EventArgs
    {
        /// <summary>
        /// the full exception for logging etc
        /// </summary>
        private Exception exception;

        /// <summary>
        /// the friendly message to use
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public ApiErrorEventArgs(Exception exception, string message)
        {
            this.exception = exception;
            this.message = message;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get
            {
                return this.exception;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}