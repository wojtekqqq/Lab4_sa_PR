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
            Console.WriteLine(currencyRateChoseCurrency);


            Task<object> task1 = new Task<object>(
                () => getCurrencyRate(urlUsd));
            task1.Start();

            Task<object> task2 = new Task<object>(
                () => getGoldRate(urlGold));
            task2.Start();
            /*            Task<object> task2 = Task.Factory.StartNew<object>(
                            () => getGoldRate(urlGold));*/



            for (int i = 0; i < currencyRateChoseCurrency[0].rates.Count; i++)
            {
                Console.WriteLine( i + " " + currencyRateChoseCurrency[0].rates[i].code + " " + currencyRateChoseCurrency[0].rates[i].currency);
            }
            Console.WriteLine("Wybierz nr waluty, którą chcesz wykoszystać do przeliczeń na złoto :");
            int choosenCurrencyId = Convert.ToInt32(Console.ReadLine());
            choosenCurrencyCode = currencyRateChoseCurrency[0].rates[choosenCurrencyId].code;
            string urlCurrency = "http://api.nbp.pl/api/exchangerates/rates/a/" + choosenCurrencyCode +
                                 "/last/30/?format=json";



          



            object getCurrencyRate(string urlCurrency)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(urlCurrency).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>(result);
                currencyRate.GetType();
                return currencyRate;
            }

            object getGoldRate(string urlGold)
            {
                HttpClient clientGold = new HttpClient();
                HttpResponseMessage responseGold = clientGold.GetAsync(urlGold).Result;
                string resultGold = responseGold.Content.ReadAsStringAsync().Result;
                var goldRate = JsonConvert.DeserializeObject<GoldRate[]>(resultGold);
                goldRate.GetType();
                return goldRate;
            }



/*            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(urlCurrency).Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>(result);*/


            /*            HttpClient client = new HttpClient();
                        HttpResponseMessage response = client.GetAsync(urlUsd).Result;
                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;
                        var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>(result);*/




/*            HttpClient clientGold = new HttpClient();
            HttpResponseMessage responseGold = clientGold.GetAsync(urlGold).Result;
            string resultGold = responseGold.Content.ReadAsStringAsync().Result;
            var goldRate = JsonConvert.DeserializeObject<GoldRate[]>(resultGold);*/




/*            List<Exchange> exchange = new List<Exchange>();

            double[] ep = new double[30];
            object[,] exchangeRate = new object[2, 30];
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
            Console.WriteLine("Różnica między aktualnym a minimalnym kursem wynosi :" + (currently - ep.Min()));*/

            var task = Task.Factory.ContinueWhenAll<object>(
                /*                new Task[] { task1, task2 }, (values) =>*/
                new Task[] { task1, task2 }, (values) =>
                {
                    List<object> resul = new List<object>();
                    List<CurrencyRoot> ra = new List<CurrencyRoot>();
                    foreach (Task<object> task in values)
                    {
                       
                        resul.Add(task.Result);
                        
                    }
                    Console.WriteLine((values[0] as Task<object>).Result);
                   Console.WriteLine((values[1] as Task<object>).Result);
                    
                    double[] ep = new double[30];
                    object[,] exchangeRate = new object[2, 30];
                    
                    var currencyRate = (values[0] as Task<object>).Result;
                    var goldRate = (values[1] as Task<object>).Result;


                    /*                    object currencyRate =  (values[0] as Task<object>).Result;*/
                    System.Reflection.PropertyInfo pi = currencyRate.GetType().GetProperty("rates");
                    exchangeRate = (object[,])(pi.GetValue(currencyRate, null));
                    /*object currencyRate = task1.Result;*/
                    /*                    var a = (values[0] as Task<object>).Result;
                                        var bb = task1.Result;

                                        object goldRate = (values[1] as Task<object>).Result;*/
                    /*                    var currencyRate = task1.Result;
                                        var goldRate = task2.Result;*/
                    Console.WriteLine(currencyRate);
/*                    for (int i = 0; i < 30; i++)
                    {
                        exchangeRate[0, i] = currencyRate.rates[i].effectiveDate;
                        exchangeRate[1, i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                        ep[i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                    }

                    double currently = Convert.ToDouble(exchangeRate[1, 29]);
                    Console.WriteLine("Maksymalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Max() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Max())].ToString()));
                    Console.WriteLine("Minimalny kurs złota liczony w " + choosenCurrencyCode + " wynosił: " + ep.Min() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Min())].ToString()));
                    Console.WriteLine("Różnica między aktualnym a maksymalnym kursem wynosi :" + (currently - ep.Max()));
                    Console.WriteLine("Różnica między aktualnym a minimalnym kursem wynosi :" + (currently - ep.Min()));*/
                    return "ok";

                }
            );
            Console.ReadLine();
        }
     
    }
    
}
