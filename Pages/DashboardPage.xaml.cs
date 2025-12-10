namespace Intello.Pages;

/// <summary>
/// Main landing page displaying available game modes.
/// </summary>
public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void OnQcmTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//QcmListPage");
    }

    private async void OnFlashcardTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//FlashcardPage");
    }

    private async void OnCodeGameTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CodeGamePage");
    }
}
