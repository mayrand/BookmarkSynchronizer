using System.Xml;
using BookmarkSynchronizerAPI;
using HtmlAgilityPack;

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
    return;
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