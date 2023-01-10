using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using SolrSearchSystem.Enum;
using SolrSearchSystem.Models.Enum;

namespace SolrSearchSystem.Models.SolrSearchParameters
{
    public class SolrParameters
    {
        public SolrParameters()
        {
            Parameters = new Params();
        }

        [JsonProperty("params")]
        public Params Parameters { get; set; }
    }

    public class Params
    {
        public Params()
        {
            FacetFields = new List<string>();
            FacetPivot = new List<string>();
            FilterQuery = new List<string>();
            FacetRange = new List<string>();
        }

        [JsonProperty("q")]
        public string Query { get; set; }

        [JsonProperty("facet")]
        public bool Facet { get { return true; } }

        [JsonProperty("facet.range")]
        public List<string> FacetRange { get; set; }

        [JsonProperty("facet.range.start")]
        public int FacetRangeStart { get; set; }

        [JsonProperty("facet.range.end")]
        public int FacetRangeEnd { get; set; }

        [JsonProperty("facet.pivot")]
        public List<string> FacetPivot { get; set; }

        [JsonProperty("facet.field")]
        public List<string> FacetFields { get; set; }

        [JsonProperty("fq")]
        public List<string> FilterQuery { get; set; }

        [JsonProperty("start")]
        public int Start { get { return 0; } }

        [JsonProperty("rows")]
        public int Rows { get; set; }

        [JsonProperty("sort")]
        public string Sort { get; set; }

        public Params GetQuery()
        {
            Query = "*:*";
            return this;
        }

        public Params GetRows(int rows)
        {
            Rows = rows;
            return this;
        }

        public Params GetFacetPivot()
        {
            FacetPivot.Add("Color");
            FacetPivot.Add("Brand");
            FacetPivot.Add("State");
            FacetPivot.Add("Model");

            return this;
        }

        public Params GetFacetFields()
        {
            FacetFields.Add("Condition");
            return this;
        }

        public Params GetSort(SortEnum sort)
        {
            switch (sort)
            {
                case SortEnum.PRICEHIGHERTOLOWER:
                    Sort = "PriceFor desc";
                    return this;
                case SortEnum.PRICELOWERTOHIGHER:
                    Sort = "PriceFor asc";
                    return this;
                default:
                    Sort = "score desc";
                    return this;
            }
        }

        private Params AddChildFacetPivots(string childPivots)
        {
            FacetPivot.Add(childPivots);
            return this;
        }

        private string ToTitleCase(string value) => 
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());

        public Params RemovePivotUsedToSearch(MenuEnum option)
        {
            var result = (int)option;
            var description = System.Enum.GetName(typeof(MenuEnum), result);

            if (option.Equals(MenuEnum.CONDITION))
            {
                FacetFields
                    .RemoveAll(f =>
                        f.Equals(ToTitleCase(description))
                    );
            }

            FacetPivot
                .RemoveAll(f =>
                    f.Equals(ToTitleCase(description))
                );

            return this;
        }

        public Params GetFilterQueryByType(MenuEnum type, string value)
        {
            Func<string, string, string> func = GetFilterQuery;
            Func<string, string, string> funcRange = GetFilterQueryRange;

            switch (type)
            {
                case MenuEnum.MODEL:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.MODEL)), value));
                    AddChildFacetPivots("Model,Version");
                    return this;
                case MenuEnum.STATE:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.STATE)), value));
                    AddChildFacetPivots("State,City");
                    return this;
                case MenuEnum.CITY:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.CITY)), value));
                    return this;
                case MenuEnum.VERSION:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.VERSION)), value));
                    return this;
                case MenuEnum.BRAND:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.BRAND)), value));
                    return this;
                case MenuEnum.CONDITION:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.CONDITION)), value));
                    return this;
                case MenuEnum.PRICEFOR:
                    FilterQuery.Add(funcRange(ToTitleCase(nameof(MenuEnum.PRICEFOR)), value));
                    return this;
                case MenuEnum.KM:
                    FilterQuery.Add(funcRange(ToTitleCase(nameof(MenuEnum.KM)), value));
                    return this;
                case MenuEnum.YEAR:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.YEAR)), value));
                    return this;
                case MenuEnum.COLOR:
                    FilterQuery.Add(func(ToTitleCase(nameof(MenuEnum.COLOR)), value));
                    return this;
                default:
                    FilterQuery.Add(string.Empty);
                    return this;
            }
        }

        private string GetFilterQuery(string nameDescription, string value) =>
            $"{nameDescription}:{value}";

        private string GetFilterQueryRange(string nameDescription, string value)
        {
            if (nameDescription.ToLower().Equals("pricefor"))
            {
                return value switch
                {
                    "Até R$ 55.000" => $"{nameDescription}:[0 TO 54.9]",
                    "De R$ 55.000 a R$ 90.000" => $"{nameDescription}:[55 TO 89.9]",
                    "Mais de R$ 90.000" => $"{nameDescription}:[90 TO *]",
                    _ => string.Empty,
                };
            }
            else
            {
                return value switch
                {
                    "0 Km" => $"{nameDescription}:0",
                    "0 a 50.000 Km" => $"{nameDescription}:[0 TO 49.9]",
                    "50 a 70.000 Km" => $"{nameDescription}:[50 TO 69.9]",
                    "70.000 a 100.000 Km" => $"{nameDescription}:[70 TO 100]",
                    "100.000 Km ou mais" => $"{nameDescription}:[100 TO *]",
                    _ => string.Empty,
                };
            }
        }
    }
}
