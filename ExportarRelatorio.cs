using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using PrototipoRelatorio.BLL;
using PrototipoRelatorio.Type;
using System;
using System.IO;

namespace PrototipoRelatorio
{
    public class ExportarRelatorio
    {
        MontarRelatorio montar = new MontarRelatorio();
        object dataSourceSubReport = null;
        object dataSourceSubReportDisc = null;

        public string Relatorio(RelatorioType.TipoRelatorio tipo)
        {
            switch (tipo)
            {
                case RelatorioType.TipoRelatorio.DocentePorCurso:
                    return ExportaRelatorio(tipo);

                case RelatorioType.TipoRelatorio.ListaGerencialNotasDirecao:
                    return ExportaRelatorio(tipo);

                case RelatorioType.TipoRelatorio.GerencialNotasCursoDirecao:
                    return ExportaRelatorio(tipo);

                case RelatorioType.TipoRelatorio.GerencialNotaParaCoordenadores:
                    return "Relatorio ainda não foi desenvolvido";

                case RelatorioType.TipoRelatorio.GerencialComentários:
                    return "Relatorio ainda não foi desenvolvido";

                default:
                    return "";

            }
        }

        private string ExportaRelatorio(RelatorioType.TipoRelatorio tipo)
        {
            try
            {
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.ReportEmbeddedResource = NomeReportEmbeddedResource(tipo);

                if (tipo == RelatorioType.TipoRelatorio.DocentePorCurso)
                {
                    dataSourceSubReport =   new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReport();
                    dataSourceSubReportDisc = new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReportDiscursiva();

                    reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubRelatorioDocentePorCurso);

                    return "Relatório criados no diretorio: " + ExportarDocentePorCurso(reportViewer);
                }
                else
                {
                    string path = @"c:\RelatorioDocentes";
                    string path2 = path;

                    if (Directory.Exists(path))
                        path += @"\" + tipo.ToString() + ".pdf";
                    else
                    {
                        Directory.CreateDirectory(path);
                        path += @"\" + tipo.ToString() + ".pdf";
                    }


                    var dataSource = montar.DataSource(tipo);
                    reportViewer.LocalReport.DataSources.Add(dataSource);
                    var bytes = reportViewer.LocalReport.Render("pdf");
                    System.IO.File.WriteAllBytes(path, bytes);
                    return "Relatório criados no diretorio: " + path2;
                }               

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private void SubRelatorioDocentePorCurso(object sender, SubreportProcessingEventArgs e)
        {
            
            e.DataSources.Add(new ReportDataSource("DataSet1", dataSourceSubReport));
            e.DataSources.Add(new ReportDataSource("DataSet2", dataSourceSubReportDisc));


        }
        private string NomeReportEmbeddedResource(RelatorioType.TipoRelatorio tipo)
        {
            switch (tipo)
            {
                case RelatorioType.TipoRelatorio.DocentePorCurso:
                    return "PrototipoRelatorio.Relatorios.DocenteXdicentePorCurso.rdlc";

                case RelatorioType.TipoRelatorio.ListaGerencialNotasDirecao:
                    return "PrototipoRelatorio.Relatorios.GerencialNotasByDirecao.rdlc";

                case RelatorioType.TipoRelatorio.GerencialNotasCursoDirecao:
                    return "PrototipoRelatorio.Relatorios.GerencialNotasPorCursoDirecao.rdlc";

                case RelatorioType.TipoRelatorio.GerencialNotaParaCoordenadores:
                    return "";

                case RelatorioType.TipoRelatorio.GerencialComentários:
                    return "";

                default:
                    return "";

            }
        }

        private string ExportarDocentePorCurso(ReportViewer report)
        {
            string path = @"c:\RelatorioDiscenteXDocentes";
           
           
            var dataSource = new GerarDadosRelatorioBLL().ListaDocenteXdocenteMasterReport();
            if (!Directory.Exists(path))
            {               
                Directory.CreateDirectory(path);
            }            

            foreach (var item in dataSource)
            {
                IList<object> obj = new List<object>();
                obj.Add( new {
                    IdCurso = item.IdCurso,
                    IdProfessor = item.IdProfessor,
                    IdDisciplina = item.IdDisciplina,
                    NomeProfessor = item.NomeProfessor,
                    DescricaoDisciplina = item.DescricaoDisciplina
                });

                var curso = item.DescricaoCurso.Replace(':', ' ').
                                                Replace('|', ' ').
                                                Replace('?', ' ').
                                                Replace('<', ' ').
                                                Replace('>', ' ').
                                                Replace('*', ' ').
                                                Replace(':', ' ').
                                                Replace('“', ' ').
                                                Replace('/', '-');

                var disciplina = item.DescricaoDisciplina.Replace(':', ' ').
                                                            Replace('|', ' ').
                                                            Replace('?', ' ').
                                                            Replace('<', ' ').
                                                            Replace('>', ' ').
                                                            Replace('*', ' ').
                                                            Replace(':', ' ').
                                                            Replace('“', ' ').
                                                            Replace('/','-');

                var pathAux = path + @"\" + item.NomeProfessor + "-" + curso + "-" + disciplina + ".pdf";
                var source = new ReportDataSource(RelatorioType.TipoRelatorio.DocentePorCurso.ToString(), obj);
                report.LocalReport.DataSources.Add(source);
                var bytes = report.LocalReport.Render("pdf");
                File.WriteAllBytes(pathAux, bytes);
                report.LocalReport.DataSources.Clear();
            }

            return path;


        }
    }
}
