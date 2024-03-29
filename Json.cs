﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAMBurner
{
    interface ExpireJson
    {
        public string detail { get; set; }
    }

    interface LoginJson
    {
        public string email { get; set; }
        public string password { get; set; }
        public string tablet { get; set; }
    //}

    //interface LoginResponseJson
    //{
        public string token { get; set; }
        public UserJson user { get; set; }
    }

    class UserJson
    {
        //public int? pk { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool? is_password_changed { get; set; }
    }

    class DistributorJson
    {
        //public int? id { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool? is_active { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        //public string company_id { get; set; }
        //public int? parent_distributor { get; set; }
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

    class ContactJson : ExpireJson
    {
        //public int? id { get; set; }
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
        public string detail { get; set; }
    }

    interface FarmJson
    {
        public int? id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public bool? is_active { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string contact_name { get; set; }
        public string farm_type { get; set; }
        public string breed_type { get; set; }
        public string milking_setup_type { get; set; }
        public string location_of_treatment_type { get; set; }
        public string contract_type { get; set; }
        public int? number_of_lactating_cows { get; set; }
        public bool? dhi_test { get; set; }
    }

    interface ServiceJson
    {
        public int? id { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public int? number_of_dairy_farms { get; set; }
        public int? number_of_dairy_cows { get; set; }
        public string name { get; set; }
        public string contact_name { get; set; }
        public string contract_type { get; set; }
    }

    class SettingsJson
    {
        public int? max_am_pulses { get; set; }
        public int? number_of_pulses_per_treatment { get; set; }
    }

    interface TreatmentPackageJson
    {
        //public int? id { get; set; }
        public bool? is_active { get; set; }
        public string part_number { get; set; }
        public int? amount_of_treatments { get; set; }
        public string description { get; set; }
        public string contract_type { get; set; }
        public string added_date { get; set; }
    }

    interface ActionJson
    {
        //public int? id { get; set; }
        //public string date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public string part_number { get; set; }
        //public int? contact { get; set; }
        public string tablet { get; set; }
        public int? farm { get; set; }
        public int? service_provider { get; set; }
        //public int? distributor { get; set; }
    }

    class Data
    {
        public Am am;
        public Web web;
        public Login login;
        public UserJson user;
        public Password password;
        public Reset reset;
        public Farm farm;
        public List<Farm> farms;
        public Service service;
        public List<Service> services;
        public SettingsJson settings;
        public TreatmentPackage treatmentPackage;
        public List<TreatmentPackage> treatmentPackages;
        public Action action;
        public delegate void dProgress(Object progress, bool reset);
        public dProgress dprogress;

        public Data(dProgress dprogress)
        {
            this.dprogress = dprogress;
        }

        public uint Current
        {
            get => calcCurrent(am.Current);
        }

        public uint CurrentPrev
        {
            get => calcCurrent(am.CurrentPrev);
        }

        private uint calcCurrent(uint amCurrent)
        {
            uint current = (uint)(amCurrent / settings.number_of_pulses_per_treatment + 
                ((amCurrent % settings.number_of_pulses_per_treatment) > 
                (settings.number_of_pulses_per_treatment * 0.8) ? 1 : 0));
            return current;
        }
    }

    class Gui: ExpireJson
    {
        public string detail { get; set; }

        public delegate Task<JsonDocument> dWeb<TJson>(TJson jentity, string entityUrl);

        public delegate Task<ErrCode> dResponseOk<T>(Data data, T entity);
        public delegate ErrCode dCheck(Data data);
        public delegate Task<ErrCode> dApprove<T>(Data data, T entity);
        public delegate Task<List<string>> dResponseErr<T>(Data data, ErrCode errcode);

        public delegate void dLogout();
        public delegate Task dNotify(string title, string messages, string cancel);
        public delegate Task<bool> dNotifyAnswer(string title, string messages, string accept, string cancel);
        public delegate void dEnabled(bool enabled);
        public delegate void dShow();
        public delegate void dHide();
        //public delegate void dDraw<T>(T entity);
        public delegate ErrCode dDraw(object entity, Type type = null);
        public delegate ErrCode dDrawPage(Type T, Type I);

        public dLogout dlogout;
        public dShow dshow;
        public dHide dhide;
        public dEnabled denabled;
        public dNotify dnotify;
        public dNotifyAnswer dnotifyAnswer;
        public dDraw ddraw;
        public dDrawPage ddrawPage;

        public Field Picture { get; set; }
        public Field Welcome { get; set; }
        
        public Gui()
        {
        }

        public Gui(dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dDrawPage ddrawPage)
        {
            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            this.ddrawPage = ddrawPage;
        }

        //public ErrCode drawPage<T>(T entity, Type type)
        //{
        //    ErrCode errcode = ErrCode.ERROR;
        //    foreach (PropertyInfo prop in type.GetProperties())
        //    {
        //        //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
        //        //PropertyInfo prop = props.ElementAt(controls.IndexOf(control));
        //        Field field = prop.GetValue(entity) as Field;
        //        if (field != null)
        //        {
        //            if (field.view)
        //            {
        //                if (field.error == ErrCode.EPARAM)
        //                {
        //                    dhide();
        //                    ddraw(entity, type: type);
        //                    checkFields();
        //                    return ErrCode.EPARAM;
        //                }
        //            }
        //        }
        //    }
        //    return ErrCode.OK;
        //}

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

        public void checkField(string jname)
        {
            //PropertyInfo [] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                Field field = prop.GetValue(this) as Field;
                if ((field != null) && field.view)
                {
                    if (prop.Name.ToLower() == jname.Replace('_', '\0'))
                    {
                        field.val = field.dflt;
                        field.error = ErrCode.EPARAM;
                    }
                }
            }
        }

        public ErrCode responseParse(JsonDocument jsonDocument, List<string> errors, List<string> messages)
        {
            ErrCode errcode = ErrCode.ESERVER;

            if (jsonDocument != null)
            {
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    //Console.WriteLine("{0} = {1}", prop.Name, prop.GetValue(user, null));
                    JsonElement jsonElement;
                    if (jsonDocument.RootElement.TryGetProperty(prop.Name, out jsonElement))
                    {
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                        {
                            checkField(prop.Name);
                            if(errors.Count() == 0)
                                errors.AddRange(jsonElement.EnumerateArray().Select(e => prop.Name + ": " + e.ToString()));
                        }
                        else if (jsonElement.ValueKind == JsonValueKind.String)
                            messages.Add(jsonElement.GetString());
                    }
                }
            }

            if ((messages.Count != 0) && (errors.Count == 0))
                errcode = ErrCode.OK;

            return errcode;
        }

        public async Task<ErrCode> send<TJson, T>(TJson jentity, string url, dWeb<TJson> dweb, string captionOk, string captionErr,
            bool showMsgs, dResponseOk<T> dresponseOk, Data data, dCheck dcheck = null, dApprove<TJson> dapprove = null, dResponseErr<T> dresponseErr = null, List<string> messagesOk = null, List<string> messagesErr = null)
        {
            ErrCode errcode = ErrCode.ECONNECT;

            if ((jentity != null) && (url != null) && (dweb != null) && (captionOk != null) && (captionErr != null) && (dresponseOk != null) && (data != null) && (denabled != null) && (dnotify != null) && (dhide != null) && (dlogout != null))
            {
                JsonDocument jsonDocument = null;
                T rentity = default;
                List<string> errors = new List<string>();
                List<string> messages = new List<string>();

                denabled(false);

                if (dcheck != null)
                    errcode = dcheck(data);
                else
                    errcode = ErrCode.OK;

                if(errcode == ErrCode.OK)
                {
                    if ((errcode = checkFields()) == ErrCode.OK)
                    {
                        if (dapprove != null)
                            errcode = await dapprove(data, jentity);
                        else
                            errcode = ErrCode.OK;

                        if (errcode == ErrCode.OK)
                        {
                            jsonDocument = await dweb(jentity, url);
                            if (jsonDocument != null)
                            {
                                if ((errcode = responseParse(jsonDocument, errors, messages)) == ErrCode.OK)
                                {
                                    try { rentity = JsonSerializer.Deserialize<T>(jsonDocument.RootElement.ToString()); }
                                    catch (Exception ex) { LogFile.logWrite(ex.ToString()); }

                                    if (rentity != null)
                                        errcode = checkExpire(rentity);
                                    else
                                        errcode = ErrCode.ESERVER;
                                }
                            }
                            else
                                errcode = ErrCode.ESERVER;
                        }
                    }
                }

                if (errcode == ErrCode.OK)
                {
                    List<string> notifys = new List<string>();
                    
                    if (showMsgs && (messages.Count() > 0))
                        //await dnotify(captionOk, messages.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");
                        notifys.AddRange(messages);

                    if (messagesOk != null)
                        //await dnotify(captionOk, messagesOk.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");
                        notifys.AddRange(messagesOk);

                    if (notifys.Count() > 0)
                        await dnotify(captionOk, notifys.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");

                    errcode = await dresponseOk(data, rentity);
                }

                if (errcode != ErrCode.OK)
                {
                    if (errcode == ErrCode.EXPIRE)
                    {
                        await dnotify(captionErr, "The session is expired, please login to continue", "Ok");
                        dlogout();
                    }
                    else
                    {
                        List<string> notifys = new List<string>();

                        if (errors.Count() > 0)
                            //await dnotify(captionErr, errors.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");
                            notifys.AddRange(errors);

                        if (messagesErr != null)
                            //await dnotify(captionErr, messagesErr.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");
                            notifys.AddRange(messagesErr);

                        if (dresponseErr != null)
                        {

                            List<string> responseErr = await dresponseErr(data, errcode);
                            if (responseErr != null)
                                //dnotify(captionErr, responseErr.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");
                                notifys.AddRange(responseErr);
                        }

                        if (notifys.Count() > 0)
                            await dnotify(captionErr, notifys.Aggregate(string.Empty, (r, m) => r += "\n" + m), "Ok");

                        denabled(true);
                    }
                }
            }
            return errcode;
        }

        public ErrCode checkExpire<T>(T rentity)
        {
            ErrCode errcode;
            ExpireJson expire = rentity as ExpireJson;
            if (expire != null)
            {
                if ((expire.detail != null) && (expire.detail.Contains("Signature has expired")))
                    errcode = ErrCode.EXPIRE;
                else
                    errcode = ErrCode.OK;
            }
            else
                errcode = ErrCode.EXPIRE;
            return errcode;
        }

    }

    class Login : Gui, LoginJson
    {
        public string email { get { return Email.getValue(); } set { Email.setValue(value); } }
        public string password { get { return Password.getValue(); } set { Password.setValue(value); } }
        public string tablet { get; set; }

        public string token { get; set; }
        public UserJson user { get; set; }

        public Field Email { get; set; }

        public Field Password { get; set; }
        
        public Field ShowPassword { get; set; }

        public Field Forgot { get; set; }

        public Field Press { get; set; }

        //public dDraw<Login> ddraw;

        public Login()
        {
            initFields();
        }

        public Login(EventHandler forgotEventHandler, EventHandler buttonEventHandler, EventHandler checkBoxEventHandler,
            //dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw<Login> ddraw, dShow dshow = null)
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            initFields();
            initFields(forgotEventHandler, buttonEventHandler, checkBoxEventHandler,
                dlogout, dhide, denabled, dnotify, ddraw, dshow);
        }

        private void initFields()
        {
            Email = new Field(type: typeof(RichTextBox), dflt: "Username", width: Field.DefaultWidthLarge, placev: Place.Two);
            Password = new Field(type: typeof(TextBox), dflt: "Password", width: Field.DefaultWidthLarge, placev: Place.Three);
            ShowPassword = new Field(type: typeof(CheckBox), dflt: "Show password", autosize: false, width: Field.DefaultWidthLarge, placev: Place.Four);
            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Forgot = new Field(ltype: typeof(LinkLabel), ltext: "Forgot password", autosize: false, width: Field.DefaultWidthLarge, lplacev: Place.Five);
            Press = new Field(ltype: typeof(Button), ltext: "Login", lplacev: Place.Seven);
        }

        private void initFields(EventHandler forgotEventHandler, EventHandler buttonEventHandler, EventHandler checkBoxEventHandler,
            //dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw<Login> ddraw, dShow dshow = null)
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            Forgot.eventHandler = forgotEventHandler;
            Press.eventHandler = buttonEventHandler;
            ShowPassword.eventHandler = checkBoxEventHandler;

            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            if (dshow != null)
                this.dshow = dshow;
        }

        public async Task<ErrCode> responseOk(Data data, Login rlogin)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (data.web != null) && (data.password != null) && (data.password.ddraw != null) && (rlogin != null) && 
                (rlogin.user != null) && (rlogin.token != null) && (dhide != null) && (dshow != null))
            {
                // if ok
                data.user = rlogin.user;
                if (data.user.is_password_changed == false)
                {
                    dhide();
                    data.password.ddraw(data.password);
                    errcode = ErrCode.OK;
                }
                else
                {
                    JsonDocument jsonDocument = await data.web.getConstants();
                    if (jsonDocument != null)
                        Const.parseConstants(jsonDocument);
                    data.farms = await data.web.entityGet<List<Farm>>("api/p/farms/");
                    if (data.farms != null)
                        data.farms = data.farms.Where(f => f.is_active == true).ToList();
                    data.services = await data.web.entityGet<List<Service>>("api/p/service_providers/");
                    data.treatmentPackages = await data.web.entityGet<List<TreatmentPackage>>("api/p/treatment_package/");
                    if (data.treatmentPackages != null)
                        data.treatmentPackages = data.treatmentPackages.Where(t => t.is_active == true).ToList();
                    data.settings = await data.web.entityGet<SettingsJson>("api/p/settings/");

                    if ((data.user != null) && (data.farms != null) && (data.services != null) && (data.treatmentPackages != null) && (data.settings != null) &&
                        (Const.DCOUNTRY != null) && (Const.COUNTRY != null) && (Const.DSTATE != null) && (Const.STATE != null) &&
                        (Const.FARM_TYPE != null) && (Const.BREED_TYPE != null) && (Const.MILKING_SETUP_TYPE != null) &&
                        (Const.LOCATION_OF_TREATMENT_TYPE != null) && (Const.CONTRACT_TYPE != null))
                    {
                        dhide();
                        dshow();
                        errcode = ErrCode.OK;
                    }
                }
            }
            return errcode;
        }

        public async Task send(Data data)
        {
            if ((data != null) && (data.login != null) && (data.web != null))
            { 
                await send<LoginJson, Login>(data.login, "api/p/login/", data.web.login,
                    "Login Success", "Login Failed", false, responseOk, data,
                    messagesErr: new List<string>() { "Login failed, check your username and password, ",
                    "make sure your tablet is connected to the internet" });
            }
        }
    }

    class Password : Gui, PasswordJson
    {
        public string new_password1 { get { return Password1.getValue(); } set { Password1.setValue(value); } }
        public string new_password2 { get { return Password2.getValue(); } set { Password2.setValue(value); } }
        //public string detail { get; set; }

        public Field Password1 { get; set; }

        public Field Password2 { get; set; }

        public Field ChangePassword { get; set; }

        //public dDraw<Password> ddraw;

        public Password()
        {
            initFields();
        }

        public Password(EventHandler buttonEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            initFields();
            initFields(buttonEventHandler,
                dlogout, dhide, denabled, dnotify, ddraw, dshow);
        }

        private void initFields()
        {
            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Password1 = new Field(type: typeof(TextBox), ltype: typeof(Label), dflt: "Password", ltext: "Please Enter a new password. \nPassword should be complex and at least 8 chars long.", 
                width: Field.DefaultWidthLarge, placev: Place.Five, lplacev: Place.Three);
            Password2 = new Field(type: typeof(TextBox), dflt: "Confirm Password", width: Field.DefaultWidthLarge, placev: Place.Six);
            ChangePassword = new Field(ltype: typeof(Button), ltext: "Change Password", width: Field.DefaultWidthMedium, lplacev: Place.Seven);
        }

        private void initFields(EventHandler buttonEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            ChangePassword.eventHandler = buttonEventHandler;
            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            if (dshow != null)
                this.dshow = dshow;
        }

        public async Task<ErrCode> responseOk(Data data, Password newPassword)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (data.login != null) && (dhide != null) && (data.login.password != null) && (data.login.ddraw != null))
            {
                dhide();
                data.login.password = null;
                data.login.ddraw(data.login);
                errcode = ErrCode.OK;
            }
            return errcode;
        }

        public async Task<List<string>> responseErr(Data data, ErrCode errcode)
        {
            if (errcode == ErrCode.EMATCH)
                return new List<string>() { "Can't confirm the password,", "the two values don't match" };
            else if (errcode == ErrCode.ELENGTH)
                return new List<string>() { "The password specified is less than 8 characters long" };
            return new List<string>() { "Change password failed,", "please enter a valid values" };
        }

        public ErrCode check(Data data)
        {
            ErrCode errcode = ErrCode.OK;
            if ((data != null) && (data.password != null) &&
                (data.password.Password1 != null) && (data.password.Password2 != null) &&
                (data.password.new_password1 != null))
            {
                if (data.password.new_password1 != data.password.new_password2)
                    errcode = ErrCode.EMATCH;
                else if (data.password.new_password1 == null)
                    errcode = ErrCode.ELENGTH;
                else if (data.password.new_password1.Length < 8)
                    errcode = ErrCode.ELENGTH;

                if (errcode != ErrCode.OK)
                {
                    data.password.Password1.error = ErrCode.EPARAM;
                    data.password.Password2.error = ErrCode.EPARAM;
                }
            }
            return errcode;
        }

        public async Task send(Data data)
        {
            if ((data != null) && (data.password != null) && (data.web != null))
            { 
                await send<PasswordJson, Password>(data.password, "api/p/password/change/", data.web.entityAdd,
                    "Change Password Success", "Change Password Failed", true, responseOk, data, dcheck: check, dresponseErr: responseErr);
            }
        }
    }

    class Reset : Gui, ResetJson
    {
        public string email { get { return Email.getValue(); } set { Email.setValue(value); } }
        public string message { get; set; }

        public Field Email { get; set; }

        public Field ResetPassword { get; set; }

        //public dDraw<Reset> ddraw;

        public Reset()
        {
            initFields();
        }

        public Reset(EventHandler buttonEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            initFields();
            initFields(buttonEventHandler,
                dlogout, dhide, denabled, dnotify, ddraw, dshow);
        }

        private void initFields()
        {
            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email@email.com", ltext: "The password will be reseted, please enter an email to send a temporary password:", width: Field.DefaultWidthLarge, placev: Place.Five, lplacev: Place.Three);
            ResetPassword = new Field(ltype: typeof(Button), ltext: "Reset Password", width: Field.DefaultWidthMedium, lplacev: Place.Seven);
        }

        private void initFields(EventHandler buttonEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null)
        {
            ResetPassword.eventHandler = buttonEventHandler;
            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            if (dshow != null)
                this.dshow = dshow;
        }

        public async Task<ErrCode> responseOk(Data data, Reset reset)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (data.login != null) && (dhide != null) && (data.login.ddraw != null))
            {
                dhide();
                data.login.ddraw(data.login);
                errcode = ErrCode.OK;
            }
            return errcode;
        }

        public async Task send(Data data)
        {
            if ((data != null) && (data.reset != null) && (data.web != null))
            { 
                await send<ResetJson, Reset>(data.reset, "api/p/password_reset/", data.web.entityAdd,
                    "Reset Password Success", "Reset Password Failed", true, responseOk, data,
                    messagesOk: new List<string>() { "An email with your logon details was sent.", "Please use those details to logon." },
                    messagesErr: new List<string>() { "Reset password failed,", "please enter a valid values" });
            }
        }
    }

    interface IPage1
    {
        public Field Name { get; set; }

        public Field Address { get; set; }

        public Field Country { get; set; }

        public Field State { get; set; }

        public Field City { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Next { get; set; }
        public Field Submit { get; set; }
    }

    class Entity : Gui, IPage1
    {

        public int? id { get; set; }
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
        public Field Back { get; set; }
        public Field Next { get; set; }

        public Field ContractType { get; set; }

        public Field Name { get; set; }

        public Field Address { get; set; }

        public Field Country { get; set; }

        public Field State { get; set; }

        public Field City { get; set; }

        public Field ContactName { get; set; }

        public Field Mobile { get; set; }

        public Field Email { get; set; }

        public Entity()
        { }

        public Entity(dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dDrawPage ddrawPage) : base(dlogout, dhide, denabled, dnotify, ddraw, ddrawPage)
        { }

        public void initFields(
            bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler,
            EventHandler toPage1Handler, EventHandler toPage2Handler, EventHandler toPage3Handler, EventHandler toPage4Handler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dDrawPage ddrawPage, dShow dshow)
        {
            if (edit)
            {
                Caption.ltext = "Edit " + Caption.ltext;
                ContractType.view = false;
            }
            else
            {
                Caption.ltext = "Add " + Caption.ltext;
                ContractType.view = true;
            }
            if (Country.val == "United States of America")
                State.enable = true;
            else
                State.enable = false;
            Cancel.eventHandler = cancelHandler;
            Submit.eventHandler = submitHandler;
            Country.eventHandler = countryHandler;
            Back.toPage1Handler = toPage1Handler;
            Back.toPage2Handler = toPage2Handler;
            Back.toPage3Handler = toPage3Handler;
            Back.toPage4Handler = toPage4Handler;
            Next.toPage1Handler = toPage1Handler;
            Next.toPage2Handler = toPage2Handler;
            Next.toPage3Handler = toPage3Handler;
            Next.toPage4Handler = toPage4Handler;

            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            this.ddrawPage = ddrawPage;
            this.dshow = dshow;
        }

        public async Task<ErrCode> responseAddOk<T>(Data data, T rentity)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (rentity != null) && (dhide != null) && (dshow != null))
            {
                List<T> entities = default(List<T>);
                T entity = default(T);
                if (typeof(T) == typeof(Farm))
                {
                    if ((data.farms != null) && (data.farm != null))
                    {
                        entities = data.farms as List<T>;
                        entity = (T)(data.farm as object);
                    }
                }
                else if (typeof(T) == typeof(Service))
                {
                    if ((data.services != null) && (data.service != null))
                    {
                        entities = data.services as List<T>;
                        entity = (T)(data.service as object);
                    }
                }
                if ((entities != null) && (entity != null))
                {
                    entities.Add(rentity);
                    dhide();
                    dshow();
                    errcode = ErrCode.OK;
                }
            }
            return errcode;
        }

        public async Task<ErrCode> responseEditOk<T>(Data data, T rentity)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (rentity != null) && (dhide != null) && (dshow != null))
            {
                List<T> entities = default(List<T>);
                T entity = default(T);
                if (typeof(T) == typeof(Farm))
                {
                    if ((data.farms != null) && (data.farm != null))
                    {
                        entities = data.farms as List<T>;
                        entity = (T)(data.farm as object);
                    }
                }
                else if (typeof(T) == typeof(Service))
                {
                    if ((data.services != null) && (data.service != null))
                    {
                        entities = data.services as List<T>;
                        entity = (T)(data.service as object);
                    }
                }
                if ((entities != null) && (entity != null))
                {
                    entities.Insert(entities.IndexOf(entity), rentity);
                    entities.Remove(entity);
                    dhide();
                    dshow();
                    errcode = ErrCode.OK;
                }
            }
            return errcode;
        }

        public async Task<List<string>> responseErr<T>(Data data, ErrCode errcode)
        {
            if (errcode != ErrCode.OK)
            {
                Type[] pages = new Type[] { };
                if (typeof(T) == typeof(Farm))
                    pages = new Type[] { typeof(IPage1), typeof(IFarmPage2), typeof(IFarmPage3), typeof(IFarmPage4) };
                else if (typeof(T) == typeof(Service))
                    pages = new Type[] { typeof(IPage1), typeof(IServicePage2), typeof(IServicePage3) };
                foreach (Type page in pages)
                {
                    if (ddrawPage(typeof(T), page) != ErrCode.OK)
                    {
                        checkFields();
                        return null;
                    }
                }
            }
            return null;
        }

        public override string ToString()
        {
            return Name.val;
        }
    }

    interface IFarmPage2
    {
        public Field ContactName { get; set; }

        public Field Mobile { get; set; }

        public Field Email { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Back { get; set; }
        public Field Next { get; set; }
        public Field Submit { get; set; }
    }

    interface IFarmPage3
    {
        public Field ContractType { get; set; }

        public Field NumberOfLactatingCows { get; set; }

        public Field FarmType { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Back { get; set; }
        public Field Next { get; set; }
        public Field Submit { get; set; }
    }

    interface IFarmPage4
    {
        public Field BreedType { get; set; }

        public Field DhiTest { get; set; }

        public Field MilkingSetupType { get; set; }

        public Field LocationOfTreatmentType { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Back { get; set; }
        public Field Submit { get; set; }
    }

    class Farm : Entity, FarmJson, IFarmPage2, IFarmPage3, IFarmPage4
    {
        public static string[] DHI_TEST = new string[] { "No", "Yes" };

        public bool? is_active { get; set; }
        public string farm_type { get { return FarmType.getValue(); } set { FarmType.setValue(value); } }
        public string breed_type { get { return BreedType.getValue(); } set { BreedType.setValue(value); } }
        public string milking_setup_type { get { return MilkingSetupType.getValue(); } set { MilkingSetupType.setValue(value); } }
        public string location_of_treatment_type { get { return LocationOfTreatmentType.getValue(); } set { LocationOfTreatmentType.setValue(value); } }
        public int? number_of_lactating_cows  { get { return Field.stringToInt(NumberOfLactatingCows.getValue()); } set { NumberOfLactatingCows.setValue(Field.intToString(value)); } }
        public bool? dhi_test { get { return Field.stringToBool(DhiTest.getValue()); } set { DhiTest.setValue(Field.boolToString(value)); } }

        public object IsActive { get { return Field.boolToString(is_active); } set { is_active = Field.stringToBool(value as string); } }

        public Field FarmType { get; set; }

        public Field BreedType { get; set; }

        public Field NumberOfLactatingCows { get; set; }

        public Field DhiTest { get; set; }

        public Field MilkingSetupType { get; set; }

        public Field LocationOfTreatmentType { get; set; }

        //public dDraw<Farm> ddraw;

        public Farm()
        {
            initFields();
        }

        private void initFields()
        {
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Farm Name", ltext: "Farm Name:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Address", ltext: "Address:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Country", ltext: "Country:", width: Field.DefaultWidthEntity, autosize: false, items: Const.COUNTRY, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "State", ltext: "State:", width: Field.DefaultWidthEntity, autosize: false, items: Const.STATE, enable: false, check: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Five);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "City", ltext: "City:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Six);
            IsActive = Const.IS_ACTIVE;
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Contact Name", ltext: "Contact Name:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Mobile", ltext: "Mobile:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email Address", ltext: "Email Address:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            FarmType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm Type", ltext: "Farm Type:", width: Field.DefaultWidthEntity, autosize: false, items: Const.FARM_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            NumberOfLactatingCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of Lactating Cows", ltext: "# of Lactating Cows:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Contract Type", ltext: "Contract Type:", width: Field.DefaultWidthEntity, autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            BreedType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Breed Type", ltext: "Breed Type:", width: Field.DefaultWidthEntity, autosize: false, items: Const.BREED_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            DhiTest = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Milk Recording", ltext: "Milk Recording:", width: Field.DefaultWidthEntity, autosize: false, items: DHI_TEST, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            MilkingSetupType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Milking Setup", ltext: "Milking Setup:", width: Field.DefaultWidthEntity, autosize: false, items: Const.MILKING_SETUP_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            LocationOfTreatmentType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Treatment Location", ltext: "Treatment Location:", width: Field.DefaultWidthEntity, autosize: false, items: Const.LOCATION_OF_TREATMENT_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Five);

            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Caption = new Field(ltype: typeof(Label), ltext: "Farm", width: Field.DefaultWidthLarge, font: Field.DefaultFontLarge, lplacev: Place.One);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Twoh, lplacev: Place.Seven);
            Submit = new Field(ltype: typeof(Button), ltext: "Submit", lplaceh: Place.Fiveh, lplacev: Place.Seven);
            Back = new Field(ltype: typeof(Button), ltext: "Back", lplaceh: Place.Threeh, lplacev: Place.Seven);
            Next = new Field(ltype: typeof(Button), ltext: "Next", lplaceh: Place.Fourh, lplacev: Place.Seven);
    }

    public Farm(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler,
            EventHandler toPage1Handler, EventHandler toPage2Handler, EventHandler toPage3Handler, EventHandler toPage4Handler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dDrawPage ddrawPage, dShow dshow)
        {
            initFields();
            initFields(edit, countryHandler, cancelHandler, submitHandler,
                toPage1Handler, toPage2Handler, toPage3Handler, toPage4Handler,
                dlogout, dhide, denabled, dnotify, ddraw, ddrawPage, dshow);
        }

        public async Task send(Data data, bool edit)
        {
            if ((data != null) && (data.farm != null) && (data.web != null))
            {
                if (edit)
                {
                    await send<FarmJson, Farm>(data.farm, "api/p/farms/" + data.farm.Id + "/", data.web.entityEdit,
                        "Submit Success", "Submit Failed", false, responseEditOk, data,
                        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
                        "make sure all the fields are filled with valid values" }, dresponseErr: responseErr<Farm>);
                }
                else
                { 
                    await send<FarmJson, Farm>(data.farm, "api/p/farms/", data.web.entityAdd,
                        "Submit Success", "Submit Failed", false, responseAddOk, data,
                        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
                        "make sure all the fields are filled with valid values" }, dresponseErr: responseErr<Farm>);
                }
            }
        }
    }

    interface IServicePage2
    {
        public Field ContactName { get; set; }

        public Field Mobile { get; set; }

        public Field Email { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Back { get; set; }
        public Field Next { get; set; }
        public Field Submit { get; set; }
    }

    interface IServicePage3
    {
        public Field NumberOfDairyFarms { get; set; }

        public Field NumberOfDairyCows { get; set; }

        public Field ContractType { get; set; }

        public Field Caption { get; set; }
        public Field Cancel { get; set; }
        public Field Back { get; set; }
        public Field Submit { get; set; }
    }

    interface IServicePage4
    {
    }

    class Service : Entity, ServiceJson, IServicePage2, IServicePage3
    {
        public int? number_of_dairy_farms { get { return Field.stringToInt(NumberOfDairyFarms.getValue()); } set { NumberOfDairyFarms.setValue(Field.intToString(value)); } }
        public int? number_of_dairy_cows { get { return Field.stringToInt(NumberOfDairyCows.getValue()); } set { NumberOfDairyCows.setValue(Field.intToString(value)); } }

        public Field NumberOfDairyFarms { get; set; }

        public Field NumberOfDairyCows { get; set; }

        //public dDraw<Service> ddraw;

        public Service()
        {
            initFields();
        }

        private void initFields()
        {
            Name = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Service Provider Name", ltext: "Service Provider Name:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            Address = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Address", ltext: "Address:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            Country = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Country", ltext: "Country:", width: Field.DefaultWidthEntity, autosize: false, items: Const.COUNTRY, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            State = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "State", ltext: "State:", width: Field.DefaultWidthEntity, autosize: false, items: Const.STATE, enable: false, check: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Five);
            City = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "City", ltext: "City:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Six);
            ContactName = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Contact Name", ltext: "Contact Name:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            Mobile = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Mobile", ltext: "Mobile:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            Email = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "Email Address", ltext: "Email Address:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);
            NumberOfDairyFarms = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of dairy farms", ltext: "# of dairy farms:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Two);
            NumberOfDairyCows = new Field(type: typeof(RichTextBox), ltype: typeof(Label), dflt: "# of dairy cows", ltext: "# of dairy cows:", width: Field.DefaultWidthEntity, autosize: false, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Three);
            ContractType = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Contract Type", ltext: "Contract Type:", width: Field.DefaultWidthEntity, autosize: false, items: Const.CONTRACT_TYPE, placeh: Place.Fourh, lplaceh: Place.Threeh, placev: Place.Four);

            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Caption = new Field(ltype: typeof(Label), ltext: "Service Provider", width: Field.DefaultWidthLarge, font: Field.DefaultFontLarge, lplacev: Place.One);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Twoh, lplacev: Place.Seven);
            Submit = new Field(ltype: typeof(Button), ltext: "Submit", lplaceh: Place.Fiveh, lplacev: Place.Seven);
            Back = new Field(ltype: typeof(Button), ltext: "Back", lplaceh: Place.Threeh, lplacev: Place.Seven);
            Next = new Field(ltype: typeof(Button), ltext: "Next", lplaceh: Place.Fourh, lplacev: Place.Seven);
        }

        public Service(bool edit, EventHandler countryHandler, EventHandler cancelHandler, EventHandler submitHandler,
            EventHandler toPage1Handler, EventHandler toPage2Handler, EventHandler toPage3Handler, EventHandler toPage4Handler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dDrawPage ddrawPage, dShow dshow)
        {
            initFields();
            initFields(edit, countryHandler, cancelHandler, submitHandler,
                toPage1Handler, toPage2Handler, toPage3Handler, toPage4Handler,
                dlogout, dhide, denabled, dnotify, ddraw, ddrawPage, dshow);
        }

        public async Task send(Data data, bool edit)
        {
            if ((data != null) && (data.service != null) && (data.web != null))
            {
                if (edit)
                { 
                    await send<ServiceJson, Service>(data.service, "api/p/service_providers/" + data.service.Id + "/", data.web.entityEdit,
                        "Submit Success", "Submit Failed", false, responseEditOk, data,
                        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
                        "make sure all the fields are filled with valid values" }, dresponseErr: responseErr<Service>);
                }
                else
                { 
                    await send<ServiceJson, Service>(data.service, "api/p/service_providers/", data.web.entityAdd,
                        "Submit Success", "Submit Failed", false, responseAddOk, data,
                        messagesErr: new List<string>() { "Submit failed, can't add empty or negative fields,",
                        "make sure all the fields are filled with valid values" }, dresponseErr: responseErr<Service>);
                }
            }
        }
    }

    class TreatmentPackage : Gui, TreatmentPackageJson
    {
        //public int? id { get; set; }
        public bool? is_active { get; set; }
        public string part_number { get; set; }
        public int? amount_of_treatments { get; set; }
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
        //public int? id { get; set; }
        public string added_date { get; set; }
        public string aptx_id { get; set; }
        public string am_id { get; set; }
        public string part_number { get { return PartNumber.getValue(); } set { PartNumber.setValue(value); } }
        //public int? contact { get; set; }
        public string tablet { get; set; }
        public int? farm { get { return Field.stringToIntOrNull(Farm.getValue()); } set { Farm.setValue(Field.intToString(value)); } }
        public int? service_provider { get { return Field.stringToIntOrNull(Service.getValue()); } set { Service.setValue(Field.intToString(value)); } }

        public Field Farm { get; set; }

        public Field Service { get; set; }

        public Field RadioFarm { get; set; }
        public Field RadioService { get; set; }

        public Field PartNumber { get; set; }

        public Field Progress { get; set; }
        public Field Cancel { get; set; }
        public Field Approve { get; set; }

        //public dDraw<Action> ddraw;

        public Action()
        {
            initFields();
        }

        public Action(Am am, string tablet, object[] farms, object[] services, EventHandler partNumberEventHandler, EventHandler farmEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null, dNotifyAnswer dnotifyAnswer = null)
        {
            initFields();
            initFields(am, tablet, farms, services, partNumberEventHandler, farmEventHandler, radioEventHandler, canselEventHandler, approveEventHandler,
                dlogout, dhide, denabled, dnotify, ddraw, dshow, dnotifyAnswer);
        }

        private void initFields()
        {
            PartNumber = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Part Number", ltext: "Add treatments to AM – SN ", width: Field.DefaultWidthMedium, placev: Place.Five, lplacev: Place.Four);
            Farm = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm / Service provider", ltext: "Select Farm / Service provider", width: Field.DefaultWidthMedium, placev: Place.Three, lplacev: Place.Two);
            Service = new Field(type: typeof(ComboBox), ltype: typeof(Label), dflt: "Farm / Service provider", ltext: "Select Farm / Service provider", width: Field.DefaultWidthMedium, placev: Place.Three, lplacev: Place.Two);

            Picture = new Field(ltype: typeof(PictureBox), lplaceh: Place.Twoh, lplacev: Place.One);
            Welcome = new Field(ltype: typeof(Label), ltext: "Welcome distributor", font: Field.DefaultFontLarge, lplacev: Place.One);
            RadioFarm = new Field(ltype: typeof(RadioButton), ltext: "Farm", autosize: false, width: Field.DefaultWidthRadio, lplaceh: Place.Fiveh, lplacev: Place.Two);
            //RadioService = new Field(ltype: typeof(RadioButton), ltext: "Service provider", height: Field.DefaultHeightLarge, lplaceh: Place.Fiveh, lplacev: Place.Three) ;
            RadioService = new Field(ltype: typeof(RadioButton), ltext: "Service provider", autosize: false, width: Field.DefaultWidthRadio, lplaceh: Place.Fiveh, lplacev: Place.Three) ;
            Progress = new Field(ltype: typeof(ProgressBar), width: Field.DefaultWidthLarge, height: Field.DefaultHeightSmall, lplacev: Place.Six);
            Cancel = new Field(ltype: typeof(Button), ltext: "Cancel", lplaceh: Place.Twoh, lplacev: Place.Seven);
            Approve = new Field(ltype: typeof(Button), ltext: "Approve", lplaceh: Place.Fiveh, lplacev: Place.Seven);
        }

        private void initFields(Am am, string tablet, object[] farms, object[] services, EventHandler partNumberEventHandler, EventHandler farmEventHandler, EventHandler radioEventHandler, EventHandler canselEventHandler, EventHandler approveEventHandler,
            dLogout dlogout, dHide dhide, dEnabled denabled, dNotify dnotify, dDraw ddraw, dShow dshow = null, dNotifyAnswer dnotifyAnswer = null)
        {
            PartNumber.ltext += am.SNum.ToString();
            PartNumber.eventHandler = partNumberEventHandler;
            Farm.items = farms;
            Farm.eventHandler = farmEventHandler;
            Service.items = services;
            Service.eventHandler = farmEventHandler;

            aptx_id = string.Format("{0:X} {1:X} {2:X}", am.AptxId[0], am.AptxId[1], am.AptxId[2]);
            am_id = am.SNum.ToString();
            this.tablet = tablet;

            RadioFarm.eventHandler = radioEventHandler;
            RadioService.eventHandler = radioEventHandler;
            Cancel.eventHandler = canselEventHandler;
            Approve.eventHandler = approveEventHandler;
            this.dlogout = dlogout;
            this.dhide = dhide;
            this.denabled = denabled;
            this.dnotify = dnotify;
            this.ddraw = ddraw;
            if (dshow != null)
                this.dshow = dshow;
            if (dnotifyAnswer != null)
                this.dnotifyAnswer = dnotifyAnswer;
        }

        public async Task<ErrCode> approve(Data data, ActionJson action)
        {
            ErrCode errcode = ErrCode.ECONNECT;

            if (((action.farm != null) || (action.service_provider != null)) && (dlogout != null))
            {
                ContactJson contact = await data.web.entityGet<ContactJson>("api/p/contacts/current/");
                if ((errcode = checkExpire(contact)) == ErrCode.OK)
                {
                    if ((data.am.Maxi + data.am.MaxiSet) < data.settings.max_am_pulses)
                    {
                        bool answer = await dnotifyAnswer("Approve", string.Format("{0} current treatments available\n", data.Current) +
                                    string.Format("{0} treatments will be added\n", data.am.MaxiSet / data.settings.number_of_pulses_per_treatment) +
                                    string.Format("AM - SN: {0}\n", data.am.SNum) +
                                    ((action.farm != null) ? string.Format("Farm: {0}\n", data.farms.Find(f => f.id == action.farm).Name.val) :
                                    ((action.service_provider != null) ? string.Format("Service Provider: {0}\n", data.services.Find(s => s.id == action.service_provider).Name.val) : string.Empty)) +
                                    "Do you want to proceed?", "Yes", "No");
                        if (answer)
                        {
                            if ((data.action.PartNumber.val.ToLower() != Const.PART_NUMBER_STATUS_PURCHASE.ToLower()) &&
                                (data.action.PartNumber.val.ToLower() != Const.PART_NUMBER_STATUS_SUBSCRIPTION.ToLower()))
                            {
                                if ((errcode = await data.am.AMCmd(Cmd.WRITE)) == ErrCode.OK)
                                {
                                    if ((errcode = await data.am.AMCmd(Cmd.READ)) == ErrCode.OK)
                                    {
                                        errcode = ErrCode.OK;
                                    }
                                    else
                                        errcode = ErrCode.ESERVER;
                                }
                                else
                                    errcode = ErrCode.ESERVER;
                            }
                            else
                            {
                                data.am.MaxiPrev = data.am.Maxi;
                                errcode = ErrCode.OK;
                            }
                        }
                        else
                            errcode = ErrCode.CANSEL;
                    }
                    else
                        errcode = ErrCode.EMAX;
                }
            }
            else
                errcode = ErrCode.EPARAM;
            return errcode;
        }

        public async Task<ErrCode> responseOk(Data data, Action action)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (data.am != null) && (data.settings != null) && (dhide != null) && (dshow != null))
            {
                await dnotify("Approve Success", string.Format("The original amount of treatments: {0}\n", data.CurrentPrev) +
                        string.Format("Added treatments: {0}\n", data.am.MaxiSet / data.settings.number_of_pulses_per_treatment) +
                        string.Format("The treatments available on AM - SN {1}: {0}\n", data.Current, data.am.SNum) +
                        "please disconnect the AM", "OK");
                dhide();
                dshow();
                errcode = ErrCode.OK;
            }
            return errcode;
        }

        public async Task<List<string>> responseErr(Data data, ErrCode errcode)
        {
            List<string> errors;
            if (errcode == ErrCode.EPARAM)
                errors = new List<string>() { "Wrong parameters,", "please choose the Farm / Service provider",
                                "and the number of treatments" };
            else if (errcode == ErrCode.EMAX)
                errors = new List<string>() { "Wrong part number,", "the maximum number of treatments reached,",
                                "please choose a smaller number of treatments" };
            else if (errcode == ErrCode.ESERVER)
            {
                data.am.Maxi = data.am.MaxiPrev;
                uint maxiset = data.am.MaxiSet;
                data.am.MaxiSet = 0;
                await dnotify("Approve Failed", string.Format("Approve failed, restoring AM to {0} treatments", data.Current), "OK");
                data.dprogress(data.am.progress, true);
                if (await data.am.AMCmd(Cmd.WRITE) == ErrCode.OK)
                {
                    if (await data.am.AMCmd(Cmd.READ) == ErrCode.OK)
                        errors = new List<string>() { "Restore sucsses", string.Format("AM sucsessfully restored to {0} treatments", data.Current) };
                    else
                        errors = new List<string>() { "Restore failed", string.Format("Failed to restore to {0} treatments", data.Current) };
                }
                else
                    errors = new List<string>() { "Restore failed", string.Format("Failed to restore to {0} treatments", data.Current) };
                data.am.MaxiSet = maxiset;
            }
            else if (errcode == ErrCode.CANSEL)
                errors = null;
            else
                errors = new List<string>() { "The operation failed, the treatments were not added" };

            return errors;
        }

        public async Task<ErrCode> send(Data data)
        {
            ErrCode errcode = ErrCode.ECONNECT;
            if ((data != null) && (data.action != null) && (data.web != null))
            {
                errcode = await send<ActionJson, Action>(data.action, "api/p/actions/", data.web.entityAdd,
                    "Approve Success", "Approve Failed", false, responseOk, data, dapprove: approve, dresponseErr: responseErr);
            }
            return errcode;
        }
    }
}
