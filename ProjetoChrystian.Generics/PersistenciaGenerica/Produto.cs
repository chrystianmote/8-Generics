using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistenciaGenerica
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Estoque { get; set; }
        public int IdCategoria { get; set; }
    }
}
