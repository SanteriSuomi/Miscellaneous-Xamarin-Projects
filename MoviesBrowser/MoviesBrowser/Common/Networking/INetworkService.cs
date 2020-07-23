using System;
using System.Threading.Tasks;

namespace MoviesBrowser.Common.Networking
{
    public interface INetworkService
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<TResult> GetAsync<TResult>(Uri uri);
    }
}