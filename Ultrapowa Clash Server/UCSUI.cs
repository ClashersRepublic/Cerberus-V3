using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UCS.Core;
using UCS.Core.Network;
using UCS.PacketProcessing.Messages.Server;
using System.Timers;
using UCS.Logic.AvatarStreamEntry;
using UCS.Core.Settings;

namespace UCS
{
    public partial class UCSUI : MaterialForm
    {
        public UCSUI()
        {
            InitializeComponent();
            var sm = MaterialSkinManager.Instance;
            sm.AddFormToManage(this);
            sm.Theme = MaterialSkinManager.Themes.DARK;
            sm.ColorScheme = new ColorScheme(Primary.Red800, Primary.Red900, Primary.Grey500, Accent.Red200, TextShade.WHITE);
        }

        private void UCSUI_Load(object sender, EventArgs e)
        {
            labelIP.Text = Convert.ToString(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]);
            labelPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            labelOnlinePlayers.Text = Convert.ToString(ResourcesManager.GetOnlinePlayers().Count);
            labelConnectedPlayers.Text = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
            labelMemoryPlayers.Text = Convert.ToString(ResourcesManager.GetInMemoryLevels().Count);


            /* materialLabel14.Text = Convert.ToString(ResourcesManager.GetAllPlayerIds()) + Convert.ToString(ResourcesManager.);
            materialLabel16.Text = Convert.ToString(ResourcesManager.GetAllPlayersFromDB()); */

            // CONFIG EDITOR
            txtStartingGems.Text = ConfigurationManager.AppSettings["startingGems"];
            txtStartingGold.Text = ConfigurationManager.AppSettings["startingGold"];
            txtStartingElixir.Text = ConfigurationManager.AppSettings["startingElixir"];
            txtStartingDarkElixir.Text = ConfigurationManager.AppSettings["startingDarkElixir"];
            txtStartingTrophies.Text = ConfigurationManager.AppSettings["startingTrophies"];
            txtStartingLevel.Text = ConfigurationManager.AppSettings["startingLevel"];
            txtUpdateURL.Text = ConfigurationManager.AppSettings["UpdateUrl"];
            txtUsePatch.Text = ConfigurationManager.AppSettings["useCustomPatch"];
            txtPatchURL.Text = ConfigurationManager.AppSettings["patchingServer"];
            txtMintenance.Text = ConfigurationManager.AppSettings["maintenanceTimeleft"];
            txtDatabaseType.Text = ConfigurationManager.AppSettings["databaseConnectionName"];
            txtPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            txtAdminMessage.Text = ConfigurationManager.AppSettings["AdminMessage"];
            txtLogLevel.Text = ConfigurationManager.AppSettings["LogLevel"];
            txtClientVersion.Text = ConfigurationManager.AppSettings["ClientVersion"];

            //PLAYER MANAGER
            txtPlayerName.Enabled = false;
            txtPlayerScore.Enabled = false;
            txtPlayerGems.Enabled = false;
            txtTownHallLevel.Enabled = false;
            txtAllianceID.Enabled = false;

