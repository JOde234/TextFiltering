using System.Text.RegularExpressions;

namespace TextCleanupService.ProcessingMethodSets;

public class TextFilters
{
    private static readonly char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
    
    public void RemoveWordsVowelInTheMiddle( string[] words )
    {
        if (words.Length == 0) return;
        
        for( var i = 0; i < words.Length; i++ )
        {
            if (!Regex.Match(words[i], "\\w+").Success) continue;

            var middle = GetMiddle( words[ i ] ).ToLower();

            if (middle.Any(c => vowels.Contains(c)))
                words[i] = string.Empty;
        }
        
        //Console.WriteLine("RemoveWordsVowelInTheMiddle processed");
    }
    
    public void RemoveWordsShorterThan3Letters(string[] words)
    {
        if (words.Length == 0) return;

        for( var i = 0; i < words.Length; i++ )
        {
            if (!Regex.Match(words[i], "\\w+").Success) continue;
            
            if (words[i].Length < 3) 
                words[i] = string.Empty;
        }
        
        //Console.WriteLine("RemoveWordsShorterThan3Letters processed");
    }
    
    public void RemoveWordsWithLetterT(string[] words)
    {
        if (words.Length == 0) return;

        for( var i = 0; i < words.Length; i++ )
        {
            if (words[i].Contains('t', StringComparison.OrdinalIgnoreCase )) 
                words[i] = string.Empty;
        }
        
        //Console.WriteLine("RemoveWordsWithLetterT processed");
    }
    
    private static string GetMiddle(string s)
    {
        var l = s.Length;
        if ( l <= 2 ) return s;
        if ( l % 2 == 1 ) return s[l/2].ToString();
        return s.Substring( l/2 - 1, 2 );
    }
}