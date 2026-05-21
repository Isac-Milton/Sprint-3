using PedidosLanchonete.Interfaces;
using PedidosLanchonete.Models;

namespace PedidosLanchonete.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(
            IProdutoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Produto> Listar()
        {
            return _repository.Listar();
        }

        public Produto Criar(Produto produto)
        {
            return _repository.Criar(produto);
        }

        public Produto Atualizar(int id, Produto produto)
        {
            return _repository.Atualizar(id, produto);
        }

        public bool Deletar(int id)
        {
            return _repository.Deletar(id);
        }
    }
}