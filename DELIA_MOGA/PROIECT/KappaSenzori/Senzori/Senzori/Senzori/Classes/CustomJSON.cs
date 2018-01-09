using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senzori
{
    class CustomJSON
    {
        private int field;
        private DateTime date_time;
        private float temperature;
        private float humidity;

        public CustomJSON(int field, float temperature,float humidity, DateTime date_time)
        {
            this.field = field;
            this.date_time = date_time;
            this.temperature = temperature;
            this.humidity = humidity;
        }

        public int Field
        {
           get { return this.field; }
        }
        public string Date_Time
        {
            get { return this.date_time.ToString("dd/MM/yyyy HH:mm:ss"); }
        }
        public float Temperature
        {
            get { return this.temperature; }
        }
        public float Humidity
        {
            get { return this.humidity; }
        }

    }
}
