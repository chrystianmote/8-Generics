using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections;

namespace PersistenciaGenerica
{
    public class BDUtilidades<T>
    {

        public int RetonarId(object obj)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=Vendas;Integrated Security=True;Pooling=False");
            string sql = "Select scope_identity()";
            int id = 0;
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            var valor = cmd.ExecuteScalar();
            if (valor != DBNull.Value)
                id = Convert.ToInt32(valor);
            else
                id = 1;
            con.Close();
            return id;
  
        }

        public List<string> RetornarConteudoSemLista()
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=Vendas;Integrated Security=True;Pooling=False");
            Type tipo = typeof(T);
            var Propriedades = tipo.GetProperties();
            string sql = "SELECT * FROM " + tipo.Name;

            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader leitor = cmd.ExecuteReader();

            List<string> valores = new List<string>();

            if (leitor.HasRows)
            {
                while (leitor.Read())
                {
                    foreach (var pro in Propriedades)
                    {
                        valores.Add(leitor[pro.Name].ToString());
                    }
                }
            }
            return valores;
        }

        public void AdicionarConteudo(T obj)
        {

            ExecutandoSQL(obj);
            Console.WriteLine(obj.GetType().Name + " ADICIONADO NO BANCO DE DADOS");

            foreach (var p in obj.GetType().GetProperties())
            {
                if (p.PropertyType.Name == typeof(List<>).Name)
                {
                    var lista = (IList)typeof(List<>).MakeGenericType(p.PropertyType).GetConstructor(Type.EmptyTypes).Invoke(null);
                    lista = (IList)p.GetValue(obj, null);

                    foreach (var ob in lista)
                    {
                        ExecutandoSQL(ob);
                        Console.WriteLine(p.Name + " ADICIONADO NO BANCO DE DADOS");
                    }
                }
            }
        }

        private static void ExecutandoSQL(object valor)
        {

            SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=Vendas;Integrated Security=True;Pooling=False");

            Type tipo = valor.GetType();
            var Propriedades = tipo.GetProperties();
            string sql = "INSERT INTO " + tipo.Name + " ($) VALUES({0})";
            string anttext = "";
            string propstext = "";

            foreach (var pro in Propriedades)
            {
                if (tipo.Name.Contains("Item"))
                {
                    anttext += pro.Name + ",";
                    propstext += "@" + pro.Name + ",";
                }
                else if (pro.Name != "Id" + tipo.Name)
                {
                    if (pro.PropertyType.Name != typeof(List<>).Name)
                    {
                        anttext += pro.Name + ",";
                        propstext += "@" + pro.Name + ",";
                    }
                }
            }
            anttext = anttext.Remove(anttext.LastIndexOf(","), 1);
            propstext = propstext.Remove(propstext.LastIndexOf(","), 1);
            sql = sql.Replace("$", anttext);
            sql = sql.Replace("{0}", propstext);

            con.Open();
            
            SqlTransaction transaction = con.BeginTransaction();
            
            SqlCommand cmd = new SqlCommand(sql, con, transaction);

            foreach (var prop in Propriedades)
            {
                if (valor.GetType().Name.Contains("Item"))
                    cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(valor, null));
                else if (prop.Name != "Id" + tipo.Name)
                {
                    if (prop.PropertyType.Name != typeof(List<>).Name)
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(valor, null));
                }
            }

            try
            {
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n{0}", ex.Message);
                transaction.Rollback();
                Console.ReadKey();
            }
            finally
            {
                con.Close();
            }
        }

    }
}
