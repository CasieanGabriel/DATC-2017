using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senzori
{
    public class Sensors_Settings
    {
        public int temp_min_value;
        public int temp_max_value;
        public int hum_min_value;
        public int hum_max_value;

        public Sensors_Settings(int temp_min_value,int temp_max_value,int hum_min_value,int hum_max_value)
        {
            this.temp_min_value = temp_min_value;
            this.temp_max_value = temp_max_value;
            this.hum_min_value = hum_min_value;
            this.hum_max_value = hum_max_value;
        }
    }
}
