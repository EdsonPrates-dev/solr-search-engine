using Newtonsoft.Json;

namespace SolrSearchSystem.ModelsSchema
{
    public class SolrReplaceParametersSchema
    {
        public SolrReplaceParametersSchema()
        {
            ReplaceFields = new ReplaceFields();
        }

        [JsonProperty("replace-field")]
        public ReplaceFields ReplaceFields { get; set; }
    }

    public class ReplaceFields
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("multiValued")]
        public bool MultiValued { 
            get 
            { 
                return false; 
            } 
        }

        //[JsonProperty("stored")]
        //public bool Stored
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        //[JsonProperty("docValues")]
        //public bool DocValues { get; set; }

        //[JsonProperty("indexed")]
        //public bool Indexed { get; set; }

        //caso houver a necessidade de configurar novos campos do schema, só adicionar aqui dentro 
        //do próprio modelo, métodos que fazem o preenchimento das configurações que serão aplicadas
        //No momento quero configurar somente o multivalued para que não seja um array
    }
}
