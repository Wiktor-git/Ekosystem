using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    public partial class Plansza : ObservableObject
    {
        private string? nazwaGracza;
        public string? NazwaGracza
        {
            get => nazwaGracza;
            set => SetProperty(ref nazwaGracza, value);
        }

        private int całkowitePunkty;
        public int CałkowitePunkty
        {
            get => całkowitePunkty;
            set => SetProperty(ref całkowitePunkty, value);
        }

        private ObservableCollection<RządEmotikonów>? planszaEmotikonówObservable;
        public ObservableCollection<RządEmotikonów>? PlanszaEmotikonówObservable
        {
            get => planszaEmotikonówObservable;
            set => SetProperty(ref planszaEmotikonówObservable, value);
        }

        private List<List<int>>? planszaDoObliczeń;
        public List<List<int>>? PlanszaDoObliczeń
        {
            get => planszaDoObliczeń;
            set => SetProperty(ref planszaDoObliczeń, value);
        }

        private int łąka;
        public int Łąka
        {
            get => łąka;
            set => SetProperty(ref łąka, value);
        }

        private int name;
        public int Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int potok;
        public int Potok
        {
            get => potok;
            set => SetProperty(ref potok, value);
        }

        private int jeleń;
        public int Jeleń
        {
            get => jeleń;
            set => SetProperty(ref jeleń, value);
        }

        private int niedźwiedź;
        public int Niedźwiedź
        {
            get => niedźwiedź;
            set => SetProperty(ref niedźwiedź, value);
        }

        private int lis;
        public int Lis
        {
            get => lis;
            set => SetProperty(ref lis, value);
        }

        private int wilk;
        public int Wilk
        {
            get => wilk;
            set => SetProperty(ref wilk, value);
        }

        private int pstrąg;
        public int Pstrąg
        {
            get => pstrąg;
            set => SetProperty(ref pstrąg, value);
        }

        private int ważka;
        public int Ważka
        {
            get => ważka;
            set => SetProperty(ref ważka, value);
        }

        private int pszczoła;
        public int Pszczoła
        {
            get => pszczoła;
            set => SetProperty(ref pszczoła, value);
        }

        private int bielik;
        public int Bielik
        {
            get => bielik;
            set => SetProperty(ref bielik, value);
        }

        private int zając;
        public int Zając
        {
            get => zając;
            set => SetProperty(ref zając, value);
        }

        private int nisze;
        public int Nisze
        {
            get => nisze;
            set => SetProperty(ref nisze, value);
        }
    }
    public partial class RządEmotikonów : ObservableObject
    {
        private ObservableCollection<string> rząd;
        public ObservableCollection<string> Rząd
        {
            get => rząd;
            set => SetProperty(ref rząd, value);
        }
        public RządEmotikonów(List<string> rządAdd)
        {
            rząd = new ObservableCollection<string>(rządAdd);
        }
    }
}
