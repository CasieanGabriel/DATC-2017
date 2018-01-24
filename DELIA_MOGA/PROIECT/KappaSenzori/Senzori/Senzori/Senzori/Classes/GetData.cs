using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senzori
{
    public class GetData
    {
        public static void GetRandomSensorDate()
        {
            int temperature;
            int humidity;
            int first = 0;
            Random myRandomGenerator = new Random();
            for(int sensor_iteration=0; sensor_iteration<Variables.sensors_nr;sensor_iteration++)
            {
                for (int value_iteration = 0; value_iteration < Variables.values_nr; value_iteration++)
                {
                    temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMin, Variables.My_Sensors_Settings[first].TemperatureMax);
                    humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMin, Variables.My_Sensors_Settings[first].HumidityMax);
                    CustomJSON newData = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);   
                    Variables.Data_List.Add(newData);
                    //JsonConvert.SerializeObject(Variables.Data_List);
                }
            }
        }

        public static void GetRandomBrokenSensorDate()
        {
            GetRandomBrokenSensor();
            int temperature;
            int humidity;
            int first = 0;
            Random myRandomGenerator = new Random();
            for (int sensor_iteration = 0; sensor_iteration < Variables.sensors_nr; sensor_iteration++)
            {
                for (int value_iteration = 0; value_iteration < Variables.values_nr; value_iteration++)
                {
                    if (Variables.broken_field == sensor_iteration)
                    {
                        switch(Variables.broken_sensor)
                        {
                            case "temperature":
                                switch(Variables.broken_boundary)
                                {
                                    case false:
                                        temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMin-50, Variables.My_Sensors_Settings[first].TemperatureMin-1);
                                        humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMin, Variables.My_Sensors_Settings[first].HumidityMax);
                                        CustomJSON newData = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);
                                        Variables.Data_List.Add(newData);
                                        break;
                                    case true:
                                        temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMax+1, Variables.My_Sensors_Settings[first].TemperatureMax+50);
                                        humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMin, Variables.My_Sensors_Settings[first].HumidityMax);
                                        CustomJSON newData1 = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);
                                        Variables.Data_List.Add(newData1);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "humidity":
                                switch (Variables.broken_boundary)
                                {
                                    case false:
                                        temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMin, Variables.My_Sensors_Settings[first].TemperatureMax);
                                        humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMin-50, Variables.My_Sensors_Settings[first].HumidityMin-1);
                                        CustomJSON newData = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);
                                        Variables.Data_List.Add(newData);
                                        break;
                                    case true:
                                        temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMin, Variables.My_Sensors_Settings[first].TemperatureMax);
                                        humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMax+1, Variables.My_Sensors_Settings[first].HumidityMax+50);
                                        CustomJSON newData1 = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);
                                        Variables.Data_List.Add(newData1);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        temperature = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].TemperatureMin, Variables.My_Sensors_Settings[first].TemperatureMax);
                        humidity = myRandomGenerator.Next(Variables.My_Sensors_Settings[first].HumidityMin, Variables.My_Sensors_Settings[first].HumidityMax);
                        CustomJSON newData = new CustomJSON(sensor_iteration, temperature, humidity, DateTime.Now);
                        Variables.Data_List.Add(newData);
                    }
                }
            }
        }

        public static void GetRandomBrokenSensor()
        {
            Random myRandomGenerator_field = new Random();
            Variables.broken_field = myRandomGenerator_field.Next(0, Variables.sensors_nr);
            Random myRandomGenerator_sensor = new Random();
            int sensor = myRandomGenerator_sensor.Next(0, 2);
            Variables.broken_sensor = sensor==0 ? "temperature" : "humidity";
            Random myRandomGenerator_boundary = new Random();
            int boundary = myRandomGenerator_boundary.Next(0, 2);
            Variables.broken_boundary = boundary == 0 ? false : true;
        }
    }
}
