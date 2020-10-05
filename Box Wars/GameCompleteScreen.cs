using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Box_Wars
{
    public partial class GameCompleteScreen : UserControl
    {
        public WindowsMediaPlayer winMusic = new WindowsMediaPlayer(); //make a new music player

        public GameCompleteScreen()
        {
            InitializeComponent();
            winMusic.URL = "Shadow - Game Complete.wav"; //set the file to play to the menu theme
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Form f = this.FindForm();
            f.Controls.Remove(this);

            MenuScreen ms = new MenuScreen();
            f.Controls.Add(ms);

            ms.Location = new Point((f.Width - ms.Width) / 2, (f.Height - ms.Height) / 2); //center the screen
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void GameCompleteScreen_Load(object sender, EventArgs e)
        {
            winMusic.controls.play();
        }
    }
}
