namespace ArchiverApp.Domain.Events;

public class ArchiveProgressEvent
{
    public string CurrentFileName { get; init; } = string.Empty;
    public int CurrentFileIndex { get; init; }
    public int TotalFiles { get; init; }
    public int ProgressPercentage => TotalFiles == 0 ? 0 : (int)((double)CurrentFileIndex / TotalFiles * 100);

    public string Message { get; init; } = string.Empty;
}