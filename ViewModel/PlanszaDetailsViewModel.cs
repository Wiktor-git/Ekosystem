using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Ekosystem;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static MauiApp1.ViewModel.PlanszaDetailsViewModel;

namespace MauiApp1.ViewModel
{
    [QueryProperty(nameof(Plansza), "Plansza")]
    public partial class PlanszaDetailsViewModel : BaseViewModel, IRecipient<SelectedOptionsMessage>
    {
        public Command OpenCameraCommand { get; }
        PlanszaSerwis PlanszaSerwis;
        public record SelectedOptionsMessage(byte[]? bytes);

        public PlanszaDetailsViewModel(PlanszaSerwis planszaService)
        {
            Title = "add player name";
            this.PlanszaSerwis = planszaService;
            OpenCameraCommand = new Command(async () => await OnOpenCameraPageClicked());
            WeakReferenceMessenger.Default.Register(this);
        }

        [ObservableProperty]
        Plansza plansza;

        async Task OnOpenCameraPageClicked()
        {
            await Shell.Current.GoToAsync(nameof(CameraPage), true);
        }
        public async void Receive(SelectedOptionsMessage message)
        {
            var photoInBytes = message.bytes;
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await PlanszaSerwis.DodajZdjęcie(this.Plansza, photoInBytes);
                OnPropertyChanged(nameof(Plansza));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("nie udało się dodać zdjęcia");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
