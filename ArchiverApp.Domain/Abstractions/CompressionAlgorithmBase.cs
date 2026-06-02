namespace ArchiverApp.Domain.Abstractions;

public abstract class CompressionAlgorithmBase
{
    // --- abstract props ---
    public abstract string Name { get; }
    public abstract string FileExtension { get; }
    public abstract string Description { get; }

    // --- abstract methods ---
    public abstract byte[] Compress(byte[] data);
    public abstract byte[] Decompress(byte[] data);
    
    // --- virtual methods ---
    public virtual bool CanProcess(byte[] data) => data is { Length: > 0 };

    // --- redefinition ---
    public override string ToString() => $"{Name} -({FileExtension}) – {Description}";
}