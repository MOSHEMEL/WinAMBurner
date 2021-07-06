using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace WinAMBurner
{
    class FarmJson
    {
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
    }

    class Farm : FarmJson
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string EMAIL = "email@email.com";
        public static string ADDRESS = "Adress";
        public static Dictionary<string, string> DCOUNTRY;
        public static List<string> COUNTRY;
        public static string CITY = "City";
        public static string STATE = "State";
        public static string IS_ACTIVE = "No";
        public static string NAME = "Name";
        public static List<string> FARM_TYPE;
        public static List<string> BREED_TYPE;
        public static List<string> MILKING_SETUP_TYPE;
        public static List<string> LOCATION_OF_TREATMENT_TYPE;
        public static List<string> CONTRACT_TYPE;
        public static string NUMBER_OF_LACTATING_COWS = "0";
        public static string DHI_TEST = "No";
        public static string CONTACT = "0";
        public static string DISTRIBUTOR = "0";

        public class Field
        {
            public string val;
            public string deflt;
            public List<string> items;
            public Control control;
            public Type type;
            public string text;
            public Gui.Place placeh;
            public Gui.Place lplaceh;
            public Gui.Place placev;

            public Field(string deflt, List<string> items, Type type, string text, Gui.Place placeh, Gui.Place lplaceh, Gui.Place placev)
            {
                this.deflt = deflt;
                this.val = deflt;
                this.items = items;
                this.type = type;
                this.text = text;
                this.placeh = placeh;
                this.lplaceh = lplaceh;
                this.placev = placev;
            }
        }

        public int Id { get { return id; } set { id = value; } }

        public string pMobile { get { return mobile; } set { mobile = value; } }
        public Field fMobile;// = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Nine);
        public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }

        public string pAddress { get { return address; } set { address = value; } }
        public Field fAddress;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Address { get { fAddress.val = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.val; } }

        public string pCountry 
        { 
            get 
            { 
                string value; 
                DCOUNTRY.TryGetValue(country, out value); 
                return value; 
            } 
            set 
            { 
                country = DCOUNTRY.Single(c => (c.Value == value)).Key;
            } 
        }
        public Field fCountry;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        public string City { get { return city; } set { city = value; } }
        //public Field fCity = new Field("City:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Seven);

        public string State { get { return state; } set { state = value; } }
        //public Field fState = new Field("State:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Six);
        
        public string IsActive
        {
            get
            {
                return Gui.boolToString(is_active);
            }
            set
            {
                is_active = Gui.stringToBool(value);
            }
        }
        //public Field fIsActive = new Field(string.Empty, 0, 0, 0);
        
        public string Name { get { return name; } set { name = value; } }
        //public Field fName = new Field("Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three);
        
        public string FarmType { get { return farm_type; } set { farm_type = value; } }
        //public Field fFarmType = new Field("Farm Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three);
        
        public string BreedType { get { return breed_type; } set { breed_type = value; } }
        //public Field fBreedType = new Field("Breed Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four);
        
        public string MilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value; } }
        //public Field fMilkingSetupType = new Field("Milking Setup:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Seven);
        
        public string LocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value; } }
        //public Field fLocationOfTreatmentType = new Field("Location of Treatment:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Eight);
        
        public string ContractType { get { return contract_type; } set { contract_type = value; } }
        //public Field fContractType = new Field("Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine);
        
        public string NumberOfLactatingCows
        {
            get
            {
                return Gui.intToString(number_of_lactating_cows);
            }
            set
            {
                number_of_lactating_cows = Gui.stringToInt(value);
            }
        }
        //public Field fNumberOfLactatingCows = new Field("# of Lactating Cows:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five);
        
        public string DhiTest
        {
            get
            {
                return Gui.boolToString(dhi_test);
            }
            set
            {
                dhi_test = Gui.stringToBool(value);
            }
        }
        //public Field fDhiTest = new Field(string.Empty, 0, 0, 0);

        public string Contact
        {
            get
            {
                return Gui.intToString(contact);
            }
            set
            {
                contact = Gui.stringToInt(value);
            }
        }
        //public Field fContact = new Field("Contact Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Eight);
        
        public string Distributor
        {
            get
            {
                return Gui.intToString(distributor);
            }
            set
            {
                distributor = Gui.stringToInt(value);
            }
        }
        //public Field fDistributor = new Field(string.Empty, 0, 0, 0);

        public Farm()
        {
            Id = ID;
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Nine); //MOBILE;
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four); //ADDRESS;
            Country = new Field(COUNTRY.First(), COUNTRY, typeof(ComboBox), "Country:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Five); //COUNTRY.First();
            City = CITY;
            State = STATE;
            IsActive = IS_ACTIVE;
            Name = NAME;
            FarmType = FARM_TYPE.First();
            BreedType = BREED_TYPE.First();
            MilkingSetupType = MILKING_SETUP_TYPE.First();
            LocationOfTreatmentType = LOCATION_OF_TREATMENT_TYPE.First();
            ContractType = CONTRACT_TYPE.First();
            NumberOfLactatingCows = NUMBER_OF_LACTATING_COWS;
            DhiTest = DHI_TEST;
            Contact = CONTACT;
            Distributor = DISTRIBUTOR;
        }
    }
}
