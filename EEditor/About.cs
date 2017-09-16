using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

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
            UsingLabel.Text = "Using: " + this.ProductVersion;
            UpdaterButton.Enabled = true;
        }

        #region Main links
        //Forum topic
        private void Forum_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forums.everybodyedits.com/viewtopic.php?id=32502");
        }

        //Wiki
        private void Wiki_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/capasha/eeditor/wiki");
        }

        //Bugs/features
        private void BugsOrFeature_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forums.everybodyedits.com/viewtopic.php?id=32502");
        }

        //Homepage
        private void HomePage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/capasha/eeditor/");
        }
        #endregion

        #region Check version
        //Check for newer version
        private void Updater_Click(object sender, EventArgs e)
        {
            thread = new Thread(() => checkVersion(false));
            thread.Start();
        }

        //Go to download page
        private void Download_Click(object sender, EventArgs e)
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
                        UsingLabel.Text = "Using: " + this.ProductVersion;
                        UpdaterButton.Enabled = false;
                    });
                }
                string text;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repos/capasha/eeditor/releases/latest");
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
                            //if (stuff1["browser_download_url"] != null) Console.WriteLine(stuff1["browser_download_url"]);
                            if (stuff1["tag_name"] != null) newversion = stuff1["tag_name"].ToString();
                            if (stuff1["html_url"] != null) downloadLink = stuff1["html_url"].ToString();
                            //Console.WriteLine("Version: " + stuff1["tag_name"] + "\nDownload Link: " + stuff1["html_url"] + "\n\nChangelog: \n" + stuff1["body"]);
                                if (stuff1["assets"] != null)
                                {
                                    foreach (var val in stuff1["assets"])
                                    {
                                        if (val["browser_download_url"] != null)
                                        {
                                            //Console.WriteLine(val["browser_download_url"]);

                                        }
                                }
                            }
                            if (button)
                            {
                                if (Convert.ToInt32(this.ProductVersion.Replace(".", "")) < Convert.ToInt32(newversion.Replace(".", "")))
                                {
                                    AboutUpdate abupdate = new AboutUpdate();
                                    if (!abupdate.IsAccessible)
                                    {
                                        abupdate.labelNewVer = "Newest: " + newversion;
                                        abupdate.labelOldVer = "Current: " + this.ProductVersion;
                                        abupdate.downloadlink = downloadLink;
                                        if (stuff1["body"] != null)
                                        {
                                            string[] split = stuff1["body"].ToString().Split('*');

                                            for (int i = 0; i < split.Length; i++)
                                            {
                                                if (split[i].Length > 1)
                                                {
                                                    var value = split[i].Substring(1, split[i].Length - 1);
                                                    if (value.StartsWith("Added"))
                                                    {
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Green;
                                                        abupdate.richtextboxChangelog.AppendText("Added: ");
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Black;
                                                        abupdate.richtextboxChangelog.AppendText(value.Replace("Added", "").Substring(1, 1).ToUpper() + value.Replace("Added", "").Substring(2, value.Replace("Added", "").Length - 2));
                                                    }
                                                    else if (value.StartsWith("Removed"))
                                                    {
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Red;
                                                        abupdate.richtextboxChangelog.AppendText("Removed: ");
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Black;
                                                        abupdate.richtextboxChangelog.AppendText(value.Replace("Removed", "").Substring(1, 1).ToUpper() + value.Replace("Removed", "").Substring(2, value.Replace("Removed", "").Length - 2));
                                                    }
                                                    else if (value.StartsWith("Fixed"))
                                                    {
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Blue;
                                                        abupdate.richtextboxChangelog.AppendText("Fixed: ");
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Black;
                                                        abupdate.richtextboxChangelog.AppendText(value.Replace("Fixed", "").Substring(1, 1).ToUpper() + value.Replace("Fixed", "").Substring(2, value.Replace("Fixed", "").Length - 2));
                                                    }
                                                    else
                                                    {
                                                        abupdate.richtextboxChangelog.SelectionColor = Color.Orange;
                                                        abupdate.richtextboxChangelog.AppendText(value);
                                                    }
                                                }
                                            }
                                        }
                                        abupdate.ShowDialog();
                                    }
                                    /*DialogResult dr = MessageBox.Show("EEditor " + newversion.ToString() + " is available! Would you like to download it now?", "Updater", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (dr == DialogResult.Yes)
                                    {
                                        Process.Start(downloadLink);
                                    }*/
                                }
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (stuff1["body"] != null)
                                    {
                                        string[] split = stuff1["body"].ToString().Split('*');
                                        NewestLabel.Text = "Newest: " + newversion;
                                        
                                        for (int i = 0; i < split.Length; i++)
                                        {
                                            if (split[i].Length > 1)
                                            {
                                                var value = split[i].Substring(1, split[i].Length - 1);
                                                if (value.StartsWith("Added"))
                                                {
                                                    ChangelogRichTextBox.SelectionColor = Color.Green;
                                                    ChangelogRichTextBox.AppendText("Added: ");
                                                    ChangelogRichTextBox.SelectionColor = Color.Black;
                                                    ChangelogRichTextBox.AppendText(value.Replace("Added", "").Substring(1, 1).ToUpper() + value.Replace("Added","").Substring(2, value.Replace("Added", "").Length - 2));
                                                }
                                                else if (value.StartsWith("Removed"))
                                                {
                                                    ChangelogRichTextBox.SelectionColor = Color.Red;
                                                    ChangelogRichTextBox.AppendText("Removed: ");
                                                    ChangelogRichTextBox.SelectionColor = Color.Black;
                                                    ChangelogRichTextBox.AppendText(value.Replace("Removed", "").Substring(1, 1).ToUpper() + value.Replace("Removed", "").Substring(2, value.Replace("Removed", "").Length - 2));
                                                }
                                                else if (value.StartsWith("Fixed"))
                                                {
                                                    ChangelogRichTextBox.SelectionColor = Color.Blue;
                                                    ChangelogRichTextBox.AppendText("Fixed: ");
                                                    ChangelogRichTextBox.SelectionColor = Color.Black;
                                                    ChangelogRichTextBox.AppendText(value.Replace("Fixed", "").Substring(1, 1).ToUpper() + value.Replace("Fixed", "").Substring(2, value.Replace("Fixed", "").Length - 2));
                                                }
                                                else
                                                {
                                                    ChangelogRichTextBox.SelectionColor = Color.Orange;
                                                    ChangelogRichTextBox.AppendText(value);
                                                }
                                            }
                                        }
                                    }
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
                                if (UpdaterButton.InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        UpdaterButton.Enabled = true;
                                    });
                                }
                                else
                                {
                                    UpdaterButton.Enabled = true;
                                }
                            }
                            else
                            {
                                if (!button)
                                {
                                    if (UpdaterButton.InvokeRequired)
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            UpdaterButton.Enabled = true;
                                        });
                                    }
                                    else
                                    {
                                        UpdaterButton.Enabled = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!button)
                            {
                                MessageBox.Show("Couldn't look after update.", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (UpdaterButton.InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        UpdaterButton.Enabled = true;
                                    });
                                }
                                else
                                {
                                    UpdaterButton.Enabled = true;
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
                            MessageBox.Show("Took too long to load the information.", "Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (UpdaterButton.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    UpdaterButton.Enabled = true;
                                });
                            }
                            else
                            {
                                UpdaterButton.Enabled = true;
                            }
                        }
                        else if (e.Status == WebExceptionStatus.ProtocolError)
                        {
                            MessageBox.Show("Error: " + ((HttpWebResponse)e.Response).StatusCode, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (UpdaterButton.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    UpdaterButton.Enabled = true;
                                });
                            }
                            else
                            {
                                UpdaterButton.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: " + e.Status, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (UpdaterButton.InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    UpdaterButton.Enabled = true;
                                });
                            }
                            else
                            {
                                UpdaterButton.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        if (e.Status == WebExceptionStatus.Timeout)
                        {
                            Thread thread = new Thread(() => checkVersion(true));
                            thread.Start();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!button)
                    {
                        MessageBox.Show("Error: " + e.Message, "Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (UpdaterButton.InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                UpdaterButton.Enabled = true;
                            });
                        }
                        else
                        {
                            UpdaterButton.Enabled = true;
                        }
                    }
                }
            }
        }
        #endregion

        private void About_FormClosed(object sender, FormClosedEventArgs e)
        {
            thread?.Abort();
        }
    }
}
