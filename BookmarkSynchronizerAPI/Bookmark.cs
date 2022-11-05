using System.Diagnostics.CodeAnalysis;

namespace BookmarkSynchronizerAPI;

public sealed record Bookmark : Item
{
    public string Url { get; set; }

    public string Name { get; set; }

    public Folder Parent { get; set; }
}

public sealed record Folder : Item
{
    public string Name { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public Folder Parent { get; set; }
}

public interface Item { string Name { get; set; } Folder Parent { get; set; } }

public class ItemEqualityComparer : IEqualityComparer<Item>
{
    public bool Equals(Item? x, Item? y)
    {
        if (x.Name != y.Name)
            return false;
        if (x is Folder xf && y is Folder yf)
            return Enumerable.SequenceEqual<Item>(xf.Items, yf.Items, new ItemEqualityComparer());
        else if (x is Bookmark xb && y is Bookmark yb)
            return xb.Url == yb.Url;
        return false;
    }

    public int GetHashCode([DisallowNull] Item obj)
    {
        return obj.Name.GetHashCode();
    }
}
