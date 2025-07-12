using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Domain;
using backend.Application;
namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        InventarioService inventarioService;
        public InventarioController(InventarioService inventarioService)
        {
            this.inventarioService = inventarioService;
        }
        [HttpGet("Refrescos")]
        public List<Refresco> GetRefrescos()
        {
            return inventarioService.GetRefrescos();
        }
    }
}
