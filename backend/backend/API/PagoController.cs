using backend.Application;
using backend.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        InventarioService inventarioService;
        public PagoController(InventarioService inventarioService)
        {
            this.inventarioService = inventarioService;
        }
        [HttpPost("Compra")]
        public Recibo Facturar(Compra compra)
        {
            return inventarioService.Facturar(compra);
        }
    }
}
