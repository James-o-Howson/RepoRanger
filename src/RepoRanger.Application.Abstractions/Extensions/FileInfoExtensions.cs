// ReSharper disable once CheckNamespace
namespace System.IO;

public static class FileInfoExtensions
{
    public static string RelativeTo(this FileInfo fileInfo, DirectoryInfo parent)
    {
        var parentUri = new Uri($@"{parent.FullName}\");
        var fullUri = new Uri(fileInfo.FullName);

        if (!parentUri.Scheme.Equals(fullUri.Scheme, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The parent directory and the full path must have the same scheme.");
        
        var relativeUri = parentUri.MakeRelativeUri(fullUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', '\\');

    }
}