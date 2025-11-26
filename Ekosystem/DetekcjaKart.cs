using Compunet.YoloSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;


using Compunet.YoloSharp.Plotting;
using SixLabors.ImageSharp;
using Compunet.YoloSharp.Data;
using Image = SixLabors.ImageSharp.Image;
using Microsoft.ML.OnnxRuntime;

namespace MauiApp1.Ekosystem
{
    public class DetekcjaKart
    {
        private static readonly string _nazwaModelu = "halfop12.onnx"; //nazwa modelu YOLO
        private static readonly string _ścieżkaDoModelu = $"Resources/Model/{_nazwaModelu}";
        //przyjmuje PATH do zdjęcia oraz PATH do modelu i zwraca liste która wymaga przeróbki

        public static async Task<List<List<int>>> detect(byte[] zdjęcieBuffer)
        {
            var trueŚcieżkaDoModelu = await StwórzŚcieżkęDoModelu();
            var ignorujNiskieConfidence = new YoloConfiguration();
            ignorujNiskieConfidence.Confidence = 0.63f;

            var options = new YoloPredictorOptions() { SessionOptions = new Microsoft.ML.OnnxRuntime.SessionOptions() };
            options.SessionOptions.AppendExecutionProvider_Nnapi();
            //options.SessionOptions.AppendExecutionProvider_CPU();

            using var predictor = new YoloPredictor(trueŚcieżkaDoModelu, options);
            var results = predictor.Detect(zdjęcieBuffer, ignorujNiskieConfidence);

            return PrzetłumaczNazwyNaInty(PoukładajKarty(results));
        }
        //przyjmuje listę kart z ich położeniem i oddaje standardową plansze gotową do obliczeń
        private static List<List<string>> PoukładajKarty(YoloResult<Detection> kartyZeZdjęcia)
        {
            List<List<string>> plansza = new();
            //var poukładaneKartyZeZdjęcia = kartyZeZdjęcia.OrderBy(x => x.Bounds.Y).Take(5).ToList();
            for (int i = 0; i < Ekosystem.ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                var poukładaneKartyZeZdjęcia = kartyZeZdjęcia.OrderBy(x => x.Bounds.Y).Skip(i * 5).Take(5).OrderBy(x => x.Bounds.X).ToList();
                List<string> nowyRząd = new();
                for (int j = 0; j < Ekosystem.ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    nowyRząd.Add(poukładaneKartyZeZdjęcia[j].Name.Name);
                }
                plansza.Add(nowyRząd);
            }
            return plansza;
        }
        //id list karty z yolo nie odpowiada id listy enumów kart używanych w programie do obliczania punktów, więc musimy przerobić tą liste
        private static List<List<int>> PrzetłumaczNazwyNaInty(List<List<string>> planszaString)
        {
            var yoloDoEnum = new Dictionary<string, Ekosystem.Karta>() 
            {
                    { "Łąka", Karta.Łąka },
                    { "Potok", Karta.Potok },
                    { "Jeleń", Karta.Jeleń },
                    { "Niedźwiedź", Karta.Niedźwiedź },
                    { "Lis", Karta.Lis },
                    { "Wilk", Karta.Wilk },
                    { "Pstrąg", Karta.Pstrąg },
                    { "Ważka", Karta.Ważka },
                    { "Pszczoła", Karta.Pszczoła },
                    { "Bielik", Karta.Bielik },
                    { "Zając", Karta.Zając },
                    { "Nisze", Karta.Nisze }
            };
            List<List<int>> planszaEnum = new();
            for (int i = 0; i < Ekosystem.ROZMIAR_PLANSZY.WYSOKOŚĆ; i++)
            {
                var poukładaneKartyZeZdjęcia = new List<int>();
                List<int> nowyRząd = new();
                for (int j = 0; j < Ekosystem.ROZMIAR_PLANSZY.DŁUGOŚĆ; j++)
                {
                    nowyRząd.Add((int)yoloDoEnum[planszaString[i][j]]);
                }
                planszaEnum.Add(nowyRząd);
            }
            return planszaEnum;
        }
        private static async Task<string> StwórzŚcieżkęDoModelu()
        {
            var targetPath = Path.Combine(FileSystem.AppDataDirectory, _nazwaModelu);

            using var stream = await FileSystem.OpenAppPackageFileAsync(_ścieżkaDoModelu);
            using var newStream = File.Create(targetPath);
            await stream.CopyToAsync(newStream);
            return targetPath;
        }
    }
}
