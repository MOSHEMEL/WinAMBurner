using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinAMBurner
{
    class LoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string tablet { get; set; }
    }

    class LoginResponseJson
    {
        public string token { get; set; }
        public UserJson user { get; set; }
    }

    class UserJson
    {
        public int pk { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

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

    class ServiceJson
    {
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
    }

    class SettingsJson
    {
        public int max_device_pulses { get; set; }
        public int number_of_pulses_per_treatment { get; set; }
    }

    class TreatmentPackageJson
    {
        public int id { get; set; }
        public bool is_active { get; set; }
        public int part_number { get; set; }
        public int amount_of_treatments { get; set; }
        public string description { get; set; }
        public string contract_type { get; set; }
        public string added_date { get; set; }
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
        //public static string IS_ACTIVE = "Yes";
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

        public string pEmail { get { return email; } set { email = value; } }
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
            IsActive = Cnst.IS_ACTIVE;
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

    class Service : ServiceJson
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string ADDRESS = "Adress";
        //public static string COUNTRY = "Country";
        public static string CITY = "City";
        //public static string STATE = "State";
        public static string EMAIL = "Email@Email.com";
        public static string NUMBER_OF_DAIRY_FARMS = "0";
        public static string NUMBER_OF_DAIRY_COWS = "0";
        public static string NAME = "Name";
        //public static List<string> CONTRACT_TYPE = new List<string>() { "Purchase" };
        public static string CONTACT = "1";
        public static string DISTRIBUTOR = "0";

        public int Id { get { return id; } set { id = value; } }

        //public string Mobile { get { return mobile; } set { mobile = value; } }
        public string pMobile { get { return mobile; } set { mobile = value; } }
        public Field fMobile;// = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Nine);
        public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }

        //public string Address { get { return address; } set { address = value; } }
        public string pAddress { get { return address; } set { address = value; } }
        public Field fAddress;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Address { get { fAddress.val = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.val; } }

        //public string Country { get { return country; } set { country = value; } }
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

        //public string City { get { return city; } set { city = value; } }
        public string pCity { get { return city; } set { city = value; } }
        public Field fCity;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field City { get { fCity.val = pCity; return fCity; } set { fCity = value; pCity = fCity.val; } }

        //public string State { get { return state; } set { state = value; } }
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

        //public string Email { get { return email; } set { email = value; } }
        public string pEmail { get { return email; } set { email = value; } }
        public Field fEmail;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }

        public string pNumberOfDairyFarms
        {
            get
            {
                return Gui.intToString(number_of_dairy_farms);
            }
            set
            {
                number_of_dairy_farms = Gui.stringToInt(value);
            }
        }
        public Field fNumberOfDairyFarms;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field NumberOfDairyFarms { get { fNumberOfDairyFarms.val = pNumberOfDairyFarms; return fNumberOfDairyFarms; } set { fNumberOfDairyFarms = value; pNumberOfDairyFarms = fNumberOfDairyFarms.val; } }

        public string pNumberOfDairyCows
        {
            get
            {
                return Gui.intToString(number_of_dairy_cows);
            }
            set
            {
                number_of_dairy_cows = Gui.stringToInt(value);
            }
        }
        public Field fNumberOfDairyCows;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field NumberOfDairyCows { get { fNumberOfDairyCows.val = pNumberOfDairyCows; return fNumberOfDairyCows; } set { fNumberOfDairyCows = value; pNumberOfDairyCows = fNumberOfDairyCows.val; } }

        //public string Name { get { return name; } set { name = value; } }
        public string pName { get { return name; } set { name = value; } }
        public Field fName;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }

        //public string ContractType { get { return contract_type; } set { contract_type = value; } }
        public string pContractType { get { return contract_type; } set { contract_type = value; } }
        public Field fContractType;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

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

        public Service()
        {
            Id = ID;
            //Mobile = MOBILE;
            //    Gui.textBoxSmallDraw(this, ref richTextBox8, service.Mobile, Service.MOBILE, placeh: Gui.Place.RightTwo, placev: Gui.Place.Five);
            //    Gui.labelDraw(this, ref label8, "Mobile:", autoSize: false, placeh: Gui.Place.RightOne, placev: Gui.Place.Five);
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five); //MOBILE;
            //Address = ADDRESS;
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Five); //ADDRESS;
            //Country = COUNTRY;
            Country = new Field(Cnst.COUNTRY.First(), Cnst.COUNTRY, typeof(ComboBox), "Country:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Six); //COUNTRY.First();
            //City = CITY;
            City = new Field(CITY, null, typeof(RichTextBox), "City:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three); //CITY;
            //State = STATE;
            State = new Field(Cnst.STATE.First(), Cnst.STATE, typeof(ComboBox), "State:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Seven); //STATE;
            //Email = EMAIL;
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Six);
            //NumberOfDairyFarms = NUMBER_OF_DAIRY_FARMS;
            //    Gui.labelDraw(this, ref label1, "# of dairy farms:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Three);
            NumberOfDairyFarms = new Field(NUMBER_OF_DAIRY_FARMS, null, typeof(RichTextBox), "# of dairy farms:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three);
            //NumberOfDairyCows = NUMBER_OF_DAIRY_COWS;
            //    Gui.labelDraw(this, ref label2, "# of dairy cows:", autoSize: false, placeh: Gui.Place.LeftTwo, placev: Gui.Place.Four);
            NumberOfDairyCows = new Field(NUMBER_OF_DAIRY_COWS, null, typeof(RichTextBox), "# of dairy cows:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
            //Name = NAME;
            //    Gui.textBoxSmallDraw(this, ref richTextBox7, service.Name, Service.NAME, placeh: Gui.Place.RightTwo, placev: Gui.Place.Four);
            Name = new Field(NAME, null, typeof(RichTextBox), "Name of contact::", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four); //NAME;
            //ContractType = CONTRACT_TYPE[0];
            ContractType = new Field(Cnst.CONTRACT_TYPE.First(), Cnst.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine); //CONTRACT_TYPE.First();
            Contact = CONTACT;
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

    class TreatmentPackage : TreatmentPackageJson
    {
        public static int ID = 0;
        public static string PART_NUMBER = string.Empty;
        public static string AMOUNT_OF_TREATMENTS = string.Empty;
        public static string DESCRIPTION = string.Empty;
        public static string CONTRACT_TYPE = string.Empty;
        public static string ADDED_DATE = string.Empty;

        public int Id { get { return id; } set { id = value; } }
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
        //public int part_number;
        public string PartNumber
        {
            get
            {
                return Gui.intToString(part_number);
            }
            set
            {
                part_number = Gui.stringToInt(value);
            }
        }
        //public int amount_of_pulses;
        public string AmountOfTreatments
        {
            get
            {
                return Gui.intToString(amount_of_treatments);
            }
            set
            {
                amount_of_treatments = Gui.stringToInt(value);
            }
        }
        //public string description;
        public string Description { get { return description; } set { description = value; } }
        //public string contract_type;
        public string ContractType { get { return contract_type; } set { contract_type = value; } }
        //public string added_date;
        public string AddedDate { get { return added_date; } set { added_date = value; } }

        public TreatmentPackage()
        {
            Id = 0;
            IsActive = Cnst.IS_ACTIVE;
            PartNumber = PART_NUMBER;
            AmountOfTreatments = AMOUNT_OF_TREATMENTS;
            Description = DESCRIPTION;
            ContractType = CONTRACT_TYPE;
            AddedDate = ADDED_DATE;
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
