using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
    interface LoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string tablet { get; set; }

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
    }

    class LoginResponseJson
    {
        public string token { get; set; }
        public UserJson user { get; set; }

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
    }

    class UserJson
    {
        public int pk { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
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

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
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

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
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

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
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

    interface ActionJson
    {
        //public int id { get; set; }
        //public string date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public string part_number { get; set; }
        //public int contact { get; set; }
        public string tablet { get; set; }
        public int? farm { get; set; }
        public int? service_provider { get; set; }
        //public int distributor { get; set; }

        //public override string ToString()
        //{
        //    return Const.entityToString(this);
        //}
    }

    class Gui
    {
        public Field Picture { get; set; }
        public Field Welcome { get; set; }

        public void drawFields(Form thisForm)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    if (field.view)
                    {
                        field.draw(thisForm, false);
                        field.draw(thisForm, true);
                    }
                }
            }
        }

        public void enableControls()
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    if (field.control != null)
                        field.control.Enabled = true;
                    if (field.lcontrol != null)
                        field.lcontrol.Enabled = true;
                }
            }
        }

        public void disableControls()
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    if (field.control != null)
                        field.control.Enabled = false;
                    if (field.lcontrol != null)
                        field.lcontrol.Enabled = false;
                }
            }
        }

        public void updateParams()
        {
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    field.updateField();
                    prop.SetValue(this, field);
                }
                //prop.SetValue(entity, control.Text);
            }
        }

        public ErrCode checkParams()
        {
            ErrCode errcode = ErrCode.OK;
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    //if (field.error == ErrCode.EPARAM)
                    //{
                    //    if (field.view)
                    //    {
                    //        if (field.control != null)
                    //        {
                    //            field.control.ForeColor = Color.Red;
                    //            field.control.Text = field.dtext;
                    //            field.dflag = true;
                    //        }
                    //        errCode = ErrCode.EPARAM;
                    //    }
                    //}
                    if (field.checkField() != ErrCode.OK)
                        if (!field.clear)
                            errcode = ErrCode.EPARAM;
                }
            }
            return errcode;
        }

        //public ErrCode responseParse(JsonDocument jsonDocument, List<string> errors)
        public void responseParse(JsonDocument jsonDocument, List<string> errors)
        {
            //ErrCode errcode = ErrCode.EPARAM;

            if (jsonDocument != null)
            {
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                    JsonElement jsonElement;
                    if (jsonDocument.RootElement.TryGetProperty(prop.Name, out jsonElement))
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                            errors.AddRange(jsonElement.EnumerateArray().Select(e => e.ToString()));
                }
                //errcode = ErrCode.OK;
            }
            //else
            //    errcode = ErrCode.EPARAM;

            //return errcode;
        }

        public void hide()
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(this) as Field;
                if (field != null)
                {
                    if (field.control != null)
                        field.control.Dispose();
                    if (field.lcontrol != null)
                        field.lcontrol.Dispose();
                }
                //prop.SetValue(entity, control.Text);
            }
        }

        public async Task<ErrCode> edit(Web web, string url)
        {
            ErrCode errcode = ErrCode.ERROR;
            List<string> errors = new List<string>();

            disableControls();

            updateParams();

            if ((errcode = checkParams()) == ErrCode.OK)
            {
                JsonDocument jsonDocument = await web.entityEdit(this, url);
                responseParse(jsonDocument, errors);
            }
            return errcode;
        }
    }

    class Login : Gui, LoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string tablet { get; set; }

        public object pEmail { get { return email; } set { email = value as string; } }
        public Field fEmail;
        //public Field Email { get { fEmail.text = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.text; } }
        public Field Email { get { return Field.getField(fEmail, pEmail); } set { pEmail = Field.setField(ref fEmail, value, pEmail); } }

        public object pPassword { get { return password; } set { password = value as string; } }
        public Field fPassword;
        //public Field Password { get { fPassword.text = pPassword; return fPassword; } set { fPassword = value; pPassword = fPassword.text; } }
        public Field Password { get { return Field.getField(fPassword, pPassword); } set { pPassword = Field.setField(ref fPassword, value, pPassword); } }

        public Field Forgot { get; set; }

        public Field Press { get; set; }

        public Login(LinkLabelLinkClickedEventHandler linkEventHandler, EventHandler buttonEventHandler)
        {
            Email = new Field(type: typeof(RichTextBox), text: "Username", width: Field.DefaultWidthLarge, placev: Place.Three);
            Password = new Field(type: typeof(TextBox), text: "Password", width: Field.DefaultWidthLarge, placev: Place.Five);
            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Forgot = new Field(ltype: typeof(LinkLabel), ltext: "Forgot password", linkEventHandler: linkEventHandler, lplacev: Place.Seven);
            Press = new Field(ltype: typeof(Button), ltext: "Login", buttonEventHandler: buttonEventHandler, lplacev: Place.End);
        }
    }

    class Entity : Gui
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

        public object Id
        {
            get
            {
                return Field.intToString(id as int?);
            }
            set
            {
                id = Field.stringToInt(value as string);
            }
        }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Submit { get; set; }

        public object pContractType { get { return contract_type; } set { contract_type = value as string; } }
        public Field fContractType;
        public Field ContractType { get { return Field.getField(fContractType, pContractType); } set { pContractType = Field.setField(ref fContractType, value, pContractType); } }

        public object pName { get { return name; } set { name = value as string; } }
        public Field fName;
        public Field Name { get { return Field.getField(fName, pName); } set { pName = Field.setField(ref fName, value, pName); } }
        //public Field Name { get { fName.text = pName; return fName; } set { if (value.text != string.Empty) { value.error = ErrCode.OK; fName = value; pName = fName.text; } else { value.error = ErrCode.EPARAM; } } }

        //public bool setField(ref Field field, Field value)
        //{
        //    if (value.fcheck(value))
        //    {
        //        field = value;
        //        //field.error = ErrCode.OK;
        //        return true;
        //    }
        //    else
        //    {
        //        field = value;
        //        //field.error = ErrCode.EPARAM;
        //        return false;
        //    }
        //}

        //public Field getField(Field field, string param)
        //{
        //    if (field.pcheck(param))
        //        field.text = param;
        //    //field.error = ErrCode.OK;
        //    return field;
        //}

        public object pAddress { get { return address; } set { address = value as string; } }
        public Field fAddress;
        //public Field Address { get { fAddress.text = pAddress; return fAddress; } set { fAddress = value; pAddress = fAddress.text; } }
        public Field Address { get { return Field.getField(fAddress, pAddress); } set { pAddress = Field.setField(ref fAddress, value, pAddress) ; } }

        public object pCountry
        {
            get
            {
                return Const.getFromDictionary(Const.DCOUNTRY, country);
            }
            set
            {
                country = Const.DCOUNTRY.FirstOrDefault(c => (c.Value == value.ToString())).Key;
            }
        }
        public Field fCountry;
        //public Field Country { get { fCountry.text = pCountry; return fCountry; } set { fCountry = value; pCountry = fCountry.text; } }
        public Field Country { get { return Field.getField(fCountry, pCountry); } set { pCountry = Field.setField(ref fCountry, value, pCountry) ; } }

        public object pState
        {
            get
            {
                return Const.getFromDictionary(Const.DSTATE, state);
            }
            set
            {
                state = Const.DSTATE.FirstOrDefault(c => (c.Value == value.ToString())).Key;
            }
        }
        public Field fState;
        public Field State { get { fState.val = pState; return fState; } set { fState = value; pState = fState.val; } }

        public object pCity { get { return city; } set { city = value as string; } }
        public Field fCity;
        //public Field City { get { fCity.text = pCity; return fCity; } set { fCity = value; pCity = fCity.text; } }
        public Field City { get { return Field.getField(fCity, pCity); } set { pCity = Field.setField(ref fCity, value, pCity); } }

        public object pContactName { get { return contact_name; } set { contact_name = value as string; } }
        public Field fContactName;
        //public Field ContactName { get { fContactName.text = pContactName; return fContactName; } set { fContactName = value; pContactName = fContactName.text; } }
        public Field ContactName { get { return Field.getField(fContactName, pContactName); } set { pContactName = Field.setField(ref fContactName, value, pContactName); } }

        public object pMobile { get { return mobile; } set { mobile = value as string; } }
        public Field fMobile;
        //public Field Mobile { get { fMobile.text = pMobile; return fMobile; } set { fMobile = value; pMobile = fMobile.text; } }
        public Field Mobile { get { return Field.getField(fMobile, pMobile); } set { pMobile = Field.setField(ref fMobile, value, pMobile); } }

        public object pEmail { get { return email; } set { email = value as string; } }
        public Field fEmail;
        //public Field Email { get { fEmail.text = pEmail; return fEmail; } set { fEmail = value; pEmail = fEmail.text; } }
        public Field Email { get { return Field.getField(fEmail, pEmail); } set { pEmail = Field.setField(ref fEmail, value, pEmail); } }

        //public string pContractType { get { return contract_type; } set { contract_type = value; } }
        //public Field fContractType;
        //public Field ContractType { get { return Field.getField(fContractType, pContractType); } set { if (Field.setField(ref fContractType, value)) pContractType = fContractType.text; } }

        public object clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return Name.val.ToString();
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

        public object IsActive
        {
            get
            {
                return Field.boolToString(is_active);
            }
            set
            {
                is_active = Field.stringToBool(value as string);
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

        public object pFarmType { get { return farm_type; } set { farm_type = value as string; } }
        public Field fFarmType;
        //public Field FarmType { get { fFarmType.text = pFarmType; return fFarmType; } set { fFarmType = value; pFarmType = fFarmType.text; } }
        public Field FarmType { get { return Field.getField(fFarmType, pFarmType); } set { pFarmType = Field.setField(ref fFarmType, value, pFarmType); } }

        public object pBreedType { get { return breed_type; } set { breed_type = value as string; } }
        public Field fBreedType;
        //public Field BreedType { get { fBreedType.text = pBreedType; return fBreedType; } set { fBreedType = value; pBreedType = fBreedType.text; } }
        public Field BreedType { get { return Field.getField(fBreedType, pBreedType); } set { pBreedType = Field.setField(ref fBreedType, value, pBreedType); } }

        private object pNumberOfLactatingCows
        {
            get
            {
                return Field.intToString(number_of_lactating_cows as int?);
            }
            set
            {
                number_of_lactating_cows = Field.stringToInt(value as string);
            }
        }
        private Field fNumberOfLactatingCows;
        //public Field Name { get { return getField(fName, pName); } set { if (setField(value, ref fName)) pName = value.text; } }
        //public Field NumberOfLactatingCows { get { return getField(fNumberOfLactatingCows, pNumberOfLactatingCows); } set { if (setField(value, ref fNumberOfLactatingCows)) pNumberOfLactatingCows = value.text; } }
        //public Field NumberOfLactatingCows { get { fNumberOfLactatingCows.text = pNumberOfLactatingCows; return fNumberOfLactatingCows; } set { if (value.text != string.Empty) { value.error = ErrCode.OK; fNumberOfLactatingCows = value; pNumberOfLactatingCows = fNumberOfLactatingCows.text; } else { value.error = ErrCode.EPARAM; } } }
        public Field NumberOfLactatingCows { get { return Field.getField(fNumberOfLactatingCows, pNumberOfLactatingCows); } set { pNumberOfLactatingCows = Field.setField(ref fNumberOfLactatingCows, value, pNumberOfLactatingCows); } }

        public object pDhiTest
        {
            get
            {
                return Field.boolToString(dhi_test);
            }
            set
            {
                dhi_test = Field.stringToBool(value as string);
            }
        }
        public Field fDhiTest;
        //public Field DhiTest { get { fDhiTest.text = pDhiTest; return fDhiTest; } set { fDhiTest = value; pDhiTest = fDhiTest.text; } }
        public Field DhiTest { get { return Field.getField(fDhiTest, pDhiTest); } set { pDhiTest = Field.setField(ref fDhiTest, value, pDhiTest); } }

        public object pMilkingSetupType { get { return milking_setup_type; } set { milking_setup_type = value as string; } }
        public Field fMilkingSetupType;
        //public Field MilkingSetupType { get { fMilkingSetupType.text = pMilkingSetupType; return fMilkingSetupType; } set { fMilkingSetupType = value; pMilkingSetupType = fMilkingSetupType.text; } }
        public Field MilkingSetupType { get { return Field.getField(fMilkingSetupType, pMilkingSetupType); } set { pMilkingSetupType = Field.setField(ref fMilkingSetupType, value, pMilkingSetupType); } }

        public object pLocationOfTreatmentType { get { return location_of_treatment_type; } set { location_of_treatment_type = value as string; } }
        public Field fLocationOfTreatmentType;

        //public Field LocationOfTreatmentType { get { fLocationOfTreatmentType.text = pLocationOfTreatmentType; return fLocationOfTreatmentType; } set { fLocationOfTreatmentType = value; pLocationOfTreatmentType = fLocationOfTreatmentType.text; } }
        public Field LocationOfTreatmentType { get { return Field.getField(fLocationOfTreatmentType, pLocationOfTreatmentType); } set { pLocationOfTreatmentType = Field.setField(ref fLocationOfTreatmentType, value, pLocationOfTreatmentType); } }

        //public string pContractType { get { return contract_type; } set { contract_type = value; } }
        //public Field fContractType;
        //public Field ContractType { get { fContractType.val = pContractType; return fContractType; } set { fContractType = value; pContractType = fContractType.val; } }

        public Farm()
        {
            initFields();
        }

        private void initFields()
        {
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: NAME, ltext: "Name:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Three);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: ADDRESS, ltext: "Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Four);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "Country", ltext: "Country:", autosize: false, items: Const.COUNTRY, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Five);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "State", ltext: "State:", autosize: false, items: Const.STATE, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Six);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: CITY, ltext: "City:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Seven);
            IsActive = Const.IS_ACTIVE;
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: CONTACT_NAME, ltext: "Contact Name:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Eight);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: MOBILE, ltext: "Mobile:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Nine);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: EMAIL, ltext: "Email Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Ten);
            FarmType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.FARM_TYPE.FirstOrDefault(), ltext: "Farm Type:", autosize: false, items: Const.FARM_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Three);
            BreedType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.BREED_TYPE.FirstOrDefault(), ltext: "Breed Type:", autosize: false, items: Const.BREED_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Four);
            NumberOfLactatingCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: NUMBER_OF_LACTATING_COWS, ltext: "# of Lactating Cows:", pcheck: Field.icheckValid, autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Five);
            DhiTest = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: DHI_TEST.FirstOrDefault(), ltext: "Milk Recording:", autosize: false, items: DHI_TEST, placeh: Place.Three, lplaceh: Place.One, placev: Place.Six);
            MilkingSetupType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.MILKING_SETUP_TYPE.FirstOrDefault(), ltext: "Milking Setup:", autosize: false, items: Const.MILKING_SETUP_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Seven);
            LocationOfTreatmentType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.LOCATION_OF_TREATMENT_TYPE.FirstOrDefault(), ltext: "Treatment Location:", autosize: false, items: Const.LOCATION_OF_TREATMENT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Eight);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.CONTRACT_TYPE.FirstOrDefault(), ltext: "Contract Type:", autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Nine);

            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Caption = new Field(ltype: typeof(Label), ltext: "Add Farm", font: Field.DefaultFontLarge, lplacev: Place.Two);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Six, lplacev: Place.Eleven);
            Submit = new Field(ltype: typeof(Button), ltext: "Submit", lplaceh: Place.Three, lplacev: Place.Eleven);
        }

        public Farm(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler)
        {
            initFields();
            initFields(edit, countryHandler, cancelHandler, submitHandler);
        }

        public void initFields(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler)
        {
            if (edit)
            {
                Caption.ltext = "Edit Farm";
                ContractType.view = false;
            }
            else
            {
                Caption.ltext = "Add Farm";
                ContractType.view = true;
            }
            if (Country.val.ToString() != "United States of America")
                State.enable = false;
            Cancel.buttonEventHandler = cancelHandler;
            Submit.buttonEventHandler = submitHandler;
            Country.comboEventHandler = countryHandler;
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

        public object pNumberOfDairyFarms
        {
            get
            {
                return Field.intToString(number_of_dairy_farms);
            }
            set
            {
                number_of_dairy_farms = Field.stringToInt(value as string);
            }
        }
        public Field fNumberOfDairyFarms;
        //public Field NumberOfDairyFarms { get { fNumberOfDairyFarms.text = pNumberOfDairyFarms; return fNumberOfDairyFarms; } set { fNumberOfDairyFarms = value; pNumberOfDairyFarms = fNumberOfDairyFarms.text; } }
        public Field NumberOfDairyFarms { get { return Field.getField(fNumberOfDairyFarms, pNumberOfDairyFarms); } set { pNumberOfDairyFarms = Field.setField(ref fNumberOfDairyFarms, value, pNumberOfDairyFarms); } }

        public object pNumberOfDairyCows
        {
            get
            {
                return Field.intToString(number_of_dairy_cows);
            }
            set
            {
                number_of_dairy_cows = Field.stringToInt(value as string);
            }
        }
        public Field fNumberOfDairyCows;
        //public Field NumberOfDairyCows { get { fNumberOfDairyCows.text = pNumberOfDairyCows; return fNumberOfDairyCows; } set { fNumberOfDairyCows = value; pNumberOfDairyCows = fNumberOfDairyCows.text; } }
        public Field NumberOfDairyCows { get { return Field.getField(fNumberOfDairyCows, pNumberOfDairyCows); } set { pNumberOfDairyCows = Field.setField(ref fNumberOfDairyCows, value, pNumberOfDairyCows); } }

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
            initFields();
        }

        private void initFields()
        {
            NumberOfDairyFarms = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: NUMBER_OF_DAIRY_FARMS, ltext: "# of dairy farms:", pcheck: Field.icheckValid, autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Three);
            NumberOfDairyCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: NUMBER_OF_DAIRY_COWS, ltext: "# of dairy cows:", pcheck: Field.icheckValid, autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Four);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: ADDRESS, ltext: "Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Five);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "Country", ltext: "Country:", autosize: false, items: Const.COUNTRY, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Six);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "State", ltext: "State:", autosize: false, items: Const.STATE, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Seven);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: CITY, ltext: "City:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Eight);
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: NAME, ltext: "Name:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Three);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: MOBILE, ltext: "Mobile:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Four);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: EMAIL, ltext: "Email Address:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Five);
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), text: CONTACT_NAME, ltext: "Contact Name:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Six);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: Const.CONTRACT_TYPE.FirstOrDefault(), ltext: "Contract Type:", autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Seven);
            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);

            Caption = new Field(ltype: typeof(Label), ltext: "Add Service", font: Field.DefaultFontLarge, lplacev: Place.Two);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Six, lplacev: Place.Eleven);
            Submit = new Field(ltype: typeof(Button), ltext: "Submit", lplaceh: Place.Three, lplacev: Place.Eleven);
        }

        public Service(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler)
        {
            initFields();
            initFields(edit, countryHandler, cancelHandler, submitHandler);
        }

        public void initFields(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler)
        {
            if (edit)
            {
                Caption.ltext = "Edit Service";
                ContractType.view = false;
            }
            else
            {
                Caption.ltext = "Add Service";
                ContractType.view = true;
            }
            Cancel.buttonEventHandler = cancelHandler;
            Submit.buttonEventHandler = submitHandler;
            Country.comboEventHandler = countryHandler;
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
        //public string pPartNumber { get { return part_number; } set { part_number = value; } }
        //public Field fPartNumber;
        //public Field PartNumber { get { return Field.getField(fPartNumber, pPartNumber); } set { if (Field.setField(ref fPartNumber, value)) pPartNumber = fPartNumber.text; } }

        //public TreatmentPackage()
        //{
        //    PartNumber = new Field(type: typeof(ComboBox), ltype: typeof(Label), ltext: "Add treatments to AM – SN", placev: Gui.Place.Eight, lplacev: Gui.Place.Seven);
        //    //+ am.SNum
        //}

        public override string ToString()
        {
            //return PartNumber.text;
            return description;
        }
    }

    class Action : Gui, ActionJson
    {
        public int id { get; set; }
        public string added_date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public string part_number { get; set; }
        public int contact { get; set; }
        public string tablet { get; set; }
        public int? farm { get; set; }
        public int? service_provider { get; set; }

        private object ppFarm
        {
            get
            {
                return Field.intToString(farm);
            }
            set
            {
                farm = Field.stringToIntOrNull(value as string);
            }
        }
        private object ffFarm;
        private object pFarm { get { return Field.getObject(fFarm, ffFarm, ppFarm); } set { ppFarm = Field.setObject(fFarm, ref ffFarm, value, ppFarm); } }
        private Field fFarm;
        public Field Farm { get { return Field.getField(fFarm, pFarm); } set { pFarm = Field.setField(ref fFarm, value, pFarm); } }

        private object ppService
        {
            get
            {
                return Field.intToString(service_provider);
            }
            set
            {
                service_provider = Field.stringToIntOrNull(value as string);
            }
        }
        private object ffService;
        private object pService { get { return Field.getObject(fService, ffService, ppService); } set { ppService = Field.setObject(fService, ref ffService, value, ppService); } }
        private Field fService;
        public Field Service { get { return Field.getField(fService, pService); } set { pService = Field.setField(ref fService, value, pService); } }

        public Field RadioFarm { get; set; }
        public Field RadioService { get; set; }

        public object ppPartNumber { get { return part_number; } set { part_number = value as string; } }
        private object ffPartNumber;
        private object pPartNumber { get { return Field.getObject(fPartNumber, ffPartNumber, ppPartNumber); } set { ppPartNumber = Field.setObject(fPartNumber, ref ffPartNumber, value, ppPartNumber); } }
        public Field fPartNumber;
        public Field PartNumber { get { return Field.getField(fPartNumber, pPartNumber); } set { pPartNumber = Field.setField(ref fPartNumber, value, pPartNumber); } }

        public Field Progress { get; set; }
        public Field Cancel { get; set; }
        public Field Approve { get; set; }

        public Action()
        {
            initFields();
        }

        public Action(AM am, string tablet, object[] farms, object[] services, EventHandler comboEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler)
        {
            initFields();
            initFields(am, tablet, farms, services, comboEventHandler, radioEventHandler, canselEventHandler, approveEventHandler);
        }

        private void initFields()
        {
            PartNumber = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "Part Number", ltext: "Add treatments to AM – SN ", placev: Place.Eight, lplacev: Place.Seven);
            Farm = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "Farm / Service provider", ltext: "Select Farm / Service provider", placev: Place.Five, lplacev: Place.Four);
            Service = new Field(type: typeof(ComboBox), ltype: typeof(Label), text: "Farm / Service provider", ltext: "Select Farm / Service provider", placev: Place.Five, lplacev: Place.Four);

            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Welcome = new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two);
            RadioFarm = new Field(ltype: typeof(RadioButton), ltext: "Farm", lplaceh: Place.Two, lplacev: Place.Five);
            RadioService = new Field(ltype: typeof(RadioButton), ltext: "Service provider", lplaceh: Place.Two, lplacev: Place.Six);
            Progress = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Ten);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Five, lplacev: Place.End);
            Approve = new Field(ltype: typeof(Button), ltext: "Approve", lplaceh: Place.Two, lplacev: Place.End);
        }

        private void initFields(AM am, string tablet, object[] farms, object[] services, EventHandler comboEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler)
        {
            PartNumber.ltext += am.SNum.ToString();
            Farm.items = farms;
            Farm.comboEventHandler = comboEventHandler;
            Service.items = services;
            Service.comboEventHandler = comboEventHandler;

            aptx_id = string.Format("{0:x} {1:x} {2:x}", am.AptxId[0], am.AptxId[1], am.AptxId[2]);
            am_id = am.SNum.ToString();
            this.tablet = tablet;

            RadioFarm.radioEventHandler = radioEventHandler;
            RadioService.radioEventHandler = radioEventHandler;
            Cancel.buttonEventHandler = canselEventHandler;
            Approve.buttonEventHandler = approveEventHandler;
        }
    }
}
