using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorDateV1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            List<LocParcare> l = new List<LocParcare>();
            int nrRnd=0;
            while(true)
            {
                for (int i = 0; i < 70; i++)
                {
                    nrRnd = rnd.Next(0, 2);
                    LocParcare loc = new LocParcare();
                    loc.nrLoc = (i + 1).ToString();
                    loc.stareLoc = ((Stare)nrRnd).ToString();
                    l.Add(loc);
                }
                System.Threading.Thread.Sleep(3000);
               for(int i=0;i<70;i++)
                {
                    Console.WriteLine(l.ElementAt(i).nrLoc);
                    Console.WriteLine(l.ElementAt(i).stareLoc);
                }
            }

        }
    }
}
