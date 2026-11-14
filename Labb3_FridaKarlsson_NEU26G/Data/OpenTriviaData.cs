

using System.Net.Http;
using Labb3_FridaKarlsson_NEU26G.Models;
using System.Text.Json;
using System.Net;


namespace Labb3_FridaKarlsson_NEU26G.Data;

class OpenTriviaData
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<Question>> GetQuestionsAsync(int amount, int? categoryId, string difficulty)
    {

        string triviaUrl = BuildTriviaUrl(amount, categoryId, difficulty);

        string json = await _httpClient.GetStringAsync(triviaUrl);

        var triviaResponse = JsonSerializer.Deserialize<TriviaResponse>(json);

        if (triviaResponse != null && triviaResponse.ResponseCode == 0)
        {
            return DecodeResults(triviaResponse);
        }
        else
        {
            string errorMessage = GetErrorMessage(triviaResponse);

            throw new Exception(errorMessage);
        }
    }


    private static string BuildTriviaUrl(int amount, int? categoryId, string difficulty)
    {
        string triviaAddress = $"https://opentdb.com/api.php?amount={amount}&type=multiple";

        if (categoryId > 0) 
        {
            triviaAddress += $"&category={categoryId}";
        }

        if (!string.IsNullOrEmpty(difficulty) && difficulty != "Any")
        {
            triviaAddress += $"&difficulty={difficulty.ToLower()}";
        }

        return triviaAddress;
    }


    private static List<Question> DecodeResults(TriviaResponse triviaResponse)
    {
        var questions = new List<Question>();

        foreach (var result in triviaResponse.Results)
        {
            Question newTriviaQuestion = DecodeQuestion(result);

            questions.Add(newTriviaQuestion);

        }
        return questions;
    }

    private static Question DecodeQuestion(TriviaQuestion result)
    {
        string decodedQuestion = WebUtility.HtmlDecode(result.Question);
        string decodedCorrectAnswer = WebUtility.HtmlDecode(result.CorrectAnswer);
        string decodedIncorrectAnswer1 = WebUtility.HtmlDecode(result.IncorrectAnswers[0]);
        string decodedIncorrectAnswer2 = WebUtility.HtmlDecode(result.IncorrectAnswers[1]);
        string decodedIncorrectAnswer3 = WebUtility.HtmlDecode(result.IncorrectAnswers[2]);

        Question newTriviaQuestion = new Question(decodedQuestion, decodedCorrectAnswer, decodedIncorrectAnswer1, decodedIncorrectAnswer2, decodedIncorrectAnswer3);
        return newTriviaQuestion;
    }

    private static string GetErrorMessage(TriviaResponse? triviaResponse)
    {
        string errorMessage = string.Empty;

        switch (triviaResponse?.ResponseCode)
        {
            case 1:
                errorMessage = "No Results: There´s not enough question for your inquiry";
                break;
            case 2:
                errorMessage = "Invalid Parameter: Arguments passed in aren't valid.";
                break;
            case 3:
                errorMessage = "Token Not Found: Session token does not exist.";
                break;
            case 4:
                errorMessage = "Token Empty: Session token has returned all possible questions.";
                break;
            case 5:
                errorMessage = "Rate Limit: Too many requests have occured. Try again later.";
                break;
            default:
                errorMessage = "Unknown error from Open Trivia Database";
                break;
        }

        return errorMessage;
    }


    public async Task<List<TriviaCategory>> GetTriviaCategoriesAsync()
    {
        List<TriviaCategory> categories = new List<TriviaCategory>();
        categories.Add(new TriviaCategory { Id = 0, Name = "Any" });

        string json = await _httpClient.GetStringAsync("https://opentdb.com/api_category.php");

        var triviaCategories = JsonSerializer.Deserialize<TriviaCategoryResponse>(json);

        if (triviaCategories != null)
        {

            foreach (var category in triviaCategories.TriviaCategories)
            {
                category.Name = WebUtility.HtmlDecode(category.Name);
                categories.Add(category);
            }
        }

        return categories;
    }
}
