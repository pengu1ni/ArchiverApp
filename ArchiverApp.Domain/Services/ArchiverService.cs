using ArchiverApp.Domain.Abstractions;
using ArchiverApp.Domain.Enums;
using ArchiverApp.Domain.Events;
using ArchiverApp.Domain.Factories;
using ArchiverApp.Domain.Models;

namespace ArchiverApp.Domain.Services;

public class ArchiverService : IArchiverService
{
    // --- private fields ---
    private Archive? _currentArchive;

    // --- public props ---
    public Archive? CurrentArchive => _currentArchive;

    // --- smth ---
    public Archive CreateArchive(string name, CompressionType compressionType)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва архіву не може бути порожня!", nameof(name));

        _currentArchive = new Archive(name, compressionType);
        return _currentArchive;
    }

    public void AddFile(string name, string extension, byte[] content)
    {
        EnsureArchiveExists();

        var file = new ArchivedFile(name, extension, content);
        _currentArchive!.AddFile(file);
    }

    public bool RemoveFile(string fullName)
    {
        EnsureArchiveExists();
        return _currentArchive!.RemoveFile(fullName);
    }

    public void ClearArchive() => _currentArchive = null;

    // --- compression operations ---
    public async Task CompressAsync(Action<ArchiveProgressEvent>? onProgress = null)
    {
        EnsureArchiveExists();
        EnsureArchiveHasFiles();

        var algorithm = CompressionAlgorithmFactory.Create(_currentArchive!.CompressionType);

        _currentArchive.SetStatus(ArchiveStatus.New);

        var files = _currentArchive.Files.ToList();

        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];

            onProgress?.Invoke(new ArchiveProgressEvent
            {
               CurrentFileName  = file.FullName,
               CurrentFileIndex = i + 1, 
               TotalFiles       = files.Count,
               Message          = $"Стискаємо: {file.FullName}...",
            });

            await Task.Delay(300);

            var compressedData = algorithm.Compress(file.GetOriginalContent());
            file.ApplyCompression(compressedData);
        }

        _currentArchive.SetStatus(ArchiveStatus.Compressed);

        onProgress?.Invoke(new ArchiveProgressEvent
        {
           CurrentFileName  = string.Empty,
           CurrentFileIndex = files.Count,
           TotalFiles       = files.Count,
           Message          = $"Стиснення звершено! Алгоритм {algorithm.Name}", 
        });
    }

    public async Task DecompressAsync(Action<ArchiveProgressEvent>? onProgress = null)
    {
        EnsureArchiveExists();
        EnsureArchiveHasFiles();

        if (_currentArchive!.Status != ArchiveStatus.Compressed)
            throw new InvalidOperationException("Спочатку стисніть архів!");

        var algorithm = CompressionAlgorithmFactory.Create(_currentArchive!.CompressionType);

        var files = _currentArchive.Files.ToList();

        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];

            onProgress?.Invoke(new ArchiveProgressEvent
            {
               CurrentFileName  = file.FullName,
               CurrentFileIndex = i + 1, 
               TotalFiles       = files.Count,
               Message          = $"Розпаковуємо: {file.FullName}...",
            });

            await Task.Delay(300);

            if (file.IsCompressed)
            {
                algorithm.Decompress(file.GetProcessedContent());
                file.RevertCompression();
            }
        }

        _currentArchive.SetStatus(ArchiveStatus.Compressed);

        onProgress?.Invoke(new ArchiveProgressEvent
        {
           CurrentFileName  = string.Empty,
           CurrentFileIndex = files.Count,
           TotalFiles       = files.Count,
           Message          = $"Розпакування завершено!", 
        });
    }

    // --- private methods ---
    private void EnsureArchiveExists()
    {
        if (_currentArchive is null)
            throw new InvalidOperationException("Спочатку створіть архів!");
    }

    private void EnsureArchiveHasFiles()
    {
        if (_currentArchive!.FileCount == 0)
            throw new InvalidOperationException("Додайте файли до архіву перед стисненням!");
    }
}