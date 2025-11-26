using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp1;

public partial class PlanszaView : ContentPage
{
    public PlanszaView(PlanszaDetailsViewModel plansza)
    {
        InitializeComponent();

        BindingContext = ServiceHelper.GetService<PlanszaDetailsViewModel>();
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CameraPage), true);
    }
}