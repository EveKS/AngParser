using System;
using System.Collections.Generic;

namespace AngParser.Services.Html
{
    public interface IHtmlParser
    {
        IEnumerable<string> AngParser(string html);
        IEnumerable<string> GetLinks(string html, Uri uri);
    }
}