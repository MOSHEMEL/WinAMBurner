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

    class DistributorJson
        {
        public int id { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool is_active { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string company_id { get; set; }
        public string parent_distributor { get; set; }
        }

    class ContactJson
    {
        public int id { get; set; }
        public DistributorJson distributor { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public List<string> assigned_tablets { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
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
        //public int contact_name { get; set; }
        //public int contact { get; set; }
        //public int distributor { get; set; }
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
        //public int contact { get; set; }
        //public int distributor { get; set; }
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

    class ActionJson
    {
        //public int id { get; set; }
        //public string date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public int part_number { get; set; }
        //public int contact { get; set; }
        public string tablet { get; set; }
        public int farm { get; set; }
        public int? service_provider { get; set; }
        //public int distributor { get; set; }
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
        public static string CONTACT_NAME = "Contact name";
        //public static string CONTACT = "0";
        //public static string DISTRIBUTOR = "0";

        public int Id { get { return id; } set { id = value; } }

        public string pMobile { get { return mobile; } set { mobile = value; } }
        public Field fMobile;
        public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }

        public string pAddress { get { return address; } set { address = value; } }
        public Field fAddress;
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
                country = Cnst.DCOUNTRY.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fCountry;
        public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        public string pCity { get { return city; } set { city = value; } }
        public Field fCity;
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
                state = Cnst.DSTATE.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fState;
        public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

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

        public string pEmail { get { return email; } set { email = value; } }
        public Field fEmail;
        public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }

        public string pName { get { return name; } set { name = value; } }
        public Field fName;
        public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }

        public string pFarmType { get { return farm_type; } set { farm_type = value; } }
        public Field fFarmType;
        public Field FarmType { get { fFarmType.val = pFarmType; return fFarmType; } set { fFarmType = value; pFarmType = fFarmType.val; } }

        public string pBreedType { get { return breed_type; } set { breed_type = value; } }
        public Field fBreedType;
        public Field BreedType { get { fBreedType.val = pBreedType; return fBreedType; } set { fBreedType = value; pBreedType = fBreedType.val; } }

        public string pMilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value; } }
        public Field fMilkingSetupType;
        public Field MilkingSetupType { get { fMilkingSetupType.val = pMilkingSetupType; return fMilkingSetupType; } set { fMilkingSetupType = value; pMilkingSetupType = fMilkingSetupType.val; } }

        public string pLocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value; } }
        public Field fLocationOfTreatmentType;
        public Field LocationOfTreatmentType { get { fLocationOfTreatmentType.val = pLocationOfTreatmentType; return fLocationOfTreatmentType; } set { fLocationOfTreatmentType = value; pLocationOfTreatmentType = fLocationOfTreatmentType.val; } }

        public string pContractType { get { return contract_type; } set { contract_type = value; } }
        public Field fContractType;
        public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

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
        public Field fNumberOfLactatingCows;
        public Field NumberOfLactatingCows { get { fNumberOfLactatingCows.val = pNumberOfLactatingCows; return fNumberOfLactatingCows; } set { fNumberOfLactatingCows = value; pNumberOfLactatingCows = fNumberOfLactatingCows.val; } }

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
        public Field fDhiTest;
        public Field DhiTest { get { fDhiTest.val = pDhiTest; return fDhiTest; } set { fDhiTest = value; pDhiTest = fDhiTest.val; } }

        //public string pContactName { get { return contact_name; } set { contact_name = value; } }
        //public Field fContactName;
        //public Field ContactName{ get { fContactName.val = pContactName; return fContactName; } set { fContactName= value; pContactName= fContactName.val; } }

        //public string pContact
        //{
        //    get
        //    {
        //        return Gui.intToString(contact);
        //    }
        //    set
        //    {
        //        contact = Gui.stringToInt(value);
        //    }
        //}
        //public Field fContact;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        //public Field Contact { get { fContact.val = pContact; return fContact; } set { fContact = value; pContact = fContact.val; } }

        //public string Distributor
        //{
        //    get
        //    {
        //        return Gui.intToString(distributor);
        //    }
        //    set
        //    {
        //        distributor = Gui.stringToInt(value);
        //    }
        //}

        public Farm()
        {
            Id = ID;
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Nine);
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
            Country = new Field(Cnst.COUNTRY.FirstOrDefault(), Cnst.COUNTRY, typeof(ComboBox), "Country:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Five);
            City = new Field(CITY, null, typeof(RichTextBox), "City:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Seven);
            State = new Field(Cnst.STATE.FirstOrDefault(), Cnst.STATE, typeof(ComboBox), "State:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Six);
            IsActive = Cnst.IS_ACTIVE;
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Ten);
            Name = new Field(NAME, null, typeof(RichTextBox), "Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three);
            FarmType = new Field(FARM_TYPE.FirstOrDefault(), FARM_TYPE, typeof(ComboBox), "Farm Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three);
            BreedType = new Field(BREED_TYPE.FirstOrDefault(), BREED_TYPE, typeof(ComboBox), "Breed Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four);
            MilkingSetupType = new Field(MILKING_SETUP_TYPE.FirstOrDefault(), MILKING_SETUP_TYPE, typeof(ComboBox), "Milking Setup:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Seven);
            LocationOfTreatmentType = new Field(LOCATION_OF_TREATMENT_TYPE.FirstOrDefault(), LOCATION_OF_TREATMENT_TYPE, typeof(ComboBox), "Location of Treatment:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Eight);
            ContractType = new Field(Cnst.CONTRACT_TYPE.FirstOrDefault(), Cnst.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine);
            NumberOfLactatingCows = new Field(NUMBER_OF_LACTATING_COWS, null, typeof(RichTextBox), "# of Lactating Cows:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five);
            DhiTest = new Field(DHI_TEST.FirstOrDefault(), DHI_TEST, typeof(ComboBox), "Monthly DHI test:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Six);
            //ContactName = new Field(CONTACT_NAME, null, typeof(RichTextBox), "Contact Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Eight);
            //Contact = new Field(CONTACT, null, typeof(RichTextBox), "Contact Name:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Eight); //CONTACT;
            //Distributor = DISTRIBUTOR;
        }

        //public override string? ToString()
        //{
        //    return this.Name.val;
        //}
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
        //public static string CONTACT = "0";
        //public static string DISTRIBUTOR = "0";

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
                country = Cnst.DCOUNTRY.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fCountry;
        public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        public string pCity { get { return city; } set { city = value; } }
        public Field fCity;
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
                state = Cnst.DSTATE.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fState;
        public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

        public string pEmail { get { return email; } set { email = value; } }
        public Field fEmail;
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
        public Field fNumberOfDairyFarms;
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
        public Field fNumberOfDairyCows;
        public Field NumberOfDairyCows { get { fNumberOfDairyCows.val = pNumberOfDairyCows; return fNumberOfDairyCows; } set { fNumberOfDairyCows = value; pNumberOfDairyCows = fNumberOfDairyCows.val; } }

        public string pName { get { return name; } set { name = value; } }
        public Field fName;
        public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }

        public string pContractType { get { return contract_type; } set { contract_type = value; } }
        public Field fContractType;
        public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

        //public string Contact
        //{
        //    get
        //    {
        //        return Gui.intToString(contact);
        //    }
        //    set
        //    {
        //        contact = Gui.stringToInt(value);
        //    }
        //}
        
        //public string Distributor
        //{
        //    get
        //    {
        //        return Gui.intToString(distributor);
        //    }
        //    set
        //    {
        //        distributor = Gui.stringToInt(value);
        //    }
        //}

        public Service()
        {
            Id = ID;
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Five);
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Five);
            Country = new Field(Cnst.COUNTRY.FirstOrDefault(), Cnst.COUNTRY, typeof(ComboBox), "Country:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Six);
            City = new Field(CITY, null, typeof(RichTextBox), "City:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Three);
            State = new Field(Cnst.STATE.FirstOrDefault(), Cnst.STATE, typeof(ComboBox), "State:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Seven);
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Six);
            NumberOfDairyFarms = new Field(NUMBER_OF_DAIRY_FARMS, null, typeof(RichTextBox), "# of dairy farms:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Three);
            NumberOfDairyCows = new Field(NUMBER_OF_DAIRY_COWS, null, typeof(RichTextBox), "# of dairy cows:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
            Name = new Field(NAME, null, typeof(RichTextBox), "Name of contact::", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Four);
            ContractType = new Field(Cnst.CONTRACT_TYPE.FirstOrDefault(), Cnst.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", Gui.Place.RightTwo, Gui.Place.RightOne, Gui.Place.Nine);
            //Contact = CONTACT;
            //Distributor = DISTRIBUTOR;
        }

        //public override string? ToString()
        //{
        //    return this.Name.val;
        //}
    }

    class TreatmentPackage : TreatmentPackageJson
    {
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
        //    public static List<string> PART_NUMBER = new List<string>() { ""};
        //    
        //    public string pPartNumber
        //    {
        //        get
        //        {
        //            return Gui.intToString(part_number);
        //        }
        //        set
        //        {
        //            part_number = Gui.stringToInt(value);
        //        }
        //    }
        //    public Field fPartNumber;
        //    public Field PartNumber { get { fPartNumber.val = pPartNumber; return fPartNumber; } set { fPartNumber = value; pPartNumber = fPartNumber.val; } }
        //    
        //    public TreatmentPackage()
        //    {
        //        PartNumber = new Field(PART_NUMBER.First(), PART_NUMBER, typeof(ComboBox), "Add treatments to AM – SN", placev: Gui.Place.Seven, lplacev: Gui.Place.Six);
        //    }
    }

    //class Action: ActionJson
    //{
    //    public int Id { get { return id; } set { id = value; } }
    //    public string Date { get { return date; } set { date = value; } }
    //    public string AptxId { get { return aptx_id; } set { aptx_id = value; } }
    //    public string AmId { get { return am_id; } set { am_id = value; } }
    //    public int PartNumber { get { return part_number; } set { part_number = value; } }
    //    public int Contact { get { return contact; } set { contact = value; } }
    //    public string Tablet { get { return tablet; } set { tablet = value; } }
    //    public int Farm { get { return farm; } set { farm = value; } }
    //    public int ServiceProvider { get { return service_provider; } set { service_provider = value; } }
    //    public int Distributor { get { return distributor; } set { distributor = value; } }
    //}

    class Field
    {
        public string val;
        public string deflt;
        public List<string> items;
        public Control control;
        public Control lcontrol;
        public Type type;
        public string text;
        public Gui.Place placeh;
        public Gui.Place lplaceh;
        public Gui.Place placev;
        public Gui.Place lplacev;

        public Field(string deflt, List<string> items, Type type, string text, 
            Gui.Place placeh = Gui.Place.Center, Gui.Place lplaceh = Gui.Place.Center, Gui.Place placev = Gui.Place.None, Gui.Place lplacev = Gui.Place.None)
        {
            this.deflt = deflt;
            this.val = deflt;
            this.items = items;
            this.type = type;
            this.text = text;
            this.placeh = placeh;
            this.lplaceh = lplaceh;
            this.placev = placev;
            if (lplacev == Gui.Place.None)
                this.lplacev = placev;
            else
                this.lplacev = lplacev;
        }
    }
}
