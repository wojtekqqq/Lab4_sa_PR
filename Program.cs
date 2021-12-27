using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
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
            string urlCurrency = "http://api.nbp.pl/api/exchangerates/rates/a/" + choosenCurrencyCode +
                                 "/last/30/?format=json";
            string urlGold = "http://api.nbp.pl/api/cenyzlota/last/30/?format=json";
            string urlAviliableCurrency = "http://api.nbp.pl/api/exchangerates/tables/a/?format=json";

            HttpClient clientChoseCurrency = new HttpClient();
            HttpResponseMessage responseChoseCurrency = clientChoseCurrency.GetAsync(urlAviliableCurrency).Result;
            responseChoseCurrency.EnsureSuccessStatusCode();
            string resultChoseCurrency = responseChoseCurrency.Content.ReadAsStringAsync().Result;
            var currencyRateChoseCurrency = JsonConvert.DeserializeObject<ChooseRoot[]>(resultChoseCurrency);
            Console.WriteLine(currencyRateChoseCurrency);

            for (int i = 0; i < currencyRateChoseCurrency[0].rates.Count; i++)
            {
                Console.WriteLine( i + " " + currencyRateChoseCurrency[0].rates[i].code + " " + currencyRateChoseCurrency[0].rates[i].currency);
            }
            Console.WriteLine("Wybierz nr waluty, którą chcesz wykoszystać do przeliczeń na złoto :");
            int choosenCurrencyId = Convert.ToInt32(Console.ReadLine());
            choosenCurrencyCode = currencyRateChoseCurrency[0].rates[choosenCurrencyId].code;













            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(urlUsd).Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            var currencyRate = JsonConvert.DeserializeObject<CurrencyRoot>(result);

            HttpClient clientGold = new HttpClient();
            HttpResponseMessage responseGold = clientGold.GetAsync(urlGold).Result;
            string resultGold = responseGold.Content.ReadAsStringAsync().Result;
            var goldRate = JsonConvert.DeserializeObject<GoldRate[]>(resultGold);
            /*            Console.WriteLine(currencyRate.rates[0].effectiveDate);
                        Console.WriteLine(goldRate);
                        Console.WriteLine(currencyRate)*/
            ;
            /*            Console.WriteLine(result);*/

            /*            Exchange exchange = new Exchange();*/
            List<Exchange> exchange = new List<Exchange>();

            /*            object [,] exchangeRate = new object[30,2];
                            for (int i = 0; i < 30; i++)
                            {
                                exchangeRate[i, 0] = currencyRate.rates[i].effectiveDate;
                                exchangeRate[i, 1] = (goldRate[i].cena / currencyRate.rates[i].mid);



                            }*/
            double[] ep = new double[30];
            object[,] exchangeRate = new object[2, 30];
            for (int i = 0; i < 30; i++)
            {
                var max = exchangeRate[1, 0];
                exchangeRate[0, i] = currencyRate.rates[i].effectiveDate;
                exchangeRate[1, i] = (goldRate[i].cena / currencyRate.rates[i].mid);
                ep[i] = (goldRate[i].cena / currencyRate.rates[i].mid);
            }
            /*            Console.WriteLine(exchange);

                        Console.WriteLine(ep.ToList().IndexOf(ep.Max()));*/

            /*            Console.WriteLine(ep.Max());*/
            double currently = Convert.ToDouble(exchangeRate[1, 29]);
            Console.WriteLine("Maksymalny kurs złota wynosił: " + ep.Max() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Max())].ToString()));
            Console.WriteLine("Minimalny kurs złota wynosił: " + ep.Min() + " w dniu " + (exchangeRate[0, ep.ToList().IndexOf(ep.Min())].ToString()));
            Console.WriteLine("Różnica między aktualnym a maksymalnym kursem wynosi :" + (currently - ep.Max()));
            Console.WriteLine("Różnica między aktualnym a minimalnym kursem wynosi :" + (currently - ep.Min()));

            /*            Console.WriteLine(ep.Max());*/




            /*          double[] ep = new double[30];
                          for (int i = 0; i < 30; i++)
                          {
                              ep[i] = (goldRate[i].cena / currencyRate.rates[i].mid);




                          }
                          Console.WriteLine(ep.Max());


                      Console.WriteLine(exchange);


                          Console.WriteLine(exchangeRate);
                          double min = 0, max = 0;
                          for (int i = 0; i < 30; i++)
                          {



                          }



                          foreach (var element in exchangeRate)
                          {

                              Console.WriteLine(element.GetType());
                          }
          */


        }
    }
    
}
