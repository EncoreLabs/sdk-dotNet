using System.IO;
using System.IO.Compression;
using System.Net;

namespace EncoreTickets.SDK.Api.Helpers
{
    /// <summary>
    /// The helper for working with streams.
    /// </summary>
    internal static class StreamReceiver
    {
        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static Stream GetResource(string url)
        {
            using (var webClient = new WebClient())
            {
                var data = webClient.DownloadData(url);
                return new MemoryStream(data);
            }
        }

        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static Stream GetResponseStream(HttpWebResponse response)
        {
            var contentEncoding = response.ContentEncoding.ToLower();
            var responseStream = response.GetResponseStream();

            if (contentEncoding.Equals("gzip"))
            {
                return new GZipStream(responseStream, CompressionMode.Decompress);
            }

            if (contentEncoding.Equals("deflate"))
            {
                return new DeflateStream(responseStream, CompressionMode.Decompress);
            }

            return responseStream;
        }
    }
}
