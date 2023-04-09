using Moq;
using Shouldly;
using Xunit;
using TextCleanupService.ProcessingMethodSets;
using TextCleanupService.Services;

namespace TextCleanupServiceTests;

public class TextFilteringServiceTests
{
    private readonly Mock<ITextLoader> textLoader;
    private readonly ITextFilteringService textFilteringService;
    private readonly TextFilters textFilters;

    public TextFilteringServiceTests()
    {
        textLoader = new Mock<ITextLoader>();
        textLoader.Setup( tl => tl.Load( "test.txt" )).ReturnsAsync("Alice was beginning to get very tired, and of having nothing to do: ");
        textLoader.Setup( tl => tl.Load( "test_empty.txt" ) ).ReturnsAsync( "" );
        
        textFilteringService = new TextFilteringService();
        textFilters = new TextFilters();
    }

    [Fact]
    public void ShouldRemoveWordsWithVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsVowelInTheMiddle;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "beginning tired, and nothing : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldRemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsShorterThan3Letters;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "Alice was beginning get very tired, and having nothing : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldRemoveWordsWithLetterT()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "Alice was beginning very , and of having do: ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldApplyTwoFilters_RemoveWordsWithLetterT_RemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsShorterThan3Letters;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "Alice was beginning very , and having : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldApplyTwoFilters_RemoveWordsShorterThan3Letters_RemoveWordsVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsShorterThan3Letters;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "beginning tired, and nothing : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldApplyTwoFilters_RemoveWordsWithLetterT_RemoveWordsVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "beginning , and : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldApplyThreeFilters_RemoveWordsWithLetterT_RemoveWordsVowelInTheMiddle_RemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsShorterThan3Letters;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        var rawText = textLoader.Object.Load( "test.txt" ).Result;
        const string expected = "beginning , and : ";
        textFilteringService.ApplyFilters( rawText, filterHandler, true ).ShouldBe( expected );
    }
    
    [Fact]
    public void ShouldThrowOnEmptyInput()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        var rawText = textLoader.Object.Load( "test_empty.txt" ).Result;
        
        var exception = Record.Exception(() => textFilteringService.ApplyFilters( rawText, filterHandler ));
        exception?.Message.ShouldContain("Cannot apply filters to the empty input");
    }
}