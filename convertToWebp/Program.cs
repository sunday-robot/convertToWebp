using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.ComponentModel;
using Win32Api;

internal class Program
{
    /// <summary>
    /// JPEGファイル用のWebpエンコーダー(JPEGと同様に不可逆)
    /// </summary>
    static readonly WebpEncoder lossyEncoder = new();

    /// <summary>
    /// PNGファイル用のWebpエンコーダー(PNGと同様に可逆)
    /// </summary>
    static readonly WebpEncoder losslessEncoder = new() { FileFormat = WebpFileFormatType.Lossless };

    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: convertToWebp <file or directory>...");
            return;
        }

        foreach (var arg in args)
        {
            if (Directory.Exists(arg))
            {
                ProcessDirectory(arg);
            }
            else if (File.Exists(arg))
            {
                ProcessFile(arg);
            }
            else
            {
                Console.WriteLine($"{arg} does not exist.");
            }
        }
    }

    /// <summary>
    /// ディレクトリを再帰的に処理する
    /// </summary>
    /// <param name="directoryPath"></param>
    private static void ProcessDirectory(string directoryPath)
    {
        foreach (var subDirectoryPath in Directory.EnumerateDirectories(directoryPath))
        {
            ProcessDirectory(subDirectoryPath);
        }
        foreach (var filePath in Directory.EnumerateFiles(directoryPath))
        {
            ProcessFile(filePath);
        }
    }

    /// <summary>
    /// 画像ファイルをwebpに変換する。元の画像ファイルはゴミ箱に移動させる。
    /// </summary>
    /// <param name="filePath"></param>
    private static void ProcessFile(string filePath)
    {
        Console.WriteLine(filePath);

        WebpEncoder encoder;
        var ext = Path.GetExtension(filePath).ToUpper();
        switch (ext)
        {
            case ".JPG":
                encoder = lossyEncoder;
                break;
            case ".PNG":
                encoder = losslessEncoder;
                break;
            case ".WEBP":
                Console.WriteLine($" Skip WEBP file.");
                return;
            default:
                Console.WriteLine($" Skip unsupported file.");
                return;
        }

        try
        {
            var image = Image.Load(filePath);
            image.SaveAsWebp(filePath + ".webp", encoder);
            SendToRecycleBin(filePath);
        }
        catch (UnknownImageFormatException)
        {
            Console.WriteLine($" Skip broken file.");
            return;
        }
    }

    /// <summary>
    /// ファイルをゴミ箱に移動させる
    /// </summary>
    /// <param name="path"></param>
    /// <exception cref="Win32Exception"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    static void SendToRecycleBin(string path)
    {
        var shf = new Win32.SHFILEOPSTRUCT
        {
            wFunc = Win32.FO_DELETE,
            pFrom = path + '\0' + '\0',
            fFlags = Win32.FOF_ALLOWUNDO | Win32.FOF_NOCONFIRMATION
        };

        var result = Win32.SHFileOperation(ref shf);
        if (result != 0)
        {
            throw new Win32Exception(result, $"SHFileOperation failed: {result}");
        }

        // ユーザー操作などで中断された場合
        if (shf.fAnyOperationsAborted != 0)
        {
            throw new OperationCanceledException("The file operation was aborted.");
        }
    }
}
