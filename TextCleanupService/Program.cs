using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using TextCleanupService.Config;
using TextCleanupService.ProcessingMethodSets;
using TextCleanupService.Services;

var builder = Host.CreateDefaultBuilder( args );
builder = builder.ConfigureAppConfiguration( config => 
        config.AddJsonFile( "appsettings.json", false ) );

var host = builder
        .UseDefaultServiceProvider( options => options.ValidateOnBuild = true )
        .ConfigureServices( ConfigureServices )
        .Build();

using var serviceScope = host.Services.CreateScope();
var serviceProvider = serviceScope.ServiceProvider;

var textLoader = serviceProvider.GetRequiredService<ITextLoader>();
var dataConfig = serviceProvider.GetService<IOptions<DataConfig>>()!.Value;

try
{
    var rawText = await textLoader.Load( dataConfig.TxtFilename );

    var textFilteringService = ( ITextFilteringService )host.Services.GetService( typeof(ITextFilteringService) )!;
    
    var textFilters = new TextFilters();
    var filterHandler = textFilters.RemoveWordsVowelInTheMiddle;
    filterHandler += textFilters.RemoveWordsShorterThan3Letters;
    filterHandler += textFilters.RemoveWordsWithLetterT;
    
    Console.WriteLine($"Output with preserved white spaces: {textFilteringService.ApplyFilters( rawText, filterHandler )}");
    Console.WriteLine($"Output with normalized white spaces: {textFilteringService.ApplyFilters( rawText, filterHandler, true )}");
}
catch( Exception e )
{
    throw new ApplicationException($"Unhandled exception occurred: {e.Message}");
}

void ConfigureServices( HostBuilderContext context, IServiceCollection services )
{
    services.Configure<DataConfig>( context.Configuration.GetSection( nameof( DataConfig ) ) );
    services.AddSingleton<ITextLoader, TextLoader>();
    services.AddScoped<ITextFilteringService, TextFilteringService>();
}