using AngParser.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
    public class HtmlNotification : IHtmlNotification
    {
        #region Singlton
        private static readonly Object s_lock = new Object();

        private static HtmlNotification instance = null;

        public static HtmlNotification Instance
        {
            get
            {
                if (instance != null) return instance;

                Monitor.Enter(s_lock);

                HtmlNotification temp = new HtmlNotification();

                Interlocked.Exchange(ref instance, temp);

                Monitor.Exit(s_lock);

                return instance;
            }
        }
        #endregion

        private volatile int _findCount;

        private volatile ConcurrentStack<ScaningUriModel> _uris;

        private volatile ConcurrentStack<ParsingEmailModel> _emails;

        public event EventHandler EmailGetted;

        public event EventHandler SiteScaning;

        private HtmlNotification()
        {
            this._uris = new ConcurrentStack<ScaningUriModel>();

            this._emails = new ConcurrentStack<ParsingEmailModel>();

            this._findCount = 0;
        }

        int IHtmlNotification.FindCount { get => _findCount; set => _findCount = value; }

        IEnumerable<ParsingEmailModel> IHtmlNotification.AngParser()
        {
            var result = new ConcurrentStack<ParsingEmailModel>();

            for (int i = 0; i < this._emails.Count; i++)
            {
                if(!this._emails.ElementAt(i).Sended)
                {
                    result.Push(this._emails.ElementAt(i));

                    this._emails.ElementAt(i).Sended = true;
                }
            }

            return result;
        }

        void IHtmlNotification.PushUri(ScaningUriModel html)
        {
            SiteScaning?.Invoke(html, EventArgs.Empty);

            this._uris.Push(html);
        }

        void IHtmlNotification.PushEmail(ParsingEmailModel email)
        {
            this._findCount++;

            EmailGetted?.Invoke(email, EventArgs.Empty);

            this._emails.Push(email);
        }

        bool IHtmlNotification.EmailContains(ParsingEmailModel email) => this._emails.Any(e => e.Email == email.Email);

        bool IHtmlNotification.UriContains(ScaningUriModel uri) => this._uris.Any(u => u.ScaningUri == uri.ScaningUri);

        int IHtmlNotification.EmailsCount() => this._emails.Count;
    }
}
