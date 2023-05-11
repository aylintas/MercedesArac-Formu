using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MercedesAraçFormu
{
    public partial class FrmArac : Form
    {
        public FrmArac()
        {
            InitializeComponent();
        }

        public Cars cars = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyGrid1.SelectedObject = cars;
        }

        private void Tamam(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Iptal(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
