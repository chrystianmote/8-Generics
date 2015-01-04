using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistenciaGenerica
{
    class Program
    {

        public static void MostrarResultado()
        {
            Console.WriteLine("Salvo no banco de dados");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.Title = "Controle de Vendas";
            Console.WriteLine("\t----------- Programa de Venda-----------");
            Console.WriteLine("\n\n\n\n");

            Produto pro;
            List<Produto> Produtos = new List<Produto>();
            BDUtilidades<Produto> BDPro = new BDUtilidades<Produto>();
            List<string> ProdutosBD = BDPro.RetornarConteudoSemLista();

            for (int i = 0; i < ProdutosBD.Count; i += 6)
            {
                pro = new Produto();
                pro.IdProduto = Convert.ToInt32(ProdutosBD[i]);
                pro.Nome = ProdutosBD[i + 1].ToString();
                pro.Preco = Convert.ToDecimal(ProdutosBD[i + 2]);
                pro.DataCadastro = Convert.ToDateTime(ProdutosBD[i + 3]);
                pro.Estoque = Convert.ToInt32(ProdutosBD[i + 4]);
                pro.IdCategoria = Convert.ToInt32(ProdutosBD[i + 5]);
                Produtos.Add(pro);
            }

            Console.Clear();
            Console.WriteLine("\tProdutos Carregados do Banco de Dados");

            string op = "";

            Pedido p = new Pedido();
            Console.Write("\tDigite o nome do Cliente: ");
            p.Comprador = Console.ReadLine();
            Console.WriteLine("\tRegistro Data: {0}", DateTime.Now);
            p.DataPedido = DateTime.Now;
            p.Itens = new List<ItemPedido>();
            BDUtilidades<Pedido> salvarpedido = new BDUtilidades<Pedido>();
            do
            {
                Console.WriteLine("\tEscolha o numero do Item: \n\n");
                foreach (Produto item in Produtos)
                {
                    Console.WriteLine("\t{0} - {1}", item.IdProduto, item.Nome);
                }
                ItemPedido ip = new ItemPedido();
                ip.IdProduto = Convert.ToInt32(Console.ReadLine());

                foreach (Produto item in Produtos)
                {
                    if (item.IdProduto == ip.IdProduto)
                    {
                        ip.ValorUnitario = item.Preco;
                        break;
                    }
                }

                ip.IdPedido = salvarpedido.RetonarId(p);
                Console.WriteLine("\tDigite Quantidade do item: ");
                ip.Quantidade = int.Parse(Console.ReadLine());
                p.Itens.Add(ip);
                Console.WriteLine("\tDeseja adicionar outro Item? S ou N");
                op = Console.ReadLine().ToUpper();
                Console.Clear();
            }
            while (op != "N");

            foreach (ItemPedido item in p.Itens)
            {
                p.ValorTotal += (item.Quantidade * item.ValorUnitario);
            }
            salvarpedido.AdicionarConteudo(p);

            Console.ReadKey();
        }


    }
}
