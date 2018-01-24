using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senzori
{
    class Variables
    {
        public static int sensors_nr;
        public static int values_nr;
        public static List<CustomJSON> Data_List = new List<CustomJSON>();
        public static List<Sensors_Settings> My_Sensors_Settings = new List<Sensors_Settings>();
        public static int broken_field;
        public static string broken_sensor;
        public static bool broken_boundary;
    }
}
