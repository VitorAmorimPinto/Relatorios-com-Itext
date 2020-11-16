using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PrototipoRelatorio.BLL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Threading;
namespace PrototipoRelatorio
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Form1());*/
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        var DadosDiscursiva =   new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReportDiscursiva();
        var Professores = new GerarDadosRelatorioBLL().ListaDocenteXdocenteMasterReport();
        var DadosObjetivas = new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReport();
           foreach (var item in Professores)
           {
                int count = 0;
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
                                                            Replace('/', '-');
                
                Document doc = new Document(PageSize.A4);
                doc.SetMargins(3, 2, 3, 2);
                string NomeArquivo = item.NomeProfessor + "-" + curso + "-" + disciplina +"-" + item.IdTurma;
                string caminho = @"C:\pdf\" + NomeArquivo +".pdf";

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                
                BaseFont bf = BaseFont.CreateFont(
                       BaseFont.TIMES_ROMAN,
                       BaseFont.CP1252,
                       BaseFont.EMBEDDED);
                Font font = new Font(bf, 10);
                BaseFont bf2 = BaseFont.CreateFont(
                      BaseFont.TIMES_ROMAN,
                      BaseFont.CP1252,
                      BaseFont.EMBEDDED);
                Font font2 = new Font(bf2, 11);
                doc.Open();
                string DiretorioImg = @"C:\Users\vitor.pinto\Desktop\Projetos\Teste Discursivas 13-11-20\Relatorio\PrototipoRelatorio\img\Logo-UniSales_Vertical.png";
                Image img = Image.GetInstance(DiretorioImg);
                img.ScaleAbsolute(60,70);
                doc.Add(img);

                PdfPTable tableCabecalho = new PdfPTable(2);
                tableCabecalho.TotalWidth = 400f;
                tableCabecalho.LockedWidth = true;
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Professor", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.NomeProfessor, font)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Disciplina", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.DescricaoDisciplina, font)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Turma", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.IdTurma, font)));

                doc.Add(tableCabecalho);
                Paragraph p = new Paragraph();
                p.Add(" ");
                
                doc.Add(p);

                PdfPTable tableObjetivas = new PdfPTable(5);
                tableObjetivas.TotalWidth = 400f;
                tableObjetivas.LockedWidth = true;
                tableObjetivas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Descrição Questão", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Docente",font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Curso", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Geral", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Respondentes", font2)));
                double MediaDoc = 0;
                double MediaCurs = 0;
                double MediaGeral = 0;
                int CountQuest = 0;
                foreach (var it in DadosObjetivas)
                {
                    if ((item.IdDisciplina == it.IdDisciplina) && (item.IdProfessor == it.IdProfessor) && (item.IdTurma == it.IdTurma) && (item.IdCurso == it.IdCurso))
                    {
                        CountQuest++;
                        MediaDoc = MediaDoc + it.MediaDocente;
                        MediaCurs = MediaCurs + it.MediaQuestaoDiciplina;
                        MediaGeral = MediaGeral + it.MediaQuestao;
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(it.DescricaoQuestao, font)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(it.MediaDocente.ToString("N2"), font)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(it.MediaQuestaoDiciplina.ToString("N2"), font)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(it.MediaQuestao.ToString("N2"), font)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(it.QuantAvaliacoes.ToString(), font)));
                    }

                }
                MediaDoc = MediaDoc / CountQuest;
                MediaCurs = MediaCurs / CountQuest;
                MediaGeral = MediaGeral / CountQuest;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Total", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaDoc.ToString("N2"), font)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaCurs.ToString("N2"), font)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaGeral.ToString("N2"), font)));
                tableObjetivas.AddCell("");

                doc.Add(tableObjetivas);
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                PdfPTable tableDiscursivas = new PdfPTable(1);
                tableDiscursivas.TotalWidth = 400f;
                tableDiscursivas.LockedWidth = true;
                tableDiscursivas.AddCell(new PdfPCell(new Phrase("Respostas Discursivas", font2)));
                foreach (var itDisc in DadosDiscursiva)
                {
                    
                    if ((item.IdProfessor == itDisc.IdProfessor) && (item.IdDisciplina == itDisc.IdDisciplina) && (item.IdTurma == itDisc.IdTurma))
                    {
                        count++;
                        
                        tableDiscursivas.AddCell(new PdfPCell(new Phrase(textInfo.ToLower(itDisc.RespostaDiscursiva), font)));
                    }


                }
                if (count == 0)
                {
                    tableDiscursivas.AddCell(new PdfPCell(new Phrase("Sem respostas", font)));
                }
                doc.Add(tableDiscursivas);
                doc.Close();


           }
           
         
        }
    }
}
