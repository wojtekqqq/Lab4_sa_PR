using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace Lab4_sa
{
    class Program
    {
        static void Main(string[] args)
        {
            string choosenCurrencyCode = null;
            string urlUsd = "http://api.nbp.pl/api/exchangerates/rates/a/usd/last/30/?format=json";

            string urlGold = "http://api.nbp.pl/api/cenyzlota/last/30/?format=json";
            string urlAviliableCurrency = "http://api.nbp.pl/api/exchangerates/tables/a/?format=json";

            HttpClient clientChoseCurrency = new HttpClient();
            HttpResponseMessage responseChoseCurrency = clientChoseCurrency.GetAsync(urlAviliableCurrency).Result;
            responseChoseCurrency.EnsureSuccessStatusCode();
            string resultChoseCurrency = responseChoseCurrency.Content.ReadAsStringAsync().Result;
            var currencyRateChoseCurrency = JsonConvert.DeserializeObject<ChooseRoot[]>(resultChoseCurrency);

            for (int i = 0; i < currencyRateChoseCurrency[0].rates.Count; i++)
            {
                Console.WriteLine( i + " " + currencyRateChoseCurrency[0].rates[i].code + " " + currencyRateChoseCurrency[0].rates[i].currency);
            }
            Console.WriteLine("Wybierz nr waluty, którą chcesz wykoszystać do przeliczeń na złoto :");
            int choosenCurrencyId = Convert.ToInt32(Console.ReadLine());
            choosenCurrencyCode = currencyRateChoseCurrency[0].rates[choosenCurrencyId].code;
            string urlCurrency = "http://api.nbp.pl/api/exchangerates/rates/a/" + choosenCurrencyCode +
                                 "/last/30/?format=json";

            string getCurrencyRate(string urlCurrency)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(urlCurrency).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }

            string getGoldRate(string urlGold)
            {
                HttpClient clientGold = new HttpClient();
                HttpResponseMessage responseGold = clientGold.GetAsync(urlGold).Result;
                string resultGold = responseGold.Content.ReadAsStringAsync().Result;
                return resultGold;
            }
            Task<string> task1 = new Task<string>(
                () => getCurrencyRate(urlCurrency));
            task1.Start();

            Task<string> task2 = new Task<string>(
                () => getGoldRate(urlGold));
            task2.Start();

            var allTasks = new Task[] { task1, task2 };
            Task.WaitAll(allTasks);
            double[] ep = new double[30];
            object[,] exchangeRate = new object[2, 30];
            var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>(task1.Result);
            var goldRate = JsonConvert.DeserializeObject<GoldRate[]>(task2.Result);
            for (int i = 0; i < 30; i++)
            {
                exchangeRate[0, i] = currencyRate.rates[i].effectiveDate;
                exchangeRate[1, i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                ep[i] = (goldRate[i].cena / currencyRate.rates[i].mid);
            }

            double currently = Convert.ToDouble(exchangeRate[1, 29]);
            Console.WriteLine("Maksymalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Max() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Max())].ToString()));
            Console.WriteLine("Minimalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Min() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Min())].ToString()));
            Console.WriteLine("Różnica między aktualnym a maksymalnym kursem wynosi :" + (currently - ep.Max()));
            Console.WriteLine("Różnica między aktualnym a minimalnym kursem wynosi :" + (currently - ep.Min()));
            Console.ReadLine();

            /*            var task = Task.Factory.ContinueWhenAll(
                            new Task[] { task1, task2 }, (values) =>
                            {
                                Console.WriteLine((values[0] as Task<object>).Result);
                                Console.WriteLine((values[1] as Task<object>).Result);
                                double[] ep = new double[30];
                                object[,] exchangeRate = new object[2, 30];
                                var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>((values[0] as Task<object>).Result.ToString());
                                var goldRate = JsonConvert.DeserializeObject<GoldRate[]>((values[1] as Task<object>).Result.ToString());
                                for (int i = 0; i < 30; i++)
                                {
                                    exchangeRate[0, i] = currencyRate.rates[i].effectiveDate;
                                    exchangeRate[1, i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                                    ep[i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                                }
                                double currently = Convert.ToDouble(exchangeRate[1, 29]);
                                Console.WriteLine("asdfasdfasdf");
                                Console.WriteLine("Maksymalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Max() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Max())].ToString()));
                                Console.WriteLine("Minimalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Min() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Min())].ToString()));
                                Console.WriteLine("Różnica między aktualnym a maksymalnym kursem wynosi :" + (currently - ep.Max()));
                                Console.WriteLine("Różnica między aktualnym a minimalnym kursem wynosi :" + (currently - ep.Min()));
                                Console.ReadLine();
                            }
                        );*/
        }
    }
}
