using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Page
    {
        private string href;
        private bool templated;

        public bool Templated
        {
            get { return templated; }
            set { templated = value; }
        }

        public string Href
        {
            get { return href; }
            set { href = value; }
        }
    }
}
