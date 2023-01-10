using Newtonsoft.Json;

namespace SolrSearchSystem.Models.SolrSearchReturned
{
    public class SolrSimpleModel
    {
        [JsonProperty("response")]
        public ResponseSolr ResponseSolr { get; set; }
    }

    public class ResponseSolr
    {
        public ResponseSolr()
        {
            Document = new SimpleDoc();
        }

        [JsonProperty("docs")]
        public SimpleDoc Document { get; set; }
    }

    public class SimpleDoc
    {
        public string CarId { get; set; }
        public string Model { get; set; }
        public string ModelId { get; set; }
        public string Brand { get; set; }
        public string BrandId { get; set; }
        public string Version { get; set; }
        public string VersionId { get; set; }
        public string Condition { get; set; }
        public float PriceFor { get; set; }
        public float Km { get; set; }
        public string Year { get; set; }
        public string YearId { get; set; }
        public string Color { get; set; }
        public string ColorId { get; set; }
        public string Image { get; set; }
    }
}
