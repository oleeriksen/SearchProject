using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Model;

namespace ConsoleSearch;

public class SearchLogicProxy
{
    private string serverEndPoint = "http://localhost:5158/api/search/";

    private HttpClient mHttp;

    public SearchLogicProxy()
    {
        mHttp = new HttpClient();
    }

    public async Task<SearchResult> Search(string[] query, int maxAmount)
    {
        var completeUrl = $"{serverEndPoint}{String.Join(",", query)}/{maxAmount}";
        return await mHttp.GetFromJsonAsync<SearchResult>(completeUrl);
    }
}