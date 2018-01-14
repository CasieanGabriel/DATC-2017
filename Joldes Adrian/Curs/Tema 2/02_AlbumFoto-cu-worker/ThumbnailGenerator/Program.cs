using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine("Here we go");
			var worker = new Worker();
			worker.Init();
			worker.Process();
		}
	}
}
