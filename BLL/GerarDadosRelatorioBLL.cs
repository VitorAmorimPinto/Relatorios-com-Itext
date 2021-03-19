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
        //Nao
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
        //Nao 
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
        public List<QuestoesDiscursivasModel> DiscursivasCoordenadores()
        {
            List<QuestoesDiscursivasModel> lista = new List<QuestoesDiscursivasModel>();

            string query = @" Select id_curso,texto from resposta_discursiva_coordenadores";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new QuestoesDiscursivasModel
                    {
                        IdCurso = dr.GetInt32(0),
                        RespostaDiscursiva = dr.GetString(1).Trim(),
                        
                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<QuestoesDiscursivasModel> DiscursivasInfra()
        {
            List<QuestoesDiscursivasModel> lista = new List<QuestoesDiscursivasModel>();

            string query = @"Select texto from resposta_discursiva_Infra";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new QuestoesDiscursivasModel
                    {
                        RespostaDiscursiva = dr.GetString(0).Trim(),

                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        //Nao
        public List<CoordenadorModel> ListaCoordenador()
        {
            List<CoordenadorModel> lista = new List<CoordenadorModel>();

            string query = @"SELECT id,nome_coordenador,nome
                             FROM curso";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new CoordenadorModel
                    {
                        IdCurso = dr.GetInt32(0),
                        NomeCoordenador = dr.GetString(1).Trim(),
                        NomeCurso = dr.GetString(2).Trim()
                        
                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        //Nao
        public List<CoordenadorModel> ListaQuestoesCoordenador()
        {
            List<CoordenadorModel> lista = new List<CoordenadorModel>();

            string query = @"select  q.texto, m.media, m.id_curso, m.qtd_avaliacoes
                                from media_questao_coord m
                                join curso c on c.id = m.id_curso
                                join questoes_coordenador q on 
                                q.id = m.id_questao";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new CoordenadorModel
                    {
                        Questao = dr.GetString(0).Trim(),
                        MediaQuestao = dr.GetDouble(1),
                        IdCurso = dr.GetInt32(2),
                        QtdAvaliacoes = dr.GetInt32(3)

                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<InfraModel> ListaQuestoesInfra()
        {
            List<InfraModel> lista = new List<InfraModel>();

            string query = @"SELECT qi.texto,mqi.media,mqi.qtd_avaliacoes FROM media_questao_infraestrutura as mqi
                             JOIN questoes_infraestrutura as qi
                             on qi.id = mqi.id_questao order by texto asc";

            try
            {
                dr = consultas.DadosdoRelatorio(query);
                while (dr.Read())
                {
                    lista.Add(new InfraModel
                    {
                        Questao = dr.GetString(0).Trim(),
                        MediaQuestao = dr.GetDouble(1),
                        QtdAvaliacoes = dr.GetInt32(2)

                    });
                }
                return lista;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        //Nao
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
    }
}
