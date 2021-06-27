using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAMBurner
{
    class FarmResponse
	{
        public string mobile { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string breed_type { get; set; }
		public string location_of_treatment_type { get; set; }
		public string number_of_lactating_cows { get; set; }
	}
}
