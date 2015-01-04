using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistenciaGenerica
{
    public class ItemPedido
    {
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
