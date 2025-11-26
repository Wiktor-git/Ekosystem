using MauiApp1;
using MauiApp1.ViewModel;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PlanszaView), typeof(PlanszaView));
            Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
        }
    }
}
