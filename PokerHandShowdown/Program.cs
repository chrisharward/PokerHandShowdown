using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Poker Hand Showdown\r\n");
            try
            {
                var round = new RoundBuilder(new StreamReader(Console.OpenStandardInput())).Build();
                if (round.Players.Count == 0)
                {
                    Console.WriteLine("Incorrect input format - no players specified\r\n");
                    PrintHelp();
                }
                else
                {
                    var winners = round.Play();
                
                    int i = 0;
                    foreach (var player in winners)
                    {
                        if (i++ != 0)
                        {
                            Console.Write(", ");
                        }
                        Console.Write(player.Name);
                    }
                    Console.WriteLine(" wins");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Incorrect input format - {0}\r\n", ex.Message));
                PrintHelp();
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Expected input format: <name> <card1> <card2> <card3> <card4> <card5> followed by a newline to start the game");
            Console.WriteLine("e.g.\tJoe\r\n\t2c 3c 4c 5c Ac\r\n\tSally\r\n\t6h 8s 4c 5c Kd\r\n");
        }
    }
}
