using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.ViewModel
{
    public partial class PlanszeViewModel : BaseViewModel
    {
        public ObservableCollection<Plansza> Plansze { get; } = new();
        public Command AddPlanszaCommand { get; }
        public Command PoliczPunktyCommand { get; }
        PlanszaSerwis PlanszaSerwis;


        public PlanszeViewModel(PlanszaSerwis planszaService)
        {
            Title = "Ekosystem";
            this.PlanszaSerwis = planszaService;
            AddPlanszaCommand = new Command(async () => await DodajGracza());
            PoliczPunktyCommand = new Command(async () => await PoliczPunkty());
            Debug.WriteLine("MainViewModel created");
        }
        async Task DodajGracza()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var plansze = await PlanszaSerwis.DodajGracza();

                if (plansze.Count != 0)
                {
                    Plansze.Clear();
                }
                foreach (var plansza in plansze)
                    Plansze.Add(plansza);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("nie udało się dodać gracza");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task PoliczPunkty()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                int MinimumGraczyDoObliczeniaPunktów = 2;
                if (Plansze.Count >= MinimumGraczyDoObliczeniaPunktów)
                    await PlanszaSerwis.ObliczPunkty(Plansze);
                OnPropertyChanged(nameof(Plansze));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("nie udało się obliczyć punktów");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
