using System.Text;

// ReSharper disable once CheckNamespace
namespace System.IO;

public static class FileExtensions
{
    public static string RelativeTo(this FileInfo fileInfo, DirectoryInfo ancestorDirectory)
    {
        var ancestorUri = new Uri($@"{ancestorDirectory.FullName}\");
        var fullUri = new Uri(fileInfo.FullName);

        if (!ancestorUri.Scheme.Equals(fullUri.Scheme, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The parent directory and the full path must have the same scheme.");
        
        var relativeUri = ancestorUri.MakeRelativeUri(fullUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', '\\');
    }

    public static async Task<string> ReadAsync(this FileStream fileStream)
    {
        var buffer = new byte[fileStream.Length];
        _ = await fileStream.ReadAsync(buffer);
        
        return Encoding.UTF8.GetString(buffer);
    }
}