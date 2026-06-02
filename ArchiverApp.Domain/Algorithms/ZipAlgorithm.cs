using System.IO.Compression;
using ArchiverApp.Domain.Abstractions;

namespace ArchiverApp.Domain.Algorithms;

public class ZipAlgorithm : CompressionAlgorithmBase
{
    public override string Name => "Zip";
    public override string FileExtension => ".zip";
    public override string Description => "Алгоритм Deflate – універсальне стиснення.";

    // --- compression by DeflateStream ---
    public override byte[] Compress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var outputStream = new MemoryStream();
        using (var deflateStream = new DeflateStream(outputStream, CompressionLevel.Optimal, leaveOpen: true))
        {
            deflateStream.Write(data, 0, data.Length);
        }

        return outputStream.ToArray();
    }

    public override byte[] Decompress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();
        using var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress);

        deflateStream.CopyTo(outputStream);
        return outputStream.ToArray();
    }
}