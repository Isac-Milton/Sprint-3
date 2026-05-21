using PedidosLanchonete.Models;

namespace PedidosLanchonete.Interfaces
{
    public interface IProdutoService
    {
        IEnumerable<Produto> Listar();

        Produto Criar(Produto produto);

        Produto Atualizar(int id, Produto produto);

        bool Deletar(int id);
    }
}