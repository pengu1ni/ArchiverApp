using System.IO.Compression;
using ArchiverApp.Domain.Abstractions;

namespace ArchiverApp.Domain.Algorithms;

public class RarAlgorithm : CompressionAlgorithmBase
{
    public override string Name => "RAR";
    public override string FileExtension => ".rar";
    public override string Description => "Алгоритм на основі GZip – висока ступінь стиснення.";

    // --- compression by GZipStream ---
    public override byte[] Compress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(outputStream, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            gzipStream.Write(data, 0, data.Length);
        }

        return outputStream.ToArray();
    }

    public override byte[] Decompress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();
        using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);

        gzipStream.CopyTo(outputStream);
        return outputStream.ToArray();
    }
}