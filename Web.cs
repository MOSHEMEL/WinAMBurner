using System;
using System.Collections.Generic;
using System.IO;
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
        private const string TEST_URL = "http://qa.armentavet.co/";
        private const string URL = TEST_URL;

        private readonly HttpClient client = new HttpClient();

        public async Task<JsonDocument> login<T>(T login, string entityUrl)
        {
            JsonDocument jsonDocument = null;
            LoginResponseJson loginResponse = null;
            try
            {
                LogFile.logWrite("Request POST: " + URL + entityUrl + "\n" + login.ToString() + JsonSerializer.Serialize(login));
                client.DefaultRequestHeaders.Authorization = null;
                HttpResponseMessage response = await client.PostAsync(URL + "api/p/login/",
                    new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json"));
                if (response != null)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
                        if (jsonDocument != null)
                        {
                            loginResponse = JsonSerializer.Deserialize<Login>(jsonDocument.RootElement.ToString());
                            if (loginResponse != null)
                                client.DefaultRequestHeaders.Authorization
                                             = new AuthenticationHeaderValue("JWT", loginResponse.token);
                        }
                    }
                }
                //LogFile.logWrite("Reply POST:\n" + response.ReasonPhrase + "\n" + response.Content.ReadAsStringAsync().Result + "\n" +
                //    loginResponse != null ? loginResponse.ToString() : string.Empty + loginResponse != null ? JsonSerializer.Serialize(loginResponse) : string.Empty);
                LogFile.logWrite(parseReply("Reply POST:\n", jsonDocument, response));
                //if (response != null)
                //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //        loginResponse = null;
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
            }
            return jsonDocument;
        }

        public async Task<T> entityGet<T>(string entityUrl)
        {
            Task<Stream> streamTask = null;
            JsonDocument jsonDocument = null;
            T entity = default(T);
            try
            {
                LogFile.logWrite("Request GET: " + URL + entityUrl + "\n");
                streamTask = client.GetStreamAsync(URL + entityUrl);
                if(streamTask != null)
                {
                    jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await streamTask);
                    if (jsonDocument != null)
                        entity = JsonSerializer.Deserialize<T>(jsonDocument.RootElement.ToString());
                }
                LogFile.logWrite(parseReply("Reply GET:\n", jsonDocument));
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
            }
            return entity;
        }

        public async Task<JsonDocument> entityAdd<T>(T entity, string entityUrl)
        {
            JsonDocument jsonDocument = null;
            try
            {
                LogFile.logWrite("Request POST: " + URL + entityUrl + "\n" + JsonSerializer.Serialize(entity));
                HttpResponseMessage response = await client.PostAsync(URL + entityUrl,
                    new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"));
                if (response != null)
                    jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
                LogFile.logWrite(parseReply("Reply POST:\n", jsonDocument, response));
                //if (response != null)
                //    if (response.StatusCode != System.Net.HttpStatusCode.Created)
                //        jsonDocument = null;
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
            }
            return jsonDocument;
        }

        public async Task<JsonDocument> entityEdit<T>(T entity, string entityUrl)
        {
            JsonDocument jsonDocument = null;
            try
            {
                LogFile.logWrite("Request PATCH: " + URL + entityUrl + "\n" + JsonSerializer.Serialize(entity));
                HttpResponseMessage response = await client.PatchAsync(URL + entityUrl,
                    new StringContent(JsonSerializer.Serialize(entity), Encoding.UTF8, "application/json"));
                if (response != null)
                    jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
                LogFile.logWrite(parseReply("Reply PATCH:\n", jsonDocument, response));
                //if (response != null)
                //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //        jsonDocument = null;
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
            }
            return jsonDocument;
        }

        public async Task<JsonDocument> getConstants()
        {
            JsonDocument jsonDocument = null;
            try
            {
                LogFile.logWrite("Request OPTIONS: " + URL + "api/p/farms/" + "\n");
                HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Options, URL + "api/p/farms/"));
                if (response != null)
                    jsonDocument = await JsonSerializer.DeserializeAsync<JsonDocument>(await response.Content.ReadAsStreamAsync());
                LogFile.logWrite(parseReply("Reply OPTIONS:\n", jsonDocument, response));
                //if (response != null)
                //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //        jsonDocument = null;
            }
            catch (Exception e)
            {
                LogFile.logWrite(e.ToString());
            }
            return jsonDocument;
        }

        private string parseReply(string caption, JsonDocument jsonDocument, HttpResponseMessage response = null)
        {
            string logStr = caption;
            if (response != null)
                logStr += response.StatusCode + "\n" + response.Content.ReadAsStringAsync().Result + "\n";
            if (jsonDocument != null)
                logStr += jsonDocument.RootElement.ToString() + "\n";
            return logStr;
        }
    }
}
