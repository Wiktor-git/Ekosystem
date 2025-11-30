using MauiApp1.Ekosystem;

namespace MauiApp1.Services
{
    public class PlanszaSerwis
    {
        DetekcjaKart detekcjaKart;
        public PlanszaSerwis()
        {
            detekcjaKart = new();
        }
        List<Plansza> listaPlansz = new();
        public async Task<List<Plansza>> DodajGracza()
        {
            if (listaPlansz.Count == 0)
            {
                listaPlansz = new();
            }
            listaPlansz.Add(new Plansza() { PlanszaEmotikonówObservable = new ObservableCollection<RządEmotikonów>(), NazwaGracza = "Gracz" });
            return listaPlansz;
        }
        public async Task DodajZdjęcie(Plansza plansza, byte[] path)
        {
            await OdczytajPlanszeZeZdjęcia(plansza, path);
        }

        public async Task ObliczPunkty(ObservableCollection<Plansza> plansze)
        {
            List<PlanszaGracza> gracze = new();
            int pustePlansze = 0;
            foreach (var plansza in plansze)
                if (plansza.PlanszaDoObliczeń is not null && plansza.PlanszaDoObliczeń.Count != 0)
                    gracze.Add(new PlanszaGracza(plansza.PlanszaDoObliczeń));
                else
                    pustePlansze++;

            if (plansze.Count - pustePlansze < 2)
                return;

            foreach (var gracz in gracze)
                gracz.wstępneObliczeniePlanszy();
            PlanszaGracza.porównajPotokIWilki_iObliczNisze(gracze);

            if (plansze != null)
            {
                for (int i = 0; i < gracze.Count; i++)
                {
                    plansze[i].Łąka = gracze[i].kartaStatus[Karta.Łąka];
                    plansze[i].Potok = gracze[i].kartaStatus[Karta.Potok];
                    plansze[i].Jeleń = gracze[i].kartaStatus[Karta.Jeleń];
                    plansze[i].Niedźwiedź = gracze[i].kartaStatus[Karta.Niedźwiedź];
                    plansze[i].Lis = gracze[i].kartaStatus[Karta.Lis];
                    plansze[i].Wilk = gracze[i].kartaStatus[Karta.Wilk];
                    plansze[i].Pstrąg = gracze[i].kartaStatus[Karta.Pstrąg];
                    plansze[i].Ważka = gracze[i].kartaStatus[Karta.Ważka];
                    plansze[i].Pszczoła = gracze[i].kartaStatus[Karta.Pszczoła];
                    plansze[i].Bielik = gracze[i].kartaStatus[Karta.Bielik];
                    plansze[i].Zając = gracze[i].kartaStatus[Karta.Zając];
                    plansze[i].Nisze = gracze[i].kartaStatus[Karta.Nisze];
                    plansze[i].CałkowitePunkty = gracze[i]._punkty;
                }
            }
        }

        private static readonly Dictionary<int, string> liczbaDoEmotikonki = new()
        {
            {0,"🌾" },
            {1, "🛶" },
            {2, "🦌"},
            {3, "🧸" },
            {4, "🦊" },
            {5, "🐺" },
            {6, "🐟"},
            {7, "🪰" },
            {8,  "🐝"},
            {9, "🦅" },
            {10, "🐇" },
            {11, "x" }
        };
        public async Task OdczytajPlanszeZeZdjęcia(Plansza planszaDoEdycji, byte[] zdjęcie)
        {
            List<List<int>> plansza = await detekcjaKart.detect(zdjęcie);

            if (planszaDoEdycji.PlanszaEmotikonówObservable is null)
                planszaDoEdycji.PlanszaEmotikonówObservable = new();
            planszaDoEdycji.PlanszaEmotikonówObservable.Clear();

            foreach (var rząd in plansza)
            {
                List<string> rządEmotikonów = new();
                foreach (var karta in rząd)
                {
                    rządEmotikonów.Add(liczbaDoEmotikonki[karta]);
                }
                planszaDoEdycji.PlanszaEmotikonówObservable.Add(new RządEmotikonów(rządEmotikonów));
            }
            planszaDoEdycji.PlanszaDoObliczeń = plansza;
        }
    }
}