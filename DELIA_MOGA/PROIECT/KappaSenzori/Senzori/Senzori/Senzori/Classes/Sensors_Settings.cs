using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senzori
{
    public class Sensors_Settings
    {
        public int TemperatureMin;
        public int TemperatureMax;
        public int HumidityMin;
        public int HumidityMax;

        public Sensors_Settings(int temp_min_value,int temp_max_value,int hum_min_value,int hum_max_value)
        {
            this.TemperatureMin = temp_min_value;
            this.TemperatureMax = temp_max_value;
            this.HumidityMin = hum_min_value;
            this.HumidityMax = hum_max_value;
        }
    }
}
