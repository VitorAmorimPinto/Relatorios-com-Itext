using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PrototipoRelatorio.Repositorio
{
    public class Consultas
    {
        Conexao.Conexao con = new Conexao.Conexao();
        SqlCommand cmd = new SqlCommand();

        /// <summary>
        /// retorna a conslta feira no banco
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlDataReader DadosdoRelatorio(string query)
        {
            cmd.CommandText = query;
            cmd.Connection = con.Conectar();

            cmd.CommandType = CommandType.Text;

            return cmd.ExecuteReader();
        }

    }
}
