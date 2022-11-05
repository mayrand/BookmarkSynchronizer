namespace Tests;

public class EqualityTests
{
    [Fact]
    public void Test1()
    {
        var a = new Folder
        {
            Name = "a",
            Items = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" },
                    new Folder {Name = "b", Items = new List<Item>{new Bookmark { Name="2", Url="qwe"} } }
                }
        };
        var b = new Folder
        {
            Name = "a",
            Items = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" },
                    new Folder {Name = "b", Items = new List<Item>{new Bookmark { Name="2", Url="qwe"} } }
                }
        };
        //Assert.Equal(a, b);
        Assert.Equal(a, b, new ItemEqualityComparer());
    }

    [Fact]
    public void TreeNotEqualByLeaf()
    {
        var a = new Folder
        {
            Name = "a",
            Items = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" },
                    new Folder {Name = "b", Items = new List<Item>{new Bookmark { Name="2", Url="qwe"} } }
                }
        };
        var b = new Folder
        {
            Name = "a",
            Items = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" },
                    new Folder {Name = "b", Items = new List<Item>{new Bookmark { Name="2", Url="123"} } }
                }
        };
        Assert.NotEqual(a, b);
        Assert.NotEqual(a, b, new ItemEqualityComparer());
    }

    [Fact]
    public void Test2()
    {
        var a = new Bookmark { Name = "1", Url = "uyg" };
        var b = new Bookmark { Name = "1", Url = "uyg" };
        Assert.Equal(a, b);
        Assert.Equal(a, b, new ItemEqualityComparer());
    }

    [Fact]
    public void FolderIsNotEqualToBookmark()
    {
        var a = new Bookmark { Name = "1" };
        var b = new Folder { Name = "1" };
        Assert.NotEqual<Item>(a, b);
        Assert.NotEqual(a, b, new ItemEqualityComparer());
    }

    [Fact]
    public void Test3()
    {
        var a = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" }
                };
        var b = new List<Item>
                {
                    new Bookmark{Name="1",Url="uyg" }
                };
        Assert.True(Enumerable.SequenceEqual(a, b));
        Assert.Equal(a, b);
    }
}
