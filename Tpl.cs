/*using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4_sa
{
    class Tpl
    {
           private string Work(object sender, CancellationToken token)
            {
                List<object> argslist = sender as List<object>;
                string podsumowanie = null;

                if (argslist != null)
                {
                    IFunction funkcja = (IFunction)argslist[0];

                    for (int i = 1; i < 100; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("Przerwano zadanie.");
                            podsumowanie = "";
                            break;
                        }

                        powierzchnia += funkcja.GetY((decimal)rangeFrom + i * krok);
                        Thread.Sleep(10);
                        if (i % 10 == 0)
                        {
                            Console.WriteLine($"Postęp {i}%");
                        }
                    }

                    powierzchnia = (powierzchnia + (funkcja.GetY(rangeFrom) + funkcja.GetY(rangeTo)) / 2) * krok;
                    Console.WriteLine("Zadanie zakończone");
                    podsumowanie += "Przybliżona wartość całki metodą trapezów dla przedziału: " + name + " wynosi " +
                                    powierzchnia + Environment.NewLine;
                    Console.WriteLine(podsumowanie);
                }

                return podsumowanie;
            }

            public string Name => "TPL";
            public int Id => 2;

            public void Run(IFunction function, decimal rangeFrom, decimal rangeTo, string name)
            {
                List<object> arguments1 = new()
                {
                    function,
                    rangeFrom,
                    rangeTo,
                    name
                };
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                Task<string> task1 = new Task<string>(
                    () => Work(arguments1, token), token);
                task1.Start();

                if (Console.ReadKey(true).KeyChar == 'c')
                {
                    tokenSource.Cancel();
                }

                Console.WriteLine("Koniec zadania");
            }
        
    }
}
*/