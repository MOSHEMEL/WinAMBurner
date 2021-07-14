using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace WinAMBurner
{
    class Cnst
    {
        public static Dictionary<string, string> DCOUNTRY;
        public static List<string> COUNTRY;
        public static Dictionary<string, string> DSTATE;
        public static List<string> STATE;
        public static List<string> FARM_TYPE;
        public static List<string> BREED_TYPE;
        public static List<string> MILKING_SETUP_TYPE;
        public static List<string> LOCATION_OF_TREATMENT_TYPE;
        public static List<string> CONTRACT_TYPE;
        public static string IS_ACTIVE = "Yes";

        public static string getDictionary(Dictionary<string, string> dictionary, string prop)
        {
            string value = string.Empty;
            if (prop != null)
                dictionary.TryGetValue(prop, out value);

            return value;
        }

        public static void parseConstants(JsonDocument jsonDocument)
        {
            JsonElement jsonElement = jsonDocument.RootElement.GetProperty("actions").GetProperty("POST");
            //Farm.DCOUNTRY = jsonElement.GetProperty("country").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
            DCOUNTRY = convertToDic(jsonElement, "country");
            COUNTRY = Cnst.DCOUNTRY.Values.ToList();
            //Farm.DSTATE = jsonElement.GetProperty("state").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
            DSTATE = convertToDic(jsonElement, "state");
            STATE = Cnst.DSTATE.Values.ToList();
            FARM_TYPE = convertTolist(jsonElement, "farm_type");
            BREED_TYPE = convertTolist(jsonElement, "breed_type");
            MILKING_SETUP_TYPE = convertTolist(jsonElement, "milking_setup_type");
            LOCATION_OF_TREATMENT_TYPE = convertTolist(jsonElement, "location_of_treatment_type");
            CONTRACT_TYPE = convertTolist(jsonElement, "contract_type");
        }
    
        private static Dictionary<string, string> convertToDic(JsonElement jsonElement, string key)
        {
            return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
        }

        private static List<string> convertTolist(JsonElement jsonElement, string key)
        {
            return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().Select(c => c.GetProperty("value").ToString()).ToList();
        }
    }
}
