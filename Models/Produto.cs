using System.ComponentModel.DataAnnotations;

namespace PedidosLanchonete.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public string Categoria { get; set; } = string.Empty;
    }
}
