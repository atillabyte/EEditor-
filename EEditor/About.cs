using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft;
using System.Diagnostics;
namespace EEditor
{
    public partial class About : Form
    {
        private string botexec = "EEditor";
        private int timeout = 10000;
        private string downloadLink = null;
        private HttpWebResponse response;
        MainForm Frm1;
        Thread thread;
        public About(MainForm F)
        {
            InitializeComponent();
            this.Text = "About EEditor " + this.ProductVersion;
            Frm1 = F;
        }

        private void About_Load(object sender, EventArgs e)
        {
            label3.Text = "Using: " + this.ProductVersion;

            richTextBox1.Text = "Click \"Check for updates\" to see the latest changelog here.";
            button3.Enabled = true;

        }

        #region Main links
        //Forum topic
        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forums.everybodyedits.com/viewtopic.php?id=32502");
        }

        //Wiki
        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Madis0/eeditor/wiki");
        }

        //Bugs/features
        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bitbucket.org/capasha/eeditor/issues?status=new&status=open");
        }

        //Homepage
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Madis0/eeditor/");
        }
        #endregion

        #region Check version
        //Check for newer version
        private void button3_Click(object sender, EventArgs e)
        {
            thread = new Thread(delegate () { checkVersion(false); });
            thread.Start();
        }

        //Go to download page
        private void button4_Click(object sender, EventArgs e)
        {
            if (downloadLink != null) System.Diagnostics.Process.Start(downloadLink);
        }

        public void checkVersion(bool button)
        {
            if (File.Exists(botexec + ".exe"))
            {
                if (!button)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Using: " + this.ProductVersion;
                        button3.Enabled = false;
                    });
                }
                string text;

                try
                {

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/madis0/eeditor/releases/latest");
                    request.Method = "GET";
                    request.Accept = "application/vnd.github.v3+json";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:48.0) Gecko/20100101 Firefox/48.0";

                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        if (request.HaveResponse && response != null)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                text = reader.ReadToEnd();
                            }
                            //Console.WriteLine(text);
                            string newversion = null;
                            downloadLink = null;
                            dynamic stuff1 = Newtonsoft.Json.JsonConvert.DeserializeObject(text);
                            if (stuff1["tag_name"] != null) newversion = stuff1["tag_name"].ToString();
                            if (stuff1["html_url"] != null) downloadLink = stuff1["html_url"].ToString();
                            //Console.WriteLine("Version: " + stuff1["tag_name"] + "\nDownload Link: " + stuff1["html_url"] + "\n\nChangelog: \n" + stuff1["body"]);

                            if (button)
                            {
                                if (Convert.ToInt32(this.ProductVersion.Replace(".", "")) < Convert.ToInt32(newversion.Replace(".", "")))
                                {
                                    DialogResult dr = MessageBox.Show("EEditor " + newversion.ToString() + " is available! Would you like to download it now?", "Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (dr == DialogResult.Yes)
                                    {
                                        Process.Start(downloadLink);
                                    }
                                }
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (stuff1["body"] != null) richTextBox1.Text = stuff1["body"].ToString();
                                    label4.Text = "Newest: " + newversion;
                                });
                            }

                            //Do something if version is lower or equal/higher
                            /*if (Convert.ToInt32(this.ProductVersion.Replace(".","")) < Convert.ToInt32(newversion.Replace(".",""))) 
                            {

                            }
                            else if (Convert.ToInt32(this.ProductVersion.Replace(".", "")) >= Convert.ToInt32(newversion.Replace(".", ""))) 
                            {

                            }*/
                            if (!button)
                            {
                                if (button3.InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        button3.Enabled = true;
                                    });
                                }
                                else
                                {
                                    button3.Enabled = true;
                                }
                            }

                            else
                            {
                                if (!button)
                                {
                                    if (button3.InvokeRequired)
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            button3.Enabled = true;
                                        });
                                    }
                                    else
                                    {
                                        button3.Enabled = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!button)
                            {
                                MessageBox.Show("Couldn't look after update.", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (button3.InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        button3.Enabled = true;
                                    });
                                }
                                else
                                {
                                    button3.Enabled = true;
                                }
                            }
                        }
                    }

                }
                catch (WebException e)
                {
                    if (!button)
                    {
                        if (e.Status == WebExceptionStatus.Timeout)
                        {

                            MessageBox.Show("Took too long time to load the information.", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (button3.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    button3.Enabled = true;
                                });
                            }
                            else
                            {
                                button3.Enabled = true;
                            }

                        }
                        else if (e.Status == WebExceptionStatus.ProtocolError)
                        {
                            MessageBox.Show("Error: " + ((HttpWebResponse)e.Response).StatusCode, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (button3.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    button3.Enabled = true;
                                });
                            }
                            else
                            {
                                button3.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: " + e.Status, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (button3.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    button3.Enabled = true;
                                });
                            }
                            else
                            {
                                button3.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        if (e.Status == WebExceptionStatus.Timeout)
                        {
                            Thread thread = new Thread(delegate () { checkVersion(true); });
                            thread.Start();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!button)
                    {
                        MessageBox.Show("Error: " + e.Message, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (button3.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                button3.Enabled = true;
                            });
                        }
                        else
                        {
                            button3.Enabled = true;
                        }
                    }
                }
            }
        }
        #endregion

        private void About_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null) thread.Abort();
        }

    }
}
