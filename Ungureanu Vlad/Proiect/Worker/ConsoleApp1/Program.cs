using System;
using System.Drawing;
using System.Globalization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("red:"+Color.Red.ToArgb());
            int intValue = Color.Red.ToArgb();
            string hexValue = intValue.ToString("X");
            //int intAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            Console.WriteLine(hexValue);
            Console.WriteLine("white:" + Color.White.ToArgb());
             intValue = Color.White.ToArgb();
             hexValue = intValue.ToString("X");   
            Console.WriteLine(hexValue);
            Console.WriteLine("black:" + Color.Black.ToArgb());
             intValue = Color.Black.ToArgb();
             hexValue = intValue.ToString("X");
            Console.WriteLine(hexValue);*/
            while (true)
            {
                int R, G, B;
                Console.Write("R:");
                R= int.Parse(Console.ReadLine());
                Console.Write("G:");
                G = int.Parse(Console.ReadLine());
                Console.Write("B:");
                B = int.Parse(Console.ReadLine());
                Color color = Color.FromArgb(R, G, B);
                int intValue = color.ToArgb();
                string hexValue = intValue.ToString("X");
                Console.WriteLine(hexValue);
            }

            /*string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            string[] dateStrings = {"5/1/2009 6:32 PM", "05/01/2009 6:32:05 PM",
                              "5/1/2009 6:32:00", "05/01/2009 06:32",
                              "05/01/2009 06:32:00 PM", "05/01/2009 06:32:00"};
            DateTime dateValue;

            foreach (string dateString in dateStrings)
            {
                if (DateTime.TryParseExact(dateString, formats,
                                           new CultureInfo("en-US"),
                                           DateTimeStyles.None,
                                           out dateValue))
                    Console.WriteLine("Converted '{0}' to {1}.", dateString, dateValue);
                else
                    Console.WriteLine("Unable to convert '{0}' to a date.", dateString);
            }
            */
            Console.ReadKey();
        }
    }
}
