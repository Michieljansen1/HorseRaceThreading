using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace Horserace.Common
{
    /// <summary>
    /// Deterimen the size of a webpage class
    /// </summary>
    class PageLoader
    {

        private int _totalSize;

        /// <summary>
        /// list of pages
        /// </summary>
        readonly string[] pages = new string[]
        {
            "/",
            "/robots.txt",
            "/contact",
            "/sitemap.xml",
        };

        /**************************************************************************
            * Private methods 
        **************************************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        private async Task<int> GetDomSize(string url)
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

            var header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            
            var requestUri = new Uri(url);

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();

            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                return httpResponse.Content.ToString().Length;


            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        /**************************************************************************
            * Public methods 
        **************************************************************************/

        /// <summary>
        ///  the size of the total site
        /// </summary>
        /// <param name="size"></param>
        public async Task<int> Run(string url)
        {
            foreach (var page in pages)
            {
                _totalSize += await GetDomSize("https://"+url+page);
            }

            return _totalSize;
        }
    }
}