            listView1.Items.Clear();
            foreach (var acc in ResourcesManager.GetOnlinePlayers())
            {
                ListViewItem item = new ListViewItem(acc.GetPlayerAvatar().GetAvatarName());
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetId()));
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetAvatarLevel()));
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetScore()));
                item.SubItems.Add(Convert.ToString(acc.GetAccountPrivileges()));
                listView1.Items.Add(item);
            }
            UCSControl.UpdateGuiStatus();
        }

        /* MAIN TAB */

        //Restart Button 
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            // UCSControl.UCSRestart();
            // Programs canno't simply be restarted, it need to be cleaned , fully cleaned
        }

        //Shutdown UCS Button
        private void materialRaisedButton12_Click(Object sender, EventArgs e)
        {
            Close();
        }

        //Close Button
        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Reload Button
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            labelIP.Text = Convert.ToString(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]);
            labelPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            labelOnlinePlayers.Text = Convert.ToString(ResourcesManager.GetOnlinePlayers().Count);
            labelConnectedPlayers.Text = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
            labelMemoryPlayers.Text = Convert.ToString(ResourcesManager.GetInMemoryLevels().Count);
        }

        /* END MAIN TAB */

        /* PLAYER MANAGER TAB*/

        /* PLAYER LIST TAB*/

        //Refresh Button
        private void materialRaisedButton11_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var acc in ResourcesManager.GetOnlinePlayers())
            {
                ListViewItem item = new ListViewItem(acc.GetPlayerAvatar().GetAvatarName());
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetId()));
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetAvatarLevel()));
                item.SubItems.Add(Convert.ToString(acc.GetPlayerAvatar().GetScore()));
                item.SubItems.Add(Convert.ToString(acc.GetAccountPrivileges()));
                listView1.Items.Add(item);
            }
        }
        /* END OF PLAYER LIST TAB*/

        /* END OF EDIT PLAYER TAB*/

        //Load Player Button
        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            txtPlayerName.Enabled = true;
            txtPlayerScore.Enabled = true;
            txtPlayerGems.Enabled = true;
            txtTownHallLevel.Enabled = true;
            txtAllianceID.Enabled = true;

            /* LOAD PLAYER */
            try
            {
                txtPlayerName.Text = Convert.ToString(ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetAvatarName());
                txtPlayerScore.Text = Convert.ToString(ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetScore());
                txtPlayerGems.Text = Convert.ToString(ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetDiamonds());
                txtTownHallLevel.Text = Convert.ToString(ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetTownHallLevel());
                txtAllianceID.Text = Convert.ToString(ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetAllianceId());
                materialLabel7.Text = ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().GetUserRegion();
            }
            catch (NullReferenceException)
            {
                var title = "Error";
                MessageBox.Show("Player with ID " + txtPlayerID.Text + " not found!", title, MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
            }
            /* LOAD PLAYER */
        }

        //Clear Button
        private void materialRaisedButton8_Click(object sender, EventArgs e)
        {
            /* CLEAR */
            txtPlayerName.Clear();
            txtPlayerScore.Clear();
            txtPlayerGems.Clear();
            txtTownHallLevel.Clear();
            txtAllianceID.Clear();
            txtPlayerID.Clear();
            /* CLEAR */
        }

        //Save Button
        private void materialRaisedButton7_Click(object sender, EventArgs e)
        {
            /* SAVE PLAYER */
            ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().SetName(txtPlayerName.Text);
            ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().SetScore(Convert.ToInt32(txtPlayerScore.Text));
            ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().SetDiamonds(Convert.ToInt32(txtPlayerGems.Text));
            ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().SetTownHallLevel(Convert.ToInt32(txtTownHallLevel.Text));
            ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text)).GetPlayerAvatar().SetAllianceId(Convert.ToInt32(txtAllianceID.Text));

            var title = "Finished!";
            MessageBox.Show("Player has been saved!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            /* SAVE PLAYER */
        }

        /* END OF EDIT PLAYER TAB*/

        /* END OF PLAYER MANAGER TAB*/

        /*CONFIG EDITOR TAB*/

        //Reset Button
        private void materialRaisedButton5_Click(object sender, EventArgs e)
        {
            txtStartingGems.Text = "999999999";
            txtStartingGold.Text = "999999999";
            txtStartingElixir.Text = "999999999";
            txtStartingDarkElixir.Text = "999999999";
            txtStartingTrophies.Text = "0";
            txtStartingLevel.Text = "1";
            txtUpdateURL.Text = "https://ultrapowa.com";
            txtUsePatch.Text = "false";
            txtPatchURL.Text = "";
            txtMintenance.Text = "0";
            txtDatabaseType.Text = "sqlite";
            txtPort.Text = "9339";
            txtAdminMessage.Text = "Welcome to UCS Beta! Visit https://ultrapowa.com for more info.";
            txtLogLevel.Text = "0";
            txtClientVersion.Text = "8.551";
        }

        //Save Changes Button
        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
           var doc = new XmlDocument();
            var path = "config.ucs";
            doc.Load(path);
            var ie = doc.SelectNodes("appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGems")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingGems.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGold")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingGold.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingElixir.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingDarkElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingDarkElixir.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingTrophies")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingTrophies.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingLevel")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingLevel.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "UpdateUrl")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtUpdateURL.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "useCustomPatch")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtUsePatch.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "patchingServer")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtPatchURL.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceTimeleft")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtMintenance.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "databaseConnectionName")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtDatabaseType.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "ServerPort")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtPort.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "AdminMessage")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtAdminMessage.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "LogLevel")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtLogLevel.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "ClientVersion")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtClientVersion.Text;
                }
            }

            doc.Save(path);
            var title = "Ultrapowa Clash Server Manager";
            var message = "Changes has been saved!";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* END OF CONFIG EDITOR TAB*/

        /* MAIL TAB*/

        //Send To Gobal Chat Button
        private void materialRaisedButton9_Click(object sender, EventArgs e)
        {
            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.GetClient());
                pm.SetChatMessage(textBox21.Text);
                pm.SetPlayerId(0);
                pm.SetLeagueId(22);
                pm.SetPlayerName(textBox22.Text);
                pm.Send();
            }
        }

        //Send To Mailbox Button
        private void materialRaisedButton10_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName(textBox23.Text);
            mail.SetIsNew(2); // 0 = Seen, 2 = New
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(1526735450);
            mail.SetAllianceName("Ultrapowa");
            mail.SetMessage(textBox24.Text);
            mail.SetSenderLevel(300);
            mail.SetSenderLeagueId(22);

            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                p.SetAvatarStreamEntry(mail);
                p.Send();
            }
        }

        /* END OF MAIL TAB*/

        /* UNUSED */

        private void materialLabel30_Click(Object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void materialCheckBox1_CheckedChanged(object sender2, EventArgs e2)
        {

        }

        private void materialLabel6_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel7_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel8_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel13_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel14_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel15_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel16_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel23_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialLabel26_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialLabel5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void materialRaisedButton13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                var id = Convert.ToInt64(txtPlayerID.Text);
                var player = ResourcesManager.GetPlayer(id);
                new OutOfSyncMessage(player.GetClient()).Send();
            }
        }

        private void materialRaisedButton14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                var id = Convert.ToInt64(txtPlayerID.Text);
                var player = ResourcesManager.GetPlayer(id);
                player.SetAccountStatus(100);
                var save = DatabaseManager.Single().Save(player);
                save.Wait();
                new OutOfSyncMessage(player.GetClient()).Send();
            }
        }

        private void materialRaisedButton15_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                var id = Convert.ToInt64(txtPlayerID.Text);
                var player = ResourcesManager.GetPlayer(id);
                player.SetAccountStatus(0);
                var save = DatabaseManager.Single().Save(player);
                save.Wait();
            }
        }

        private void materialRaisedButton16_Click(object sender, EventArgs e)
        {
            var name = txtSearchPlayer.Text;
            listView1.Items.Clear();
            if (string.IsNullOrEmpty(txtSearchPlayer.Text))
            {
                MessageBox.Show("The Player-Name can't be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (var n in ResourcesManager.GetInMemoryLevels())
                {
                    var na = ResourcesManager.GetPlayer(n.GetPlayerAvatar().GetId()).GetPlayerAvatar().GetAvatarName();
                    if (na == name || na == name.ToUpper() || na == name.ToLower())
                    {
                        ListViewItem item = new ListViewItem(n.GetPlayerAvatar().GetAvatarName());
                        item.SubItems.Add(Convert.ToString(n.GetPlayerAvatar().GetId()));
                        item.SubItems.Add(Convert.ToString(n.GetPlayerAvatar().GetAvatarLevel()));
                        item.SubItems.Add(Convert.ToString(n.GetPlayerAvatar().GetScore()));
                        item.SubItems.Add(Convert.ToString(n.GetAccountPrivileges()));
                        listView1.Items.Add(item);
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
