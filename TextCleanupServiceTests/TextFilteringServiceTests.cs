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
        
        textFilteringService = new TextFilteringService(textLoader.Object);
        textFilters = new TextFilters();
    }

    [Fact]
    public async void ShouldRemoveWordsWithVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsVowelInTheMiddle;
        const string expected = "beginning tired, and nothing : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldRemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsShorterThan3Letters;
        const string expected = "Alice was beginning get very tired, and having nothing : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldRemoveWordsWithLetterT()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        const string expected = "Alice was beginning very , and of having do: ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldApplyTwoFilters_RemoveWordsWithLetterT_RemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsShorterThan3Letters;
        const string expected = "Alice was beginning very , and having : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldApplyTwoFilters_RemoveWordsShorterThan3Letters_RemoveWordsVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsShorterThan3Letters;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        const string expected = "beginning tired, and nothing : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldApplyTwoFilters_RemoveWordsWithLetterT_RemoveWordsVowelInTheMiddle()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        const string expected = "beginning , and : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldApplyThreeFilters_RemoveWordsWithLetterT_RemoveWordsVowelInTheMiddle_RemoveWordsShorterThan3Letters()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        filterHandler += textFilters.RemoveWordsShorterThan3Letters;
        filterHandler += textFilters.RemoveWordsVowelInTheMiddle;
        const string expected = "beginning , and : ";
        var result = await textFilteringService.ApplyFilters( "test.txt", filterHandler, true );
        result.ShouldBe( expected );
    }
    
    [Fact]
    public async void ShouldThrowOnEmptyInput()
    {
        var filterHandler = textFilters.RemoveWordsWithLetterT;
        
        var exception = await Record.ExceptionAsync(() => textFilteringService.ApplyFilters( "test_empty.txt", filterHandler ));
        exception?.Message.ShouldContain("Cannot apply filters to the empty input");
    }
}