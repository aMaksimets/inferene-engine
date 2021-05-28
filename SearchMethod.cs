using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {
    public abstract class SearchMethod {

        public List<string> path = new List<string>();
        public abstract string Search (KB KB, CL query);
    }
}
