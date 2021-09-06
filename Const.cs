using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace WinAMBurner
{
    static class Const
    {
        public const string Version = "1.0";

        public static Dictionary<string, string> DCOUNTRY;
        //public static List<string> COUNTRY;
        public static string[] COUNTRY;
        public static Dictionary<string, string> DSTATE;
        //public static List<string> STATE;
        public static string[] STATE;
        //public static List<string> FARM_TYPE;
        public static string[] FARM_TYPE;
        //public static List<string> BREED_TYPE;
        public static string[] BREED_TYPE;
        public static string[] MILKING_SETUP_TYPE;
        public static string[] LOCATION_OF_TREATMENT_TYPE;
        public static string[] CONTRACT_TYPE;
        public static string IS_ACTIVE = "Yes";

        public static string getFromDictionary(Dictionary<string, string> dictionary, string prop)
        {
            string value = string.Empty;
            if (prop != null)
                dictionary.TryGetValue(prop, out value);

            return value;
        }

        public static void parseConstants(JsonDocument jsonDocument)
        {
            JsonElement jsonElement;
            if (jsonDocument.RootElement.TryGetProperty("actions", out jsonElement))
            {
                if (jsonElement.TryGetProperty("POST", out jsonElement))
                {
                    DCOUNTRY = convertToDic(jsonElement, "country");
                    if (DCOUNTRY != null)
                        COUNTRY = DCOUNTRY.Values.ToArray();
                    DSTATE = convertToDic(jsonElement, "state");
                    if (DSTATE != null)
                        STATE = DSTATE.Values.ToArray();
                    FARM_TYPE = convertTolist(jsonElement, "farm_type");
                    BREED_TYPE = convertTolist(jsonElement, "breed_type");
                    MILKING_SETUP_TYPE = convertTolist(jsonElement, "milking_setup_type");
                    LOCATION_OF_TREATMENT_TYPE = convertTolist(jsonElement, "location_of_treatment_type");
                    CONTRACT_TYPE = convertTolist(jsonElement, "contract_type");
                }
            }
        }

        private static Dictionary<string, string> convertToDic(JsonElement jsonElement, string key)
        {
            //return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
            if (convertToArray(ref jsonElement, key))
                return jsonElement.EnumerateArray().ToDictionary(
                    c => c.TryGetProperty("value", out c) ? c.ToString() : string.Empty, 
                    c => c.TryGetProperty("display_name", out c) ? c.ToString() : string.Empty);
            return null;
        }

        private static string[] convertTolist(JsonElement jsonElement, string key)
        {
            //return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().Select(c => c.GetProperty("value").ToString()).ToArray();
            if (convertToArray(ref jsonElement, key))
                return jsonElement.EnumerateArray().Select(c => c.TryGetProperty("value", out c) ? c.ToString() : string.Empty).ToArray();
            return null;
        }

        private static bool convertToArray(ref JsonElement jsonElement, string key)
        {
            if (jsonElement.TryGetProperty(key, out jsonElement))
                if (jsonElement.TryGetProperty("choices", out jsonElement))
                    if (jsonElement.ValueKind == JsonValueKind.Array)
                        return true;
            return false;
        }

        public static string entityToString<T>(T t)
        {
            string str = t.GetType().ToString() + "\n" + "{\n";
            foreach (PropertyInfo prop in t.GetType().GetProperties())
                str += string.Format("{0}: {1}\n", prop.Name, prop.GetValue(t));
            return str + "}\n";
        }
    }

    enum ErrCode
    {
        OK = 0,
        ERROR = -1,
        EPARAM = -2,
        EMAX = -3,
        CANSEL = -4,
        EMATCH = -5,
        ELENGTH = -6,
        SERROR = -7
    }

    static class LogFile
    {
        private const string logFileName = "logFile.txt";

        public static void logWrite(List<string> cmd, string dataRdStr)
        {
            File.AppendAllText(logFileName, "------------------------------------");
            File.AppendAllText(logFileName, DateTime.Now.ToString() + "\n");
            foreach (string cm in cmd)
                File.AppendAllText(logFileName, cm.ToString() + "\n");
            File.AppendAllText(logFileName, "------------------------------------");
            File.AppendAllText(logFileName, dataRdStr);
        }

        public static void logWrite(string str)
        {
            File.AppendAllText(logFileName, "------------------------------------");
            File.AppendAllText(logFileName, DateTime.Now.ToString() + "\n");
            File.AppendAllText(logFileName, str + "\n");
        }
    }
}
