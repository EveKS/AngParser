using AngParser.Models;
using System;
using System.Collections.Generic;

namespace AngParser.Services.Html
{
    public interface IHtmlNotification
    {
        int FindCount { get; set; }

        event EventHandler EmailGetted;
        event EventHandler SiteScaning;

        IEnumerable<ParsingEmailModel> AngParser();
        void PushEmail(ParsingEmailModel email);
        void PushUri(ScaningUriModel html);
        bool EmailContains(ParsingEmailModel email);
        bool UriContains(ScaningUriModel uri);
        int EmailsCount();
    }
}