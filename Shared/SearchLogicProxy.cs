using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Model;

namespace Shared;

public class SearchLogicProxy
{
    private string serverEndPoint = "http://localhost:5158/api";

    private HttpClient mHttp;

    public SearchLogicProxy()
    {
        mHttp = new HttpClient();
    }

    public async Task<SearchResult> Search(string[] query, int maxAmount)
    {
        var completeUrl = $"{serverEndPoint}/search/{String.Join(",", query)}/{maxAmount}";
        return await mHttp.GetFromJsonAsync<SearchResult>(completeUrl);
    }

    public async Task<string> GetFileContent(string url)
    {
        var encodedurl = Uri.EscapeDataString(url);
        var completeUrl = $"{serverEndPoint}/file/get?path={encodedurl}";
        var fileContent = await mHttp.GetStringAsync(completeUrl);
        return fileContent;
    }
}