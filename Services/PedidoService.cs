using Microsoft.EntityFrameworkCore;
using PedidosLanchonete.Data;
using PedidosLanchonete.DTOs;
using PedidosLanchonete.Models;

namespace PedidosLanchonete.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> CriarPedido(PedidoDTO dto)
        {
            if (dto.Itens == null || !dto.Itens.Any())
            {
                throw new Exception(
                    "O pedido precisa ter itens."
                );
            }

            decimal valorTotal = 0;

            List<ItemPedido> itensPedido = new();

            foreach (var item in dto.Itens)
            {
                var produto =
                    await _context.Produtos
                    .FirstOrDefaultAsync(
                        p => p.Id == item.ProdutoId
                    );

                if (produto == null)
                {
                    throw new Exception(
                        $"Produto ID {item.ProdutoId} não encontrado."
                    );
                }

                decimal subtotal =
                    produto.Preco * item.Quantidade;

                valorTotal += subtotal;

                itensPedido.Add(
                    new ItemPedido
                    {
                        ProdutoId = produto.Id,

                        Quantidade = item.Quantidade,

                        Subtotal = subtotal
                    });
            }

            var pedido = new Pedido
            {
                DataPedido = DateTime.Now,

                ValorTotal = valorTotal,

                Itens = itensPedido
            };

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();

            return pedido;
        }
    }
}