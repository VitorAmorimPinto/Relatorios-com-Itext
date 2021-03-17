using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.Conexao
{
   public class Conexao
    {
        SqlConnection con = new SqlConnection();
        public Conexao()
        {
            con.ConnectionString = @"Data Source=02-FAB-02;Initial Catalog=cpa_Integrador;Integrated Security=True";
        }

        public SqlConnection Conectar()
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();

            return con;
        }

        public void FecharConexao()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
    }
}
