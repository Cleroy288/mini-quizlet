namespace Intello.Pages;

/// <summary>
/// Placeholder page for code game mode.
/// </summary>
public partial class CodeGamePage : ContentPage
{
    public CodeGamePage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//DashboardPage");
    }
}
