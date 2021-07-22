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
        }
    }

    //private string entityToString()
    //{
    //    string str = string.Empty;
    //    foreach (PropertyInfo prop in this.GetType().GetProperties())
    //        str += string.Format("{0}: {1}\n", prop.Name, prop.GetValue(this));
    //    return str;
    //}
}
