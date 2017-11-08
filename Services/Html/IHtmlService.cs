using System;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
    public interface IHtmlService
    {
        Task DeepAdd(Uri uri, Uri mainUri, int count);
    }
}