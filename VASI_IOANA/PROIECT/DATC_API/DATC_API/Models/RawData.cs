using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATC_API.Models
{
    public class RawData
    {
        public RawData(string s_FreeOrNot, string s_ResOrOcc, string s_Malfunction)
        {
            FreeOrNot = s_FreeOrNot;
            ResOrOcc = s_ResOrOcc;
            Malfunction = s_Malfunction;
        }

        public RawData()
        {

        }

        public string FreeOrNot { get; set; }
        public string ResOrOcc { get; set; }
        public string Malfunction { get; set; }
    }
}