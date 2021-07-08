using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
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

        //public int id { get; set; }
        //public string mobile { get; set; }
        //public string address { get; set; }
        //public string country { get; set; }
        //public string city { get; set; }
        //public string state { get; set; }
        //public string email { get; set; }
        //public int number_of_dairy_farms { get; set; }
        //public int number_of_dairy_cows { get; set; }
        //public string name { get; set; }
        //public string contract_type { get; set; }
        //public int contact { get; set; }
        //public int distributor { get; set; }

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
        public Field NumberOfDairyFarms{ get { fNumberOfDairyFarms.val = pNumberOfDairyFarms; return fNumberOfDairyFarms; } set { fNumberOfDairyFarms = value; pNumberOfDairyFarms = fNumberOfDairyFarms.val; } }

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
}
