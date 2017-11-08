using AngParser.Models;
using AngParser.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
    public class HtmlService : IHtmlService
    {
        #region Singlton
        private static readonly Object s_lock = new Object();

        private static HtmlService instance = null;

        public static HtmlService Instance
        {
            get
            {
                if (instance != null) return instance;

                Monitor.Enter(s_lock);

                HtmlService temp = new HtmlService();

                Interlocked.Exchange(ref instance, temp);

                Monitor.Exit(s_lock);

                return instance;
            }
        }
        #endregion

        private volatile static IHtmlNotification _htmlNotification;

        private IHtmlParser _htmlParser;

        private IHttpService _httpService;

        private HtmlService()
        {
            HtmlService._htmlNotification = HtmlNotification.Instance;

            this._httpService = new HttpService();

            this._htmlParser = new HtmlParser();
        }

        async Task IHtmlService.DeepAdd(Uri uri, Uri mainUri, int count)
        {
            await this.DeepAdd(uri, mainUri, count);
        }

        private async Task DeepAdd(Uri uri, Uri mainUri, int count)
        {
            if (mainUri == null || this.UriHaveCircle(uri)
                || HtmlService._htmlNotification.FindCount > count) return;

            var baseDomain = mainUri.Host.Replace("www.", string.Empty);

            var thisDomain = uri.Host.Replace("www.", string.Empty);

            var isbaseDomain = (!string.IsNullOrWhiteSpace(baseDomain) || !string.IsNullOrWhiteSpace(thisDomain))
                && (baseDomain.Contains(thisDomain) || thisDomain.Contains(baseDomain));

            if (isbaseDomain && uri.ToString() != "#"
                && !_htmlNotification.UriContains(new ScaningUriModel { ScaningUri = uri })
                && _htmlNotification.EmailsCount() < count)
            {
                _htmlNotification.PushUri(new ScaningUriModel { ScaningUri = uri });

                var html = await _httpService.GetAsync(uri);

                var emails = _htmlParser.AngParser(html);

                foreach (var email in emails)
                {
                    this.AddEmail(email, mainUri);
                }

                var urls = _htmlParser.GetLinks(html, uri).ToList();

                foreach (var url in urls)
                {
                    if (Uri.TryCreate(url, UriKind.Absolute, out Uri u))
                    {
                        if (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps)
                        {
                            await this.DeepAdd(u, mainUri, count);
                        }
                        else if (u.Scheme == Uri.UriSchemeMailto)
                        {
                            var email = _htmlParser.AngParser(url).FirstOrDefault();

                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                this.AddEmail(email, mainUri);
                            }
                        }
                    }
                }
            }
        }

        private bool UriHaveCircle(Uri uri)
        {
            var haveCircle = uri.Query.Split('+')
                .GroupBy(query => query)
                .Any(group => group.Count() > 1);

            haveCircle |= uri.Query.Split('%')
                .GroupBy(query => query)
                .Any(group => group.Count() > 1);

            return haveCircle;
        }

        private void AddEmail(string email, Uri mainUri)
        {
            var parsingEmailModel = new ParsingEmailModel
            {
                Sended = false,
                Email = email.ToLower(),
                HtmlAdres = new Uri(mainUri.GetLeftPart(UriPartial.Authority))
            };

            if (!_htmlNotification.EmailContains(parsingEmailModel))
            {
                _htmlNotification.PushEmail(parsingEmailModel);
            }
        }
    }
}
