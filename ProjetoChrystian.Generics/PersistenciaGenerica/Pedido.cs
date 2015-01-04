using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistenciaGenerica
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string Comprador { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public List<ItemPedido> Itens { get; set; }
    }
}
