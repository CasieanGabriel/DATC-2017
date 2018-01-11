using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebJob1
{
    public class DateLocParcare
    {
        private int id_loc;
        private int loc_liber;
        private int loc_ocupat;
        public int GetLocL { get { return loc_liber; } }
        public int SetLocL { set { loc_liber = value; } }
        public int GetLocO { get { return loc_ocupat; } }
        public int SetLocO { set { loc_ocupat = value; } }
        public int GetNrLoc { get { return id_loc; } }
        public int SetNrLoc { set { id_loc = value; } }

    }
}
