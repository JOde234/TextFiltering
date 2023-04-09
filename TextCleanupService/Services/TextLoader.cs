namespace TextCleanupService.Services;

public class TextLoader: ITextLoader
{
    public async Task<string> Load( string txtFilename )
    {
        try
        {
            var filePath = $"./Data/{txtFilename}";

            // Read entire text file content in one string
            var text = await File.ReadAllTextAsync( filePath );
            return text;
        }
        catch( FileNotFoundException )
        {
            throw new FileNotFoundException( $"The file {txtFilename} cannot be found" );
        }
        catch( DirectoryNotFoundException )
        {
            throw new DirectoryNotFoundException( $"The file location path is invalid, check the file to be placed in Data directory within the solution" );
        }
        catch( Exception e )
        {
            throw new ApplicationException( $"Unhandled exception occurred: {e.Message}" );
        }
    }
}