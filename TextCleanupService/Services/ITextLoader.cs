namespace TextCleanupService.Services;

public interface ITextLoader
{
    public Task<string> Load( string txtFilename );
}