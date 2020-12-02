﻿using System;
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
        public void cabecalho(Document doc, PdfWriter writer)
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
            #region Logo Empresa

            string DiretorioImg = @"C:\Users\vitor.pinto\Desktop\Projetos\Teste Discursivas 13-11-20\Relatorio\PrototipoRelatorio\img\Logo-UniSales_Vertical.png";
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
            cell = new PdfPCell(new Phrase("Avaliação dos Coordenadores", titulo));
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
            #endregion

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
        public void corpoRelatorio()
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
               cabecalho(doc, writer);
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

    }
}