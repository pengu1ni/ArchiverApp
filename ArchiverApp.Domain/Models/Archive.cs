using ArchiverApp.Domain.Enums;

namespace ArchiverApp.Domain.Models;

public class Archive
{
    // --- private fields ---
    private readonly string _name;
    private readonly List<ArchivedFile> _files;
    private CompressionType _compressionType;
    private ArchiveStatus _status;
    private DateTime _createdAt;
    private DateTime? _lastModifiedAt;
    private const int MinNameLength = 2;
    private const int MaxNameLength = 20;

    // --- constructor ---
    public Archive(string name, CompressionType compressionType = CompressionType.Zip)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва архіву не може бути порожньою!", nameof(name));

        if (name.Trim().Length < MinNameLength)
            throw new ArgumentException($"Назва архіву має містити щонайменше {MinNameLength} символи.", nameof(name));

        if (name.Trim().Length > MaxNameLength)
            throw new ArgumentException($"Назва архіву не може перевищувати {MaxNameLength} символів.", nameof(name));

        _name            = name.Trim();
        _files           = new List<ArchivedFile>();
        _compressionType = compressionType;
        _status          = ArchiveStatus.New;
        _createdAt       = DateTime.UtcNow;
        _lastModifiedAt  = null;
    }

    // --- public props ---
    public string Name => _name;
    public CompressionType CompressionType => _compressionType;
    public ArchiveStatus Status => _status;
    public DateTime CreatedAt => _createdAt;
    public DateTime? LastModifiedAt => _lastModifiedAt;
    public int FileCount => _files.Count;
    public long TotalOriginalSize => _files.Sum(f => f.OriginalSize);
    public long TotalProcessedSize => _files.Sum(f => f.ProcessedSize);
    public IReadOnlyList<ArchivedFile> Files => _files.AsReadOnly();

    // --- public methods ---
    public void AddFile(ArchivedFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (_files.Any(f => f.FullName.Equals(file.FullName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Файл '{file.FullName}' вже існує в архіві!");

        _files.Add(file);
        UpdateModified();
    }

    public bool RemoveFile(string fullName)
    {
        var file = _files.FirstOrDefault(f => f.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));

        if (file is null) 
            return false;

        _files.Remove(file);
        UpdateModified();
        return true;
    }

    public void ChangeCompressionType(CompressionType newType)
    {
        if (_compressionType != newType) return;
        
        _compressionType = newType;
        _status          = ArchiveStatus.New;
        UpdateModified();
    }

    internal void SetStatus(ArchiveStatus status)
    {
        _status = status;
        UpdateModified();
    }

    public void UpdateModified() => _lastModifiedAt = DateTime.UtcNow;

    // --- redefinition ---
    public override string ToString() =>
        $"Архів: {_name} | Тип: {_compressionType} | Файлів: {_files.Count} | Статус: {_status}";
}
