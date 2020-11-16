using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoRelatorio.Type
{
    public class RelatorioType
    {
        public enum TipoRelatorio
        {
            [Description("Discente X Docente por Curso")]
            DocentePorCurso = 1,
            [Description("Docente - Lista gerencial notas para direção")]
            ListaGerencialNotasDirecao = 2,
            [Description("Docente - Gerencial Notas por Curso para direção")]
            GerencialNotasCursoDirecao = 3,
            [Description("Docente - Lista gerencial Notas por Curso para coordenadores")]
            GerencialNotaParaCoordenadores = 4,
            [Description("Docente - Gerencial comentários para direção")]
            GerencialComentários = 5
        }

        public static List<KeyValuePair<T, string>> DataSourceFromEnum<T>()
        {
            List<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), value);
                MemberInfo[] mi = typeof(T).GetMember(name, BindingFlags.Public | BindingFlags.Static);
                object[] attrs = mi[0].GetCustomAttributes(typeof(DescriptionAttribute), true);
                string description =
                    attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : name;
                list.Add(new KeyValuePair<T, string>(value, description));
            }


            return list.OrderBy(x => x.Value).ToList();

        }

    }
}
