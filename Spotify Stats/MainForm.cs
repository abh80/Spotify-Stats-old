using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spotify_Stats.Runner.Helpers;
using Spotify_Stats.Runner.spotify;
using Spotify_Stats.Interfaces;

namespace Spotify_Stats
{
    public partial class MainForm : Form
    {
        private WebBrowser webBrowser1;
        public MainForm()
        {
            this.AutoScroll = true;
            InitializeComponent();
            Load += (object sender, EventArgs e) =>
            { 
               webBrowser1 = new WebBrowser();
                webBrowser1.Location = new Point(1, 2);
                webBrowser1.MinimumSize = new Size(20, 20);
                webBrowser1.Name = "webBrowser1";
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Size = new System.Drawing.Size(799, 449);
                webBrowser1.TabIndex = 1;
                webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(this.onNavigate);
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    MessageBox.Show("Cant display while offline!");
                    header.Text = "You are offline!";
                    return;
                }
                NavigateThis();
            };
        }
        private void onNavigate(object sender, WebBrowserNavigatedEventArgs e)
        {
            if((webBrowser1.Url != null) && (webBrowser1.Url.ToString().StartsWith("https://accounts.spotify.com/en/authorize?client_id=8a0c080885d34f1f8fa4821f6194d9d7&redirect_uri=https:%2F%2Fspotify.com&response_type=token&scope=user-top-read"))) {
                return;
            }
            else if(webBrowser1.Url.ToString().StartsWith("https://www.spotify.com")) {
               var str =  webBrowser1.Url.ToString().Split(new[] { "access_token=" }, StringSplitOptions.None)[1].Split(new[] { "&" }, StringSplitOptions.None);
                var token = str[0];
                if(token == "")
                {
                    
                    MessageBox.Show("You rejected the authorization");
                    return;
                }
                TargetCreater.Create(token);
                webBrowser1.Hide();
                MessageBox.Show("Updated your token a restart is required!");
                Application.Exit();
                return;
            }
            else
            {
                return;
            }
        }
        private async void NavigateThis()
        {
           var k = ParseTarget.Parse();
            if(k == null)
            {
                header.Visible = false;
                this.Controls.Add(webBrowser1);
                header.Text = "Please wait updating your token!";
                webBrowser1.Visible = true;
                webBrowser1.Navigate("https://accounts.spotify.com/en/authorize?client_id=8a0c080885d34f1f8fa4821f6194d9d7&redirect_uri=https:%2F%2Fspotify.com&response_type=token&scope=user-top-read");
                return;
            }
            else
            {

                header.Text = "Your Top Songs";
                var x = DateTime.UtcNow - k.updatedAt;
                if (x.TotalMinutes >= 60)
                {
                    this.Controls.Add(webBrowser1);
                    webBrowser1.Visible = false;
                    webBrowser1.Navigate("https://accounts.spotify.com/en/authorize?client_id=8a0c080885d34f1f8fa4821f6194d9d7&redirect_uri=https:%2F%2Fspotify.com&response_type=token&scope=user-top-read");
                }
                
                var res = await API.GetTopSongs(k.token);
                PictureBox[] covers = new PictureBox[500];
                Label[] songNames = new Label[500];
                Label[] artistNames = new Label[500];
                int currenth = 70;
                int currentw = 12;
                for(var i = 0; i < res.items.Length;i++)
                { 
                    covers[i] = new PictureBox();
                    covers[i].Location = new Point(currentw,currenth);
                    covers[i].Load(res.items[i].album.images[0].url.ToString()) ;
                    covers[i].Size = new Size(160, 137);
                    covers[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    songNames[i] = new Label();
                    songNames[i].Font = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    songNames[i].Text = res.items[i].name.Length > 20 ? res.items[i].name.Substring(0,20) + ".." : res.items[i].name;
                    songNames[i].Location = new Point(currentw, currenth + 137 + 10);
                    songNames[i].TextAlign = ContentAlignment.MiddleCenter;
                    songNames[i].ForeColor = Color.White;
                    songNames[i].AutoSize = true;
                    songNames[i].TabIndex = 0;
                    artistNames[i] = new Label();
                    artistNames[i].Font = new Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    artistNames[i].Text = res.items[i].album.artists[0].name.Length > 20 ? res.items[i].album.artists[0].name.Substring(0, 20) + ".." : res.items[i].album.artists[0].name;
                    artistNames[i].Location = new Point(currentw, currenth + 137 + 30);
                    artistNames[i].TextAlign = ContentAlignment.MiddleCenter;
                    artistNames[i].ForeColor = Color.White;
                    artistNames[i].AutoSize = true;
                    artistNames[i].TabIndex = 0;
                    this.Controls.Add(covers[i]);
                    this.Controls.Add(songNames[i]);
                    this.Controls.Add(artistNames[i]);
                    if ((i+1) % 4 == 0)
                    {
                        currenth += 200;
                        currentw = 12;
                    }
                    else
                    {
                        currentw += 200;
                    }
                }
                
            }
                
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
