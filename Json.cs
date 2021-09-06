using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
    }

    interface LoginResponseJson
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
        public bool is_password_changed { get; set; }
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

    interface PasswordJson
    {
        public string new_password1 { get; set; }
        public string new_password2 { get; set; }
    }

    interface ResetJson
    {
        public string email { get; set; }
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
    }

    class Gui
    {
        public delegate Task<JsonDocument> dWeb<TJson>(TJson jentity, string entityUrl);
        public delegate Task<ErrCode> dResponseOk<T>(T entity);
        public delegate ErrCode dCheck();
        public delegate Task<ErrCode> dApprove();
        public delegate Task<List<string>> dResponseErr(ErrCode errcode);

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
                if ((field != null) && field.enable)
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

        public ErrCode checkFields()
        {
            ErrCode errcode = ErrCode.OK;
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(this) as Field;
                if ((field != null) && field.view)
                {
                    if (field.checkField() != ErrCode.OK)
                        errcode = ErrCode.EPARAM;
                }
            }
            return errcode;
        }

        public ErrCode responseParse(JsonDocument jsonDocument, List<string> errors, List<string> messages)
        {
            ErrCode errcode = ErrCode.ERROR;

            if (jsonDocument != null)
            {
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                    JsonElement jsonElement;
                    if (jsonDocument.RootElement.TryGetProperty(prop.Name, out jsonElement))
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                            errors.AddRange(jsonElement.EnumerateArray().Select(e => e.ToString()));
                        else if (jsonElement.ValueKind == JsonValueKind.String)
                            messages.Add(jsonElement.GetString());

                }
            }

            if (errors.Count == 0)
                errcode = ErrCode.OK;
            else
                errcode = ErrCode.EPARAM;

            return errcode;
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

        public async Task<ErrCode> send<TJson, T>(TJson jentity, string url, dWeb<TJson> dweb, string captionOk, string captionErr,
            bool showMsgs, dResponseOk<T> dresponseOk, dCheck dcheck = null, dApprove dapprove = null, dResponseErr dresponseErr = null, List<string> messagesOk = null, List<string> messagesErr = null)
        {
            ErrCode errcode = ErrCode.ERROR;

            if ((jentity != null) && (url != null) && (dweb != null) && (captionOk != null) && (captionErr != null) && (dresponseOk != null))
            {
                JsonDocument jsonDocument = null;
                T rentity = default;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                disableControls();
                //updateParams();

                if (dcheck != null)
                    errcode = dcheck();
                else
                    errcode = ErrCode.OK;

                if(errcode == ErrCode.OK)
                {
                    if ((errcode = checkFields()) == ErrCode.OK)
                    {
                        if (dapprove != null)
                            errcode = await dapprove();
                        else
                            errcode = ErrCode.OK;

                        if (errcode == ErrCode.OK)
                        {
                            jsonDocument = await dweb(jentity, url);
                            if (jsonDocument != null)
                            {
                                responseParse(jsonDocument, errors, messages);

                                try { rentity = JsonSerializer.Deserialize<T>(jsonDocument.RootElement.ToString()); }
                                catch (Exception ex) { LogFile.logWrite(ex.ToString()); }

                                if (rentity != null)
                                    errcode = ErrCode.OK;
                                else
                                    errcode = ErrCode.SERROR;
                            }
                            else
                                errcode = ErrCode.SERROR;
                        }
                    }
                }
                if (errcode == ErrCode.OK)
                {
                    if (showMsgs && (messages.Count() > 0))
                        notify(messages, NotifyButtons.OK, captionOk);

                    if(messagesOk != null)
                        notify(messagesOk, NotifyButtons.OK, captionOk);

                    if ((errcode = await dresponseOk(rentity)) == ErrCode.OK)
                        hide();
                }
                if (errcode != ErrCode.OK)
                {
                    if (errors.Count() > 0)
                        notify(errors, NotifyButtons.OK, captionErr);

                    if(messagesErr != null)
                        notify(messagesErr, NotifyButtons.OK, captionErr);

                    if (dresponseErr != null)
                        notify(await dresponseErr(errcode), NotifyButtons.OK, captionErr);

                    enableControls();
                }
            }
            return errcode;
        }

        public DialogResult notify(List<string> text, NotifyButtons notifyButtons, string caption)
        {
            DialogResult dialogResult = default;
            if ((text != null) && (text.Count > 0))
            {
                FormNotify formNotify = new FormNotify(text, notifyButtons, caption);
                formNotify.ShowDialog();
                dialogResult = formNotify.DialogResult;
                formNotify.Dispose();
            }
            return dialogResult;
        }
    }

    class Login : Gui, LoginJson, LoginResponseJson
    {
        public string email { get { return Email.getValue(); } set { Email.setValue(value); } }
        public string password { get { return Password.getValue(); } set { Password.setValue(value); } }
        public string tablet { get; set; }

        public string token { get; set; }
        public UserJson user { get; set; }

        public Field Email { get; set; }

        public Field Password { get; set; }

        public Field Forgot { get; set; }

        public Field Press { get; set; }

        public Login()
        {
            initFields();
        }

        public Login(EventHandler forgotEventHandler, EventHandler buttonEventHandler)
        {
            initFields();
            initFields(forgotEventHandler, buttonEventHandler);
        }

        private void initFields()
        {
            Email = new Field(type: typeof(RichTextBox), dflt: "Username", width: Field.DefaultWidthLarge, placev: Place.Three);
            Password = new Field(type: typeof(TextBox), dflt: "Password", width: Field.DefaultWidthLarge, placev: Place.Five);
            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Forgot = new Field(ltype: typeof(LinkLabel), ltext: "Forgot password", lplacev: Place.Seven);
            Press = new Field(ltype: typeof(Button), ltext: "Login", lplacev: Place.End);
        }

        private void initFields(EventHandler forgotEventHandler, EventHandler buttonEventHandler)
        {
            Forgot.eventHandler = forgotEventHandler;
            Press.eventHandler = buttonEventHandler;
        }
    }

    class Password : Gui, PasswordJson
    {
        public string new_password1 { get { return Password1.getValue(); } set { Password1.setValue(value); } }
        public string new_password2 { get { return Password2.getValue(); } set { Password2.setValue(value); } }
        public string detail { get; set; }

        public Field Password1 { get; set; }

        public Field Password2 { get; set; }

        public Field ChangePassword { get; set; }

        public Password()
        {
            initFields();
        }

        public Password(EventHandler buttonEventHandler)
        {
            initFields();
            initFields(buttonEventHandler);
        }

        private void initFields()
        {
            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Password1 = new Field(type: typeof(TextBox), ltype: typeof(Label), dflt: "Password", ltext: "Please Enter a new password. \nPassword should be complex and at least 8 chars long.", 
                width: Field.DefaultWidthLarge, placev: Place.Five, lplacev: Place.Three);
            Password2 = new Field(type: typeof(TextBox), dflt: "Confirm Password", width: Field.DefaultWidthLarge, placev: Place.Six);
            ChangePassword = new Field(ltype: typeof(Button), ltext: "Change Password", width: Field.DefaultWidthLarge, lplacev: Place.End);
        }

        private void initFields(EventHandler buttonEventHandler)
        {
            ChangePassword.eventHandler = buttonEventHandler;
        }
    }

    class Reset : Gui, ResetJson
    {
        public string email { get { return Email.getValue(); } set { Email.setValue(value); } }
        public string detail { get; set; }

        public Field Email { get; set; }

        public Field ResetPassword { get; set; }

        public Reset()
        {
            initFields();
        }

        public Reset(EventHandler buttonEventHandler)
        {
            initFields();
            initFields(buttonEventHandler);
        }

        private void initFields()
        {
            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email@email.com", ltext: "The password will be reseted, please enter an email to send a temporary password:", width: Field.DefaultWidthLarge, placev: Place.Six, lplacev: Place.Four);
            ResetPassword = new Field(ltype: typeof(Button), ltext: "Reset Password", width: Field.DefaultWidthLarge, lplacev: Place.End);
        }

        private void initFields(EventHandler buttonEventHandler)
        {
            ResetPassword.eventHandler = buttonEventHandler;
        }
    }

    class Entity : Gui
    {

        public int id { get; set; }
        public string mobile { get { return Mobile.getValue(); } set { Mobile.setValue(value); } }
        public string address { get { return Address.getValue(); } set { Address.setValue(value); } }
        public string country { get { return Const.DCOUNTRY.FirstOrDefault(c => c.Value == Country.getValue()).Key; } 
            set { Country.setValue(Const.getFromDictionary(Const.DCOUNTRY, value)); } }
        public string city { get { return City.getValue(); } set { City.setValue(value); } }
        public string state { get { return Const.DSTATE.FirstOrDefault(c => c.Value == State.getValue()).Key; }
            set { State.setValue(Const.getFromDictionary(Const.DSTATE, value)); } }
        public string email { get { return Email.getValue(); } set { Email.setValue(value); } }
        public string name { get { return Name.getValue(); } set { Name.setValue(value); } }
        public string contact_name { get { return ContactName.getValue(); } set { ContactName.setValue(value); } }
        public string contract_type { get { return ContractType.getValue(); } set { ContractType.setValue(value); } }

        public string Id { get { return Field.intToString(id); } set { id = Field.stringToInt(value); } }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Submit { get; set; }

        public Field ContractType { get; set; }

        //public object pName { get { return name; } set { name = value as string; } }
        //public Field fName;
        //public Field Name { get { return Field.getField(fName, pName); } set { pName = Field.setField(ref fName, value, pName); } }
        public Field Name { get; set; }

        public Field Address { get; set; }

        //public object pCountry
        //{
        //    get
        //    {
        //        return Const.getFromDictionary(Const.DCOUNTRY, country);
        //    }
        //    set
        //    {
        //        country = Const.DCOUNTRY.FirstOrDefault(c => c.Value == (value as string)).Key;
        //    }
        //}
        //public Field fCountry;
        //public Field Country { get { return Field.getField(fCountry, pCountry); } set { pCountry = Field.setField(ref fCountry, value, pCountry) ; } }
        public Field Country { get; set; }

        public Field State { get; set; }

        public Field City { get; set; }

        public Field ContactName { get; set; }

        public Field Mobile { get; set; }

        public Field Email { get; set; }

        public object clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return Name.val;
        }
    }

    class Farm : Entity, FarmJson
    {
        public static string[] DHI_TEST = new string[] { "No", "Yes" };

        public bool is_active { get; set; }
        public string farm_type { get { return FarmType.getValue(); } set { FarmType.setValue(value); } }
        public string breed_type { get { return BreedType.getValue(); } set { BreedType.setValue(value); } }
        public string milking_setup_type { get { return MilkingSetupType.getValue(); } set { MilkingSetupType.setValue(value); } }
        public string location_of_treatment_type { get { return LocationOfTreatmentType.getValue(); } set { LocationOfTreatmentType.setValue(value); } }
        public int number_of_lactating_cows  { get { return Field.stringToInt(NumberOfLactatingCows.getValue()); } set { NumberOfLactatingCows.setValue(Field.intToString(value)); } }
        public bool dhi_test { get { return Field.stringToBool(DhiTest.getValue()); } set { DhiTest.setValue(Field.boolToString(value)); } }

        public object IsActive { get { return Field.boolToString(is_active); } set { is_active = Field.stringToBool(value as string); } }

        public Field FarmType { get; set; }

        //public object pBreedType { get { return breed_type; } set { breed_type = value as string; } }
        //public Field fBreedType;
        //public Field BreedType { get { return Field.getField(fBreedType, pBreedType); } set { pBreedType = Field.setField(ref fBreedType, value, pBreedType); } }
        public Field BreedType { get; set; }

        //private object pNumberOfLactatingCows
        //{
        //    get
        //    {
        //        return Field.intToString(number_of_lactating_cows as int?);
        //    }
        //    set
        //    {
        //        number_of_lactating_cows = Field.stringToInt(value as string);
        //    }
        //}
        //private Field fNumberOfLactatingCows;
        //public Field NumberOfLactatingCows { get { return Field.getField(fNumberOfLactatingCows, pNumberOfLactatingCows); } set { pNumberOfLactatingCows = Field.setField(ref fNumberOfLactatingCows, value, pNumberOfLactatingCows); } }
        public Field NumberOfLactatingCows { get; set; }

        //public object pDhiTest
        //{
        //    get
        //    {
        //        return Field.boolToString(dhi_test);
        //    }
        //    set
        //    {
        //        dhi_test = Field.stringToBool(value as string);
        //    }
        //}
        //public Field fDhiTest;
        //public Field DhiTest { get { return Field.getField(fDhiTest, pDhiTest); } set { pDhiTest = Field.setField(ref fDhiTest, value, pDhiTest); } }
        public Field DhiTest { get; set; }

        public Field MilkingSetupType { get; set; }

        public Field LocationOfTreatmentType { get; set; }

        public Farm()
        {
            initFields();
        }

        private void initFields()
        {
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Name", ltext: "Name:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Three);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Address", ltext: "Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Four);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Country", ltext: "Country:", autosize: false, items: Const.COUNTRY, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Five);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "State", ltext: "State:", autosize: false, items: Const.STATE, enable: false, check: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Six);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "City", ltext: "City:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Seven);
            IsActive = Const.IS_ACTIVE;
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Contact Name", ltext: "Contact Name:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Eight);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Mobile", ltext: "Mobile:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Nine);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email Address", ltext: "Email Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Ten);
            FarmType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm Type", ltext: "Farm Type:", autosize: false, items: Const.FARM_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Three);
            BreedType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Breed Type", ltext: "Breed Type:", autosize: false, items: Const.BREED_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Four);
            NumberOfLactatingCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of Lactating Cows", ltext: "# of Lactating Cows:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Five);
            DhiTest = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Milk Recording", ltext: "Milk Recording:", autosize: false, items: DHI_TEST, placeh: Place.Three, lplaceh: Place.One, placev: Place.Six);
            MilkingSetupType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Milking Setup", ltext: "Milking Setup:", autosize: false, items: Const.MILKING_SETUP_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Seven);
            LocationOfTreatmentType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Treatment Location", ltext: "Treatment Location:", autosize: false, items: Const.LOCATION_OF_TREATMENT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Eight);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Contract Type", ltext: "Contract Type:", autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Nine);

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
            if (Country.val == "United States of America")
                State.enable = true;
            else
                State.enable = false;
            Cancel.eventHandler = cancelHandler;
            Submit.eventHandler = submitHandler;
            Country.eventHandler = countryHandler;
        }
    }

    class Service : Entity, ServiceJson
    {
        public int number_of_dairy_farms { get { return Field.stringToInt(NumberOfDairyFarms.getValue()); } set { NumberOfDairyFarms.setValue(Field.intToString(value)); } }
        public int number_of_dairy_cows { get { return Field.stringToInt(NumberOfDairyCows.getValue()); } set { NumberOfDairyCows.setValue(Field.intToString(value)); } }

        public Field NumberOfDairyFarms { get; set; }

        public Field NumberOfDairyCows { get; set; }

        public Service()
        {
            initFields();
        }

        private void initFields()
        {
            NumberOfDairyFarms = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of dairy farms", ltext: "# of dairy farms:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Three);
            NumberOfDairyCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of dairy cows", ltext: "# of dairy cows:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Four);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Address", ltext: "Address:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Five);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Country", ltext: "Country:", autosize: false, items: Const.COUNTRY, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Six);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "State", ltext: "State:", autosize: false, items: Const.STATE, enable: false, check: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Seven);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "City", ltext: "City:", autosize: false, placeh: Place.Six, lplaceh: Place.Four, placev: Place.Eight);
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Name", ltext: "Name:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Three);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Mobile", ltext: "Mobile:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Four);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email Address", ltext: "Email Address:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Five);
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Contact Name", ltext: "Contact Name:", autosize: false, placeh: Place.Three, lplaceh: Place.One, placev: Place.Six);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Contract Type", ltext: "Contract Type:", autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Three, lplaceh: Place.One, placev: Place.Seven);
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
            if (Country.val == "United States of America")
                State.enable = true;
            else
                State.enable = false;
            Cancel.eventHandler = cancelHandler;
            Submit.eventHandler = submitHandler;
            Country.eventHandler = countryHandler;
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
            return description;
        }
    }

    class Action : Gui, ActionJson
    {
        public int id { get; set; }
        public string added_date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public string part_number { get { return PartNumber.getValue(); } set { PartNumber.setValue(value); } }
        public int contact { get; set; }
        public string tablet { get; set; }
        public int? farm { get { return Field.stringToIntOrNull(Farm.getValue()); } set { Farm.setValue(Field.intToString(value)); } }
        public int? service_provider { get { return Field.stringToIntOrNull(Service.getValue()); } set { Service.setValue(Field.intToString(value)); } }

        //private object ppFarm
        //{
        //    get
        //    {
        //        return Field.intToString(farm);
        //    }
        //    set
        //    {
        //        farm = Field.stringToIntOrNull(value as string);
        //    }
        //}
        //private object ffFarm;
        //private object pFarm { get { return Field.getObject(fFarm, ref ffFarm, ppFarm); } set { ppFarm = Field.setObject(fFarm, ref ffFarm, value, ppFarm); } }
        //private Field fFarm;
        //public Field Farm { get { return Field.getField(fFarm, pFarm); } set { pFarm = Field.setField(ref fFarm, value, pFarm); } }
        public Field Farm { get; set; }

        public Field Service { get; set; }

        public Field RadioFarm { get; set; }
        public Field RadioService { get; set; }

        //public object ppPartNumber { get { return part_number; } set { part_number = value as string; } }
        //private object ffPartNumber;
        //private object pPartNumber { get { return Field.getObject(fPartNumber, ref ffPartNumber, ppPartNumber); } set { ppPartNumber = Field.setObject(fPartNumber, ref ffPartNumber, value, ppPartNumber); } }
        //public Field fPartNumber;
        //public Field PartNumber { get { return Field.getField(fPartNumber, pPartNumber); } set { pPartNumber = Field.setField(ref fPartNumber, value, pPartNumber); } }
        public Field PartNumber { get; set; }

        public Field Progress { get; set; }
        public Field Cancel { get; set; }
        public Field Approve { get; set; }

        public Action()
        {
            initFields();
        }

        public Action(AM am, string tablet, object[] farms, object[] services, EventHandler partNumberEventHandler, EventHandler farmEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler)
        {
            initFields();
            initFields(am, tablet, farms, services, partNumberEventHandler, farmEventHandler, radioEventHandler, canselEventHandler, approveEventHandler);
        }

        private void initFields()
        {
            PartNumber = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Part Number", ltext: "Add treatments to AM – SN ", width: Field.DefaultWidthMedium, placev: Place.Eight, lplacev: Place.Seven);
            Farm = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm / Service provider", ltext: "Select Farm / Service provider", width: Field.DefaultWidthMedium, placev: Place.Five, lplacev: Place.Four);
            Service = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm / Service provider", ltext: "Select Farm / Service provider", width: Field.DefaultWidthMedium, placev: Place.Five, lplacev: Place.Four);

            Picture = new Field(ltype: typeof(PictureBox), lplacev: Place.One);
            Welcome = new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.Two);
            RadioFarm = new Field(ltype: typeof(RadioButton), ltext: "Farm", width: Field.DefaultWidthMedium, lplaceh: Place.Two, lplacev: Place.Five);
            RadioService = new Field(ltype: typeof(RadioButton), ltext: "Service provider", width: Field.DefaultWidthMedium, lplaceh: Place.Two, lplacev: Place.Six);
            Progress = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Ten);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Five, lplacev: Place.End);
            Approve = new Field(ltype: typeof(Button), ltext: "Approve", lplaceh: Place.Two, lplacev: Place.End);
        }

        private void initFields(AM am, string tablet, object[] farms, object[] services, EventHandler partNumberEventHandler, EventHandler farmEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler)
        {
            PartNumber.ltext += am.SNum.ToString();
            PartNumber.eventHandler = partNumberEventHandler;
            Farm.items = farms;
            Farm.eventHandler = farmEventHandler;
            Service.items = services;
            Service.eventHandler = farmEventHandler;

            aptx_id = string.Format("{0:x} {1:x} {2:x}", am.AptxId[0], am.AptxId[1], am.AptxId[2]);
            am_id = am.SNum.ToString();
            this.tablet = tablet;

            RadioFarm.eventHandler = radioEventHandler;
            RadioService.eventHandler = radioEventHandler;
            Cancel.eventHandler = canselEventHandler;
            Approve.eventHandler = approveEventHandler;
        }
    }
}
