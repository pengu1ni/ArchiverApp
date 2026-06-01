namespace ArchiverApp.Domain.Models;

public class ArchivedFile
{
    // --- private fields ---
    private readonly string _name;
    private readonly string _extension;
    private readonly byte[] _originalContent;
    private byte[] _processedContent;
    private long _originalSize;
    private long _compressedSize;
    private bool _isCompressed;
    private DateTime _addedAt;

    // --- creating new file ---
    public ArchivedFile(string name, string extension, byte[] content)
    {
        _name             = name.Trim();
        _extension        = extension.StartsWith(".") ? extension : $".{extension}";
        _originalContent  = content;
        _processedContent = content;
        _originalSize     = content.Length;
        _isCompressed     = false;
        _addedAt          = DateTime.UtcNow;
    }

    // --- public fields ---
    public string Name => _name;
    public string Extension => _extension;
    public string FullName => $"{_name}{_extension}";
    public long OriginalSize => _originalSize;
    public long ProcessedSize => _compressedSize;
    public bool isCompressed => _isCompressed;
    public DateTime AddedAt => _addedAt;

    // --- compression ratio (%) ---
    public double CompressionRatio
    {
        get
        {
            if (!_isCompressed || _originalSize == 0)
                return 0.0;

            return Math.Round((1.0 - (double)_compressedSize / _originalSize) * 100, 2);
        }
    }

    // --- writing compression result in file ---
    internal void ApplyCompression(byte[] compressedContent)
    {
        if (compressedContent is null || compressedContent.Length == 0)
            throw new ArgumentException("Стиснений вміст не може бути порожнім!");

        _processedContent = compressedContent;
        _compressedSize = compressedContent.Length;
        _isCompressed = true;
    }

    // --- unpacking method ---
    internal void RevertCompression()
    {
        _processedContent = (byte[])_originalContent.Clone();
        _compressedSize = _originalSize;
        _isCompressed = false;
    }

    // --- redefinition ---
    public override string ToString()
    {
        return $"{FullName} | Оригінал: {OriginalSize} байт | Після обробки: {ProcessedSize} байт | Стиснено: {_isCompressed}";
    }
}