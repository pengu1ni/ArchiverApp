using ArchiverApp.Domain.Abstractions;
using ArchiverApp.Domain.Algorithms;
using ArchiverApp.Domain.Enums;

namespace ArchiverApp.Domain.Factories;

public static class CompressionAlgorithmFactory
{
    // --- creates and returns compreession alg by type ---
    public static CompressionAlgorithmBase Create(CompressionType type) => type switch
    {
        CompressionType.Store => new StoreAlgorithm(),
        CompressionType.Zip => new ZipAlgorithm(),
        CompressionType.Rar => new RarAlgorithm(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Алгоритм '{type}' не підтримується!"),
    };

    public static IReadOnlyList<CompressionAlgorithmBase> GetAll() =>
    [
        new StoreAlgorithm(),
        new ZipAlgorithm(),
        new RarAlgorithm(),
    ];
}