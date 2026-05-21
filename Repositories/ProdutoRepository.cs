using PedidosLanchonete.Data;
using PedidosLanchonete.Interfaces;
using PedidosLanchonete.Models;

namespace PedidosLanchonete.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> Listar()
        {
            return _context.Produtos.ToList();
        }

        public Produto Criar(Produto produto)
        {
            _context.Produtos.Add(produto);

            _context.SaveChanges();

            return produto;
        }

        public Produto Atualizar(int id, Produto produto)
        {
            var produtoBanco =
                _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produtoBanco == null)
            {
                return null;
            }

            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Categoria = produto.Categoria;

            _context.SaveChanges();

            return produtoBanco;
        }

        public bool Deletar(int id)
        {
            var produto =
                _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto == null)
            {
                return false;
            }

            _context.Produtos.Remove(produto);

            _context.SaveChanges();

            return true;
        }
    }
}