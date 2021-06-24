using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAMBurner
{
    class Farm
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string EMAIL = "email@email.com";
        public static string ADDRESS = "Adress";
        public static string COUNTRY = "Country";
        public static string CITY = "City";
        public static string STATE = "State";
        public static string IS_ACTIVE = "No";
        public static string NAME = "Name";
        public static List<string> FARM_TYPE = new List<string>() { "Organic" };
        public static List<string> BREED_TYPE = new List<string>() { "Holshtien" };
        public static List<string> MILKING_SETUP_TYPE = new List<string>() { "Parallel" };
        public static List<string> LOCATION_OF_TREATMENT_TYPE = new List<string>() { "Milking Parolee" };
        public static List<string> CONTRACT_TYPE = new List<string>() { "Purchase" };
        public static string NUMBER_OF_LACTATING_COWS = "0";
        public static string DHI_TEST = "No";
        public static string CONTACT = "0";
        public static string DISTRIBUTOR = "0";

        public int id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool is_active { get; set; }
        public string name { get; set; }
        public string farm_type { get; set; }
        public string breed_type { get; set; }
        public string milking_setup_type { get; set; }
        public string location_of_treatment_type { get; set; }
        public string contract_type { get; set; }
        public int number_of_lactating_cows { get; set; }
        public bool dhi_test { get; set; }
        public int contact { get; set; }
        public int distributor { get; set; }

        public int Id { get { return id; } set { id = value; } }
        public string Mobile { get { return mobile; } set { mobile = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string Country { get { return country; } set { country = value; } }
        public string City { get { return city; } set { city = value; } }
        public string State { get { return state; } set { state = value; } }
        public string IsActive
        {
            get
            {
                return Misc.boolToString(is_active);
            }
            set
            {
                is_active = Misc.stringToBool(value);
            }
        }
        public string Name { get { return name; } set { name = value; } }
        public string FarmType { get { return farm_type; } set { farm_type = value; } }
        public string BreedType { get { return breed_type; } set { breed_type = value; } }
        public string MilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value; } }
        public string LocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value; } }
        public string ContractType { get { return contract_type; } set { contract_type = value; } }
        public string NumberOfLactatingCows
        {
            get
            {
                return Misc.intToString(number_of_lactating_cows);
            }
            set
            {
                number_of_lactating_cows = Misc.stringToInt(value);
            }
        }
        public string DhiTest
        {
            get
            {
                return Misc.boolToString(dhi_test);
            }
            set
            {
                dhi_test = Misc.stringToBool(value);
            }
        }
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

        public Farm()
        {
            Id = ID;
            Mobile = MOBILE;
            Address = ADDRESS;
            Country = COUNTRY;
            City = CITY;
            State = STATE;
            IsActive = IS_ACTIVE;
            Name = NAME;
            FarmType = FARM_TYPE[0];
            BreedType = BREED_TYPE[0];
            MilkingSetupType = MILKING_SETUP_TYPE[0];
            LocationOfTreatmentType = LOCATION_OF_TREATMENT_TYPE[0];
            ContractType = CONTRACT_TYPE[0];
            NumberOfLactatingCows = NUMBER_OF_LACTATING_COWS;
            DhiTest = DHI_TEST;
            Contact = CONTACT;
            Distributor = DISTRIBUTOR;
        }
    }
}
