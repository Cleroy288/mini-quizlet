using Intello.Models;
using Intello.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace Intello.Pages;

/// <summary>
/// Displays quiz results with score and error review.
/// </summary>
public partial class QcmResultsPage : ContentPage
{
    private readonly QcmViewModel _viewModel;

    public QcmResultsPage(QcmViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadResults();
    }

    private void LoadResults()
    {
        DisplayScore();
        UpdateResultIcon();
        DisplayErrors();
    }

    private void DisplayScore()
    {
        ScoreLabel.Text = $"{_viewModel.CorrectCount}/{_viewModel.TotalCount}";
        var percentage = _viewModel.TotalCount > 0
            ? (_viewModel.CorrectCount * 100 / _viewModel.TotalCount)
            : 0;
        PercentLabel.Text = $"{percentage}%";
    }

    private void UpdateResultIcon()
    {
        var percentage = _viewModel.TotalCount > 0
            ? (_viewModel.CorrectCount * 100 / _viewModel.TotalCount)
            : 0;

        if (percentage >= 80)
        {
            PercentLabel.TextColor = Color.FromArgb("#00b894");
            ResultIconBorder.BackgroundColor = Color.FromArgb("#1a241a");
            ResultIcon.Text = "✓";
            ResultIcon.TextColor = Color.FromArgb("#00b894");
        }
        else if (percentage >= 50)
        {
            PercentLabel.TextColor = Color.FromArgb("#f39c12");
            ResultIconBorder.BackgroundColor = Color.FromArgb("#24241a");
            ResultIcon.Text = "~";
            ResultIcon.TextColor = Color.FromArgb("#f39c12");
        }
        else
        {
            PercentLabel.TextColor = Color.FromArgb("#e94560");
            ResultIconBorder.BackgroundColor = Color.FromArgb("#241a1a");
            ResultIcon.Text = "×";
            ResultIcon.TextColor = Color.FromArgb("#e94560");
        }
    }

    private void DisplayErrors()
    {
        var errors = _viewModel.Errors;
        if (errors.Count == 0)
        {
            ErrorsSection.IsVisible = false;
            AllCorrectCard.IsVisible = true;
            return;
        }

        ErrorsSection.IsVisible = true;
        AllCorrectCard.IsVisible = false;
        ErrorsList.Children.Clear();

        int errorNum = 1;
        foreach (var error in errors)
        {
            var correctAnswer = error.Question.Answers.FirstOrDefault(a => a.IsCorrect);
            var errorCard = BuildErrorCard(error, correctAnswer, errorNum);
            ErrorsList.Children.Add(errorCard);
            errorNum++;
        }
    }

    private Border BuildErrorCard(QuizResult error, Answer? correctAnswer, int errorNum)
    {
        var border = new Border
        {
            BackgroundColor = Color.FromArgb("#1a1a2e"),
            Stroke = Color.FromArgb("#2a2a3e"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            Padding = new Thickness(0)
        };

        var mainGrid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            }
        };

        // Header
        var headerBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#2a1a1e"),
            StrokeThickness = 0,
            Padding = new Thickness(24, 16)
        };
        var headerStack = new HorizontalStackLayout { Spacing = 12 };
        headerStack.Add(new Border
        {
            BackgroundColor = Color.FromArgb("#e94560"),
            StrokeThickness = 0,
            Padding = new Thickness(10, 4),
            StrokeShape = new RoundRectangle { CornerRadius = 6 },
            Content = new Label
            {
                Text = $"#{errorNum}",
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.White
            }
        });
        headerStack.Add(new Label
        {
            Text = error.Question.Text,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            VerticalOptions = LayoutOptions.Center,
            LineBreakMode = LineBreakMode.TailTruncation
        });
        headerBorder.Content = headerStack;
        mainGrid.Add(headerBorder, 0, 0);

        // Answer comparison
        var answersStack = new VerticalStackLayout { Padding = new Thickness(24, 20), Spacing = 12 };
        
        var yourAnswerStack = new HorizontalStackLayout { Spacing = 12 };
        yourAnswerStack.Add(new Label { Text = "Your answer:", FontSize = 13, TextColor = Color.FromArgb("#6b6b7e"), WidthRequest = 100 });
        yourAnswerStack.Add(new Border
        {
            BackgroundColor = Color.FromArgb("#2a1a1e"),
            Stroke = Color.FromArgb("#e94560"),
            StrokeThickness = 1,
            Padding = new Thickness(12, 8),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Content = new Label { Text = error.SelectedAnswer.Text, FontSize = 14, TextColor = Color.FromArgb("#e94560") }
        });
        answersStack.Add(yourAnswerStack);

        var correctStack = new HorizontalStackLayout { Spacing = 12 };
        correctStack.Add(new Label { Text = "Correct:", FontSize = 13, TextColor = Color.FromArgb("#6b6b7e"), WidthRequest = 100 });
        correctStack.Add(new Border
        {
            BackgroundColor = Color.FromArgb("#1a2a1e"),
            Stroke = Color.FromArgb("#00b894"),
            StrokeThickness = 1,
            Padding = new Thickness(12, 8),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Content = new Label { Text = correctAnswer?.Text ?? "N/A", FontSize = 14, TextColor = Color.FromArgb("#00b894") }
        });
        answersStack.Add(correctStack);
        mainGrid.Add(answersStack, 0, 1);

        // Explanation
        var explanationBorder = new Border { BackgroundColor = Color.FromArgb("#1f1f2e"), StrokeThickness = 0, Padding = new Thickness(24, 16) };
        var explanationStack = new VerticalStackLayout { Spacing = 8 };
        explanationStack.Add(new Label { Text = "EXPLANATION", FontSize = 10, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#5b5b6e"), CharacterSpacing = 1 });
        explanationStack.Add(new Label { Text = error.Question.Explanation, FontSize = 14, TextColor = Color.FromArgb("#a0a0b0"), LineBreakMode = LineBreakMode.WordWrap });
        explanationBorder.Content = explanationStack;
        mainGrid.Add(explanationBorder, 0, 2);

        border.Content = mainGrid;
        return border;
    }

    private async void OnTryAnotherClicked(object? sender, TappedEventArgs e)
    {
        _viewModel.Reset();
        await Shell.Current.GoToAsync("//QcmListPage");
    }

    private async void OnDashboardClicked(object? sender, TappedEventArgs e)
    {
        _viewModel.Reset();
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
