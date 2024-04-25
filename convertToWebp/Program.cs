using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

internal class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
            Convert(arg);
    }

    static void Convert(string fileOrDirectoryPath)
    {
        if (File.Exists(fileOrDirectoryPath))
        {
            var parentDirectoryPath = Path.GetDirectoryName(fileOrDirectoryPath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileOrDirectoryPath);
            var destinationFilePath = Path.Combine(parentDirectoryPath, fileNameWithoutExtension + ".webp");
            ConvertFile(fileOrDirectoryPath, destinationFilePath);
        }
        else if (Directory.Exists(fileOrDirectoryPath))
        {
            // 引数に指定されたディレクトリ:
            // a/
            // 上記ディレクトリの中:
            // a/b.jpg
            // a/c/d.png
            // a/c/e/f.png
            // ↓
            // 新規に以下のディレクトリを作成し、
            // a.webp/
            // 以下のファイルを作成する
            // a.webp/b.webp
            // a.webp/c/d.webp
            // a.webp/c/e/f.webp
            var destinationDirectoryPath = fileOrDirectoryPath + ".webp";
            ConvertDirectory(fileOrDirectoryPath, destinationDirectoryPath);
        }
        else
        {
            Console.WriteLine($"{fileOrDirectoryPath} does not exist.");
        }
    }

    private static void ConvertDirectory(string sourceDirectoryPath, string destinationDirectoryPath)
    {
        _ = Directory.CreateDirectory(destinationDirectoryPath);

        foreach (var directoryPath in Directory.EnumerateDirectories(sourceDirectoryPath))
        {
            // a/c\
            Console.WriteLine(directoryPath);
            ConvertDirectory(directoryPath, Path.Combine(destinationDirectoryPath, Path.GetFileName(directoryPath)));
        }
        foreach (var filePath in Directory.EnumerateFiles(sourceDirectoryPath))
        {
            // a/b.jpg -> a.webp/b.webp
            Console.WriteLine(filePath);
            var destinationFilePath = Path.Combine(destinationDirectoryPath, Path.GetFileNameWithoutExtension(filePath) + ".webp");
            ConvertFile(filePath, destinationFilePath);
        }
    }

    // a/b.jpg, a.webp
    private static void ConvertFile(string sourceFilePath, string destinationFilePath)
    {
        WebpEncoder encoder;
        var ext = Path.GetExtension(sourceFilePath).ToUpper();
        switch (ext)
        {
            case ".JPG":
                encoder = new WebpEncoder();
                break;
            case ".PNG":
                encoder = new WebpEncoder() { FileFormat = WebpFileFormatType.Lossless };
                break;
            case ".WEBP":
                Console.WriteLine($" Skip WEBP file.");
                return;
            default:
                Console.WriteLine($" Extension <{ext}> is not supportted.");
                return;
        }

        var image = Image.Load(sourceFilePath);
        image.SaveAsWebp(destinationFilePath, encoder);
    }
}
