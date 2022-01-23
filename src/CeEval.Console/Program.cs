using CeEval.Shared.Extensions;
using CeEval.Shared.Models;
using CeEval.Shared.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using static System.Console;

AnsiConsole.Markup("[bold red]Top 5 Sold Products[/]");
WriteLine();

await PrintTopSoldProducts();

PrintMessage();
var input = string.Empty;

while (input != "q")
{
    input = ReadLine();

    if (input == "q")
        break;

    if (input == "r")
    {
        await PrintTopSoldProducts();
        PrintMessage();
    }
}

return 0;

IServiceProvider BuildServiceProvider()
{
    var services = new ServiceCollection();

    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddEnvironmentVariables()
        .Build();

    var loggingConfig = configuration.GetSection("Logging");

    services.AddSingleton<IConfiguration>(configuration);
    services.AddLogging(builder => builder.AddConsole().AddConfiguration(loggingConfig));
    services.AddCeEval(configuration);

    return services.BuildServiceProvider();
}

Table GetTable()
{
    var table = new Table();
    table.AddColumn(new TableColumn("[yellow]GTIN[/]").Centered());
    table.AddColumn(new TableColumn("[yellow]Name[/]").Centered());
    table.AddColumn(new TableColumn("[yellow]Quantity[/]").Centered());

    return table;
}

void PrintMessage()
{
    Write("Type 'q' to quit and 'r' to fetch products again: ");
}

async Task PrintTopSoldProducts()
{
    var serviceProvider = BuildServiceProvider();
    await using var scope = serviceProvider.CreateAsyncScope();

    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var products = await mediator.Send(new ProductTopSoldListQuery(OrderStatus.InProgress, 5));

    var table = GetTable();
    foreach (var product in products)
        table.AddRow(product.Gtin, product.Name, product.Quantity.ToString());

    AnsiConsole.Write(table);
    WriteLine();
}