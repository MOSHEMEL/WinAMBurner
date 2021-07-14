using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinAMBurner
{
    class Web
    {
        private const string URL = "http://qa.armentavet.co/";
        private readonly HttpClient client = new HttpClient();
        //private string token;

        //public List<string> partNumbers;

        public async Task<LoginResponseJson> loginPost(LoginJson login)
        {
            var response = await client.PostAsync(URL + "api/p/login/",
                new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json"));
            var loginResponse = await JsonSerializer.DeserializeAsync<LoginResponseJson>(await response.Content.ReadAsStreamAsync());
            client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("JWT", loginResponse.token);
            return loginResponse;
        }

        public async Task<T> entityGet<T>(string entityUrl)
        {
            //try
            //{
            var streamTask = client.GetStreamAsync(URL + entityUrl);
            var entitiy = await JsonSerializer.DeserializeAsync<T>(await streamTask);
            //var farms1 = await JsonSerializer.DeserializeAsync<List<FarmJson>>(await streamTask);
            return entitiy;
            //return null;
            //}
            //catch { }
            //return null;
        }

        public async Task<JsonDocument> entityAdd<T>(T entity, string entityUrl)
        {
            var response = await client.PostAsync(URL + entityUrl,
                new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"));
            //var farmResponse = await JsonSerializer.DeserializeAsync<FarmResponse>(await response.Content.ReadAsStreamAsync());
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> entityEdit<T>(T entity, string entityUrl)
        {
            var response = await client.PatchAsync(URL + entityUrl,
                new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> getConstants()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, URL + "api/p/farms/"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());

            //parseConstants(jsonDocument);

            return jsonDocument;
        }

        //private void parseConstants(JsonDocument jsonDocument)
        //{
        //    JsonElement jsonElement = jsonDocument.RootElement.GetProperty("actions").GetProperty("POST");
        //    //Farm.DCOUNTRY = jsonElement.GetProperty("country").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
        //    Cnst.DCOUNTRY = convertToDic(jsonElement, "country");
        //    Cnst.COUNTRY = Cnst.DCOUNTRY.Values.ToList();
        //    //Farm.DSTATE = jsonElement.GetProperty("state").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
        //    Cnst.DSTATE = convertToDic(jsonElement, "state");
        //    Cnst.STATE = Cnst.DSTATE.Values.ToList();
        //    Farm.FARM_TYPE = convertTolist(jsonElement, "farm_type");
        //    Farm.BREED_TYPE = convertTolist(jsonElement, "breed_type");
        //    Farm.MILKING_SETUP_TYPE = convertTolist(jsonElement, "milking_setup_type");
        //    Farm.LOCATION_OF_TREATMENT_TYPE = convertTolist(jsonElement, "location_of_treatment_type");
        //    Cnst.CONTRACT_TYPE = convertTolist(jsonElement, "contract_type");
        //}

        //private Dictionary<string, string> convertToDic(JsonElement jsonElement, string key)
        //{
        //    return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
        //}

        //private List<string> convertTolist(JsonElement jsonElement, string key)
        //{
        //    return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().Select(c => c.GetProperty("value").ToString()).ToList();
        //}
    }
}
