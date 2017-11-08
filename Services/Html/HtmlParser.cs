using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
    public class HtmlParser : IHtmlParser
    {
        IEnumerable<string> IHtmlParser.AngParser(string html)
        {
            Regex reEmail = new Regex(@"(?inx)
             \w+([-+.]\w+)
                *@\w+([-.]\w+)
            *\.\w+([-.]\w+)*");

            return reEmail.Matches(html).OfType<Match>()
                .Select(match =>
                {
                    var url = match.Value.ToString();

                    return url;
                });
        }

        /// <summary>
        /// Парсим html, для получения ссылок
        /// </summary>
        /// <param name="html"></param>
        /// <param name="uri">Адрес, содержащий домен</param>
        /// <returns></returns>
        IEnumerable<String> IHtmlParser.GetLinks(string html, Uri uri)
        {
            Regex reHref = new Regex(@"(?inx)
                <a \s [^>]*
                    href \s* = \s*
                        (?<q> ['""] )
                            (?<url> [^""]+ )
                        \k<q>
                [^>]* >");

            return reHref.Matches(html).OfType<Match>()
                .Select(match =>
                {
                    var url = match.Groups["url"].ToString();

                    if (url.FirstOrDefault() == '/')
                    {
                        url = uri.GetLeftPart(UriPartial.Authority) + url;
                    }

                    return url;
                });
        }
    }
}
