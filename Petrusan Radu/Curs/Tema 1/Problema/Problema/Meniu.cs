using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problema
{
    class Meniu
    {
        public void AfiseazaOptiuni()
        {
            Console.WriteLine("1. Vezi resurse");
            Console.WriteLine("2. Adauga bere");
            Console.WriteLine("3. Iesire");
            Console.WriteLine("");
        }

        public int CitesteOptiune()
        {
            int optiune;
            Console.Write("Alegeti o optiune: ");

            try
            {
                optiune = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Optiune invalida!");
                optiune = CitesteOptiune();
            }
            
            return optiune;
        }
    }
}
