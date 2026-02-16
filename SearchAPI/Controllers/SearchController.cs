using Shared.Model;
using Microsoft.AspNetCore.Mvc;
using SearchAPI.Logic;
using System.IO;

namespace SearchAPI.Controllers;

[ApiController]
[Route("api")]
public class SearchController : ControllerBase
{
    private static IDatabase mDatabase = new DatabaseSqlite();
    
    [HttpGet]
    [Route("search/{query}/{maxAmount}")]
    public SearchResult Search(string query, int maxAmount)
    {
        var logic = new SearchLogic(mDatabase);
        var result = logic.Search(query.Split(","), maxAmount);
        return result;
    }

    [HttpGet]
    [Route("ping")]
    public string? Ping()
    {
        return Environment.GetEnvironmentVariable("id");
    }
    
    [HttpGet]
    [Route("getfile")]
    public string GetFile([FromQuery] string path)
    {
        var uri = "file://" + path;
        var s = System.IO.File.ReadAllText(path);
        //var s = await Http.GetStringAsync(uri);
        return s;
    }
    
    
}
