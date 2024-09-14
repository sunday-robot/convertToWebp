using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

internal class Program
{
    private static void Main(string[] args)
    {
        foreach (var arg in args)
            ConvertFileOrDirectory(arg);
    }

    /// <summary>
    /// 画像ファイルが指定された場合、その画像ファイルと同じディレクトリに、WEBPファイルを作成する。
    /// 画像ファイルを含むディレクトリが指定された場合、そのディレクトリ名の末尾に".webp"を追加したディレクトリを作成し、
    /// この新たなディレクトリに、WEBPファイルを作成する。
    /// </summary>
    /// <param name="fileOrDirectoryPath"></param>
    static void ConvertFileOrDirectory(string fileOrDirectoryPath)
    {
        if (File.Exists(fileOrDirectoryPath))
        {
            // 画像ファイルの場合は、そのファイルと同じ場所に、拡張子を".webp"に変えたファイル名でWEBPファイルを作成する。
            var parentDirectoryPath = Path.GetDirectoryName(fileOrDirectoryPath);
            if (parentDirectoryPath == null)
            {
                // nullが返ることは以下の理由でありえないので何もしない。
                // Path.GetDirectoryName()がnullを返すのは、以下の二つの場合のみである。
                // (1) 引数がnullである。
                // → Convert()の引数が"string?"ではなく"string"なのでnullであるということはあり得ない。
                // (2) 引数が"/"あるいは"\"である。
                // → File.Exists()はfalseなので、Path.GetDirectoryName()に渡されることはあり得ない。
                return;
            }
            var webpFileName = Path.GetFileNameWithoutExtension(fileOrDirectoryPath) + ".webp";
            var destinationFilePath = Path.Combine(parentDirectoryPath, webpFileName);
            ConvertFile(fileOrDirectoryPath, destinationFilePath);
        }
        else if (Directory.Exists(fileOrDirectoryPath))
        {
            // ディレクトリの場合は、そのディレクトリ名の末尾に".webp"を追加した名前のディレクトリを
            // 同じ階層に作成し、その中にWEBPファイルを作成する。
            var destinationDirectoryPath = fileOrDirectoryPath + ".webp";
            ConvertDirectory(fileOrDirectoryPath, destinationDirectoryPath);
        }
        else
        {
            // ファイルもディレクトリもない場合は、警告メッセージを出力する。(エラー終了したりしない。)
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
