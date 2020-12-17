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

            
            MontarRelatorio corpoRelatorio = new MontarRelatorio();
            
            corpoRelatorio.RelatorioProfessores();


            //Relatórios Coordenadores
           
            
                        //Relatórios Professores
                       
                       

        }
    }
}
