using System.IO;
using System.Text.Json;
using Labb3_FridaKarlsson_NEU26G.Models;

namespace Labb3_FridaKarlsson_NEU26G.Data;

internal class JsonData
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

    public JsonData()
    {
        var dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _filePath = Path.Combine(dataFolder, "Labb3_FridaKarlsson_NEU26G", "question_packs.json");
    }


    public async Task SavePacksAsync(IEnumerable<QuestionPack> packs)
    {
        try
        {
            var directoryPath = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string jsonText = JsonSerializer.Serialize(packs, _options);

            await File.WriteAllTextAsync(_filePath, jsonText);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Kunde inte spara datan:" + ex.Message);
        }
    }


    public async Task<List<QuestionPack>> LoadPacksAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<QuestionPack>();
        }

        try
        {
            string jsonText = await File.ReadAllTextAsync(_filePath);

            var packs = JsonSerializer.Deserialize<List<QuestionPack>>(jsonText, _options);

            return packs ?? new List<QuestionPack>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Kunde inte ladda datan:" + ex.Message);

            return new List<QuestionPack>();
        }
    }

}
