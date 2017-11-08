using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AngParser.Services.Http
{
    public class HttpService : IHttpService
    {
        async Task<string> IHttpService.GetAsync(Uri url)
        {
            try
            {
                using (var hendler = Handler())
                {
                    using (var httpClient = new HttpClient(hendler))
                    {
                        SetClientHeaders(httpClient);

                        using (HttpResponseMessage response = await httpClient.GetAsync(url))
                        using (HttpContent content = response.Content)
                        {
                            var bytes = await content.ReadAsByteArrayAsync();

                            Encoding encoding = Encoding.GetEncoding("utf-8");

                            return encoding.GetString(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #region settings
        private void SetClientHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, sdch");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
        }

        private HttpClientHandler Handler()
        => new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            MaxAutomaticRedirections = 15,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None,
        };
    }
    #endregion
}
