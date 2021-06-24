using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAMBurner
{
    class Service
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string ADDRESS = "Adress";
        public static string COUNTRY = "Country";
        public static string CITY = "City";
        public static string STATE = "State";
        public static string EMAIL = "Email@Email.com";
        public static string NUMBER_OF_DAIRY_FARMS = "0";
        public static string NUMBER_OF_DAIRY_COWS = "0";
        public static string NAME = "Name";
        public static List<string> CONTRACT_TYPE = new List<string>() { "Purchase" };
        public static string CONTACT = "0";
        public static string DISTRIBUTOR = "0";

        public int id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public int number_of_dairy_farms { get; set; }
        public int number_of_dairy_cows { get; set; }
        public string name { get; set; }
        public string contract_type { get; set; }
        public int contact { get; set; }
        public int distributor { get; set; }

        public int Id { get { return id; } set { id = value; } }
        public string Mobile { get { return mobile; } set { mobile = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string Country { get { return country; } set { country = value; } }
        public string City { get { return city; } set { city = value; } }
        public string State { get { return state; } set { state = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string NumberOfDairyFarms
        {
            get
            {
                return Misc.intToString(number_of_dairy_farms);
            }
            set
            {
                number_of_dairy_farms = Misc.stringToInt(value);
            }
        }
        public string NumberOfDairyCows
        {
            get
            {
                return Misc.intToString(number_of_dairy_cows);
            }
            set
            {
                number_of_dairy_cows = Misc.stringToInt(value);
            }
        }
        public string Name { get { return name; } set { name = value; } }
        public string ContractType { get { return contract_type; } set { contract_type = value; } }
        public string Contact
        {
            get
            {
                return Misc.intToString(contact);
            }
            set
            {
                contact = Misc.stringToInt(value);
            }
        }
        public string Distributor
        {
            get
            {
                return Misc.intToString(distributor);
            }
            set
            {
                distributor = Misc.stringToInt(value);
            }
        }

        public Service()
        {
            Id = ID;
            Mobile = MOBILE;
            Address = ADDRESS;
            Country = COUNTRY;
            City = CITY;
            State = STATE;
            Email = EMAIL;
            NumberOfDairyFarms = NUMBER_OF_DAIRY_FARMS;
            NumberOfDairyCows = NUMBER_OF_DAIRY_COWS;
            Name = NAME;
            ContractType = CONTRACT_TYPE[0];
            Contact = CONTACT;
            Distributor = DISTRIBUTOR;
        }
    }
}
