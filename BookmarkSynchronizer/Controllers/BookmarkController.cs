using Microsoft.AspNetCore.Mvc;

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

}

