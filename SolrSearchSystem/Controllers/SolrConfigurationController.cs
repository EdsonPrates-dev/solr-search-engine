using Microsoft.AspNetCore.Mvc;
using SolrSearchSystem.Infraestructure.Middleware;
using SolrSearchSystem.Models.SolrCreate;
using SolrSearchSystem.ModelsSchema;
using SolrSearchSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolrSearchSystem.Controllers
{

    [ApiController]
    [Route("structureOperations")]
    public class SolrConfigurationController : ControllerBase
    {
        [HttpPost, Route("changeSchema")]
        public async Task<IActionResult> ChengeConfigurationsSchema([FromBody] IEnumerable<SchemaModel> fieldsSchema)
        {
            await new SolrSchemaService().ChangeConfigurationsSchema(fieldsSchema); 

            return NoContent();
        }

        [HttpPost, Route("createCore")]
        public async Task<IActionResult> CreateCore([FromBody] IEnumerable<SolrCreateCore> core)
        {
            await new SolrSchemaService().CreateCore(core);

            return Created(string.Empty, "Core criado com sucesso! ");
        }

        [HttpPost, Route("deleteFields")]
        public async Task<IActionResult> DeleteAllFields()
        {
            await new SolrSchemaService().DeleteAllFields();

            return NoContent();
        }

        [HttpGet, Route("reload")]
        public async Task<IActionResult> ReloadCore()
        {
            await new SolrMiddleware().ReloadCoreSolr();

            return NoContent();
        }
    }
}
