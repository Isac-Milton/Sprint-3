using PedidosLanchonete.Models;

namespace PedidosLanchonete.Interfaces
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> Listar();

        Produto Criar(Produto produto);

        Produto Atualizar(int id, Produto produto);

        bool Deletar(int id);
    }
}