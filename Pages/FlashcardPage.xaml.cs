namespace Intello.Pages;

/// <summary>
/// Placeholder page for flashcard game mode.
/// </summary>
public partial class FlashcardPage : ContentPage
{
    public FlashcardPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
