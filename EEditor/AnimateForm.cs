using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PlayerIOClient;

namespace EEditor
{
    public partial class AnimateForm : Form
    {
        List<Frame> frames;
        Thread thread;
        Client client;
        Connection conn;
        public Button buttonName {  get { return button1; } set { button1 = value; } }
        private Dictionary<string, string> data = new Dictionary<string, string>();
        public static bool crewEdit = false;
        public static bool crewWorld = false;
        public static bool editRights = false;
        public static bool saveRights = false;
        public AnimateForm(List<Frame> frames)
        {
            InitializeComponent();
            this.frames = frames;
            autoSaveCheckBox.Checked = MainForm.userdata.saveWorldCrew;
            shuffleCheckBox.Checked = MainForm.userdata.drawMixed;
            checkBoxRandom.Checked = MainForm.userdata.random;
            checkBoxReverse.Checked = MainForm.userdata.reverse;
        }

        public void UpdateStatus(object sender, StatusChangedArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate { UpdateStatus(sender, e); });
            }
            else
            {
                if (e.EpochStart == DateTime.MinValue)
                {
                    label1.Text = e.Text;
                    DateTime start = DateTime.Now;
                    
                }
                if (e.EpochStart != DateTime.MinValue)
                {
                    DateTime date1 = System.DateTime.UtcNow;
                    DateTime date2 = e.EpochStart;
                    TimeSpan dateDiff = date1.Subtract(date2);
                    TimeRunningTextBox.Text = dateDiff.ToString(@"hh\:mm\:ss");
                    TimeSpan timeleft = TimeSpan.FromTicks(DateTime.UtcNow.Subtract(e.EpochStart).Ticks * (e.totalLines - (e.CountedLines + 1)) / (e.CountedLines + 1));
                    //TimeLeftTextBox.Text = timeleft.ToString(@"hh\:mm\:ss");//TimeRunningTextBox.Text = Convert.ToDateTime((double)epoch - dt.Ticks).ToString("HH:mm:ss");
                }
                if (e.Complete)
                {

                    if (thread != null) thread.Abort();
                    if (conn != null) conn.Disconnect();
                    button1.Text = "Start";
                    uploadProgressBar.Value = 0;
                }
            }
        }
        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {

                MainForm.userdata.level = levelTextBox.Text;
                MainForm.userdata.levelPass = levelPassTextBox.Text;
                if (!levelTextBox.Text.StartsWith("OW") && !MainForm.userdata.level.StartsWith("OW") && MainForm.accs[MainForm.selectedAcc].login == "guest" && MainForm.accs[MainForm.selectedAcc].password == "guest")
                {
                    DialogResult dr = MessageBox.Show("You can't upload as a guest, please switch to another account.\nWould you like to go to account manager to add one?", "Guests can't upload", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Yes)
                    {
                        Form frm = new Accounts();
                        frm.ShowDialog();
                    }
                }
                else
                {
                    if (MainForm.userdata.level.StartsWith("OW") || levelTextBox.Text.StartsWith("OW"))
                    {
                        MainForm.userdata.level = levelTextBox.Text;
                        MainForm.userdata.levelPass = levelPassTextBox.Text;
                        button1.Text = "Stop";
                        label1.Text = "Connecting to level...";
                    }
                    else
                    {
                        MainForm.userdata.level = levelTextBox.Text;
                        MainForm.userdata.levelPass = levelPassTextBox.Text;
                        button1.Text = "Stop";
                        label1.Text = "Connecting to level...";
                    }

                    try
                    {
                        if (MainForm.accs.ContainsKey(MainForm.selectedAcc))
                        {
                            if (MainForm.accs[MainForm.selectedAcc].loginMethod == 0)
                            {
                                client = PlayerIO.QuickConnect.SimpleConnect(bdata.gameID, MainForm.accs[MainForm.selectedAcc].login, MainForm.accs[MainForm.selectedAcc].password, null);
                            }
                            else if (MainForm.accs[MainForm.selectedAcc].loginMethod == 1)
                            {
                                client = PlayerIO.QuickConnect.FacebookOAuthConnect(bdata.gameID, MainForm.accs[MainForm.selectedAcc].login, null, null);
                            }
                            else if (MainForm.accs[MainForm.selectedAcc].loginMethod == 2)
                            {
                                client = PlayerIO.QuickConnect.KongregateConnect(bdata.gameID, MainForm.accs[MainForm.selectedAcc].login, MainForm.accs[MainForm.selectedAcc].password, null);
                            }
                            else if (MainForm.accs[MainForm.selectedAcc].loginMethod == 3)
                            {
                                client = PlayerIO.Authenticate(bdata.gameID, "secure", new Dictionary<string, string> { { "userId", MainForm.accs[MainForm.selectedAcc].login }, { "authToken", MainForm.accs[MainForm.selectedAcc].password } }, null);
                            }
                            
                            conn = client.Multiplayer.CreateJoinRoom(MainForm.userdata.level.StartsWith("OW") ? MainForm.userdata.level.Replace("-"," ") : MainForm.userdata.level, MainForm.userdata.level.StartsWith("BW") ? "Beta" : "Everybodyedits" + client.BigDB.Load("config", "config")["version"], true, null, null);
                            Animator anim = new Animator(frames, conn, levelPassTextBox.Text, shuffleCheckBox.Checked,checkBoxReverse.Checked, checkBoxRandom.Checked);
                            conn.OnDisconnect += Conn_OnDisconnect;
                            Animator.pb = uploadProgressBar; //Make Animator.cs work with this form's progressbar
                            Animator.afHandle = this.Handle; //Make TaskbarProgress.cs work with this form's upload progress
                            anim.StatusChanged += new EventHandler<StatusChangedArgs>(UpdateStatus);
                            thread = new Thread(new ThreadStart(anim.Run));
                            thread.Start();
                        }
                    }
                    catch (PlayerIOError err)
                    {
                        label1.Text = "Error: " + err.Message;
                        MessageBox.Show(err.Message);
                        if (thread != null) thread.Abort();
                        if (conn != null) conn.Disconnect();
                        button1.Text = "Start";
                    }
                }
            }
            else
            {

                if (MainForm.userdata.saveWorldCrew)
                {
                    if (saveRights)
                    {
                        conn.Send("save");
                    }
                }
                label1.Text = "Level upload stopped.";
                button1.Text = "Start";
                try
                {
                    if (thread != null) thread.Abort();
                    if (conn != null) conn.Disconnect();
                }
                catch { }

            }
        }

        private void Conn_OnDisconnect(object sender, string message)
        {
        }

        private void AnimateForm_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(380, 184);
            numericUpDown2.Value = MainForm.userdata.uploadDelay;
            levelTextBox.Text = MainForm.userdata.level;
            if (MainForm.userdata.uploadOption == 0) uploadOptionButton1.Checked = true;
            else if (MainForm.userdata.uploadOption == 2) uploadOptionButton3.Checked = true;
            levelPassTextBox.Enabled = true;
            levelPassTextBox.Text = MainForm.userdata.levelPass;
            

            ToolTip tp = new ToolTip();
            tp.SetToolTip(label4, "Insert the world ID you want to upload to.");
            tp.SetToolTip(levelTextBox, "Insert the world ID you want to upload to.");

            tp.SetToolTip(label5, "Insert the code of the world you are uploading to.");
            tp.SetToolTip(levelPassTextBox, "Insert the code of the world you are uploading to.");

            tp.SetToolTip(button1, "Starts/stops uploading the world to Everybody Edits.");

            tp.SetToolTip(shuffleCheckBox, "When activated, EEditor uploads blocks in random order instead of top-to-bottom.");

            tp.SetToolTip(label2, "Sets the delay of the blocks being uploaded to Everybody Edits.\nHigher values are slower, but less laggy.");
            tp.SetToolTip(numericUpDown2, "Sets the delay of the blocks being uploaded to Everybody Edits.\nHigher values are slower, but less laggy.");

            tp.SetToolTip(autoSaveCheckBox, "Saves the Everybody Edits world automatically during the upload and after that.\nWorks only if the bot has save access (world owner/crew member).");

        }


        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            MainForm.userdata.uploadDelay = (int)numericUpDown2.Value;
        }

        private void AnimateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.userdata.level = levelTextBox.Text;
            MainForm.userdata.levelPass = levelPassTextBox.Text;
            if (conn != null) conn.Disconnect();
            if (thread != null) thread.Abort();
        }

        private void levelTextBox_TextChanged(object sender, EventArgs e)
        {
            if (levelTextBox.Text.StartsWith("PW") || levelTextBox.Text.StartsWith("BW") || levelTextBox.Text.StartsWith("OW"))
            {
                levelPassTextBox.Enabled = true;
                MainForm.userdata.level = levelTextBox.Text;
            }
        }

        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.saveWorldCrew = autoSaveCheckBox.Checked;
        }

        private void uploadOptionButton1_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.uploadOption = 0;
        }

        private void uploadOptionButton2_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.uploadOption = 1;
        }

        private void shuffleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.drawMixed = shuffleCheckBox.Checked;
            if (shuffleCheckBox.Checked)
            {
                if (checkBoxRandom.Checked) checkBoxRandom.Checked = false;
                if (checkBoxReverse.Checked) checkBoxReverse.Checked = false;
            }
        }

        private void uploadOptionButton3_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.uploadOption = 2;
        }

        private void checkBoxReverse_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.reverse = checkBoxReverse.Checked;
            if (checkBoxReverse.Checked)
            {
                if (shuffleCheckBox.Checked) shuffleCheckBox.Checked = false;
                if (checkBoxRandom.Checked) checkBoxRandom.Checked = false;
            }
        }

        private void checkBoxRandom_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.random = checkBoxRandom.Checked;
            if (checkBoxRandom.Checked)
            {
                if (shuffleCheckBox.Checked) shuffleCheckBox.Checked = false;
                if (checkBoxReverse.Checked) checkBoxReverse.Checked = false;
            }
        }

        private void levelPassTextBox_TextChanged(object sender, EventArgs e)
        {
            if (levelTextBox.Text.StartsWith("PW") || levelTextBox.Text.StartsWith("BW") || levelTextBox.Text.StartsWith("OW"))
            {
                MainForm.userdata.levelPass = levelPassTextBox.Text;
            }
        }
    }

    public class StatusChangedArgs : EventArgs
    {
        public string Text { get; private set; }
        public bool Complete { get; private set; }
        public DateTime EpochStart { get; private set; }
        public int totalLines { get; private set; }
        public int CountedLines { get; private set; }
        public StatusChangedArgs(string text,DateTime epochstart, bool complete,int totallines,int countedlines)
        {
            Text = text;
            EpochStart = epochstart;
            totalLines = totallines;
            CountedLines = countedlines;
            Complete = complete;
        }
    }
}
