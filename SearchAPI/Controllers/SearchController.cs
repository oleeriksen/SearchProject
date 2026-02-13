using Shared.Model;
using Microsoft.AspNetCore.Mvc;
using SearchAPI.Logic;

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
    
}
