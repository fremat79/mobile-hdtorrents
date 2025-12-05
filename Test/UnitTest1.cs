using HdTorrents.Biz.Providers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            AuthenticationProvider auth = new AuthenticationProvider();

            string user = "";
            string password = "";

            auth.Authenticate(user, password);

            auth.BuildTorrentProvider().GetTorrentsView();
        }

        [Fact]

        public async Task Search()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://hdtorrents.eu/api/quicksearch?query=alien");
            request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            request.Headers.Add("accept-language", "it,en-US;q=0.9,en;q=0.8,it-IT;q=0.7,en-GB;q=0.6");
            request.Headers.Add("cache-control", "max-age=0");
            request.Headers.Add("priority", "u=0, i");
            request.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"140\", \"Not=A?Brand\";v=\"24\", \"Microsoft Edge\";v=\"140\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("sec-fetch-dest", "document");
            request.Headers.Add("sec-fetch-mode", "navigate");
            request.Headers.Add("sec-fetch-site", "none");
            request.Headers.Add("sec-fetch-user", "?1");
            request.Headers.Add("upgrade-insecure-requests", "1");
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36 Edg/140.0.0.0");
            request.Headers.Add("Cookie", "XSRF-TOKEN=eyJpdiI6Ijd3RUp6N1lERTZmZEkxdXJ1aGNCUWc9PSIsInZhbHVlIjoiT2hPOUw3MEZvOVpGbGV3QXExWHE5T1Nic2plSElVL01CbEhYSFpVWVVWeEFMVkxOekt4ZVhyOHZreEt5ZnVnKzMvWHBYMElORG1USHpYeGZuWTc4aE4vQ25yK1hxaHNVOEpjZWtwbTdnRzFTeUJmWmtRZTlVMWJxRUw2aEhiUGEiLCJtYWMiOiIxMzFlYTkyYzlkZTYyOWJiOTk2ODRmNmM3YmFlZjQ1NTEzMGJiYzJiN2IzZDU4MTI5NWQwZjRiYjVmYTRmZDkwIiwidGFnIjoiIn0%3D; hdtorrents_session=eyJpdiI6IkVvVnY0NWR1SERCRFZTZUU1WU5yNUE9PSIsInZhbHVlIjoiNU5Cdm0wYUVZVEdhWlNZSnBRR0RLU1JXYVFXOGVZNTZLUHRsNEZqOExHOThnTUJxRmZQSVU3MnBxZ2pLYnEzbVYrcndJMGlDVkpsN1c4VlNqWktTekFqN1FvemRxdjRVTnN5QTMrc2lwcXJEaVdlTXBaVnZkNVhXQ2JXZG9rVjQiLCJtYWMiOiJhODdkZTdiM2Y5ZmQxZTIyYjUxZmU0NjdmNDFlNTYxODAyM2U2MjNmZWIwNDU3OThmN2E5MmJjNTlkYjhmODFiIiwidGFnIjoiIn0%3D; XSRF-TOKEN=eyJpdiI6IlNjV3NuMUdWR3FoM1lKbkxPZTdPalE9PSIsInZhbHVlIjoiaEpVRkhuQjRSbk5HbTFNYnl5NE53cWNTRFVEV3l5dmJ1eU9iZy9TMTEwWVBNRjJUTm04R0lRVWdGZm5LeFQyT3FkcTI5WWxvOEM3MmtZUVlibmQvUVVIakp0RVVWSFZ6dkFJMjhMUnQ1SXJaYkZxQW1Ed1lKZ3NXNFhmOE5SR1ciLCJtYWMiOiI2NDU0NTJiYWI5NzM3ZDc3MDM3MmEyNGI0NWE5ZWU0YzM5ZGU3ZTkzMDgyZGYxZWI4ZDdkNmNiMTZhYjMzMTEwIiwidGFnIjoiIn0%3D; hdtorrents_session=eyJpdiI6IjZuekZXY3VkN3IvSEtWUm5pUFE0NXc9PSIsInZhbHVlIjoiK25SSVRXRjFudnNDQTRrM1E2Q0g1SUFvVEZOUWh0VEVYOGYrbUxCQ1lKckd6dUxRSnpCQjhNTWs2SWZZSm1wZ3pTRG40NUhuYzNwUVYxZ3hMeWdLUzR6ckpGd0NtN3hPZTJEeFFjSTJWeW5sdlRTTWRHUHNObnR2bDVhTUV6UloiLCJtYWMiOiI2YzE4ODU3ZTE2ZjhjMjM1ODFlODc3ZDgyOGUwMDliYjQ1YjQxMDZjZTRiYTViMTUxYTYwZmRkNmRjMjM0MDkzIiwidGFnIjoiIn0%3D");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();


            string json = await response.Content.ReadAsStringAsync();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            SearchResults? search = System.Text.Json.JsonSerializer.Deserialize<SearchResults>(json, options);

            Debug.Assert(search.Results.Count > 0);

        }




        public class SearchResults
        {
            [JsonPropertyName("results")]
            public List<SearchResultItem> Results { get; set; } = [];
        }

        public class SearchResultItem
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("year")]
            [JsonConverter(typeof(YearJsonConverter))]
            public string? Year { get; set; }

            [JsonPropertyName("image")]
            public string? Image { get; set; }

            [JsonPropertyName("url")]
            public string? Url { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }
        }

        public class YearJsonConverter : JsonConverter<string?>
        {
            public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.TokenType switch
                {
                    JsonTokenType.String => reader.GetString(),
                    JsonTokenType.Number => reader.GetInt32().ToString(),
                    JsonTokenType.Null => null,
                    _ => throw new JsonException()
                };
            }

            public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
            {
                if (value == null)
                    writer.WriteNullValue();
                else
                    writer.WriteStringValue(value);
            }
        }
    }
}