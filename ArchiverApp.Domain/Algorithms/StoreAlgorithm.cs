using ArchiverApp.Domain.Abstractions;

namespace ArchiverApp.Domain.Algorithms;

public class StoreAlgorithm : CompressionAlgorithmBase
{
    public override string Name          => "Store";
    public override string FileExtension => ".store";
    public override string Description => "Стиснення відсутнє – дані зберігаються без змін.";

    // --- compression ---
    public override byte[] Compress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return (byte[])data.Clone();
    }

    // --- decompression ---
    public override byte[] Decompress(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return (byte[])data.Clone();
    }
}