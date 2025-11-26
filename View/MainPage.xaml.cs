using MauiApp1;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage(PlanszeViewModel planszeViewModel)
        {
            InitializeComponent();
            BindingContext = planszeViewModel;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var plansza = ((VisualElement)sender).BindingContext as Plansza;
            if (plansza == null)
                return;
            await Shell.Current.GoToAsync(nameof(PlanszaView), true, new Dictionary<string, object>
        {
            {"Plansza", plansza},
        });
        }
    }
}
