namespace TextCleanupService.Services;

public interface ITextFilteringService
{
    public Task<string> ApplyFilters( string txtFilename, Action<string[]> filterHandler, bool normalizeWhitespaces = false );
}