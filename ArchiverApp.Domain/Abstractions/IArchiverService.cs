using ArchiverApp.Domain.Models;

namespace ArchiverApp.Domain.Abstractions;

public interface IArchiverService
{
    Archive CreateArchive(string name, Domain.Enums.CompressionType compressionType);
    void AddFile(string name, string extension, byte[] content);
    bool RemoveFile(string fullName);
    Task CompressAsync(Action<Events.ArchiveProgressEvent>? onProgress = null); 
    Task DecompressAsync(Action<Events.ArchiveProgressEvent>? onProgress = null); 
    void ClearArchive();
}

