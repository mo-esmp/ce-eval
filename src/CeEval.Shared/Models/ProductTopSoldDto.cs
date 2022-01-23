namespace CeEval.Shared.Models;

/// <summary>
///   The product top sold.
/// </summary>
/// <param name="Gtin">The global trade item number (GTIN).</param>
/// <param name="Name">Total product name.</param>
/// <param name="Quantity">Total sales amount.</param>
public record ProductTopSoldDto(string Gtin, string Name, int Quantity);