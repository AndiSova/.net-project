using CommandLine;
using Mapster.Service.Endpoints;

namespace Mapster.Service;

public class Options
{
    [Option('i', "input", Required = true, HelpText = "Path to map data binary file")]
    public string? MapDataPath { get; set; }
}

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Options? arguments = null;
        var argParseResult = Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
        {
            arguments = options;
        });

        if (argParseResult.Errors.Any())
        {
            Environment.Exit(-1);
        }

        var appBuilder = WebApplication.CreateBuilder();

        var tileEndpoint = new TileEndpoint(arguments!.MapDataPath!);
        appBuilder.Services.AddSingleton(_ => tileEndpoint);

        var app = appBuilder.Build();
        app.Urls.Add("http://localhost:8080");

        TileEndpoint.Register(app);

        await app.RunAsync();
    }
}
