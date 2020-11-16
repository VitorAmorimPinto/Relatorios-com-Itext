using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.Model
{
   public class DocenteXdocenteModelSubReportModel
    {
        public int IdProfessor { get; set; }
        public string IdCurso { get; set; }
        public string IdDisciplina { get; set; }
        public string NomeProfessor { get; set; }
        public string DescricaoDisciplina { get; set; }
        public string DescricaoQuestao { get; set; }
        public double MediaQuestao { get; set; }
        public double MediaQuestaoDiciplina { get; set; }
        public double MediaDocente { get; set; }
        public string DescricaoCurso { get; set; }
        public int QuantAvaliacoes { get; set; }
        public string IdTurma { get; set; }
    }
}
