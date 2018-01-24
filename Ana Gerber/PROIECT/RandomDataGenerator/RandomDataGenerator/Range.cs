using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator
{
    class Range
    {
        public int Temperatura_Min;
        public int Temperatura_Max;
        public int Umididate_Min;
        public int Umididate_Max;

        public Range(int Temperatura_Min, int Temperatura_Max, int Umididate_Min, int Umididate_Max)
        {
            this.Temperatura_Min = Temperatura_Min;
            this.Temperatura_Max = Temperatura_Max;
            this.Umididate_Min = Umididate_Min;
            this.Umididate_Max = Umididate_Max;
        }
    }
}
