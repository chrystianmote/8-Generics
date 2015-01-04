using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplicandoGenerics
{
    class Program
    {
        static void Main(string[] args)
        {
            Categoria c = new Categoria();
            c.IdCategoria = 1;
            c.Descricao = "Categoria 1";
            DBUtil<Categoria> db = new DBUtil<Categoria>();
            db.Adicionar(c);
        }
    }
}
