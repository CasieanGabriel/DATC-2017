using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new Worker();
            worker.Init();
            worker.Process();
        }
    }
}
