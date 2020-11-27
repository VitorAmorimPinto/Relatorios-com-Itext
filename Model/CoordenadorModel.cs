using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.Model
{
    public class CoordenadorModel
    {
        public int IdCurso { get; set; }
        public int QtdAvaliacoes { get; set; }
        public string NomeCoordenador{get; set; }
        public string NomeCurso { get; set; }
        public string Questao { get; set; }
        public double  MediaQuestao { get; set; }
    }
}
