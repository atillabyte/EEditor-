using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EEditor
{
    public partial class NumberChanger : Form
    {
        public NumericUpDown NumberChangerNumeric { get { return numericUpDown1; } set { numericUpDown1 = value; } }
        public NumberChanger()
        {
            InitializeComponent();
        }

        private void NumberChanger_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void NumberChanger_Load(object sender, EventArgs e)
        {

        }
    }
}
