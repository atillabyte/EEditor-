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
    public partial class AboutUpdate : Form
    {
        public string labelNewVer { get { return newVersionLabel.Text; } set { newVersionLabel.Text = value; } }
        public string labelOldVer { get { return currentVersionLabel.Text; } set { currentVersionLabel.Text = value; } }
        public string richtextboxChangelog { get { return richTextBox1.Text; } set { richTextBox1.Text = value; } }
        public AboutUpdate()
        {
            InitializeComponent();
        }

        private void AboutUpdate_Load(object sender, EventArgs e)
        {
            string[] split = richtextboxChangelog.Split('*');
            richTextBox1.Clear();
            for (int i = 0;i < split.Length;i++)
            {
                if (split[i].Length > 1)
                {
                    var value = split[i].Substring(1, split[i].Length - 1);
                    if (value.StartsWith("Added"))
                    {
                        richTextBox1.SelectionColor = Color.Green;
                        richTextBox1.AppendText("Added: ");
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(value.Replace("Added", ""));
                    }
                    else if (value.StartsWith("Removed"))
                    {
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.AppendText("Removed: ");
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(value.Replace("Removed", ""));
                    }
                    else if (value.StartsWith("Fixed"))
                    {
                        richTextBox1.SelectionColor = Color.Blue;
                        richTextBox1.AppendText("Fixed: ");
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(value.Replace("Fixed", ""));
                    }
                    
                    else
                    {
                        richTextBox1.SelectionColor = Color.Orange;
                        richTextBox1.AppendText(value);
                    }
                }
            }
            
        }
    }
}
