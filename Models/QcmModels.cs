using System.Text.Json.Serialization;

namespace Intello.Models;

/// <summary>
/// Root container for QCM data loaded from JSON.
/// </summary>
public class QcmData
{
    [JsonPropertyName("sets")]
    public List<QcmSet> Sets { get; set; } = new();
}

/// <summary>
/// A collection of related questions with metadata.
/// </summary>
public class QcmSet
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("questions")]
    public List<Question> Questions { get; set; } = new();
}

/// <summary>
/// A single quiz question with multiple choice answers.
/// </summary>
public class Question
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("answers")]
    public List<Answer> Answers { get; set; } = new();

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = string.Empty;
}

/// <summary>
/// An answer option for a question.
/// </summary>
public class Answer
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; set; }
}

/// <summary>
/// Tracks user response for a single question.
/// </summary>
public class QuizResult
{
    public Question Question { get; set; } = null!;
    public Answer SelectedAnswer { get; set; } = null!;
    public bool WasCorrect { get; set; }
}
