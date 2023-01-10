using Newtonsoft.Json;
using SolrSearchSystem.Enum;
using SolrSearchSystem.Extensions;
using SolrSearchSystem.Infraestructure.Middleware;
using SolrSearchSystem.Models.Enum;
using SolrSearchSystem.Models.SolrCreate;
using SolrSearchSystem.Models.SolrOperations;
using SolrSearchSystem.Models.SolrSearchParameters;
using SolrSearchSystem.Models.SolrSearchReturned;
using SolrSearchSystem.ModelsSchema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrSearchSystem.Services
{
    public class SolrSchemaService
    {
        private const string _updateParameter = "update?commit=true";
        private const string _schemaParameter = "schema";
        private const string _selectParameter = "select";
        private const int _defaultRows = 12;

        #region Fluxos que realizam buscas  

        private SolrParameters FillGeneralSolrParameters(SortEnum sort = SortEnum.SCORE, int rows = _defaultRows)
        {
            var fillParameters = new SolrParameters();

            fillParameters
                .Parameters
                .GetQuery()
                .GetFacetFields()
                .GetFacetPivot()
                .GetSort(sort)
                .GetRows(rows);

            return fillParameters;
        }

        public async Task<SolrParametersReturned> GetSearchSolrByType(string value, MenuEnum type)
        {
            var fillParameters = FillGeneralSolrParameters();

            fillParameters
                .Parameters
                .GetFilterQueryByType(type, value)
                .RemovePivotUsedToSearch(type);

            var model = SerializeGeneric(fillParameters);
            var resultSearchSolr = await new SolrMiddleware().GetResultsFromSolr(model, _selectParameter);
            var listTasks = new List<Task>
            {
                Task.Run(() =>
                {
                    resultSearchSolr = ConfigureKmReturned(resultSearchSolr);
                    resultSearchSolr = ConfigurePriceReturned(resultSearchSolr);
                })
            };

            await Task.WhenAll(listTasks);
            return resultSearchSolr;
        }

        public async Task<SolrParametersReturned> GetSearchSolr()
        {
            var fillParameters = FillGeneralSolrParameters();

            var model = SerializeGeneric(fillParameters);
            var resultSearchSolr = await new SolrMiddleware().GetResultsFromSolr(model, _selectParameter);

            return resultSearchSolr;
        }

        private Model FillNewQuantity(SolrParametersReturned solrResult, string textKm)
        {
            var model = new Model();

            if (textKm.Contains("R$"))
            {
                model = solrResult.FacetCounts.FacetPivots.PriceFor
                   .Where(r => r.Value.Equals(textKm))
                   .First();
            }
            else
            {
                model = solrResult.FacetCounts.FacetPivots.Km
                    .Where(r => r.Value.Equals(textKm))
                    .First();
            }

            model.Quantity++;

            return model;
        }

        private SolrParametersReturned ConfigureKmReturned(SolrParametersReturned solrResult)
        {
            solrResult = FillKmValueWithDefaultDescription(solrResult);

            foreach (var doc in solrResult.Response.Documents)
            {
                if (doc.Condition.ToLower() == "new")
                {
                    var newModel = FillNewQuantity(solrResult, "0 Km");
                    solrResult.FacetCounts.FacetPivots.Km
                        .Where(r => r.Value.Equals("0 Km"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.Km.IsWithin(0m, 50m))
                {
                    var newModel = FillNewQuantity(solrResult, "0 a 50.000 Km");
                    solrResult.FacetCounts.FacetPivots.Km
                        .Where(r => r.Value.Equals("0 a 50.000 Km"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.Km.IsWithin(50m, 70m))
                {
                    var newModel = FillNewQuantity(solrResult, "50.000 a 70.000 Km");
                    solrResult.FacetCounts.FacetPivots.Km
                        .Where(r => r.Value.Equals("50.000 a 70.000 Km"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.Km.IsWithin(70m, 100m))
                {
                    var newModel = FillNewQuantity(solrResult, "70.000 a 100.000 Km");
                    solrResult.FacetCounts.FacetPivots.Km
                        .Where(r => r.Value.Equals("70.000 a 100.000 Km"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.Km > 100m)
                {
                    var newModel = FillNewQuantity(solrResult, "100.000 Km ou mais");
                    solrResult.FacetCounts.FacetPivots.Km
                        .Where(r => r.Value.Equals("100.000 Km ou mais"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
            }

            return solrResult;
        }

        private SolrParametersReturned ConfigurePriceReturned(SolrParametersReturned solrResult)
        {
            solrResult = FillPriceValueWithDefaultDescription(solrResult);

            foreach (var doc in solrResult.Response.Documents)
            {
                if (doc.PriceFor.IsWithin(0m, 55m))
                {
                    var newModel = FillNewQuantity(solrResult, "Até R$ 55.000");
                    solrResult.FacetCounts.FacetPivots.PriceFor
                        .Where(r => r.Value.Equals("Até R$ 55.000"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.PriceFor.IsWithin(55m, 90m))
                {
                    var newModel = FillNewQuantity(solrResult, "De R$ 55.000 a R$ 90.000");
                    solrResult.FacetCounts.FacetPivots.PriceFor
                        .Where(r => r.Value.Equals("De R$ 55.000 a R$ 90.000"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
                else if (doc.PriceFor > 90m)
                {
                    var newModel = FillNewQuantity(solrResult, "Mais de R$ 90.000");
                    solrResult.FacetCounts.FacetPivots.PriceFor
                        .Where(r => r.Value.Equals("Mais de R$ 90.000"))
                        .Select(r => { r.Quantity = newModel.Quantity; return r; });
                }
            }

            return solrResult;
        }

        private SolrParametersReturned FillKmValueWithDefaultDescription(SolrParametersReturned returned)
        {
            returned.FacetCounts.FacetPivots.Km.Add(new Model { Value = "0 Km" });
            returned.FacetCounts.FacetPivots.Km.Add(new Model { Value = "0 a 50.000 Km" });
            returned.FacetCounts.FacetPivots.Km.Add(new Model { Value = "50.000 a 70.000 Km" });
            returned.FacetCounts.FacetPivots.Km.Add(new Model { Value = "70.000 a 100.000 Km" });
            returned.FacetCounts.FacetPivots.Km.Add(new Model { Value = "100.000 Km ou mais" });

            return returned;
        }

        private SolrParametersReturned FillPriceValueWithDefaultDescription(SolrParametersReturned returned)
        {
            returned.FacetCounts.FacetPivots.PriceFor.Add(new Model { Value = "Até R$ 55.000" });
            returned.FacetCounts.FacetPivots.PriceFor.Add(new Model { Value = "De R$ 55.000 a R$ 90.000" });
            returned.FacetCounts.FacetPivots.PriceFor.Add(new Model { Value = "Mais de R$ 90.000" });

            return returned;
        }

        #endregion

        #region Fluxos para realizar configurações no core

        public async Task ChangeConfigurationsSchema(IEnumerable<SchemaModel> fieldsSchema)
        {
            await Task.Run(() =>
            {
                fieldsSchema
                 .ToList()
                 .ForEach(item =>
                 {
                     item
                     .NamesField
                     .ForEach(async field =>
                     {
                         var mapedFields = MapFields(field, item);
                         var model = SerializeGeneric(mapedFields);
                         await new SolrMiddleware().ManipuleConfigurationFromSolr(model, _schemaParameter);
                     });
                 });
            });
        }

        public async Task CreateCore(IEnumerable<SolrCreateCore> core)
        {
            var model = SerializeGeneric(core);
            await new SolrMiddleware().ManipuleConfigurationFromSolr(model, _updateParameter);
        }

        public async Task DeleteAllFields()
        {
            var model = SerializeGeneric(new SolrDeleteFields());
            await new SolrMiddleware().ManipuleConfigurationFromSolr(model, _updateParameter);
        }

        private SolrReplaceParametersSchema MapFields(string nameField, SchemaModel schema)
        {
            var solrModel = new SolrReplaceParametersSchema();
            solrModel.ReplaceFields.Name = nameField;
            solrModel.ReplaceFields.Type = schema.Type;

            return solrModel;
        }

        public string SerializeGeneric<T>(T model) =>
            JsonConvert.SerializeObject(model);
        #endregion
    }
}
