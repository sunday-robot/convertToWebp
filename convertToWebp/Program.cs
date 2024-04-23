using SixLabors.ImageSharp.Formats.Webp;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        foreach (var arg in args)
            Convert(arg);
    }

    static void Convert(String fileOrDirectoryPath)
    {
        if (File.Exists(fileOrDirectoryPath))
        {
            ConvertFile(fileOrDirectoryPath);
        }
        else if (Directory.Exists(fileOrDirectoryPath))
        {
            ConvertDirectory(fileOrDirectoryPath);
        }
        else
        {
            Console.WriteLine($"{fileOrDirectoryPath} does not exist.");
        }
    }

    private static void ConvertDirectory(string fileOrDirectoryPath)
    {
        throw new NotImplementedException();
    }

    private static void ConvertFile(string fileOrDirectoryPath)
    {
        var ext = Path.GetExtension(fileOrDirectoryPath);
        switch (ext.ToUpper())
        {
            case ".JPG":
                ConvertJpeg(fileOrDirectoryPath);
                return;
            case ".PNG":
                ConvertPng(fileOrDirectoryPath);
                return;
            default:
                Console.WriteLine($"Extension <{ext}> is not supportted.");
                return;
        }
    }
}