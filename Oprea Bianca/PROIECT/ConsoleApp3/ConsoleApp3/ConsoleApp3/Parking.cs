using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Parking
    {
        private int id;
        private int[] data = new int[10];

        public int Id { get { return id; }  set { id = value; } }
        public int[] Data { get { return data; } set { data = value; } }

        public int RandomGen(int x)
        {
            Random random = new Random();
            int rnd = random.Next(-10, x);
            return rnd;
        }
    }
}
