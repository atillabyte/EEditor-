using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using PlayerIOClient;

namespace EEditor
{
    public partial class Accounts : Form
    {
        private int accountOption = 0;
        private string lastSelected = "guest";
        private string accss = "\\accounts.json";
        public static Dictionary<string, accounts> accs = new Dictionary<string, accounts>();
        public Accounts()
        {
            InitializeComponent();
        }

        private void Accounts_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, accounts> acco in MainForm.accs)
            {
                accountListBox.Items.Add(acco.Key);

                accountListBox.SelectedIndex = 0; //Select topmost account (guest) in account list and load it's data to details
                accEverybodyEdits.Checked = true; //Selects EE because guest is an EE account
                instructionsField.Rtf = everybodyEditsRTF;
            }

            #region Tooltips
            ToolTip tp = new ToolTip();

            tp.SetToolTip(addAccount, "Add an account");
            tp.SetToolTip(removeAccount, "Remove selected account");
            tp.SetToolTip(saveAccount, "Save account values");
            tp.SetToolTip(reloadPacks, "Reload the list of item packs purchased with the account");
            tp.SetToolTip(accEverybodyEdits, "Use Everybody Edits Account");
            tp.SetToolTip(accEverybodyEditsTransfer, "Use transfered Accounts");
            tp.SetToolTip(accKongregate, "Use Kongregate Account");
            tp.SetToolTip(accArmorGames, "Use ArmorGames Account");
            tp.SetToolTip(accFacebook, "Use Facebook Account");
            #endregion
        }

        private void Accounts_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.editArea.MainForm.cb.Focus();
            MainForm.editArea.MainForm.cb.Items.Clear();
            foreach (KeyValuePair<string, accounts> accd in MainForm.accs)
            {
                MainForm.editArea.MainForm.cb.Items.Add(accd.Key);
            }
            MainForm.editArea.MainForm.cb.Items.Add("---------");
            MainForm.editArea.MainForm.cb.Items.Add("Edit accounts...");
            MainForm.editArea.MainForm.cb.SelectedIndex = 0;
        }

        #region RTF tutorial
        //Everybody Edits rtf line
        private string everybodyEditsRTF = @"{\rtf1\pc Use the e-mail and password you signed up with.";

        //Kongregate rtf line
        private string kongregateRTF = @"{\rtf1\pc 1. Go to http://pastebin.com/raw/SMFAdPH9 and drag the text to bookmarks bar\par
2. Go to Everybody Edits in Kongregate and click the bookmark\par
3. Copy shown values to EEditor";

        //Armor Games rtf line
        private string armorgamesRTF = @"{\rtf1\pc 1. Go to http://pastebin.com/raw/Ars6y8GN and drag the text to bookmarks bar\par
