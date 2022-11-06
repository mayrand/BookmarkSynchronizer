using System.IO;
using System.Text.Json;
using BookmarkSynchronizerAPI;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.Reports;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    var safari = GetBookmarks(@"./Data/Safari.html");
    var opera = GetBookmarks(@"./Data/Opera.html");
    var compareLogic = new CompareLogic();
    compareLogic.Config.MaxDifferences = 1000;
    compareLogic.Config.IgnoreCollectionOrder = true;
    ComparisonResult result = compareLogic.Compare(safari, opera);
    HtmlReport htmlReport = new HtmlReport();

    htmlReport.Config.GenerateFullHtml = true; // if false, it will only generate an html table
    htmlReport.Config.HtmlTitle = "Comparison Report";
    htmlReport.Config.BreadCrumbColumName = "Bread Crumb";
    htmlReport.Config.ExpectedColumnName = "Expected";
    htmlReport.Config.ActualColumnName = "Actual";
    //htmlReport.Config.IncludeCustomCSS(".diff-crumb {background: gray;}"); // add some custom css    
    return Results.Content(htmlReport.OutputString(result.Differences), "text/html");
})
.WithName("Get");

app.Run();

Folder GetBookmarks(string file)
{
    const string folder = @"<DT><H3 ";
    const string item = @"<DT><A HREF=""";
    Folder currentFolder = null;
    using var reader = new StreamReader(file);
    while (true)
    {
        var line = reader.ReadLine()?.TrimStart();
        if (line is null) break;
        if (line.StartsWith(folder))
        {
            var newFolder = new Folder
            {
                Name = GetItemName(line, folder),
                Parent = currentFolder
            };
            currentFolder?.Items.Add(newFolder);
            currentFolder = newFolder;
        }
        else if (line.StartsWith(item))
        {
            var newBookmark = new Bookmark
            {
                Url = line.Substring(item.Length, line.IndexOf("\"", item.Length) - item.Length),
                Name = GetItemName(line, item)
            };
            currentFolder.Items.Add(newBookmark);
            newBookmark.Parent = currentFolder;
        }
        else if (line.StartsWith("</DL>"))
        {
            if (currentFolder.Parent is null) break;
            currentFolder = currentFolder.Parent;
        }
    }
    return currentFolder;
}

string GetItemName(string line, string item)
{
    var start = line.IndexOf(">", item.Length) + 1;
    var length = line.LastIndexOf('<') - line.IndexOf(">", item.Length) - 1;
    return line.Substring(start, length);
}