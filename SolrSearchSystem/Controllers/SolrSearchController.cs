using Microsoft.AspNetCore.Mvc;
using SolrSearchSystem.Enum;
using SolrSearchSystem.Services;
using System.Threading.Tasks;

namespace SolrSearchSystem.Controllers
{
    [Route("solr")]
    [ApiController]
    public class SolrSearchController : ControllerBase
    {
        [HttpGet, Route("baseSearch")]
        public async Task<IActionResult> GetSearchSolr()
        {
            var resultFromSolr = await new SolrSchemaService().GetSearchSolr();

            return Ok(resultFromSolr);
        }

        [HttpGet, Route("searchByType/{value}/{type}")]
        public async Task<IActionResult> GetSearchSolrByType([FromRoute] string value, [FromRoute] MenuEnum type)
        {
            var resultFromSolr = await new SolrSchemaService().GetSearchSolrByType(value, type);

            return Ok(resultFromSolr);
        }
    }
}
