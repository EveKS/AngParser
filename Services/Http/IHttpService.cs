using System;
using System.Threading.Tasks;

namespace AngParser.Services.Http
{
    public interface IHttpService
    {
        Task<string> GetAsync(Uri url);
    }
}