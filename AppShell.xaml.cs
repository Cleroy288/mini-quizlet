using Intello.Pages;

namespace Intello;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("DashboardPage", typeof(DashboardPage));
        Routing.RegisterRoute("QcmListPage", typeof(QcmListPage));
        Routing.RegisterRoute("QcmQuizPage", typeof(QcmQuizPage));
        Routing.RegisterRoute("QcmResultsPage", typeof(QcmResultsPage));
        Routing.RegisterRoute("FlashcardPage", typeof(FlashcardPage));
        Routing.RegisterRoute("CodeGamePage", typeof(CodeGamePage));
    }
}
