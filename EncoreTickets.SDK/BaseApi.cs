using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using EncoreTickets.SDK.Inventory;
using RestSharp;
using RestSharp.Authenticators;

namespace EncoreTickets.SDK
{
    /// <summary>
    /// The base api class to be extended by concrete implementations
    /// </summary>
    public abstract class BaseApi
    {
        #region Internal Constants

        /// <summary>
        /// BufferSize = 1024
        /// </summary>
        internal const int BufferSize = 1024;

        /// <summary>
        /// Verb Post "POST"
        /// </summary>
        internal const string VerbPost = "POST";

        /// <summary>
        /// VerbGet = "GET"
        /// </summary>
        internal const string VerbGet = "GET";

        /// <summary>
        /// VerbPut = "PUT"
        /// </summary>
        internal const string VerbPut = "PUT";

        /// <summary>
        /// VerbDelete = "DELETE";
        /// </summary>
        internal const string VerbDelete = "DELETE";

        /// <summary>
        /// The host
        /// </summary>
        private string host;

        #endregion

        #region Members

        /// <summary>
        /// The api context
        /// </summary>
        protected ApiContext context;

        /// <summary>
        /// Indicates if related data is to be retrieved
        /// </summary>
        protected bool includeRelatedData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected BaseApi(ApiContext context, string host)
        {
            this.context = context;
            this.host = host;
        }

        #endregion

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Stream GetResource(string url)
        {
            WebClient wc = new WebClient();
            return new MemoryStream(wc.DownloadData(url));
        }
        
        /// <summary>
        /// Execute an API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        protected ApiResult<T> ExecuteApi<T>(string endpoint, HttpMethod method, bool wrapped, string postData) where T : class
        {
            var client = this.GetClient();
            RestRequest request = this.GetRequest(endpoint);

            IRestResponse rr = null;
            ApiResponse<T> apiResponse = null;

            if (wrapped)
            {
                rr = client.Execute<ApiResponse<T>>(request);
                apiResponse = ((IRestResponse<ApiResponse<T>>)rr).Data;

            }
            else
            {
                rr = client.Execute(request);
                T rawData = SimpleJson.SimpleJson.DeserializeObject<T>(rr.Content);
                apiResponse = new ApiResponse<T>(rawData);
            }

            return new ApiResult<T>(this.context, request, rr, apiResponse);
        }

        /// <summary>
        /// Get a list of <typeparamref name="T"/> objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        protected ApiResultList<T> ExecuteApiList<T>(string endpoint, HttpMethod method, bool wrapped, string postData) where T : class
        {
            var client = this.GetClient();
            RestRequest request = this.GetRequest(endpoint);

            IRestResponse rr = null;
            ApiResponse<T> apiResponse = null;

            if (wrapped)
            {
                rr = client.Execute<ApiResponse<T>>(request);
                apiResponse = ((IRestResponse<ApiResponse<T>>)rr).Data;

            }
            else
            {
                rr = client.Execute(request);
                T rawData = (rr.IsSuccessful) ? SimpleJson.SimpleJson.DeserializeObject<T>(rr.Content) : null;
                apiResponse = new ApiResponse<T>(rawData);
            }

            // IRestResponse respone = client.Execute(request);

            //SimpleJson.JsonArray x = SimpleJson.SimpleJson.DeserializeObject<SimpleJson.JsonArray>(respone.Content);
            //AvailabilityResponse ar = (AvailabilityResponse)x;


            //IRestResponse<ApiResponse<T>> response2 = (ApiResponse<T>)




            return new ApiResultList<T>(this.context, request, rr, apiResponse);
        }

        /// <summary>
        /// Get the request
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private RestRequest GetRequest(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);

            // add HTTP Headers
            request.AddHeader("x-SDK", "EncoreTickets.SDK.NET v1");  // todo: add build numers

            if (!string.IsNullOrWhiteSpace(this.context.affiliate))
            {
                request.AddHeader("affiliateId", this.context.affiliate); 
            }

            if (this.context.useBroadway)
            {
                request.AddHeader("x-apply-price-engine", "true");
                request.AddHeader("x-market", "broadway");

                if(request.Method == Method.GET)
                {
                    request.AddQueryParameter("countryCode", "US");
                }
            }

            return request;
        }

