using System;
using System.IO;
using System.Net;
using System.Text;

namespace TR.HttpUtilities
{
    public static class TRWebClient
    {

        /// <summary>
        /// Performs and HTTP GET and returns the response
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns>An appropriate web response or null if a response is unavailable</returns>
        public static TRWebResponse Get(Uri requestUrl)
        {
            TRWebResponse webResponse = null;
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    webResponse = new TRWebResponse();
                    webResponse.StatusCode = response.StatusCode;
                    webResponse.StatusDescription = response.StatusDescription;
                    var enc = Encoding.GetEncoding(1252);
                    using (var responseStream = new StreamReader(response.GetResponseStream(), enc))
                    {
                        webResponse.Response = responseStream.ReadToEnd();
                        responseStream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception e)
            {
            }
            return webResponse;
        }
    }

    public class TRWebResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Response { get; set; }

    }

}