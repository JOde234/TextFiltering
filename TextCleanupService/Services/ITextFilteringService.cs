namespace TextCleanupService.Services;

public interface ITextFilteringService
{
    public string ApplyFilters( string rawText, Action<string[]> filterHandler, bool normalizeWhitespaces = false );
}