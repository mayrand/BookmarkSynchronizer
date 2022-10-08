using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;

namespace BookmarkSynchronizer.Controllers;

[ApiController]
[Route("[controller]")]
public class BookmarkController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<BookmarkController> _logger;

    public BookmarkController(ILogger<BookmarkController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Bookmark> Get()
    {
        var safari = new HtmlDocument();
        safari.DetectEncodingAndLoad(@"./Data/Safari.html");
        //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //{
        //    Date = DateTime.Now.AddDays(index),
        //    TemperatureC = Random.Shared.Next(-20, 55),
        //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //}).ToArray();
        return null;
    }
}