        /// <summary>
        /// Get a rest client
        /// </summary>
        /// <returns></returns>
        private RestClient GetClient()
        {
            RestClient rc = new RestClient("https://" + string.Format(this.host, this.context.envrionment));

            if (!string.IsNullOrEmpty(this.context.userName))
            {
                rc.Authenticator = new HttpBasicAuthenticator(this.context.userName, this.context.password);
            }

            return rc;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <typeparam name="T">the type of object</typeparam>
        /// <param name="apiContext">The context.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="postData">The post data.</param>        
        /// <param name="totalRecords">Total records</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        /*
        protected ApiResult<T> ExecuteApi<T>(ApiContext apiContext, string endpoint, string verb, object postData, out int totalRecords, params Type[] types)
        {
            // check that theres's an api key
            if (verb != VerbGet && this.EnforceApiKeyCheck && apiContext.ApiKey == Guid.Empty)
            {
                throw new InvalidOperationException("No API key has been provided in the ApiContext.  If you aren't supplying one, call AuthenticationApi.Authenticate() first.");
            }

            Stream stream = null;

            try
            {
                totalRecords = -1;
                string url = "https://" + endpoint;

                List<Type> knownTypes = this.GetKnownTypes(apiContext, types);
                knownTypes.Add(typeof(T));

                if (postData != null)
                {
                    knownTypes.Add(postData.GetType());
                }

                XmlSerializer xmlSerializer = null;
                DataContractJsonSerializer jsonSerializer =  new DataContractJsonSerializer(typeof(ApiResponse), knownTypes);
           
                string postString = GetPostString(ref stream, postData, xmlSerializer, jsonSerializer);
                this.DispatchRequest(url, ref verb, postString, apiContext.UserName, apiContext.Password, apiContext.SessionId, apiContext.ApiKey, apiContext.Verbosity, apiContext.LockMode, apiContext.EventEditionId, apiContext.RestDataType, apiContext.AcceptsDataType, apiContext.SessionMode, apiContext.IncludeXmlNamespaceDataInResponse, out stream);

                ApiResponse response = null;

                if (stream != null)
                {
                    if (jsonSerializer != null)
                    {
                        // http://social.msdn.microsoft.com/Forums/en-ZA/wcf/thread/938156c7-ccb5-4436-833d-d560b9901750                    
                        string responseString = null;
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            responseString = sr.ReadToEnd();
                        }

                        // Write out for diagnosing tests (won't be included in a release build)
                        Debug.WriteLine("RESPONSE from : " + verb + " " + url + Environment.NewLine + responseString + Environment.NewLine);

                        byte[] jsonBytes = Encoding.UTF8.GetBytes(responseString);
                        DataContractJsonSerializer s = new DataContractJsonSerializer()
                        XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(jsonBytes, XmlDictionaryReaderQuotas.Max);

                        response = jsonSerializer.ReadObject(jsonReader) as ApiResponse;
                    }
                    else
                    {
                        knownTypes.Add(typeof(ApiResponse));
                        XmlSerializer responseXmlSerializer = new XmlSerializer(typeof(ApiResponse), knownTypes, null);

                        response = responseXmlSerializer.ReadObject(stream) as ApiResponse;
                    }

                    if (response != null)
                    {
                        if (!response.Result)
                        {
                            if (response.CommandStatusMessages == null)
                            {
                                Trace.WriteLine("ERROR: Command failed to url " + url);
                            }
                            else
                            {
                                foreach (CommandStatusMessage message in response.CommandStatusMessages)
                                {
                                    Trace.WriteLine(message.Type + ": " + message.Message);
                                }
                            }
                        }

                        totalRecords = response.TotalRecords ?? -1;

                        if (response.Data is ApiResult<T>)
                        {
                            return (ApiResult<T>)response.Data;
                        }
                        else
                        {
                            return new ApiResult<T>(response.Data is T ? (T)response.Data : default(T), response.Result, response.CommandStatusMessages);
                        }
                    }
                }

                return new ApiResult<T>(default(T), false, null);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
        */

        /// <summary>
        /// Gets the post string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="xmlSerializer">The XML serializer.</param>
        /// <param name="jsonSerializer">The json serializer.</param>
        /// <returns></returns>
        private static string GetPostString(ref Stream stream, object postData, DataContractJsonSerializer jsonSerializer)
        {
            string postString = null;

            if (postData != null)
            {
                using (stream = new MemoryStream())
                {
                    jsonSerializer.WriteObject(stream, postData);

                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            postString = reader.ReadToEnd();
                        }
                    }
                }
            }

            return postString;
        }

        /// <summary>
        /// Gets the known types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiContext">The API context.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        private List<Type> GetKnownTypes(ApiContext apiContext, params Type[] types)
        {
            List<Type> knownTypes = new List<Type>();
            if (types != null)
            {
                knownTypes.AddRange(types);
            }

            knownTypes.Add(typeof(Hashtable));
            knownTypes.Add(typeof(System.Collections.Specialized.StringDictionary));
            knownTypes.Add(typeof(System.Collections.Specialized.NameValueCollection));
            knownTypes.Add(typeof(MemoryStream));

            return knownTypes;
        }

