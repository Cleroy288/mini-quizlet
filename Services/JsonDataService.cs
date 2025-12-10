using System.Text.Json;
using Intello.Models;

namespace Intello.Services;

/// <summary>
/// Loads quiz data from embedded JSON resources.
/// </summary>
public class JsonDataService : IJsonDataService
{
    public async Task<List<QcmSet>> LoadQcmSetsAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("qcm.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var data = JsonSerializer.Deserialize<QcmData>(json);
            return data?.Sets ?? new List<QcmSet>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load QCM data: {ex.Message}");
            return new List<QcmSet>();
        }
    }
}
