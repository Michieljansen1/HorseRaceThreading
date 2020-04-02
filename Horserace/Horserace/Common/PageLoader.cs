using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Horserace.Common
{
    /// <summary>
    ///     Determine the size of a webpage class
    /// </summary>
    class PageLoader
    {
        /// <summary>
        ///     list of pages to check the content for
        /// </summary>
        private readonly string[] _pages = { "/", "/robots.txt", "/contact", "/sitemap.xml" };

        private int _totalSize; // Total size of all pages

        /// <summary>
        ///     Fetches the content of the give URL webpage and returns the DOM size
        /// </summary>
        /// <param name="url">URL to fetch</param>
        private async Task<int> GetDomSize(string url)
        {
            HttpClient httpClient = new HttpClient();

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
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                return httpResponse.Content.ToString().Length;
            } catch (Exception ex)
            {
                return 0;
            }
        }


        /// <summary>
        ///     the size of the total site
        /// </summary>
        /// <param name="url">Base url to fetch</param>
        public async Task<int> Run(string url)
        {
            foreach (var page in _pages)
            {
                _totalSize += await GetDomSize("https://" + url + page);
            }

            return _totalSize / 10;
        }
    }
}