using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidosLanchonete.DTOs;
using PedidosLanchonete.Services;

namespace PedidosLanchonete.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidosController(PedidoService service)
        {
            _service = service;
        }

        
        [HttpPost]
        public async Task<IActionResult> CriarPedido(PedidoDTO dto)
        {
            try
            {
                var pedido = await _service.CriarPedido(dto);

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
