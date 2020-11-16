using PrototipoRelatorio.Model;
using PrototipoRelatorio.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.BLL
{
   public class GerarDadosRelatorioBLL
    {
        Consultas consultas = new Consultas();
        SqlDataReader dr;
        public GerarDadosRelatorioBLL()
        {

        }
        /// <summary>
        /// Lista para montar o relatorio Discente X Docente por CURSO
        /// </summary>
        /// <returns></returns>
        public List<DocenteXdocenteModelSubReportModel> ListaDicenteXdocenteSubReport()
        {
            List<DocenteXdocenteModelSubReportModel> lista = new List<DocenteXdocenteModelSubReportModel>();         

            string query = @"SELECT Distinct  professor.nome,disciplina.nome,questao.texto,media_questao_disc_prof.media,
                            media_questao_curso.media as media_curso, 
                            questao.media as media_geral,
                            disciplina.codigo,
                            media_questao_disc_prof.id_curso, 
                            professor.id,media_questao_disc_prof.qtd_avaliacoes,media_questao_disc_prof.id_turma
                            FROM media_questao_disc_prof
                            join questao on questao.id = id_questao
                            join professor on professor.id = id_professor
                            join disciplina on disciplina.codigo = cod_disc
                            join media_questao_curso on media_questao_disc_prof.id_curso = media_questao_curso.id_curso
                            and questao.id= media_questao_curso.id_questao";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new DocenteXdocenteModelSubReportModel
                    {
                        NomeProfessor = dr.GetString(0).Trim(),
                        DescricaoDisciplina = dr.GetString(1).Trim(),
                        DescricaoQuestao = dr.GetString(2).Trim(),
                        MediaDocente = dr.GetDouble(3),
                        MediaQuestaoDiciplina = dr.GetDouble(4),
                        MediaQuestao = dr.GetDouble(5),
                        IdDisciplina = dr.GetString(6),
                        IdCurso = dr.GetString(7),
                        IdProfessor = dr.GetInt32(8),
                        QuantAvaliacoes = dr.GetInt32(9),
                        IdTurma = dr.GetString(10).Trim()
                    });
                }
                return lista;
            }
            catch(Exception e)
            {
                return null;
            }
            
        }
        public List<QuestoesDiscursivasModel> ListaDicenteXdocenteSubReportDiscursiva()
        {
            List<QuestoesDiscursivasModel> lista = new List<QuestoesDiscursivasModel>();

            string query = @"SELECT Distinct  media_questao_disc_prof.cod_disc,texto,                                            
                                            media_questao_disc_prof.id_professor,cod_turma
                                            FROM resposta_discursiva
											join media_questao_disc_prof on media_questao_disc_prof.cod_disc = resposta_discursiva.cod_disc
											and resposta_discursiva.id_prof = media_questao_disc_prof.id_professor";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new QuestoesDiscursivasModel
                    {
                        IdDisciplina = dr.GetString(0),
                        RespostaDiscursiva = dr.GetString(1).Trim(),
                        IdProfessor = dr.GetInt32(2),
                        IdTurma = dr.GetString(3).Trim()
                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<DocenteXdocenteModelSubReportModel> ListaDocenteXdocenteMasterReport()
        {
            List<DocenteXdocenteModelSubReportModel> lista = new List<DocenteXdocenteModelSubReportModel>();
                string query = @"SELECT DISTINCT professor.nome as professor, 
                                    disciplina.nome as disciplina,
                                    disciplina.codigo,
                                    media_questao_disc_prof.id_curso, 
                                    professor.id,
                                    curso.nome,media_questao_disc_prof.id_turma
                                    FROM media_questao_disc_prof
                                    JOIN questao on questao.id = id_questao
                                    JOIN professor on professor.id = id_professor
                                    JOIN disciplina on disciplina.codigo = cod_disc
                                    join curso on media_questao_disc_prof.id_curso = curso.id";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new DocenteXdocenteModelSubReportModel
                    {
                        NomeProfessor = dr.GetString(0).Trim(),
                        DescricaoDisciplina = dr.GetString(1).Trim(),                       
                        IdDisciplina = dr.GetString(2),
                        IdCurso = dr.GetString(3),
                        IdProfessor = dr.GetInt32(4),
                        DescricaoCurso = dr.GetString(5).Trim(),
                        IdTurma = dr.GetString(6).Trim()
                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        /// <summary>
        /// Lista para montar o relatorio Lista GERENCIAL NOTAS para DIREÇÃO
        /// </summary>
        /// <returns></returns>
        public List<ProfessorModel> ListaGerencialNotasParaDirecao()
        {
            string query = @"SELECT p.nome as NomeProfessor, AVG(m.media)as Media FROM professor as p
                                        INNER JOIN media_questao_disc_prof as m on m.id_professor = p.id
                                        GROUP BY p.nome
                                        ORDER BY Media DESC";

            List<ProfessorModel> lista = new List<ProfessorModel>();
            try
            {
                dr = consultas.DadosdoRelatorio(query);

                while (dr.Read())
                {
                    lista.Add(new ProfessorModel
                    {
                        Nome = dr.GetString(0),
                        Media = dr.GetDouble(1)                       

                    });
                }
                return lista;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// Lista para montar o relatorio Lista GERENCIAL NOTAS POR CURSO para DIREÇÃO
        /// </summary>
        /// <returns></returns>
        public List<QuetaoCursoModel> ListaGerencialNotasPorCursoDirecao()
        {
            int idq = 0;
            QuetaoCursoModel quetaoCursoModel = null;
            var lista = new List<QuetaoCursoModel>();
            string query = @"SELECT distinct q.id,
                                            q.texto,
                                            q.media,                                            
                                            m.media,
                                            c.id
                                    FROM questao as q 
		                            INNER JOIN media_questao_curso as m on m.id_questao = q.id
		                            INNER JOIN curso as c on c.id = m.id_curso
		                            ORDER BY  q.texto";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    if(dr.GetInt32(0) != idq)
                    {
                        idq = dr.GetInt32(0) ;
                        if (quetaoCursoModel != null)
                            lista.Add(quetaoCursoModel);

                        quetaoCursoModel = new QuetaoCursoModel();
                        quetaoCursoModel.NomeQuestao = dr.GetString(1).Trim();
                        quetaoCursoModel.MediaQuestao = dr.GetDouble(2);
                    }

                    switch (dr.GetInt32(4))
                    {
                        case 1:
                            quetaoCursoModel.Media_EFL = dr.GetDouble(3);
                            break;
                        case 2:
                            quetaoCursoModel.Media_FIS = dr.GetDouble(3);
                            break;
                        case 3:
                            quetaoCursoModel.Media_NUT = dr.GetDouble(3);
                            break;
                        case 4:
                            quetaoCursoModel.Media_SERV = dr.GetDouble(3);
                            break;
                        case 5:
                            quetaoCursoModel.Media_ENF = dr.GetDouble(3);
                            break;
                        case 6:
                            quetaoCursoModel.Media_FIL = dr.GetDouble(3);
                            break;
                        case 7:
                            quetaoCursoModel.Media_ADM = dr.GetDouble(3);
                            break;
                        case 8:
                            quetaoCursoModel.Media_CBB = dr.GetDouble(3);
                            break;
                        case 9:
                            quetaoCursoModel.Media_CBL = dr.GetDouble(3);
                            break;
                        case 10:
                            quetaoCursoModel.Media_TDS = dr.GetDouble(3);
                            break;
                        case 11:
                            quetaoCursoModel.Media_SI = dr.GetDouble(3);
                            break;
                        case 12:
                            quetaoCursoModel.Media_FAR = dr.GetDouble(3);
                            break;
                        case 13:
                            quetaoCursoModel.Media_PIS = dr.GetDouble(3);
                            break;
                        case 14:
                            quetaoCursoModel.Media_EFB = dr.GetDouble(3);
                            break;                       
                        case 16:
                            quetaoCursoModel.Media_CC = dr.GetDouble(3);
                            break;
                        case 17:
                            quetaoCursoModel.Media_ENGP = dr.GetDouble(3);
                            break;
                        case 18:
                            quetaoCursoModel.Media_AQT = dr.GetDouble(3);
                            break;
                        case 19:
                            quetaoCursoModel.Media_ENGC = dr.GetDouble(3);
                            break;
                        case 21:
                            quetaoCursoModel.Media_DIR = dr.GetDouble(3);
                            break;
                        case 23:
                            quetaoCursoModel.Media_ENGE = dr.GetDouble(3);
                            break;
                        case 27:
                            quetaoCursoModel.Media_FILB = dr.GetDouble(3);
                            break;
                        case 28:
                            quetaoCursoModel.Media_BIO = dr.GetDouble(3);
                            break;

                    }
                }

            }
            catch(Exception e)
            {

            }
            return lista;
        }
        /// <summary>
        /// Lista para montar o relatorio GERENCIAL NOTAS POR CURSO para COORDENADOR
        /// </summary>
        /// <returns></returns>
        public List<string> ListaGerencialNotasPorCursoByCoordenador()
        {
            var cursos = new List<string>();
            return cursos;
        }

        public List<string> ListaGerencialComentarioByDirecao()
        {
            return null;
        }

    }
}
