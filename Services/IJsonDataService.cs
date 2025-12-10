using Intello.Models;

namespace Intello.Services;

/// <summary>
/// Service interface for loading quiz data from JSON files.
/// </summary>
public interface IJsonDataService
{
    /// <summary>
    /// Loads all QCM question sets from the data source.
    /// </summary>
    Task<List<QcmSet>> LoadQcmSetsAsync();
}