        /// <summary>
        /// Sets the list parameters.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <param name="startRecord">The start record.</param>
        /// <param name="recordsToGet">The records to get.</param>
        /// <returns></returns>
        private string SetListParameters(string endPoint, int startRecord, int recordsToGet)
        {
            string url = endPoint;

            if (!endPoint.Contains("?"))
            {
                url += "?";
            }

            url += string.Format("&startRecord={0}", startRecord);
            url += string.Format("&rpp={0}", recordsToGet);

            return url;
        }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="lockMode">The lock mode.</param>
        /// <param name="eventEditionId">The event edition id.</param>
        /// <param name="restDataType">Type of the rest data.</param>
        /// <param name="acceptDataType">Type of the accept data.</param>
        /// <param name="sessionMode">The session mode.</param>
        /// <param name="includeXmlNamespaceDataInRepsonse">if set to <c>true</c> [include XML namespace data in repsonse].</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        /*
        private bool DispatchRequest(string url, ref string verb, string postData, string username, string password, Guid sessionId, Guid apiKey, ApiVerbosity verbosity, LockMode lockMode, int? eventEditionId, RestDataType restDataType, RestDataType acceptDataType, SessionMode sessionMode, bool includeXmlNamespaceDataInRepsonse, out Stream output)
        {
            output = null;
            bool requestProcessed = false;

            if (verb == null)
            {
                verb = (!string.IsNullOrEmpty(postData)) ? VerbPost : VerbGet;
            }

            if (eventEditionId.HasValue)
            {
                url = string.Concat(url, url.IndexOf('?') >= 0 ? "&" : "?", "evEdId=", eventEditionId);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = verb;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.Headers.Add("Content-Encoding", "gzip");
            request.UserAgent = string.Format("Reed Exhibitions Nova SDK v{0}", ConfigurationHelper.BuildNumber);
            request.Timeout = this.context.TimeoutMilliseconds; // source from configuration

            switch (restDataType)
            {
                case RestDataType.Xml:
                    request.ContentType = "application/xml";
                    break;
                case RestDataType.Javascript:
                    request.ContentType = "text/javascript";
                    break;
                default:
                    request.ContentType = "application/json";
                    break;
            }

            switch (acceptDataType)
            {
                case RestDataType.Xml:
                    request.Accept = "application/xml";
                    break;
                case RestDataType.Javascript:
                    request.Accept = "text/javascript";
                    break;
                case RestDataType.Pdf:
                    request.Accept = "application/pdf";
                    break;
                case RestDataType.Csv:
                    request.Accept = "text/csv; charset=utf-8";
                    break;
                default:
                    request.Accept = "application/json";
                    break;
            }

            // The Expect100Continue header needs to be set to false in order to avoid getting an 'underlying connection was closed' exception (instead of the actual exception) when an exception occurs
            request.ServicePoint.Expect100Continue = false;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                request.Headers.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)))));
            }
            else if (!string.IsNullOrEmpty(username) && sessionId != Guid.Empty)
            {
                request.Headers.Add("x-Authorization", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, sessionId))));
            }

            if (verbosity != ApiVerbosity.Default)
            {
                request.Headers.Add(string.Format("x-Verbosity: {0}", verbosity));
            }

            if (lockMode != LockMode.None)
            {
                request.Headers.Add(string.Format("x-lockMode: {0}", lockMode));
            }

            if (sessionMode != SessionMode.NonGlobal)
            {
                request.Headers.Add(string.Format("x-SessionMode: {0}", sessionMode));
            }

            if (this.includeRelatedData)
            {
                request.Headers.Add("x-includeRelated: true");
            }

            if (includeXmlNamespaceDataInRepsonse)
            {
                request.Headers.Add("x-includeXmlNamespaces: true");
            }

            // add api key
            request.Headers.Add("x-apikey", apiKey.ToString());

            if (request.Proxy != null)
            {
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            if (!string.IsNullOrEmpty(postData))
            {
                using (Stream gzipStream = new GZipStream(request.GetRequestStream(), CompressionMode.Compress, true))
                {
                    byte[] arr = Encoding.UTF8.GetBytes(postData);
                    gzipStream.Write(arr, 0, arr.Length);
                    gzipStream.Close();
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                requestProcessed = response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException e)
            {
                // Can only create once as it reads the response
                NovaWebException novaWebEx = NovaWebException.Create(e);

                if (!ApiContext.OnErrorOccurred(this, new ApiErrorEventArgs(novaWebEx, e.Message)))
                {
                    throw novaWebEx;
                }
            }

            if (requestProcessed)
            {
                output = new MemoryStream();
                Stream responseStream = this.GetResponseStream(response);

                byte[] buffer = new byte[BufferSize];
                int count;
                while ((count = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, count);
                }

                // return the stream to the beginning
                output.Seek(0, SeekOrigin.Begin);
            }

            // close the response stream
            if (response != null)
            {
                response.Close();
            }

            return requestProcessed;
        }
        */

        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private Stream GetResponseStream(HttpWebResponse response)
        {
            string contentEncoding = response.ContentEncoding.ToLower();

            if (contentEncoding.Equals("gzip"))
            {
                return new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else if (contentEncoding.Equals("deflate"))
            {
                return new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                return response.GetResponseStream();
            }
        }
    }
}


