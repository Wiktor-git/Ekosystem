namespace MauiApp1.Ekosystem
{
    #region podstawowe dane globalne
    public static class ROZMIAR_PLANSZY
    {
        public static readonly int WYSOKOŚĆ = 4;
        public static readonly int DŁUGOŚĆ = 5;
    }
    public class Pole
    {
        public Karta karta;
        public int rząd;
        public int kolumna;
    }
    public enum Karta
    {
        Łąka,
        Potok,
        Jeleń,
        Niedźwiedź,
        Lis,
        Wilk,
        Pstrąg,
        Ważka,
        Pszczoła,
        Bielik,
        Zając,
        Nisze = -1
    }
    #endregion

    public class PlanszaGracza
    {
        #region klasa
        public List<List<int>> _plansza;
        public int _punkty = 0;
        public int _wilki = 0;
        public int _najdłuższyPotok = 0;
        public Dictionary<Karta, int> kartaStatus = new()
        {
            { Karta.Łąka, 0 },
            { Karta.Potok, 0 },
            { Karta.Jeleń, 0 },
            { Karta.Niedźwiedź, 0 },
            { Karta.Lis, 0 },
            { Karta.Wilk, 0 },
            { Karta.Pstrąg, 0 },
            { Karta.Ważka, 0 },
            { Karta.Pszczoła, 0 },
            { Karta.Bielik, 0 },
            { Karta.Zając, 0 },
            { Karta.Nisze, 0}
        };
        public PlanszaGracza(List<List<int>> plansza)
        {
            _plansza = plansza;
        }
        #endregion

        /// <summary>
        /// !UWAGA! do kompletnego obliczenia ilości punktów należy jeszcze porównać <c>_wilki</c> i <c>_najdłuższyPotok</c>
        /// Oblicza wszystkie wartości związane z gotową planszą
        /// </summary>
        public void wstępneObliczeniePlanszy()
        {
            for (int rząd = 0; rząd < ROZMIAR_PLANSZY.WYSOKOŚĆ; rząd++)
            {
                for (int kolumna = 0; kolumna < ROZMIAR_PLANSZY.DŁUGOŚĆ; kolumna++)
                {
                    DodajPole(new Pole { karta = (Karta)_plansza[rząd][kolumna], rząd = rząd, kolumna = kolumna });
                }
            }
            ZliczJelenie();
            ZliczNajwiększyPotok();
            ZliczŁąki();
        }
        private void DodajPole(Pole pole)
        {
            switch (pole.karta)
            {
                case Karta.Łąka:
                    //ignore --- karty łąki obliczane są inną metodą
                    break;
                case Karta.Potok:
                    //ignore --- karty potoku obliczane są inną metodą
                    break;
                case Karta.Jeleń:
                    //ignore --- karty jeleni obliczane są inną metodą
                    break;
                case Karta.Niedźwiedź:
                    PodliczNiedźwiedź(pole);
                    break;
                case Karta.Lis:
                    PodliczLis(pole);
                    break;
                case Karta.Wilk:
                    PodliczWilk(pole);
                    break;
                case Karta.Pstrąg:
                    PodliczPstrąg(pole);
                    break;
                case Karta.Ważka:
                    PodliczWażka(pole);
                    break;
                case Karta.Pszczoła:
                    PodliczPszczoła(pole);
                    break;
                case Karta.Bielik:
                    PodliczBielik(pole);
                    break;
                case Karta.Zając:
                    PodliczZając(pole);
                    break;
                case Karta.Nisze:
                    PodliczBłąd(pole);
                    break;
                default:
                    Console.WriteLine("Nieznana karta.");
                    break;
            }
        }
        private void DodajPunktyZa(Karta karta, int punkty)
        {
            kartaStatus[karta] += punkty;
            _punkty += punkty;
            Console.WriteLine($"Punkty: {punkty} za {karta.ToString()}");
        }
        public static void porównajPotokIWilki_iObliczNisze(List<PlanszaGracza> wszyscyGracze)
        {
            //obliczanie punktów zdobytych za osiągnięcie miejsca z wielkości potoku i ilości wilków + na koniec obliczenie nisz 
            //pierwszy forloop tworzy dictionary z key jako ilość punktów i value jako lista graczy którzy osiągneli taki wynik
            var miejscaZaWilki = new Dictionary<int, List<PlanszaGracza>> ();
            var miejscaZaPotok = new Dictionary<int, List<PlanszaGracza>>();
            foreach (PlanszaGracza gracz in wszyscyGracze)
            {
                if (miejscaZaWilki.ContainsKey(gracz._wilki))
                {
                    miejscaZaWilki[gracz._wilki].Add(gracz);
                }
                else
                {
                    miejscaZaWilki.Add(gracz._wilki, [gracz]);
                }

                if (miejscaZaPotok.ContainsKey(gracz._najdłuższyPotok))
                {
                    miejscaZaPotok[gracz._najdłuższyPotok].Add(gracz);
                }
                else
                {
                    miejscaZaPotok.Add(gracz._najdłuższyPotok, [gracz]);
                }
            }

            var punktyZaWilki = new int[] { 4, 8, 12 };
            var punktyZaPotok = new int[] { 5, 8 };
            var counterWilki = 3; //punkty dostaje się za pierwsze 3 miejsca, jeśli są 2+ osoby z takim samym wynikiem zajmują one to samo miejsce, ale to poniżej znika
            while (counterWilki > 0 && miejscaZaWilki.Count > 0)
            {
                var kvpair = miejscaZaWilki.MaxBy(k => k.Key);
                if (kvpair.Key == 0)
                {
                    break;
                }
                foreach (var gracz in kvpair.Value)
                {
                    gracz.DodajPunktyZa(Karta.Wilk, punktyZaWilki[counterWilki - 1]);
                }
                miejscaZaWilki.Remove(kvpair.Key);
                counterWilki -= kvpair.Value.Count;
            }

            var counterPotok = 2; //punkty dostaje się za pierwsze 2 miejsca, jeśli są 2+ osoby z takim samym wynikiem zajmują one to samo miejsce, ale to poniżej znika
            while (counterPotok > 0 && miejscaZaPotok.Count > 0)
            {
                var kvpair = miejscaZaPotok.MaxBy(k => k.Key);
                if (kvpair.Key == 0)
                {
                    break;
                }
                foreach (var gracz in kvpair.Value)
                {
                    gracz.DodajPunktyZa(Karta.Potok, punktyZaPotok[counterPotok - 1]);
                }
                miejscaZaPotok.Remove(kvpair.Key);
                counterPotok -= kvpair.Value.Count;
            }



            foreach(PlanszaGracza gracz in wszyscyGracze)
            {
                gracz.ObliczNiszeEkologiczne();
            }
        }
        private void ObliczNiszeEkologiczne()
        {
            int brakiWNiszach = kartaStatus.Where(k => k.Value == 0).Count() - 1;//-1 bo nisze są też kartą
            if (brakiWNiszach <= 2)
                 DodajPunktyZa(Karta.Nisze, 12);
            else if (brakiWNiszach == 3)
                DodajPunktyZa(Karta.Nisze, 7);
            else if (brakiWNiszach == 4)
                DodajPunktyZa(Karta.Nisze, 3);
            else if (brakiWNiszach == 5)
                DodajPunktyZa(Karta.Nisze, 0);
            else if (brakiWNiszach >= 6)
                DodajPunktyZa(Karta.Nisze, -5);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pole">pole do którego chcesz znaleść sąsiadujące karty</param>
        /// <returns>zwraca 4 pola sąsiadujące z podaną kartą (góra, dół, lewo, prawo), jeśli nie ma karty zwraca {Karta.Błąd, -1, -1)</returns>
        private List<Pole> ZnajdźSąsiadującePola(Pole pole)
        {
            int[] kolumnyOffset = new int[4] { 0, -1, 0, 1 };
            int[] rzędyOffset = new int[4] { -1, 0, 1, 0 };
            List<Pole> returnList = new();
            for (int i = 0; i < 4; i++)
            {
                Pole tempPole = null;
                try
                {
                    var tempKarta = _plansza[pole.rząd + rzędyOffset[i]][pole.kolumna + kolumnyOffset[i]];
                    tempPole = new Pole
                    {
                        karta = (Karta)tempKarta,
                        rząd = pole.rząd + rzędyOffset[i],
                        kolumna = pole.kolumna + kolumnyOffset[i]
                    };
                }
                catch
                {
                    tempPole = new Pole
                    {
                        karta = Karta.Nisze,
                        kolumna = -1,
                        rząd = -1
                    };
                }
                returnList.Add(tempPole);
            }
            return returnList;
        }


        #region funkcje obliczające punkty z całej planszy (nie trzeba wywoływać osobno kart)
        /// <summary>
        /// Zlicza ilość kart w najdłuższym potoku
        /// wystarczy wywołać raz i wynik jest zapisany w klasie jako _długośćPotoku
        /// Uwaga! długość potoku należy porównać z innymi graczami aby otrzymać punkty!
        /// </summary>
        private void ZliczNajwiększyPotok()
        {
            bool[,] sprawdzonePole = new bool[ROZMIAR_PLANSZY.WYSOKOŚĆ, ROZMIAR_PLANSZY.DŁUGOŚĆ];
            List<int> długościPotoków = new();
            for (int i = 0; i < ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    int wielkośćPola = 0;
                    if (_plansza[i][j] == (int)Karta.Potok)
                    {
                        wielkośćPola = Helper_ZliczanieWielkościPól(sprawdzonePole, i, j, sprawdzanaKarta: Karta.Potok);
                        wielkośćPola++;
                    }
                    if (wielkośćPola != 0)
                    {
                        długościPotoków.Add(wielkośćPola);
                    }
                }
            }
            if (długościPotoków.Count != 0)
            {
                _najdłuższyPotok = długościPotoków.Max();
            }
        }
        /// <summary>
        /// Zlicza wielkości "pól" łąk następnie oblicza za nie punkty
        /// wystarczy wywołać raz i wynik jest dodany do punktacji (_punkty)
        /// </summary>
        private void ZliczŁąki()
        {
            bool[,] sprawdzonePole = new bool[ROZMIAR_PLANSZY.WYSOKOŚĆ,ROZMIAR_PLANSZY.DŁUGOŚĆ];
            List<int> wielkościŁąki = new();
            for (int i = 0; i < ROZMIAR_PLANSZY.WYSOKOŚĆ; i++) 
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    int wielkośćPola = 0;
                    if (_plansza[i][j] == (int)Karta.Łąka)
                    {
                        wielkośćPola = Helper_ZliczanieWielkościPól(sprawdzonePole, i, j, sprawdzanaKarta: Karta.Łąka);
                        wielkośćPola++;
                    }
                    if (wielkośćPola != 0)
                    {
                        wielkościŁąki.Add(wielkośćPola);
                    }
                }
            }
            foreach (var wielkośćŁąki in wielkościŁąki)
            {
                switch(wielkośćŁąki)
                {
                    case 0:
                        DodajPunktyZa(Karta.Łąka, 0);
                        break;
                    case 1:
                        DodajPunktyZa(Karta.Łąka, 0);
                        break;
                    case 2:
                        DodajPunktyZa(Karta.Łąka, 3); 
                        break;
                    case 3:
                        DodajPunktyZa(Karta.Łąka, 6);
                        break;
                    case 4:
                        DodajPunktyZa(Karta.Łąka, 10);
                        break;
                    case 5:
                        DodajPunktyZa(Karta.Łąka, 15);
                        break;
                    default:
                        DodajPunktyZa(Karta.Łąka, 15);
                        break;
                }
            }
        }
        /// <summary>
        /// funkcja pomagająca obliczaniu wielkości "pól" kart, potrzebowałem do rekursji
        /// używana z kartami Potok i Łąka (i do wypunktowania Ważka do której trzeba obliczyć wielkość "pola" potoku) 
        /// </summary>
        private int Helper_ZliczanieWielkościPól(bool[,] sprawdzone, int i, int j, Karta sprawdzanaKarta)
        {
            var stack = new Stack<(int, int)>();
            stack.Push((i, j));
            int count = 0;
            sprawdzone[i, j] = true;
            while (stack.Count > 0)
            {
                var (r, c) = stack.Pop();
                if ((Karta)_plansza[r][c] == sprawdzanaKarta && !sprawdzone[r,c])
                {
                    sprawdzone[r, c] = true;
                    count++;
                }




                var sąsiadującePola = ZnajdźSąsiadującePola(new Pole { karta = (Karta)_plansza[r][c], kolumna = c, rząd = r });
                foreach (var pole in sąsiadującePola)
                {
                    if (pole.karta == sprawdzanaKarta && !sprawdzone[pole.rząd, pole.kolumna])
                    {
                        stack.Push((pole.rząd, pole.kolumna));
                        sprawdzone[pole.rząd, pole.kolumna] = true;
                        count++;
                    }
                }
            }
            return count;
        }
        private void ZliczJelenie()
        {
            int punktyDoDodania = 0;
            for (int i = 0; i < ROZMIAR_PLANSZY.DŁUGOŚĆ; i++)
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.WYSOKOŚĆ; j++)
                {
                    if ((Karta)_plansza[j][i] == Karta.Jeleń)
                    {
                        punktyDoDodania += 2;
                        break;
                    }
                }
            }
            for (int i = 0; i < ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    if ((Karta)_plansza[i][j] == Karta.Jeleń)
                    {
                        punktyDoDodania += 2;
                        break;
                    }
                }
            }
            DodajPunktyZa(Karta.Jeleń, punktyDoDodania);
        }
        #endregion


        /// karty w tym regionie działają podobnie - ilość punktów zależy od sąsiadujących kart
        /// w wartościach <c>punktyZaSąsiadująceKarty</c> opisane są karty i ile one dają punktów,
        /// następnie punkty są obliczane i dodane do wartości _punkty
        /// wyjątki są opisane własnymi komentarzami
        #region Punkty za sąsiadujące karty
        private void PodliczNiedźwiedź(Pole pole)
        {
            var punktyZaSąsiadująceKarty = new Dictionary<Karta, int>()
            {
                {Karta.Pszczoła, 2},
                {Karta.Pstrąg, 2}
            };
            int punktyZaKartę = ObliczPunkty(ZnajdźSąsiadującePola(pole), punktyZaSąsiadująceKarty);
            DodajPunktyZa(pole.karta, punktyZaKartę);
        }
        /// <summary>
        /// sprawdza czy nie graniczy z Wilkiem lub Niedźwiedziem
        /// jeśli nie +3 punty
        /// jeśli tak +0 punktów
        /// </summary>
        private void PodliczLis(Pole pole)
        {
            var sąsiadującePola = ZnajdźSąsiadującePola(pole);
            int punkty = 3;
            foreach (var item in sąsiadującePola)
            {
                if (item.karta == Karta.Wilk || item.karta == Karta.Niedźwiedź)
                {
                    punkty = 0;
                }
            }
            DodajPunktyZa(pole.karta, punkty);
        }
        /// <summary>
        /// liczy karty wilków
        /// punkty są przyznawane osobom z największą ilością wilków
        /// dlatego żeby dodać punkty najpierw trzeba porównać ilość wilków wszystkich graczy
        /// </summary>
        /// <param name="pole"></param>
        private void PodliczWilk(Pole pole)
        {
            _wilki++;
        }
        private void PodliczPstrąg(Pole pole)
        {
            var punktyZaSąsiadująceKarty = new Dictionary<Karta, int>()
            {
                {Karta.Potok, 2},
                {Karta.Ważka, 2}
            };
            int punktyZaKartę = ObliczPunkty(ZnajdźSąsiadującePola(pole), punktyZaSąsiadująceKarty);
            DodajPunktyZa(pole.karta, punktyZaKartę);
        }
        /// <summary>
        /// tutaj musimy obliczyć wielkość "pola" sąsiadujących potoków
        /// używamy do tego funkcji <c>Helper_ZliczanieWielkościPól</c>
        /// </summary>
        private void PodliczWażka(Pole pole)
        {
            var sąsiadującePola = ZnajdźSąsiadującePola(pole);
            bool[,] sprawdzonePole = new bool[ROZMIAR_PLANSZY.WYSOKOŚĆ, ROZMIAR_PLANSZY.DŁUGOŚĆ];
            List<int> sąsiednieRzeki = new();

            foreach (var sąsiad in sąsiadującePola)
            {
                if (sąsiad.karta == Karta.Potok && !sprawdzonePole[sąsiad.rząd, sąsiad.kolumna])
                {
                    var długośćRzeki = Helper_ZliczanieWielkościPól(sprawdzonePole, sąsiad.rząd, sąsiad.kolumna, sprawdzanaKarta: Karta.Potok);
                    sprawdzonePole[sąsiad.rząd, sąsiad.kolumna] = true;
                    sąsiednieRzeki.Add(długośćRzeki+1);
                }
            }
            foreach (var długośćRzeki in sąsiednieRzeki)
            DodajPunktyZa(pole.karta, długośćRzeki);
        }
        private void PodliczPszczoła(Pole pole)
        {
            var punktyZaSąsiadująceKarty = new Dictionary<Karta, int>()
            {
                {Karta.Łąka, 3},
            };
            int punktyZaKartę = ObliczPunkty(ZnajdźSąsiadującePola(pole), punktyZaSąsiadująceKarty);
            DodajPunktyZa(pole.karta, punktyZaKartę);
        }
        /// <summary>
        /// karta działa podobnie do wszystkich innych z wyjątkiem tego że ma zasięg 2 kart
        /// dlatego musimy policzyć sąsiadujące karty i sąsiadujące sąsiadów
        /// </summary>
        private void PodliczBielik(Pole pole)
        {
            bool[,] sprawdzonePole = new bool[ROZMIAR_PLANSZY.WYSOKOŚĆ, ROZMIAR_PLANSZY.DŁUGOŚĆ];
            var punktyZaSąsiadująceKarty = new Dictionary<Karta, int>()
            {
                {Karta.Pstrąg, 2},
                {Karta.Zając, 2}
            };
            int punktyZaKartę = 0;
            var sąsiadującePola = ZnajdźSąsiadującePola(pole);
            foreach (var sąsiad in sąsiadującePola)
            {
                var oddalonePola = ZnajdźSąsiadującePola(sąsiad);
                //Console.WriteLine("sąsiad");
                //testowy_visualizer_dla_pola_usunąć(sprawdzonePole, sąsiad.rząd, sąsiad.kolumna);
                foreach (var oddalone in oddalonePola)
                {
                    //Console.WriteLine("oddalonePole");
                    //testowy_visualizer_dla_pola_usunąć(sprawdzonePole, oddalone.rząd, oddalone.kolumna);
                    if (oddalone.karta != Karta.Nisze && !sprawdzonePole[oddalone.rząd, oddalone.kolumna])
                    {
                        punktyZaKartę += ObliczPunkty(new List<Pole> { oddalone }, punktyZaSąsiadująceKarty);
                        sprawdzonePole[oddalone.rząd, oddalone.kolumna] = true;
                    }
                }
            }
            punktyZaKartę += ObliczPunkty(sąsiadującePola, punktyZaSąsiadująceKarty);

            DodajPunktyZa(pole.karta, punktyZaKartę);
        }
        /// <summary>
        /// +1 pkt za każdego zająca
        /// </summary>
        private void PodliczZając(Pole pole)
        {
            DodajPunktyZa(pole.karta, 1);
        }
        /// <summary>
        /// !UWAGA! nie powinno zostać wywołane - jak zostało oznacza to bład w kodzie
        /// karty błędu służą do pomocy z wyznaczaniem granic planszy
        /// </summary>
        private void PodliczBłąd(Pole pole)
        {
            Console.WriteLine("Błąd: Nieprawidłowa karta -- nie powinno zostać wywołane");
        }
        /// <summary>
        /// przyjmuje sąsiadujące pola i zwraca wartość punktów które dostaje nasza karta(czyli punkty za kartę pośrodku, nie punkty za sąsiadujące karty)
        /// </summary>
        /// <param name="sąsiadującePola">karty które sąsiadują z naszą kartą zdobywane za pomocą <c>ZnajdźSąsiadującePola</c></param>
        /// <param name="punktyZaKartę">ile dana karta dodaje punktów, każda metoda posiada własną tabele punktów</param>
        /// <returns>zwraca wartość punktów, które następnie trzeba dodać do _punkty w metodzie w której jest to wywoływane</returns>
        private int ObliczPunkty(List<Pole> sąsiadującePola, Dictionary<Karta,int> punktyZaKartę)
        {
            int punkty = 0;
            foreach (var pole in sąsiadującePola)
            {
                if (punktyZaKartę.ContainsKey(pole.karta))
                {
                    punkty += punktyZaKartę[pole.karta];
                }
            }

            return punkty;
        }
        #endregion
    }
    public class DebugFunctions
    {
        public static List<List<List<int>>> stringToList(string debugString)
        {
            List<List<List<int>>> listaGraczy = new();
            var graczeString = debugString.Split('\n');
            List<List<int>> plansza = new();
            foreach (var str in graczeString)
            {
                if (str.Contains("GRACZ") || string.IsNullOrWhiteSpace(str))
                {
                    if (plansza.Count > 0)
                    {
                        listaGraczy.Add(plansza);
                    }
                    plansza = new();
                    continue;
                }
                List<int> rząd = new();
                var rządArr = str.Split(',');
                foreach (var karta in rządArr)
                {
                    if (karta.Equals("\r") || string.IsNullOrEmpty(karta))
                    {
                        continue;
                    }
                    rząd.Add(int.Parse(karta));
                }
                plansza.Add(rząd);
            }
            listaGraczy.Add(plansza);
            return listaGraczy;
        }
        public void Testowy_visualizer_dla_pola_usunąć(bool[,] sprawdzone, int rząd, int kolumna, List<List<int>> _plansza)
        {
            for (int i = 0; i < ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    if (i == rząd && j == kolumna)
                    {
                        Console.Write("[ | ]");
                    }
                    else
                    {
                        int forPrint = _plansza[i][j];
                        if (forPrint != 10)
                            Console.Write($"[ {_plansza[i][j]} ]");
                        else
                            Console.Write($"[ {_plansza[i][j]}]");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("bool");
            Testowy_visualizer_dla_boola_usunąć(sprawdzone);
            Console.WriteLine("/bool");
        }
        public void Testowy_visualizer_dla_boola_usunąć(bool[,] sprawdzone)
        {
            for (int i = 0; i < ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                for (int j = 0; j < ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    Console.Write($"|{sprawdzone[i, j]}|");
                }
                Console.WriteLine();
            }
        }
    }


}
