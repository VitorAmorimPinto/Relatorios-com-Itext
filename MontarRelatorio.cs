using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using PrototipoRelatorio.BLL;
using PrototipoRelatorio.Type;


namespace PrototipoRelatorio
{
    public class MontarRelatorio
    {
        public MontarRelatorio()
        {

        }

        public ReportDataSource DataSource(RelatorioType.TipoRelatorio tipo)
        {
            return new ReportDataSource(tipo.ToString(), Dados(tipo));
            
        }

        private object Dados(RelatorioType.TipoRelatorio tipo)
        {
            var dadosRelatorio = new GerarDadosRelatorioBLL();
            switch (tipo)
            {
                case RelatorioType.TipoRelatorio.DocentePorCurso:
                    return dadosRelatorio.ListaDocenteXdocenteMasterReport();

                case RelatorioType.TipoRelatorio.ListaGerencialNotasDirecao:
                    return dadosRelatorio.ListaGerencialNotasParaDirecao();

                case RelatorioType.TipoRelatorio.GerencialNotasCursoDirecao:
                    return dadosRelatorio.ListaGerencialNotasPorCursoDirecao();

                case RelatorioType.TipoRelatorio.GerencialNotaParaCoordenadores:
                    return dadosRelatorio.ListaGerencialNotasPorCursoByCoordenador();

                case RelatorioType.TipoRelatorio.GerencialComentários:
                    return dadosRelatorio.ListaGerencialComentarioByDirecao();

                default:
                    return null;

            }

        }
    }
}
