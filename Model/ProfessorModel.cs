using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.Model
{
   public class ProfessorModel
    {
        public string Nome { get; set; }
        public int IdProfessor { get; set; }
        public double Media { get; set; }
        public int IdCurso { get; set; }
    }
}
