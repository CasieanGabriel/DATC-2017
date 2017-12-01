using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class RootObject
    {

        private Links links;
        private Embedded embedded;

        public RootObject(Links links, Embedded embedded)
        {
            this.links = links;
            this.embedded = embedded;
        }

        internal Embedded Embedded
        {
            get { return embedded; }
            set { embedded = value; }
        }

        public Links Links
        {
            get { return links; }
            set { links = value; }
        }

        public static explicit operator RootObject(JToken token)
        {
            return new RootObject((Links)token["links"], (Embedded)token["embedded"]);
        }
    }
}
