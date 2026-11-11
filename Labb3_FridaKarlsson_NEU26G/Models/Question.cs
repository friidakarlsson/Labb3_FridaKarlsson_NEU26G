
using System.Text.Json.Serialization;

namespace Labb3_FridaKarlsson_NEU26G.Models;

public class Question
{
    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
    {
        Query = query;
        CorrectAnswer = correctAnswer;
        IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, incorrectAnswer3]; 
    }

    public Question()
    {

    }

    public string Query { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string[] IncorrectAnswers { get; set; } = Array.Empty<string>();

}
