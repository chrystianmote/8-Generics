using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;


    public class DBUtil<T>
    {
        public void Adicionar(T obj)
        {
            SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=Vendas;Integrated Security=True;Pooling=False");
            Type TipoObj = typeof(T);
            var props = TipoObj.GetProperties();

            string sql = "INSERT INTO "+ TipoObj.Name + " VALUES({0})";
            string propstext = " ";
            foreach (var prop in props)
            {
                if (prop.Name != "Id" + TipoObj.Name)
                {
                    propstext += "@" + prop.Name + ",";
                }
                
            }
            propstext = propstext.Remove(propstext.LastIndexOf(","),1);

            sql = sql.Replace("{0}", propstext);

            SqlCommand cmd = new SqlCommand(sql, con);

            foreach (var prop in props)
            {
                if (prop.Name != "Id" + TipoObj.Name)
                {
                    cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(obj, null));
                }
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }
    }

