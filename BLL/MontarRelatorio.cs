using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace PrototipoRelatorio.BLL
{
   public class MontarRelatorio
    {
        public void cabecalho(Document doc, PdfWriter writer,int op,String curso)
        {
           //Escolha das cores
            BaseColor preto = new BaseColor(0, 0, 0);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 12, Font.BOLD, preto);
            float[] sizes = new float[] { 1f, 3f, 1f };
           //Tabela
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = doc.PageSize.Width - (doc.LeftMargin + doc.RightMargin);
            table.SetWidths(sizes);
            //Pulador de linha
            Paragraph p = new Paragraph();
            p.Add(" ");
            //Logo da Empresa
            //Diretório da imagem            
            string DiretorioImg = @"C:\Users\Vitor Amorim\Desktop\Relatorios-com-Itext\img\Logo-UniSales_Vertical.png";
            Image foot = Image.GetInstance(DiretorioImg);
            foot.ScaleAbsolute(60, 40);

            PdfPCell cell = new PdfPCell(foot);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = 0;
            cell.BorderWidthTop = 1.5f;
            cell.BorderWidthBottom = 1.5f;
            cell.PaddingTop = 10f;
            cell.PaddingBottom = 10f;
            table.AddCell(cell);

            PdfPTable micros = new PdfPTable(1);
            cell = new PdfPCell(new Phrase("Comissão Própria de Avaliação", font));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            micros.AddCell(cell);
            switch (op)
            {
                case  1 :
                    cell = new PdfPCell(new Phrase("Avaliação dos Coordenadores", titulo));
                    break;
                case 2:
                    cell = new PdfPCell(new Phrase("Avaliação dos Professores", titulo));
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    micros.AddCell(cell);
                    cell = new PdfPCell(new Phrase("1° Ciclo 2021/1", titulo));
                    break;
                case 3:
                    cell = new PdfPCell(new Phrase("Avaliação Infraestrutura", titulo));
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    micros.AddCell(cell);
                    cell = new PdfPCell(new Phrase(curso, titulo));
                    break;
                default:

                    break;
            }
           
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            micros.AddCell(cell);

            cell = new PdfPCell(micros);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.BorderWidthTop = 1.5f;
            cell.BorderWidthBottom = 1.5f;
            cell.PaddingTop = 10f;
            table.AddCell(cell);
          

            #region Página
            micros = new PdfPTable(1);
            cell = new PdfPCell(new Phrase("Página: " + (doc.PageNumber).ToString(), font));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            micros.AddCell(cell);

            cell = new PdfPCell(micros);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.BorderWidthTop = 1.5f;
            cell.BorderWidthBottom = 1.5f;
            cell.PaddingTop = 10f;
            table.AddCell(cell);
            #endregion

            table.WriteSelectedRows(0, -1, doc.LeftMargin, (doc.PageSize.Height - 10), writer.DirectContent);

        }
        public void RelatorioCoordenadores()
        {
            var Coordenadores = new GerarDadosRelatorioBLL().ListaCoordenador();
            var QuestoesCoordenadores = new GerarDadosRelatorioBLL().ListaQuestoesCoordenador();
            var DadosDiscursivas = new GerarDadosRelatorioBLL().DiscursivasCoordenadores();
            foreach (var coordenador in Coordenadores)
            {
                int count = 0;
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                Document doc = new Document(PageSize.A4);
                string NomeArquivo = coordenador.NomeCoordenador + "-" + coordenador.NomeCurso;
                string caminho = @"C:\pdf\" + NomeArquivo + ".pdf";
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();
                Paragraph p = new Paragraph();
                p.Add(" ");
                // Cabeçalho
               cabecalho(doc, writer,1, "a");
                //Corpo do Relatório
                BaseFont bf = BaseFont.CreateFont(
                            BaseFont.TIMES_ROMAN,
                            BaseFont.CP1252,
                            BaseFont.EMBEDDED);
                Font font1 = new Font(bf, 10);
                BaseFont bf2 = BaseFont.CreateFont(
                      BaseFont.TIMES_BOLD,
                      BaseFont.CP1252,
                      BaseFont.EMBEDDED);
                Font font2 = new Font(bf2, 10);

                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                PdfPTable tableCabecalho = new PdfPTable(2);
                tableCabecalho.TotalWidth = 450f;
                tableCabecalho.LockedWidth = true;
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Coordenador", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(coordenador.NomeCoordenador, font1)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Curso", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(coordenador.NomeCurso, font1)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Turma", font2)));

                doc.Add(tableCabecalho);



                doc.Add(p);

                PdfPTable tableObjetivas = new PdfPTable(3);
                tableObjetivas.TotalWidth = 450f;
                tableObjetivas.LockedWidth = true;
                tableObjetivas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Descrição Questão", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Questão", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Respondentes", font2)));


                double MediaGeral = 0;
                int CountQuest = 0;
                foreach (var QuestCoor in QuestoesCoordenadores)
                {
                    if ((coordenador.IdCurso == QuestCoor.IdCurso))
                    {
                        CountQuest++;
                        MediaGeral = MediaGeral + QuestCoor.MediaQuestao;
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestCoor.Questao, font1)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestCoor.MediaQuestao.ToString("N2"), font1)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestCoor.QtdAvaliacoes.ToString(), font1)));
                    }



                }
                MediaGeral = MediaGeral / CountQuest;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Total", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaGeral.ToString("N2"), font1)));
                tableObjetivas.AddCell("");
                doc.Add(tableObjetivas);
                
                doc.Add(p);
                PdfPTable tableDiscursivas = new PdfPTable(1);
                tableDiscursivas.TotalWidth = 450f;
                tableDiscursivas.LockedWidth = true;
                tableDiscursivas.AddCell(new PdfPCell(new Phrase("Respostas Discursivas", font2)));
                foreach (var itDisc in DadosDiscursivas )
                {

                    if (coordenador.IdCurso == itDisc.IdCurso)
                    {
                        count++;

                        tableDiscursivas.AddCell(new PdfPCell(new Phrase(textInfo.ToLower(itDisc.RespostaDiscursiva), font1)));
                    }


                }
                if (count == 0)
                {
                    tableDiscursivas.AddCell(new PdfPCell(new Phrase("Sem respostas", font1)));
                }
                doc.Add(tableDiscursivas);


                doc.Close();


            }
        }
        public void RelatorioProfessores()
        {
            var DadosDiscursiva = new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReportDiscursiva();
            var Professores = new GerarDadosRelatorioBLL().ListaDocenteXdocenteMasterReport();
            var DadosObjetivas = new GerarDadosRelatorioBLL().ListaDicenteXdocenteSubReport();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
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
                string NomeArquivo = item.NomeProfessor + "-" + curso + "-" + disciplina + "-" + item.IdTurma;
                string caminho = @"C:\pdf\" + NomeArquivo + ".pdf";

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
                Paragraph p = new Paragraph();
                p.Add(" ");

                
                //Cabeçalho relatório
                cabecalho(doc, writer, 2, "a");
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);

                PdfPTable tableCabecalho = new PdfPTable(2);
                tableCabecalho.TotalWidth = 450f;
                tableCabecalho.LockedWidth = true;
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Professor", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.NomeProfessor, font)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Disciplina", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.DescricaoDisciplina, font)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase("Turma", font2)));
                tableCabecalho.AddCell(new PdfPCell(new Phrase(item.IdTurma, font)));

                doc.Add(tableCabecalho);
                

                doc.Add(p);

                PdfPTable tableObjetivas = new PdfPTable(5);
                tableObjetivas.TotalWidth = 450f;
                tableObjetivas.LockedWidth = true;
                tableObjetivas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Descrição Questão", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Docente", font2)));
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
                tableDiscursivas.TotalWidth = 450f;
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
        public void RelatorioInfraEstrutura() 
        {
            var QuestoesInfra = new GerarDadosRelatorioBLL().ListaQuestoesInfra();
            var DadosDiscursivas = new GerarDadosRelatorioBLL().DiscursivasInfra();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            int count = 0;

            Document doc = new Document(PageSize.A4);
            doc.SetMargins(3, 2, 3, 2);
            string NomeArquivo = "Relatório Infraestrutura";
            string caminho = @"C:\pdf\" + NomeArquivo + ".pdf";

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
            Font font2 = new Font(bf2, 12);
            doc.Open();
            Paragraph p = new Paragraph();
            p.Add(" ");


            //Cabeçalho relatório
            cabecalho(doc, writer, 3,"a");
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);
            doc.Add(p);


            PdfPTable tableObjetivas = new PdfPTable(3);
            tableObjetivas.TotalWidth = 450f;
            tableObjetivas.LockedWidth = true;
            tableObjetivas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            tableObjetivas.AddCell(new PdfPCell(new Phrase("Descrição Questão", font2)));
            tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Questão", font2)));
            tableObjetivas.AddCell(new PdfPCell(new Phrase("Respondentes", font2)));


            double MediaGeral = 0;
            int CountQuest = 0;
            foreach (var QuestInfra in QuestoesInfra)
            {
                    CountQuest++;
                    MediaGeral = MediaGeral + QuestInfra.MediaQuestao;
                    tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.Questao, font)));
                    tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.MediaQuestao.ToString("N2"), font)));
                    tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.QtdAvaliacoes.ToString(), font)));

            }
            MediaGeral = MediaGeral / CountQuest;
            tableObjetivas.AddCell(new PdfPCell(new Phrase("Total", font2)));
            tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaGeral.ToString("N2"), font)));
            tableObjetivas.AddCell("");
            doc.Add(tableObjetivas);

            doc.Add(p);
            PdfPTable tableDiscursivas = new PdfPTable(1);
            tableDiscursivas.TotalWidth = 450f;
            tableDiscursivas.LockedWidth = true;
            tableDiscursivas.AddCell(new PdfPCell(new Phrase("Respostas Discursivas", font2)));
            foreach (var itDisc in DadosDiscursivas)
            {

                
                    count++;

                    tableDiscursivas.AddCell(new PdfPCell(new Phrase(textInfo.ToLower(itDisc.RespostaDiscursiva), font)));
                


            }
            if (count == 0)
            {
                tableDiscursivas.AddCell(new PdfPCell(new Phrase("Sem respostas", font)));
            }
            doc.Add(tableDiscursivas);


            doc.Close();


        }
        public void RelatorioInfraEstruturaPorCurso()
        {   

            var Cursos = new GerarDadosRelatorioBLL().ListaCursos();
            var QuestoesInfra = new GerarDadosRelatorioBLL().ListaQuestoesInfraPorCurso();
            var DadosDiscursivas = new GerarDadosRelatorioBLL().DiscursivasInfra();
            foreach (var curso in Cursos)
            {
                int count = 0;
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                Document doc = new Document(PageSize.A4);
                string NomeArquivo = "Relatório Infraestrutura" + "-" + curso.NomeCurso;
                string caminho = @"C:\pdf\" + NomeArquivo + ".pdf";
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();
                Paragraph p = new Paragraph();
                p.Add(" ");
                // Cabeçalho
                cabecalho(doc, writer, 3,curso.NomeCurso);
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                doc.Add(p);
                
                //Corpo do Relatório
                BaseFont bf = BaseFont.CreateFont(
                            BaseFont.TIMES_ROMAN,
                            BaseFont.CP1252,
                            BaseFont.EMBEDDED);
                Font font1 = new Font(bf, 10);
                BaseFont bf2 = BaseFont.CreateFont(
                      BaseFont.TIMES_BOLD,
                      BaseFont.CP1252,
                      BaseFont.EMBEDDED);
                Font font2 = new Font(bf2, 10);

               



                

                PdfPTable tableObjetivas = new PdfPTable(4);
                tableObjetivas.TotalWidth = 450f;
                tableObjetivas.LockedWidth = true;
                tableObjetivas.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Descrição Questão", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Curso", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média Geral", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Respondentes", font2)));

                double MediaQuest = 0;
                double MediaGeral = 0;
                int CountQuest = 0;
                foreach (var QuestInfra in QuestoesInfra)
                {
                    if ((curso.IdCurso == QuestInfra.IdCurso))
                    {
                        CountQuest++;
                        MediaQuest = MediaQuest + QuestInfra.MediaQuestao;
                        MediaGeral = MediaGeral + QuestInfra.MediaGeral;
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.Questao, font1)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.MediaQuestao.ToString("N2"), font1)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.MediaGeral.ToString("N2"), font1)));
                        tableObjetivas.AddCell(new PdfPCell(new Phrase(QuestInfra.QtdAvaliacoes.ToString(), font1)));
                    }



                }
                MediaGeral = MediaGeral / CountQuest;
                MediaQuest = MediaQuest / CountQuest;
                tableObjetivas.AddCell(new PdfPCell(new Phrase("Média", font2)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaQuest.ToString("N2"), font1)));
                tableObjetivas.AddCell(new PdfPCell(new Phrase(MediaGeral.ToString("N2"), font1)));
                tableObjetivas.AddCell("");
                doc.Add(tableObjetivas);

                doc.Add(p);
                PdfPTable tableDiscursivas = new PdfPTable(1);
                tableDiscursivas.TotalWidth = 450f;
                tableDiscursivas.LockedWidth = true;
                tableDiscursivas.AddCell(new PdfPCell(new Phrase("Respostas Discursivas", font2)));
                foreach (var itDisc in DadosDiscursivas)
                {

                    if (curso.IdCurso == itDisc.IdCurso)
                    {
                        count++;

                        tableDiscursivas.AddCell(new PdfPCell(new Phrase(textInfo.ToLower(itDisc.RespostaDiscursiva), font1)));
                    }


                }
                if (count == 0)
                {
                    tableDiscursivas.AddCell(new PdfPCell(new Phrase("Sem respostas", font1)));
                }
                doc.Add(tableDiscursivas);


                doc.Close();


            }
        }




    }

}

