using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrSearchSystem.Models.SolrOperations
{
    public class SolrDeleteFields
    {
        public SolrDeleteFields()
        {
            Delete = new Delete();
        }

        [JsonProperty("delete")]
        public Delete Delete { get; set; }
    }

    public class Delete
    {
        [JsonProperty("query")]
        public string Query { get { return "*:*"; }}
    }
}
