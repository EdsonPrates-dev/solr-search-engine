using Newtonsoft.Json;
using System.Collections.Generic;

namespace SolrSearchSystem.Models.SolrSearchReturned
{
    public class SolrParametersReturned
    {
        public SolrParametersReturned()
        {
            FacetCounts = new FacetCounts();
        }

        [JsonProperty("response")]
        public Response Response { get; set; }
        [JsonProperty("facet_counts")]
        public FacetCounts FacetCounts { get; set; }
    }

    public class Response
    {
        public Response()
        {
            Documents = new List<Docs>();
        }
        [JsonProperty("docs")]
        public List<Docs> Documents { get; set; }
    }

    public class Docs
    {
        public string CarId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Model { get; set; }
        public string ModelId { get; set; }
        public string Brand { get; set; }
        public string BrandId { get; set; }
        public string Version { get; set; }
        public string VersionId { get; set; }
        public string Condition { get; set; }
        public decimal PriceFor { get; set; }
        public decimal Km { get; set; }
        public string Year { get; set; }
        public string YearId { get; set; }
        public string Color { get; set; }
        public string ColorId { get; set; }
        public string Image { get; set; }
    }

    public class FacetCounts
    {
        public FacetCounts()
        {
            FacetPivots = new FacetPivots();
            FacetFields = new FacetFields();
        }

        [JsonProperty("facet_pivot")]
        public FacetPivots FacetPivots { get; set; }

        [JsonProperty("facet_fields")]
        public FacetFields FacetFields { get; set; }

    }

    public class FacetPivots
    {
        public FacetPivots()
        {
            Km = new List<Model>();
            PriceFor = new List<Model>();
        }

        [JsonProperty("Brand")]
        public List<Model> Brand { get; set; }

        [JsonProperty("Color")]
        public List<Model> Color { get; set; }

        [JsonProperty("State,City")]
        public List<Model> City { get; set; }

        [JsonProperty("State")]
        public List<Model> State { get; set; }

        [JsonProperty("Model")]
        public List<Model> Model { get; set; }

        [JsonProperty("Model,Version")]
        public List<Model> Version { get; set; }

        [JsonProperty("PriceFor")]
        public List<Model> PriceFor { get; set; }

        [JsonProperty("Km")]
        public List<Model> Km { get; set; }

        [JsonProperty("Year")]
        public List<Model> Year { get; set; }
    }

    public class FacetFields
    {
        [JsonProperty("Condition")]
        public List<object> Condition { get; set; }
    }

    public class Model
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("count")]
        public int Quantity { get; set; }

        [JsonProperty("pivot")]
        public List<Pivot> Detail { get; set; }

    }

    public class Pivot
    {
        [JsonProperty("Field")]
        public string NameField { get; set; }

        [JsonProperty("value")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Quantity { get; set; }
    }

    //Modelos inseridos para futuras implementações
    public class FacetQueries { }
}
