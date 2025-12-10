using Intello.Models;
using Intello.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace Intello.Pages;

/// <summary>
/// Handles quiz gameplay: question display, answer selection, and feedback.
/// </summary>
public partial class QcmQuizPage : ContentPage
{
    private readonly QcmViewModel _viewModel;
    private readonly Dictionary<Border, Answer> _answerBorders = new();

    public QcmQuizPage(QcmViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadQuestion();
    }

    private void LoadQuestion()
    {
        if (_viewModel.CurrentSet == null || _viewModel.CurrentQuestion == null)
        {
            Shell.Current.GoToAsync("//QcmListPage");
            return;
        }

        SetNameLabel.Text = _viewModel.CurrentSet.Name;
        ProgressLabel.Text = _viewModel.QuestionProgress;
        QuestionLabel.Text = _viewModel.CurrentQuestion.Text;

        UpdateProgressBar();
        BuildAnswerOptions();
        FeedbackPanel.IsVisible = false;
    }

    private void UpdateProgressBar()
    {
        var progress = (double)(_viewModel.CurrentQuestionIndex + 1) / _viewModel.CurrentSet!.Questions.Count;
        ProgressBar.WidthRequest = 120 * progress;
    }

    private void BuildAnswerOptions()
    {
        AnswersStack.Children.Clear();
        _answerBorders.Clear();

        var letters = new[] { "A", "B", "C", "D" };
        for (int i = 0; i < _viewModel.CurrentQuestion!.Answers.Count; i++)
        {
            var answer = _viewModel.CurrentQuestion.Answers[i];
            var letter = i < letters.Length ? letters[i] : "";

            var border = CreateAnswerBorder(answer, letter);
            _answerBorders[border] = answer;
            AnswersStack.Children.Add(border);
        }
    }

    private Border CreateAnswerBorder(Answer answer, string letter)
    {
        var border = new Border
        {
            BackgroundColor = Color.FromArgb("#1f1f2e"),
            Stroke = Color.FromArgb("#2a2a3e"),
            StrokeThickness = 2,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(20, 16)
        };

        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 16
        };

        var letterBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#2a2a3e"),
            StrokeThickness = 0,
            HeightRequest = 36,
            WidthRequest = 36,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Content = new Label
            {
                Text = letter,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#8b8b9e"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };

        var answerLabel = new Label
        {
            Text = answer.Text,
            FontSize = 16,
            TextColor = Colors.White,
            VerticalOptions = LayoutOptions.Center
        };

        grid.Add(letterBorder, 0, 0);
        grid.Add(answerLabel, 1, 0);
        border.Content = grid;

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnAnswerClicked;
        border.GestureRecognizers.Add(tapGesture);

        return border;
    }

    private void OnAnswerClicked(object? sender, TappedEventArgs e)
    {
        if (sender is not Border clickedBorder || _viewModel.IsAnswered) return;

        var selectedAnswer = _answerBorders[clickedBorder];
        _viewModel.SelectAnswer(selectedAnswer);

        UpdateAnswerColors(clickedBorder);
        ShowFeedback();
    }

    private void UpdateAnswerColors(Border clickedBorder)
    {
        foreach (var (border, answer) in _answerBorders)
        {
            var grid = border.Content as Grid;
            var letterBorder = grid?.Children[0] as Border;
            var letterLabel = letterBorder?.Content as Label;

            if (answer.IsCorrect)
            {
                border.BackgroundColor = Color.FromArgb("#1a2a1e");
                border.Stroke = Color.FromArgb("#00b894");
                if (letterBorder != null) letterBorder.BackgroundColor = Color.FromArgb("#00b894");
                if (letterLabel != null) letterLabel.TextColor = Colors.White;
            }
            else if (border == clickedBorder)
            {
                border.BackgroundColor = Color.FromArgb("#2a1a1e");
                border.Stroke = Color.FromArgb("#e94560");
                if (letterBorder != null) letterBorder.BackgroundColor = Color.FromArgb("#e94560");
                if (letterLabel != null) letterLabel.TextColor = Colors.White;
            }

            border.GestureRecognizers.Clear();
        }
    }

    private void ShowFeedback()
    {
        FeedbackPanel.IsVisible = true;

        if (_viewModel.LastAnswerCorrect)
        {
            FeedbackHeader.BackgroundColor = Color.FromArgb("#1a2a1e");
            FeedbackIcon.Text = "✓";
            FeedbackIcon.TextColor = Color.FromArgb("#00b894");
            FeedbackTitle.Text = "Correct!";
            FeedbackTitle.TextColor = Color.FromArgb("#00b894");
        }
        else
        {
            FeedbackHeader.BackgroundColor = Color.FromArgb("#2a1a1e");
            FeedbackIcon.Text = "✗";
            FeedbackIcon.TextColor = Color.FromArgb("#e94560");
            FeedbackTitle.Text = "Incorrect";
            FeedbackTitle.TextColor = Color.FromArgb("#e94560");
        }

        ExplanationLabel.Text = _viewModel.CurrentQuestion?.Explanation ?? "";

        var isLastQuestion = _viewModel.CurrentQuestionIndex >= (_viewModel.CurrentSet?.Questions.Count ?? 0) - 1;
        NextButtonText.Text = isLastQuestion ? "See Results" : "Next Question";
    }

    private async void OnNextClicked(object? sender, TappedEventArgs e)
    {
        if (_viewModel.MoveToNextQuestion())
        {
            LoadQuestion();
        }
        else
        {
            await Shell.Current.GoToAsync("//QcmResultsPage");
        }
    }

    private async void OnQuitClicked(object? sender, TappedEventArgs e)
    {
        bool confirm = await DisplayAlert("Quit Quiz", "Are you sure you want to quit? Your progress will be lost.", "Yes", "No");
        if (confirm)
        {
            _viewModel.Reset();
            await Shell.Current.GoToAsync("//QcmListPage");
        }
    }
}
