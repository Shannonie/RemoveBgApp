namespace RemoveBgAPI.Services
{
    public class RemoveBgService
    {
        private readonly string apiKey = "zQ4BTKSTKfgc8oYzhXBx9HnF";
        private readonly HttpClient httpClient;

        public RemoveBgService()
        {
            httpClient = new HttpClient();
        }

        public async Task<Tuple<string, byte[]>> RemoveBackgroundAsync(string? fileName, byte[] bytes)
        {
            if (fileName == null)
                return null;

            byte[]? result = null;

            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                content.Headers.Add("X-Api-Key", apiKey);
                content.Add(new ByteArrayContent(bytes), "image_file", fileName);
                content.Add(new StringContent("auto"), "size");
                HttpResponseMessage response = httpClient.PostAsync(
                    "https://api.remove.bg/v1.0/removebg", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Console.WriteLine("Error: " + response.Content.ReadAsStringAsync().Result);
                }
            }

            return new Tuple<string, byte[]>(fileName, result);
        }
    }
}