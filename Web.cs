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

        public List<string> partNumbers;

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
            Farm.DCOUNTRY = jsonElement.GetProperty("country").GetProperty("choices").EnumerateArray().ToDictionary(c => c.GetProperty("value").ToString(), c => c.GetProperty("display_name").ToString());
            Farm.COUNTRY = Farm.DCOUNTRY.Values.ToList();
            Farm.FARM_TYPE = convert(jsonElement, "farm_type");
            Farm.BREED_TYPE = convert(jsonElement, "breed_type");
            Farm.MILKING_SETUP_TYPE = convert(jsonElement, "milking_setup_type");
            Farm.LOCATION_OF_TREATMENT_TYPE = convert(jsonElement, "location_of_treatment_type");
            Farm.CONTRACT_TYPE = convert(jsonElement, "contract_type");

            return jsonDocument;
        }

        private List<string> convert(JsonElement jsonElement, string key)
        {
            return jsonElement.GetProperty(key).GetProperty("choices").EnumerateArray().Select(c => c.GetProperty("value").ToString()).ToList();
        }

        public async Task<JsonDocument> treatmentPackagesGet()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, URL + "api/p/treatment_package/"));
            var jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());

            //var parts = jsonDocument.RootElement.EnumerateArray().Select(e => e.GetProperty("part_number").ToString()).ToList();
            //partNumbers = jsonDocument.RootElement.EnumerateArray().Select(e => e.GetProperty("part_number")).ToDictionary();
            //var parts = jsonDocument.RootElement.EnumerateArray().Select(e => e.GetProperty("part_number")).ToDictionary(p => p.ValueKind);
            partNumbers = jsonDocument.RootElement.EnumerateArray().Select(e => e.GetProperty("part_number").ToString()).ToList();

            return jsonDocument;
        }

    }
}
