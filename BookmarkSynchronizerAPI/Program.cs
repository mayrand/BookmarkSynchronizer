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
    var safariFile = @"./Data/Safari.html";
    Folder currentFolder = null;
    using var reader = new StreamReader(safariFile);
    const string folder = @"<DT><H3 FOLDED>";
    const string item = @"<DT><A HREF=""";
    while (true)
    {
        var line = reader.ReadLine()?.TrimStart();
        if (line is null) break;
        if (line.StartsWith(folder))
        {
            var newFolder = new Folder { Name = line.Substring(folder.Length, line.LastIndexOf('<') - folder.Length), Parent = currentFolder };
            currentFolder?.Items.Add(newFolder);
            currentFolder = newFolder;
        }
        else if (line.StartsWith(item))
        {
            var newBookmark = new Bookmark
            {
                Url = line.Substring(item.Length, line.IndexOf(">", item.Length) - 1 - item.Length),
                Name = line.Substring(line.IndexOf(">", item.Length) + 1, line.LastIndexOf('<') - line.IndexOf(">", item.Length) - 1)
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
    return;
})
.WithName("Get");

app.Run();