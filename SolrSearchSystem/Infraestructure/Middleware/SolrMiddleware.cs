using System.Net.Http;
using System.Threading.Tasks;
using SolrSearchSystem.Models.SolrSearchReturned;
using System.Text;

namespace SolrSearchSystem.Infraestructure.Middleware
{
    public class SolrMiddleware
    {
        private const string _baseUrl = "http://localhost:8983";
        private const string _coreSolr = "carros";
        private string _reload = $"/solr/#/cores?action=RELOAD&core={_coreSolr}";
        private readonly string _uri = string.Empty;
        private const string _mediaType = "application/json";
        private readonly Encoding _encoding = Encoding.Default;

        public SolrMiddleware()
        {
            _uri = $"{_baseUrl}/solr/{_coreSolr}";
        }

        private async Task<string> GetContent(string fieldSchema, string queryString)
        {
            using var client = new HttpClient();
            var result = await client.PostAsync(_uri + $"/{queryString}", new StringContent(fieldSchema, _encoding, _mediaType));
            var content = await result.Content.ReadAsStringAsync();
            return content;
        }

        public async Task ManipuleConfigurationFromSolr(string fieldSchema, string queryString)
        {
            using var client = new HttpClient();
            var content = new StringContent(fieldSchema, _encoding, _mediaType);
            await client.PostAsync(_uri+$"/{queryString}", content);
        }

        public async Task ReloadCoreSolr()
        {
            using var client = new HttpClient();
            await client.GetAsync(_baseUrl + _reload);
        }

        public async Task<SolrParametersReturned> GetResultsFromSolr(string fieldSchema, string queryString)
        {
            var content = await GetContent(fieldSchema, queryString);
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<SolrParametersReturned>(content);

            return model;
        }
    }
}
