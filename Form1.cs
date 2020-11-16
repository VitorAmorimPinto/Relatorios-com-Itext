using PrototipoRelatorio.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrototipoRelatorio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.DataSource = RelatorioType.DataSourceFromEnum<RelatorioType.TipoRelatorio>();
            this.btnGerar.Click += new System.EventHandler(this.GeraRelatorio);
        }
        public void GeraRelatorio(object sender, System.EventArgs e)
        {
            string msg = "";
            var item = this.comboBox1.SelectedValue;
            RelatorioType.TipoRelatorio tipo = (RelatorioType.TipoRelatorio)item;
           msg =  new ExportarRelatorio().Relatorio(tipo);
            MessageBox.Show(msg);

        }
    }
}
