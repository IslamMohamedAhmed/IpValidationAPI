namespace IpValidation.Connections
{
    public class HttpConnection 
    {
        private readonly HttpClient httpClient;

        public HttpConnection(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetDataAsync(string url)
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();


        }
    }
}
