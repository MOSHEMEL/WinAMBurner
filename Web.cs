﻿using System.Collections.Generic;
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

        public async Task<LoginResponse> loginPost(Login login)
        {
            var response = await client.PostAsync(URL + "api/p/login/",
                new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json"));
            var loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(await response.Content.ReadAsStreamAsync());
            client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("JWT", loginResponse.token);
            return loginResponse;
        }

        public async Task<List<Farm>> farmsGet()
        {
            try
            {
                var streamTask = client.GetStreamAsync(URL + "api/p/farms/");
                var farms = await JsonSerializer.DeserializeAsync<List<Farm>>(await streamTask);
                return farms;
            }
            catch { }
            return null;
        }

        public async Task<List<Service>> servicesGet()
        {
            try
            {
                var streamTask = client.GetStreamAsync(URL + "api/p/service_providers/");
                var services = await JsonSerializer.DeserializeAsync<List<Service>>(await streamTask);
                return services;
            }
            catch { }
            return null;
        }

        public async Task<JsonDocument> farmAdd(Farm farm)
        {
                var response = await client.PostAsync(URL + "api/p/farms/",
                    new StringContent(JsonSerializer.Serialize(farm as FarmJson), Encoding.UTF8, "application/json"));
            //var farmResponse = await JsonSerializer.DeserializeAsync<FarmResponse>(await response.Content.ReadAsStreamAsync());
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> farmEdit(Farm farm)
        {
            var response = await client.PatchAsync(URL + "api/p/farms/" + farm.Id + "/",
                new StringContent(JsonSerializer.Serialize(farm as FarmJson), Encoding.UTF8, "application/json"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> serviceAdd(Service service)
        {
            var response = await client.PostAsync(URL + "api /p/service_providers/",
                new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> serviceEdit(Service service)
        {
            var response = await client.PatchAsync(URL + "api/p/service_providers/" + service.Id + "/",
                new StringContent(JsonSerializer.Serialize(service), Encoding.UTF8, "application/json"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
            return jsonDocument;
        }

        public async Task<JsonDocument> farmOptions()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, URL + "api/p/farms/"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());

            JsonElement jsonElement = jsonDocument.RootElement.GetProperty("actions").GetProperty("POST");
            Dictionary<JsonElement, JsonElement> countries = jsonElement.GetProperty("country").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            Dictionary<JsonElement, JsonElement> farm_types = jsonElement.GetProperty("farm_type").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            Dictionary<JsonElement, JsonElement> breed_types = jsonElement.GetProperty("breed_type").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            Dictionary<JsonElement, JsonElement> milking_setup_types = jsonElement.GetProperty("milking_setup_type").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            Dictionary<JsonElement, JsonElement> location_of_treatment_types = jsonElement.GetProperty("location_of_treatment_type").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            Dictionary<JsonElement, JsonElement> contract_types = jsonElement.GetProperty("contract_type").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value"), c => c.GetProperty("display_name"));
            
            return jsonDocument;
        }

    }
}