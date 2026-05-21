using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PedidosLanchonete.Interfaces;
using PedidosLanchonete.Models;

namespace PedidosLanchonete.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutosController(IProdutoService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _service.Listar();

            return Ok(produtos);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            try
            {
                var novoProduto =
                    _service.Criar(produto);

                return Ok(novoProduto);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(
            int id,
            [FromBody] Produto produto)
        {
            try
            {
                var produtoAtualizado =
                    _service.Atualizar(id, produto);

                if (produtoAtualizado == null)
                {
                    return NotFound(new
                    {
                        erro = "Produto não encontrado"
                    });
                }

                return Ok(produtoAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deletado =
                    _service.Deletar(id);

                if (!deletado)
                {
                    return NotFound(new
                    {
                        erro = "Produto não encontrado"
                    });
                }

                return Ok(new
                {
                    mensagem =
                        "Produto deletado com sucesso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
        }
    }
}