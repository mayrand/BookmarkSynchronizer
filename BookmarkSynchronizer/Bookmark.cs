using System.Diagnostics.CodeAnalysis;

namespace BookmarkSynchronizer;

public sealed record Bookmark : Item
{
    public string Url { get; set; }

    public string Name { get; set; }
}

public sealed record Folder : Item
{
    public string Name { get; set; }
    public List<Item> Items { get; set; }
}

public interface Item { string Name { get; set; } }

public class ItemListEqualityComparer : IEqualityComparer<List<Item>>
{
    public bool Equals(List<Item> x, List<Item> y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.SequenceEqual(y);
    }

    public int GetHashCode(List<Item> list)
    {
        var hash = 0;
        if (ReferenceEquals(list, null) || list.Count == 0) return hash;

        foreach (var o in list)
        {
            var h = o.Name.GetHashCode();
            if (hash == 0)
                hash = h;
            else
                hash = hash ^ h;
        }
        return hash;
    }
}

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
