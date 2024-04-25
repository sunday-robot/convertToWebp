using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

internal class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
            Convert(arg);
    }

    /// <summary>
    /// 画像ファイルが指定された場合、その画像ファイルと同じディレクトリに、WEBPファイルを作成する。
    /// 画像ファイルを含むディレクトリが指定された場合、そのディレクトリ名の末尾に".webp"を追加したディレクトリを作成し、
    /// この新たなディレクトリに、WEBPファイルを作成する。
    /// </summary>
    /// <param name="fileOrDirectoryPath"></param>
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
            ConvertDirectory(directoryPath, Path.Combine(destinationDirectoryPath, Path.GetFileName(directoryPath)));
        }
        foreach (var filePath in Directory.EnumerateFiles(sourceDirectoryPath))
        {
            var destinationFilePath = Path.Combine(destinationDirectoryPath, Path.GetFileNameWithoutExtension(filePath) + ".webp");
            ConvertFile(filePath, destinationFilePath);
        }
    }

    // a/b.jpg, a.webp
    private static void ConvertFile(string sourceFilePath, string destinationFilePath)
    {
        Console.WriteLine(sourceFilePath);

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
