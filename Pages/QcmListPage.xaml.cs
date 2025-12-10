using Intello.Models;
using Intello.ViewModels;

namespace Intello.Pages;

/// <summary>
/// Displays available QCM question sets for selection.
/// </summary>
public partial class QcmListPage : ContentPage
{
    private readonly QcmViewModel _viewModel;

    public QcmListPage(QcmViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        SetsCollection.ItemsSource = _viewModel.QuestionSets;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSetsAsync();
    }

    private async void OnSetTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is QcmSet set)
        {
            _viewModel.StartQuiz(set);
            await Shell.Current.GoToAsync("//QcmQuizPage");
        }
    }

    private async void OnBackClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
