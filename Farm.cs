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
        public string email { get; set; }
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

    class SettingsJson
    {
        public int max_device_pulses;
        public int number_of_pulses_per_treatment;
    }

    class Farm : FarmJson
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string EMAIL = "email@email.com";
        public static string ADDRESS = "Adress";
        //public static Dictionary<string, string> DCOUNTRY;
        //public static List<string> COUNTRY;
        public static string CITY = "City";
        //public static Dictionary<string, string> DSTATE;
        //public static List<string> STATE;
        public static string IS_ACTIVE = "Yes";
        public static string NAME = "Name";
        public static List<string> FARM_TYPE;
        public static List<string> BREED_TYPE;
        public static List<string> MILKING_SETUP_TYPE;
        public static List<string> LOCATION_OF_TREATMENT_TYPE;
        //public static List<string> CONTRACT_TYPE;
        public static string NUMBER_OF_LACTATING_COWS = "0";
        public static List<string> DHI_TEST = new List<string>() { "No", "Yes" };
        public static string CONTACT = "0";
        public static string DISTRIBUTOR = "0";

        //public class Field
        //{
        //    public string val;
        //    public string deflt;
        //    public List<string> items;
        //    public Control control;
        //    public Control lcontrol;
        //    public Type type;
        //    public string text;
        //    public Gui.Place placeh = Gui.Place.Center;
        //    public Gui.Place lplaceh = Gui.Place.Center;
        //    public Gui.Place placev = Gui.Place.Center;
        //
        //    public Field(string deflt, List<string> items, Type type, string text, Gui.Place placeh, Gui.Place lplaceh, Gui.Place placev)
        //    {
        //        this.deflt = deflt;
        //        this.val = deflt;
        //        this.items = items;
        //        this.type = type;
        //        this.text = text;
        //        this.placeh = placeh;
        //        this.lplaceh = lplaceh;
        //        this.placev = placev;
        //    }
        //}

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
                Cnst.DCOUNTRY.TryGetValue(country, out value); 
                return value; 
            } 
            set 
            { 
                country = Cnst.DCOUNTRY.Single(c => (c.Value == value)).Key;
            } 
        }
        public Field fCountry;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        public string pCity { get { return city; } set { city = value; } }
        public Field fCity;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field City { get { fCity.val = pCity; return fCity; } set { fCity = value; pCity = fCity.val; } }

        public string pState
        {
            get
            {
                string value;
                Cnst.DSTATE.TryGetValue(state, out value);
                return value;
            }
            set
            {
                state = Cnst.DSTATE.Single(c => (c.Value == value)).Key;
            }
        }
        public Field fState;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }
        //public string pState { get { return state; } set { state = value; } }
        //public Field fState;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        //public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

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

        public string pEmail{ get { return email; } set { email = value; } }
        public Field fEmail;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }

        public string pName { get { return name; } set { name = value; } }
        public Field fName;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }
        //public Field fName = new Field("Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three);

        public string pFarmType { get { return farm_type; } set { farm_type = value; } }
        public Field fFarmType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field FarmType { get { fFarmType.val = pFarmType; return fFarmType; } set { fFarmType = value; pFarmType = fFarmType.val; } }
        //public Field fFarmType = new Field("Farm Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three);

        public string pBreedType { get { return breed_type; } set { breed_type = value; } }
        public Field fBreedType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field BreedType { get { fBreedType.val = pBreedType; return fBreedType; } set { fBreedType = value; pBreedType = fBreedType.val; } }
        //public Field fBreedType = new Field("Breed Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four);

        public string pMilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value; } }
        public Field fMilkingSetupType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field MilkingSetupType { get { fMilkingSetupType.val = pMilkingSetupType; return fMilkingSetupType; } set { fMilkingSetupType = value; pMilkingSetupType = fMilkingSetupType.val; } }
        //public Field fMilkingSetupType = new Field("Milking Setup:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Seven);

        public string pLocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value; } }
        public Field fLocationOfTreatmentType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field LocationOfTreatmentType { get { fLocationOfTreatmentType.val = pLocationOfTreatmentType; return fLocationOfTreatmentType; } set { fLocationOfTreatmentType = value; pLocationOfTreatmentType = fLocationOfTreatmentType.val; } }
        //public Field fLocationOfTreatmentType = new Field("Location of Treatment:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Eight);

        public string pContractType { get { return contract_type; } set { contract_type = value; } }
        public Field fContractType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }
        //public Field fContractType = new Field("Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine);

        public string pNumberOfLactatingCows
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
        public Field fNumberOfLactatingCows;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field NumberOfLactatingCows { get { fNumberOfLactatingCows.val = pNumberOfLactatingCows; return fNumberOfLactatingCows; } set { fNumberOfLactatingCows = value; pNumberOfLactatingCows = fNumberOfLactatingCows.val; } }
        //public Field fNumberOfLactatingCows = new Field("# of Lactating Cows:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five);

        public string pDhiTest
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
        public Field fDhiTest;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field DhiTest { get { fDhiTest.val = pDhiTest; return fDhiTest; } set { fDhiTest = value; pDhiTest = fDhiTest.val; } }
        //public Field fDhiTest = new Field(string.Empty, 0, 0, 0);

        public string pContact
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
        public Field fContact;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Contact { get { fContact.val = pContact; return fContact; } set { fContact = value; pContact = fContact.val; } }
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
            Country = new Field(Cnst.COUNTRY.First(), Cnst.COUNTRY, typeof(ComboBox), "Country:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Five); //COUNTRY.First();
            City = new Field(CITY, null, typeof(RichTextBox), "City:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Seven); //CITY;
            State = new Field(Cnst.STATE.First(), Cnst.STATE, typeof(ComboBox), "State:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Six); //STATE;
            IsActive = IS_ACTIVE;
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Ten);
            Name = new Field(NAME, null, typeof(RichTextBox), "Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three); //NAME;
            FarmType = new Field(FARM_TYPE.First(), FARM_TYPE, typeof(ComboBox), "Farm Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three); //FARM_TYPE.First();
            BreedType = new Field(BREED_TYPE.First(), BREED_TYPE, typeof(ComboBox), "Breed Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four); //BREED_TYPE.First();
            MilkingSetupType = new Field(MILKING_SETUP_TYPE.First(), MILKING_SETUP_TYPE, typeof(ComboBox), "Milking Setup:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Seven); //MILKING_SETUP_TYPE.First();
            LocationOfTreatmentType = new Field(LOCATION_OF_TREATMENT_TYPE.First(), LOCATION_OF_TREATMENT_TYPE, typeof(ComboBox), "Location of Treatment:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Eight); //LOCATION_OF_TREATMENT_TYPE.First();
            ContractType = new Field(Cnst.CONTRACT_TYPE.First(), Cnst.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine); //CONTRACT_TYPE.First();
            NumberOfLactatingCows = new Field(NUMBER_OF_LACTATING_COWS, null, typeof(RichTextBox), "# of Lactating Cows:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five); //NUMBER_OF_LACTATING_COWS;
            DhiTest = new Field(DHI_TEST.First(), DHI_TEST, typeof(ComboBox), "Monthly DHI test:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Six); //DHI_TEST;
            Contact = new Field(CONTACT, null, typeof(RichTextBox), "Contact Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Eight); //CONTACT;
            Distributor = DISTRIBUTOR;
        }
    }

    class Settings : SettingsJson
    {
        public string maxDevicePulses
        {
            get
            {
                return Gui.intToString(max_device_pulses);
            }
            set
            {
                max_device_pulses = Gui.stringToInt(value);
            }
        }

        public string numberOfPulsesPerTreatment
        {
            get
            {
                return Gui.intToString(number_of_pulses_per_treatment);
            }
            set
            {
                number_of_pulses_per_treatment = Gui.stringToInt(value);
            }
        }
    }

    class Field
    {
        public string val;
        public string deflt;
        public List<string> items;
        public Control control;
        public Control lcontrol;
        public Type type;
        public string text;
        public Gui.Place placeh = Gui.Place.Center;
        public Gui.Place lplaceh = Gui.Place.Center;
        public Gui.Place placev = Gui.Place.Center;

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
}