2. Go to Everybody Edits in ArmorGames and click the bookmark\par
3. Copy shown values to EEditor";

        private void instructionsField_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // Makes RTF links work
            System.Diagnostics.Process.Start(e.LinkText);
        }
        #endregion

        #region Login method and fields
        private void radiobuttons_CheckedChanged(object sender, EventArgs e)
        {
            var value = (RadioButton)sender;
            if (value.Name == "accEverybodyEdits")
            {
                loginLabel1.Text = "Email:";
                loginField1.Width = 146;
                loginField1.Location = new Point(48, 190);
                loginField1.Visible = true;

                loginLabel2.Text = "Password:";
                loginField2.Width = 125;
                loginField2.Location = new Point(69, 219);

                instructionsField.Rtf = everybodyEditsRTF;
            }
            if (value.Name == "accEverybodyEditsTransfer")
            {
                loginLabel1.Text = "Email:";
                loginField1.Width = 146;
                loginField1.Location = new Point(48, 190);
                loginField1.Visible = true;

                loginLabel2.Text = "Password:";
                loginField2.Width = 125;
                loginField2.Location = new Point(69, 219);

                instructionsField.Rtf = everybodyEditsRTF;
            }
            /* Facebook disabled
             * else if (value.Text == "Facebook")
            {
                loginLabel1.Text = "Note: Token will expire in 1-2 hours";
                loginField1.Visible = false;

                loginLabel2.Text = "AccessToken:";
                loginField2.Width = 112;
                loginField2.Location = new Point(82, 219);

                instructionsField.Rtf = facebookRTF;
            }*/
            else if (value.Name == "accKongregate")
            {
                loginLabel1.Text = "UserID:";
                loginField1.Width = 138;
                loginField1.Location = new Point(56, 190);
                loginField1.Visible = true;

                loginLabel2.Text = "GameAuthToken:";
                loginField2.Width = 98;
                loginField2.Location = new Point(96, 219);

                instructionsField.Rtf = kongregateRTF;
            }
            else if (value.Name == "accArmorGames")
            {
                loginLabel1.Text = "UserID:";
                loginField1.Width = 138;
                loginField1.Location = new Point(56, 190);
                loginField1.Visible = true;

                loginLabel2.Text = "AuthToken:";
                loginField2.Width = 125;
                loginField2.Location = new Point(69, 219);

                instructionsField.Rtf = armorgamesRTF;
            }
        }

        private void saveAccount_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(loginField1.Text))
            {
                if (!string.IsNullOrWhiteSpace(loginField2.Text))
                {
                    if (accEverybodyEdits.Checked)
                    {
                        PlayerIO.QuickConnect.SimpleConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, successLogin, failLogin);
                        accountOption = 0;
                    }
                    
                    if (accEverybodyEditsTransfer.Checked)
                    {
                        PlayerIO.QuickConnect.SimpleConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, (Client cli) => {
                            cli.Multiplayer.CreateJoinRoom("$service-room", "AuthRoom", true, null, new Dictionary<string, string>() { { "type", "Link" } }, (Connection con) => {
                                con.OnMessage += (object sender1, PlayerIOClient.Message m) => {
                                    if (m.Type == "auth") PlayerIO.Authenticate("everybody-edits-su9rn58o40itdbnw69plyw", "linked", new Dictionary<string, string>() { { "userId", m.GetString(0) }, { "auth", m.GetString(1) } }, null, successLogin, failLogin);
                                };
                            },
                            (PlayerIOError error) => MessageBox.Show(error.Message, "Error"));
                        },
                        failLogin);
                        accountOption = 4;
                    }
                    /* Facebook disabled
                     * else if (accFacebook.Checked)
                    {
                        PlayerIO.Authenticate("everybody-edits-su9rn58o40itdbnw69plyw", "secure", new Dictionary<string, string>() { { "accessToken", loginField1.Text } }, null, successLogin, failLogin);
                        //PlayerIO.QuickConnect.FacebookOAuthConnect("everybody-edits-su9rn58o40itdbnw69plyw", loginField1.Text, null, null, successLogin, failLogin);
                        accountOption = 1;
                    }
                    */
                    else if (accKongregate.Checked)
                    {
                        PlayerIO.QuickConnect.KongregateConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, successLogin, failLogin);
                        accountOption = 2;
                    }
                    else if (accArmorGames.Checked)
                    {
                        PlayerIO.Authenticate(bdata.gameID, "secure", new Dictionary<string, string> { { "userId", loginField1.Text }, { "authToken", loginField2.Text } }, null, successLogin, failLogin);
                        accountOption = 3;
                    }
                }
                else { MessageBox.Show("Your login details isn't added", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else { MessageBox.Show("Your login details isn't added", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void reloadPacks_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(loginField1.Text))
            {
                if (!string.IsNullOrWhiteSpace(loginField2.Text))
                {
                    if (accEverybodyEdits.Checked)
                    {
                        PlayerIO.QuickConnect.SimpleConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, successLogin1, failLogin);
                    }
                    if (accEverybodyEditsTransfer.Checked)
                    {
                        PlayerIO.QuickConnect.SimpleConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, (Client cli) => {
                            cli.Multiplayer.CreateJoinRoom("$service-room", "AuthRoom", true, null, new Dictionary<string, string>() { { "type", "Link" } }, (Connection con) => {
                                con.OnMessage += (object sender1, PlayerIOClient.Message m) => {
                                    if (m.Type == "auth") PlayerIO.Authenticate("everybody-edits-su9rn58o40itdbnw69plyw", "linked", new Dictionary<string, string>() { { "userId", m.GetString(0) }, { "auth", m.GetString(1) } }, null, successLogin, failLogin);
                                };
                            },
                            (PlayerIOError error) => MessageBox.Show(error.Message, "Error"));
                        },
                        failLogin);
                        accountOption = 4;
                    }
                    /* Facebook disabled
                    else if (accFacebook.Checked)
                    {
                        PlayerIO.QuickConnect.FacebookOAuthConnect("everybody-edits-su9rn58o40itdbnw69plyw", loginField1.Text, null, null, successLogin1, failLogin);
                    }
                    */
                    else if (accKongregate.Checked)
                    {
                        PlayerIO.QuickConnect.KongregateConnect(bdata.gameID, loginField1.Text, loginField2.Text, null, successLogin1, failLogin);
                    }
                    else if (accArmorGames.Checked)
                    {
                        PlayerIO.Authenticate(bdata.gameID, "secure", new Dictionary<string, string> { { "userId", loginField1.Text }, { "authToken", loginField2.Text } }, null, successLogin1, failLogin);
                    }
                }
                else { MessageBox.Show("Your login details aren't added", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else { MessageBox.Show("Your login details aren't added", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void failLogin(PlayerIOError error)
        {
            MessageBox.Show(error.Message, "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void successLogin(Client client) //login for save
        {
            client.BigDB.LoadMyPlayerObject((DatabaseObject dbo) => {
                client.PayVault.Refresh(() => {
                    List<JToken> pv = new List<JToken>();
                    int totalpv = client.PayVault.Items.Length;
                    int incr = 0;
                    if (client.PayVault.Has("pro")) pv.Add("beta");
                    if (client.PayVault.Items.Length > 0) {
                        for (int a = 0; a < client.PayVault.Items.Length; a++) {
                            incr += 1;
                            var brick = client.PayVault.Items[a].ItemKey;
                            if (brick.StartsWith("brick")) {
                                if (!pv.Contains(brick)) pv.Add(brick);
                            }
                            this.Invoke((MethodInvoker)delegate {
                                toolStripProgressBar1.Value = Convert.ToInt32((double)incr / totalpv * 100);
                            });
                        }
                        this.Invoke((MethodInvoker)delegate {
                            MainForm.userdata.username = dbo["name"].ToString();
                            if (!MainForm.accs.ContainsKey(dbo["name"].ToString())) {
                                MainForm.accs.Add(dbo["name"].ToString(), new accounts() { login = loginField1.Text.ToLower(), password = loginField2.Text, loginMethod = accountOption, payvault = pv });
                                accountListBox.Items.Remove("(new account)");
                                accountListBox.Items.Add(dbo["name"].ToString());
                            } else if (MainForm.accs.ContainsKey(dbo["name"].ToString())) {
                                MainForm.accs[dbo["name"].ToString()] = new accounts() { login = loginField1.Text.ToLower(), password = loginField2.Text, loginMethod = accountOption, payvault = pv };
                            }
                            File.WriteAllText(Directory.GetCurrentDirectory() + accss, JsonConvert.SerializeObject(MainForm.accs, Formatting.Indented));
                            accountListBox.SelectedItem = dbo["name"].ToString();
                        });
                    } else {
                        this.Invoke((MethodInvoker)delegate {
                            MainForm.userdata.username = dbo["name"].ToString();
                            MainForm.accs.Add(dbo["name"].ToString(), new accounts() { login = loginField1.Text.ToLower(), password = loginField2.Text, loginMethod = accountOption, payvault = pv });
                            accountListBox.Items.Remove("(new account)");
                            accountListBox.Items.Add(dbo["name"].ToString());

                            File.WriteAllText(Directory.GetCurrentDirectory() + accss, JsonConvert.SerializeObject(MainForm.accs, Formatting.Indented));
                            accountListBox.SelectedItem = dbo["name"].ToString();
                            toolStripProgressBar1.Value = 100;
                        });
                    }
                },
                failLogin);
            }, failLogin);
        }

        private void successLogin1(Client client) //login for reload
        {
            if (MainForm.accs.ContainsKey(lastSelected))
            {
                client.PayVault.Refresh(() => {
                    List<JToken> pv = new List<JToken>();
                    int totalpv = client.PayVault.Items.Length;
                    int incr = 0;
                    if (client.PayVault.Has("pro")) pv.Add("beta");
                    for (int a = 0; a < client.PayVault.Items.Length; a++) {
                        incr += 1;
                        var brick = client.PayVault.Items[a].ItemKey;
                        if (brick.StartsWith("brick")) {
                            if (!pv.Contains(brick)) pv.Add(brick);
                        }
                        this.Invoke((MethodInvoker)delegate {
                            toolStripProgressBar1.Value = Convert.ToInt32((double)incr / totalpv * 100);
                        });
                    }
                    this.Invoke((MethodInvoker)delegate {
                        MainForm.accs[lastSelected].payvault = pv;
                        File.WriteAllText(Directory.GetCurrentDirectory() + accss, JsonConvert.SerializeObject(MainForm.accs, Formatting.Indented));
                    });
                },
            failLogin);
            }
        }
        #endregion

        #region Multiaccount
        private void addAccount_Click(object sender, EventArgs e)
        {
            if (!accountListBox.Items.Contains("(new account)"))
            {
                accountListBox.Items.Add("(new account)"); //Add a placeholder if doesn't exist already 
            }
            accountListBox.SelectedIndex = accountListBox.Items.Count - 1; //Select placeholder
            loginField1.Focus(); //Focus first login field
        }

        private void removeAccount_Click(object sender, EventArgs e)
        {
            if (accountListBox.SelectedIndices.Count > 0)
            {
                if (MainForm.accs.ContainsKey(lastSelected) && lastSelected == "(new account)") //Removes placeholder entry from sight
                {
                    accountListBox.Items.Remove("(new account)");
                }
                else //Remove entry and file part if it's a real account
                {
                    MainForm.accs.Remove(lastSelected);
                    accountListBox.Items.Remove(lastSelected);
                    File.WriteAllText(Directory.GetCurrentDirectory() + accss, JsonConvert.SerializeObject(MainForm.accs, Formatting.Indented));
                    //loginField1.Clear(); //no need to clear if you just select another item
                    //loginField2.Clear();
                }
            }
            accountListBox.SelectedIndex = accountListBox.Items.Count - 1; //Select last account after done removing
        }

        private void accountListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accountListBox.SelectedIndices.Count > 0)
            {
                lastSelected = accountListBox.Items[accountListBox.SelectedIndex].ToString();
                if (MainForm.accs.ContainsKey(lastSelected))
                {
                    var lm = MainForm.accs[lastSelected].loginMethod;
                    switch (lm)
                    {
                        case 0:
                            accEverybodyEdits.Checked = true;
                            break;
                        /* Facebook disabled
                         *  case 1:
                            accFacebook.Checked = true;
                            break;
                        */
                        case 2:
                            accKongregate.Checked = true;
                            break;
                        case 3:
                            accArmorGames.Checked = true;
                            break;
                        case 4:
                            accEverybodyEditsTransfer.Checked = true;
                            break;
                    }
                    loginField1.Text = MainForm.accs[lastSelected].login;
                    loginField2.Text = MainForm.accs[lastSelected].password;
                }
                else
                {
                    loginField1.Clear();
                    loginField2.Clear();
                }

                if (lastSelected == "guest") //Gray out removal button and account data when guest is selected
                {
                    addAccount.Enabled = true;
                    removeAccount.Enabled = false;
                    saveAccount.Enabled = false;
                    reloadPacks.Enabled = true;
                    groupBox2.Enabled = false;
                }
                else if (lastSelected == "(new account)") //Gray out addition button when placeholder is selected
                {
                    addAccount.Enabled = false;
                    removeAccount.Enabled = true;
                    saveAccount.Enabled = true;
                    reloadPacks.Enabled = false;
                    groupBox2.Enabled = true;
                }
                else
                {
                    addAccount.Enabled = true;
                    removeAccount.Enabled = true;
                    saveAccount.Enabled = true;
                    reloadPacks.Enabled = true;
                    groupBox2.Enabled = true;
                }
            }
        }
        #endregion
    }
}
