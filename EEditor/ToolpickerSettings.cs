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
    public partial class ToolpickerSettings : Form
    {
        public bool start = false;
        public ToolpickerSettings()
        {
            InitializeComponent();
        }

        private void SettingsFGCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!start) MainForm.userdata.ColorFG = SettingsFGCheckBox.Checked;
        }

        private void SettingsBGCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!start) MainForm.userdata.ColorBG = SettingsBGCheckBox.Checked;
        }

        private void SelectColorButton_Click(object sender, EventArgs e)
        {


        }

        private void ToolpickerSettings_Load(object sender, EventArgs e)
        {
            start = true;
            SettingsFGCheckBox.Checked = MainForm.userdata.ColorFG;
            SettingsBGCheckBox.Checked = MainForm.userdata.ColorBG;
            start = false;
        }
    }
}
