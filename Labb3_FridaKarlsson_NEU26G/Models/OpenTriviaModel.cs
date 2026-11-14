using System.Text.Json.Serialization;

namespace Labb3_FridaKarlsson_NEU26G.Models;


internal class TriviaResponse
{
    [JsonPropertyName("response_code")]
    public int ResponseCode { get; set; }

    [JsonPropertyName("results")]
    public List<TriviaQuestion> Results { get; set; } = [];

}

internal class TriviaQuestion
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("correct_answer")]
    public string CorrectAnswer { get; set; } = string.Empty;

    [JsonPropertyName("incorrect_answers")]
    public string[] IncorrectAnswers { get; set; } = [];

}

internal enum TriviaDifficulty
{
    Any, Easy, Medium, Hard
}

internal class TriviaCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

internal class TriviaCategoryResponse
{
    [JsonPropertyName("trivia_categories")]
    public List<TriviaCategory> TriviaCategories { get; set; } = [];
}
