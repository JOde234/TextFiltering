using System.Text;
using System.Text.RegularExpressions;

namespace TextCleanupService.Services;

public class TextFilteringService: ITextFilteringService
{
    public string ApplyFilters( string rawText, Action<string[]> filterHandler, bool normalizeWhitespaces = false )
    {
        if( rawText.Length == 0 ) throw new InvalidDataException( "Cannot apply filters to the empty input" );
        var words = Regex.Split( rawText, @"\b" );
        filterHandler( words );
        return normalizeWhitespaces ? NormalizeWhitespace(words) : string.Join("", words);
    }

    private static string NormalizeWhitespace( string[] words )
    {
        if ( words.Length == 0)
            return string.Empty;
        
        var outputStringBuilder = new StringBuilder();
        var skipWS = false;

        for (var i = 0; i < words.Length; i++) { 
            if (string.IsNullOrWhiteSpace(words[i])) {
                if( !skipWS )
                {
                    skipWS = true;
                }
                else words[i] = string.Empty;
            }
            else skipWS = false;
            outputStringBuilder.Append( words[ i ] );
        }
        
        return outputStringBuilder.ToString();
    }
}