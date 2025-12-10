using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Intello.Models;
using Intello.Services;

namespace Intello.ViewModels;

/// <summary>
/// Manages quiz state and user interactions for QCM game mode.
/// </summary>
public class QcmViewModel : INotifyPropertyChanged
{
    private readonly IJsonDataService _dataService;

    public ObservableCollection<QcmSet> QuestionSets { get; } = new();

    private QcmSet? _currentSet;
    public QcmSet? CurrentSet
    {
        get => _currentSet;
        set { _currentSet = value; OnPropertyChanged(); }
    }

    private Question? _currentQuestion;
    public Question? CurrentQuestion
    {
        get => _currentQuestion;
        set { _currentQuestion = value; OnPropertyChanged(); }
    }

    private int _currentQuestionIndex;
    public int CurrentQuestionIndex
    {
        get => _currentQuestionIndex;
        set
        {
            _currentQuestionIndex = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(QuestionProgress));
        }
    }

    public string QuestionProgress => CurrentSet != null
        ? $"Question {CurrentQuestionIndex + 1} / {CurrentSet.Questions.Count}"
        : "";

    private bool _isAnswered;
    public bool IsAnswered
    {
        get => _isAnswered;
        set { _isAnswered = value; OnPropertyChanged(); }
    }

    private Answer? _selectedAnswer;
    public Answer? SelectedAnswer
    {
        get => _selectedAnswer;
        set { _selectedAnswer = value; OnPropertyChanged(); }
    }

    private bool _lastAnswerCorrect;
    public bool LastAnswerCorrect
    {
        get => _lastAnswerCorrect;
        set { _lastAnswerCorrect = value; OnPropertyChanged(); }
    }

    public List<QuizResult> Results { get; } = new();
    public int CorrectCount => Results.Count(r => r.WasCorrect);
    public int TotalCount => Results.Count;
    public List<QuizResult> Errors => Results.Where(r => !r.WasCorrect).ToList();

    public QcmViewModel(IJsonDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Loads available question sets from the data service.
    /// </summary>
    public async Task LoadSetsAsync()
    {
        var sets = await _dataService.LoadQcmSetsAsync();
        QuestionSets.Clear();
        foreach (var set in sets)
        {
            QuestionSets.Add(set);
        }
    }

    /// <summary>
    /// Initializes a new quiz session with the selected set.
    /// </summary>
    public void StartQuiz(QcmSet set)
    {
        CurrentSet = set;
        CurrentQuestionIndex = 0;
        Results.Clear();
        IsAnswered = false;
        SelectedAnswer = null;
        LoadCurrentQuestion();
    }

    private void LoadCurrentQuestion()
    {
        if (CurrentSet != null && CurrentQuestionIndex < CurrentSet.Questions.Count)
        {
            CurrentQuestion = CurrentSet.Questions[CurrentQuestionIndex];
            IsAnswered = false;
            SelectedAnswer = null;
        }
    }

    /// <summary>
    /// Records the user's answer selection and evaluates correctness.
    /// </summary>
    public void SelectAnswer(Answer answer)
    {
        if (IsAnswered || CurrentQuestion == null) return;

        SelectedAnswer = answer;
        LastAnswerCorrect = answer.IsCorrect;
        IsAnswered = true;

        Results.Add(new QuizResult
        {
            Question = CurrentQuestion,
            SelectedAnswer = answer,
            WasCorrect = answer.IsCorrect
        });
    }

    /// <summary>
    /// Advances to the next question. Returns false if quiz is complete.
    /// </summary>
    public bool MoveToNextQuestion()
    {
        if (CurrentSet == null) return false;

        CurrentQuestionIndex++;
        if (CurrentQuestionIndex >= CurrentSet.Questions.Count)
        {
            return false;
        }

        LoadCurrentQuestion();
        return true;
    }

    /// <summary>
    /// Resets all quiz state for a new session.
    /// </summary>
    public void Reset()
    {
        CurrentSet = null;
        CurrentQuestion = null;
        CurrentQuestionIndex = 0;
        Results.Clear();
        IsAnswered = false;
        SelectedAnswer = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
