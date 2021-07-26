using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinAMBurner
{
    class LoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string tablet { get; set; }

        public override string ToString()
        {
            return Const.entityToString(this);
        }
    }

    class LoginResponseJson
    {
        public string token { get; set; }
        public UserJson user { get; set; }

        public override string ToString()
        {
            return Const.entityToString(this);
        }
    }

    class UserJson
    {
        public int pk { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public override string ToString()
        {
            return Const.entityToString(this);
        }
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

        public override string ToString()
        {
            return Const.entityToString(this);
        }
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

        public override string ToString()
        {
            return Const.entityToString(this);
        }
    }

    //interface EntityJson
    //{
    //    public int id { get; set; }
    //    public string mobile { get; set; }
    //    public string address { get; set; }
    //    public string country { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string email { get; set; }
    //    public string name { get; set; }
    //    public string contact_name { get; set; }
    //    public string contract_type { get; set; }
    //}

    interface FarmJson
    {
        //public int id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool is_active { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string contact_name { get; set; }
        public string farm_type { get; set; }
        public string breed_type { get; set; }
        public string milking_setup_type { get; set; }
        public string location_of_treatment_type { get; set; }
        public string contract_type { get; set; }
        public int number_of_lactating_cows { get; set; }
        public bool dhi_test { get; set; }
    }

    interface ServiceJson
    {
        //public int id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public int number_of_dairy_farms { get; set; }
        public int number_of_dairy_cows { get; set; }
        public string name { get; set; }
        public string contact_name { get; set; }
        public string contract_type { get; set; }
    }

    class SettingsJson
    {
        public int max_am_pulses { get; set; }
        public int number_of_pulses_per_treatment { get; set; }

        public override string ToString()
        {
            return Const.entityToString(this);
        }
    }

    interface TreatmentPackageJson
    {
        public int id { get; set; }
        public bool is_active { get; set; }
        public string part_number { get; set; }
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
        public int? farm { get; set; }
        public int? service_provider { get; set; }
        //public int distributor { get; set; }

        public override string ToString()
        {
            return Const.entityToString(this);
        }
    }

    class Entity
    {
        public static int ID = 0;
        public static string MOBILE = "123456";
        public static string EMAIL = "email@email.com";
        public static string ADDRESS = "Adress";
        public static string CITY = "City";
        public static string NAME = "Name";
        public static string CONTACT_NAME = "Contact Name";

        public int id { get; set; }
        //public int id { get; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string contact_name { get; set; }
        public string contract_type { get; set; }
        
        public int Id { get { return id; } set { id = value; } }
        //public int Id { get { return id; } }
    
        public string pName { get { return name; } set { name = value; } }
        public Field fName;
        public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }
    
        public string pAddress { get { return address; } set { address = value; } }
        public Field fAddress;
        public Field Address { get { fAddress.val = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.val; } }
    
        public string pCountry
        {
            get
            {
                return Const.getFromDictionary(Const.DCOUNTRY, country);
            }
            set
            {
                country = Const.DCOUNTRY.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fCountry;
        public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }
    
        public string pState
        {
            get
            {
                return Const.getFromDictionary(Const.DSTATE, state);
            }
            set
            {
                state = Const.DSTATE.FirstOrDefault(c => (c.Value == value)).Key;
            }
        }
        public Field fState;
        public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }
    
        public string pCity { get { return city; } set { city = value; } }
        public Field fCity;
        public Field City { get { fCity.val = pCity; return fCity; } set { fCity = value; pCity = fCity.val; } }
    
        public string pContactName { get { return contact_name; } set { contact_name = value; } }
        public Field fContactName;
        public Field ContactName { get { fContactName.val = pContactName; return fContactName; } set { fContactName = value; pContactName = fContactName.val; } }
    
        public string pMobile { get { return mobile; } set { mobile = value; } }
        public Field fMobile;
        public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }
    
        public string pEmail { get { return email; } set { email = value; } }
        public Field fEmail;
        public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }
    
        public string pContractType { get { return contract_type; } set { contract_type = value; } }
        public Field fContractType;
        public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

        public override string ToString()
        {
            return Name.val;
        }
    }

    class Farm : Entity, FarmJson
    {
        //public static int ID = 0;
        //public static string MOBILE = "123456";
        //public static string EMAIL = "email@email.com";
        //public static string ADDRESS = "Adress";
        //public static string CITY = "City";
        //public static string NAME = "Name";
        public static string NUMBER_OF_LACTATING_COWS = "0";
        public static string[] DHI_TEST = new string[] { "No", "Yes" };
        //public static string CONTACT_NAME = "Contact Name";

        //public int id { get; set; }
        //public string mobile { get; set; }
        //public string address { get; set; }
        //public string country { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        public bool is_active { get; set; }
        //public string email { get; set; }
        //public string name { get; set; }
        //public string contact_name { get; set; }
        public string farm_type { get; set; }
        public string breed_type { get; set; }
        public string milking_setup_type { get; set; }
        public string location_of_treatment_type { get; set; }
        //public string contract_type { get; set; }
        public int number_of_lactating_cows { get; set; }
        public bool dhi_test { get; set; }

        //public int Id { get { return id; } set { id = value; } }
        
        //public string pName { get { return name; } set { name = value; } }
        //public Field fName;
        //public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }
        
        //public string pAddress { get { return address; } set { address = value; } }
        //public Field fAddress;
        //public Field Address { get { fAddress.val = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.val; } }

        //public string pCountry
        //{
        //    get
        //    {
        //        return Cnst.getFromDictionary(Cnst.DCOUNTRY, country);
        //    }
        //    set
        //    {
        //        country = Cnst.DCOUNTRY.FirstOrDefault(c => (c.Value == value)).Key;
        //    }
        //}
        //public Field fCountry;
        //public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        //public string pState
        //{
        //    get
        //    {
        //        return Cnst.getFromDictionary(Cnst.DSTATE, state);
        //    }
        //    set
        //    {
        //        state = Cnst.DSTATE.FirstOrDefault(c => (c.Value == value)).Key;
        //    }
        //}
        //public Field fState;
        //public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

        //public string pCity { get { return city; } set { city = value; } }
        //public Field fCity;
        //public Field City { get { fCity.val = pCity; return fCity; } set { fCity = value; pCity = fCity.val; } }

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

        //public string pContactName { get { return contact_name; } set { contact_name = value; } }
        //public Field fContactName;
        //public Field ContactName { get { fContactName.val = pContactName; return fContactName; } set { fContactName = value; pContactName = fContactName.val; } }

        //public string pMobile { get { return mobile; } set { mobile = value; } }
        //public Field fMobile;
        //public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }

        //public string pEmail { get { return email; } set { email = value; } }
        //public Field fEmail;
        //public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }

        public string pFarmType { get { return farm_type; } set { farm_type = value; } }
        public Field fFarmType;
        public Field FarmType { get { fFarmType.val = pFarmType; return fFarmType; } set { fFarmType = value; pFarmType = fFarmType.val; } }

        public string pBreedType { get { return breed_type; } set { breed_type = value; } }
        public Field fBreedType;
        public Field BreedType { get { fBreedType.val = pBreedType; return fBreedType; } set { fBreedType = value; pBreedType = fBreedType.val; } }

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

        public string pMilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value; } }
        public Field fMilkingSetupType;
        public Field MilkingSetupType { get { fMilkingSetupType.val = pMilkingSetupType; return fMilkingSetupType; } set { fMilkingSetupType = value; pMilkingSetupType = fMilkingSetupType.val; } }

        public string pLocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value; } }
        public Field fLocationOfTreatmentType;
        public Field LocationOfTreatmentType { get { fLocationOfTreatmentType.val = pLocationOfTreatmentType; return fLocationOfTreatmentType; } set { fLocationOfTreatmentType = value; pLocationOfTreatmentType = fLocationOfTreatmentType.val; } }

        //public string pContractType { get { return contract_type; } set { contract_type = value; } }
        //public Field fContractType;
        //public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

        public Farm()
        {
            //Id = ID;
            Name = new Field(NAME, null, typeof(RichTextBox), "Name:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Three);
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Four);
            Country = new Field(Const.COUNTRY.FirstOrDefault(), Const.COUNTRY, typeof(ComboBox), "Country:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Five);
            State = new Field(Const.STATE.FirstOrDefault(), Const.STATE, typeof(ComboBox), "State:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Six);
            City = new Field(CITY, null, typeof(RichTextBox), "City:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Seven);
            IsActive = Const.IS_ACTIVE;
            ContactName = new Field(CONTACT_NAME, null, typeof(RichTextBox), "Contact Name:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Eight);
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Nine);
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Ten);
            FarmType = new Field(Const.FARM_TYPE.FirstOrDefault(), Const.FARM_TYPE, typeof(ComboBox), "Farm Type:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Three);
            BreedType = new Field(Const.BREED_TYPE.FirstOrDefault(), Const.BREED_TYPE, typeof(ComboBox), "Breed Type:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Four);
            NumberOfLactatingCows = new Field(NUMBER_OF_LACTATING_COWS, null, typeof(RichTextBox), "# of Lactating Cows:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Five);
            DhiTest = new Field(DHI_TEST.FirstOrDefault(), DHI_TEST, typeof(ComboBox), "Milk Recording:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Six);
            MilkingSetupType = new Field(Const.MILKING_SETUP_TYPE.FirstOrDefault(), Const.MILKING_SETUP_TYPE, typeof(ComboBox), "Milking Setup:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Seven);
            LocationOfTreatmentType = new Field(Const.LOCATION_OF_TREATMENT_TYPE.FirstOrDefault(), Const.LOCATION_OF_TREATMENT_TYPE, typeof(ComboBox), "Treatment Location:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Eight);
            ContractType = new Field(Const.CONTRACT_TYPE.FirstOrDefault(), Const.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Nine);
        }
    }

    class Service : Entity, ServiceJson 
    {
        //public static int ID = 0;
        //public static string MOBILE = "123456";
        //public static string ADDRESS = "Adress";
        //public static string CITY = "City";
        //public static string EMAIL = "Email@Email.com";
        public static string NUMBER_OF_DAIRY_FARMS = "0";
        public static string NUMBER_OF_DAIRY_COWS = "0";
        //public static string NAME = "Name";
        //public static string CONTACT_NAME = "Contact Name";

        //public int id { get; set; }
        //public string mobile { get; set; }
        //public string address { get; set; }
        //public string country { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public string email { get; set; }
        public int number_of_dairy_farms { get; set; }
        public int number_of_dairy_cows { get; set; }
        //public string name { get; set; }
        //public string contact_name { get; set; }
        //public string contract_type { get; set; }
        
        //public int Id { get { return id; } set { id = value; } }

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

        //public string pAddress { get { return address; } set { address = value; } }
        //public Field fAddress;// = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Four);
        //public Field Address { get { fAddress.val = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.val; } }

        //public string pCountry
        //{
        //    get
        //    {
        //        return Cnst.getFromDictionary(Cnst.DCOUNTRY, country);
        //    }
        //    set
        //    {
        //        country = Cnst.DCOUNTRY.FirstOrDefault(c => (c.Value == value)).Key;
        //    }
        //}
        //public Field fCountry;
        //public Field Country { get { fCountry.val = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.val; } }

        //public string pState
        //{
        //    get
        //    {
        //        return Cnst.getFromDictionary(Cnst.DSTATE, state);
        //    }
        //    set
        //    {
        //        state = Cnst.DSTATE.FirstOrDefault(c => (c.Value == value)).Key;
        //    }
        //}
        //public Field fState;
        //public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

        //public string pCity { get { return city; } set { city = value; } }
        //public Field fCity;
        //public Field City { get { fCity.val = pCity; return fCity; } set { fCity = value; pCity = fCity.val; } }
        
        //public string pName { get { return name; } set { name = value; } }
        //public Field fName;
        //public Field Name { get { fName.val = pName; return fName; } set { fName = value; pName = fName.val; } }
        
        //public string pMobile { get { return mobile; } set { mobile = value; } }
        //public Field fMobile;// = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", Gui.Place.LeftOne, Gui.Place.LeftTwo, Gui.Place.Nine);
        //public Field Mobile { get { fMobile.val = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.val; } }
        
        //public string pEmail { get { return email; } set { email = value; } }
        //public Field fEmail;
        //public Field Email { get { fEmail.val = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.val; } }
        
        //public string pContactName { get { return contact_name; } set { contact_name = value; } }
        //public Field fContactName;
        //public Field ContactName { get { fContactName.val = pContactName; return fContactName; } set { fContactName = value; pContactName = fContactName.val; } }
        
        //public string pContractType { get { return contract_type; } set { contract_type = value; } }
        //public Field fContractType;
        //public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

        public Service()
        {
            //Id = ID;
            NumberOfDairyFarms = new Field(NUMBER_OF_DAIRY_FARMS, null, typeof(RichTextBox), "# of dairy farms:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Three);
            NumberOfDairyCows = new Field(NUMBER_OF_DAIRY_COWS, null, typeof(RichTextBox), "# of dairy cows:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Four);
            Address = new Field(ADDRESS, null, typeof(RichTextBox), "Address:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Five);
            Country = new Field(Const.COUNTRY.FirstOrDefault(), Const.COUNTRY, typeof(ComboBox), "Country:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Six);
            State = new Field(Const.STATE.FirstOrDefault(), Const.STATE, typeof(ComboBox), "State:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Seven);
            City = new Field(CITY, null, typeof(RichTextBox), "City:", placeh: Gui.Place.Six, lplaceh: Gui.Place.Four, placev: Gui.Place.Eight);
            Name = new Field(NAME, null, typeof(RichTextBox), "Name:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Three);
            Mobile = new Field(MOBILE, null, typeof(RichTextBox), "Mobile:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Four);
            Email = new Field(EMAIL, null, typeof(RichTextBox), "Email Address:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Five);
            ContactName = new Field(CONTACT_NAME, null, typeof(RichTextBox), "Contact Name:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Six);
            ContractType = new Field(Const.CONTRACT_TYPE.FirstOrDefault(), Const.CONTRACT_TYPE, typeof(ComboBox), "Contract Type:", placeh: Gui.Place.Three, lplaceh: Gui.Place.One, placev: Gui.Place.Seven);
        }
    }

    class TreatmentPackage : TreatmentPackageJson
    {
        public int id { get; set; }
        public bool is_active { get; set; }
        public string part_number { get; set; }
        public int amount_of_treatments { get; set; }
        public string description { get; set; }
        public string contract_type { get; set; }
        public string added_date { get; set; }

        public string PartNumber { get { return part_number; } set { part_number = value; } }

        public override string ToString()
        {
            return PartNumber;
        }
    }

    class Field
    {
        public string val;
        public string deflt;
        public string[] items;
        public Control control;
        public Control lcontrol;
        public Type type;
        public string text;
        public Gui.Place placeh;
        public Gui.Place lplaceh;
        public Gui.Place placev;
        public Gui.Place lplacev;
        public Field depend;
        public EventHandler comboEventHandler;
        public EventHandler textEventHandler;
        public LinkLabelLinkClickedEventHandler linkEventHandler;
        private EventHandler rdioEventHandler;
        private EventHandler buttonEventHandler;

        public Field(string deflt, string[] items, Type type, string text,
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
            comboEventHandler = comboBox_SelectedIndexChanged;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox != null)
            { 
                check(depend); 
            }
            
        }

        public bool check(Field depend)
        {
            if(depend != null)
            {
                if (control.Text.Contains("United States of America"))
                    depend.control.Enabled = true;
                else
                    depend.control.Enabled = false;
                return depend.control.Enabled;
            }
            return false;
        }

        //public enum Place
        //{
        //    None,
        //    Center, Start, End,
        //    One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven
        //}

        public const int DefaultWidth = 390;
        public const int DefaultWidthLarge = 1200;
        public const int DefaultHeight = 60;
        public const int DefaultHeightSmall = 12;
        public const int DefaultHeightLarge = 90;
        public const float ScaleFactor = 0.5F;
        public const float PlaceOne = 200 * ScaleFactor;
        public const float DeltaV = 100 * ScaleFactor;
        //public const float DefaultFont = 18F * ScaleFactor;
        public const float DefaultFont = 24F * ScaleFactor;
        //public const float DefaultFontLarge = 24F * ScaleFactor;
        public const float DefaultFontLarge = 30F * ScaleFactor;
        public const string DefaultText = "DefaultText";

        //field.control = Gui.draw(this, field.type, text: field.val, name: defaultText, items: field.items, eventHandler: field.comboEventHandler, placeh: field.placeh, placev: field.placev);
        //field.lcontrol = Gui.draw(this, typeof(Label), text: field.text, autoSize: false, placeh: field.lplaceh, placev: field.placev);

        public void draw(Form thisForm)
        {
            draw(thisForm, type, text: val, placeh: placeh, placev: placev);
            draw(thisForm, typeof(Label), text: text, autoSize: false, placeh: lplaceh, placev: lplacev);
        }

        public void draw(Form thisForm, Type type)
        {
            draw(thisForm, type, text: val);
        }

        //public Control draw(Form thisForm, string name = DefaultText,
        //    float font = DefaultFont, Color color = new Color(),
        //    int width = DefaultWidth, int height = DefaultHeight, bool autoSize = true)
        public Control draw(Form thisForm, Type type, string text = null, string name = DefaultText,
            float font = DefaultFont, Color color = new Color(),
            int width = DefaultWidth, int height = DefaultHeight, bool autoSize = true,
            object[] items = null,
            Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        {
            Control control = type.GetConstructor(new Type[] { }).Invoke(null) as Control;
            thisForm.Controls.Add(control);
            control.Anchor = (AnchorStyles.Top);// | (AnchorStyles.Left);
            control.Margin = new Padding(4);
            control.Size = new Size(width, height);
            control.Scale(new SizeF(ScaleFactor, ScaleFactor));
            control.TabIndex = 1;
            control.Text = text;
            control.Font = new Font("Segoe UI", font, FontStyle.Regular, GraphicsUnit.Point);

            //if ((type == typeof(PictureBox)) || (type == typeof(LinkLabel)))
            //    control.AutoSize = true;
            if (type == typeof(Label))
            {
                if (color != Color.Empty)
                    control.ForeColor = color;
                control.AutoSize = autoSize;
            }
            if (type == typeof(PictureBox))
            {
                PictureBox pictureBox = (control as PictureBox);
                if (pictureBox != null)
                {
                    pictureBox.Image = Properties.Resources.ARmentaSmall;
                    control.AutoSize = autoSize;
                }
            }
            if (type == typeof(RichTextBox))
            {
                RichTextBox richTextBox = control as RichTextBox;
                if (richTextBox != null)
                {
                    richTextBox.Multiline = false;
                    if (textEventHandler != null)
                        richTextBox.TextChanged += textEventHandler;
                }
                defaultText(name, control);
            }
            if (type == typeof(Button))
                if (buttonEventHandler != null)
                    control.Click += buttonEventHandler;
            if (type == typeof(LinkLabel))
            {
                LinkLabel linkLabel = control as LinkLabel;
                if (linkLabel != null)
                {
                    control.AutoSize = autoSize;
                    if (linkEventHandler != null)
                        linkLabel.LinkClicked += linkEventHandler;
                }
            }
            if (type == typeof(ComboBox))
            {
                ComboBox comboBox = (control as ComboBox);
                if (comboBox != null)
                {
                    if (items != null)
                        comboBox.Items.AddRange(items);
                    if (comboEventHandler != null)
                        comboBox.SelectedIndexChanged += comboEventHandler;
                    comboBox.TextChanged += comboBox_TextChanged;
                    //comboBox.TextUpdate += comboBox_TextChanged;
                }
                defaultText(name, control);
            }
            if (type == typeof(RadioButton))
            {
                RadioButton radioButton = control as RadioButton;
                if (radioButton != null)
                    if (rdioEventHandler != null)
                        radioButton.CheckedChanged += rdioEventHandler;
            }

            control.Location = placeCalc(thisForm, control, placeh: placeh, placev: placev);

            return control;
        }

        private void defaultText(string name, Control control)
        {
            control.Name = name;
            if (control.Name == DefaultText)
            {
                control.ForeColor = Color.Silver;
                control.Enter += new EventHandler(controlEnter_Click);
                control.Leave += new EventHandler(controlLeave_Click);
            }
        }

        public static Point placeCalc(Form thisForm,
                Control control, Point location = new System.Drawing.Point(), Gui.Place placeh = Gui.Place.Center, Gui.Place placev = Gui.Place.Center)
        {
            if (placeh == Gui.Place.Center)
                location.X = thisForm.Width / 2 - control.Width / 2;
            else if (placeh == Gui.Place.Start)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width * 2 - control.Width * 2 / 4;
            else if (placeh == Gui.Place.End)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width + control.Width * 2 / 4;
            else if (placeh == Gui.Place.One)
                location.X = thisForm.Width / 2 + control.Width / 4 / 2;
            else if (placeh == Gui.Place.Two)
                location.X = thisForm.Width / 2 + control.Width / 2 + control.Width / 4;
            else if (placeh == Gui.Place.Three)
                location.X = thisForm.Width / 2 + control.Width + control.Width / 4 / 2 + control.Width / 4;
            else if (placeh == Gui.Place.Four)
                location.X = thisForm.Width / 2 - control.Width * 2 - control.Width / 4 / 2 - control.Width / 4;
            else if (placeh == Gui.Place.Five)
                location.X = thisForm.Width / 2 - control.Width / 2 - control.Width - control.Width / 4;
            else if (placeh == Gui.Place.Six)
                location.X = thisForm.Width / 2 - control.Width - control.Width / 4 / 2;

            if (placev == Gui.Place.One)
                location.Y = 30;
            else if (placev == Gui.Place.Two)
                location.Y = (int)(PlaceOne + 0 * DeltaV);//200;
            else if (placev == Gui.Place.Three)
                location.Y = (int)(PlaceOne + 1 * DeltaV);//300;
            else if (placev == Gui.Place.Four)
                location.Y = (int)(PlaceOne + 2 * DeltaV);//400;
            else if (placev == Gui.Place.Five)
                location.Y = (int)(PlaceOne + 3 * DeltaV);//500;
            else if (placev == Gui.Place.Six)
                location.Y = (int)(PlaceOne + 4 * DeltaV);//600;
            else if (placev == Gui.Place.Seven)
                location.Y = (int)(PlaceOne + 5 * DeltaV);//700;
            else if (placev == Gui.Place.Eight)
                location.Y = (int)(PlaceOne + 6 * DeltaV);//800;
            else if (placev == Gui.Place.Nine)
                location.Y = (int)(PlaceOne + 7 * DeltaV);//900;
            else if (placev == Gui.Place.Ten)
                location.Y = (int)(PlaceOne + 8 * DeltaV);//1000;
            else if (placev == Gui.Place.Eleven)
                location.Y = (int)(PlaceOne + 9 * DeltaV);//1100;
            else if (placev == Gui.Place.End)
                location.Y = (int)(PlaceOne + 10 * DeltaV);//1200;

            return location;
        }

        private void controlEnter_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Name == DefaultText)
            {
                string dflt = control.Text;
                control.Text = "";
                control.Name = dflt;
                control.ForeColor = Color.Black;
            }
        }

        private void controlLeave_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Text == string.Empty)
            {
                string dflt = control.Name;
                control.Name = DefaultText;
                control.Text = dflt;
                control.ForeColor = Color.Silver;
            }
        }

        private void comboBox_TextChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.Text != string.Empty)
                {
                    if (comboBox.Items != null)
                    {
                        if (comboBox.Items.Count > 0)
                            comboBox.SelectedItem = comboBox.Items.Cast<string>().Where(s => s.ToLower().StartsWith(comboBox.Text.ToLower())).FirstOrDefault();
                        if (comboBox.SelectedItem != null)
                            comboBox.Text = comboBox.SelectedItem.ToString();
                        else
                            comboBox.Text = string.Empty;
                    }
                }
            }
        }
    }
}
