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
    public partial class MenuScreen : UserControl
    {
        public WindowsMediaPlayer menuMusic = new WindowsMediaPlayer(); //make a new music player

        public MenuScreen()
        {
            InitializeComponent();
            menuMusic.URL = "Shadow - Menu Theme.wav"; //set the file to play to the menu theme (put file in project_name/project_name/bin/debug)
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            musicTimer.Enabled = false; //stop looping of music
            menuMusic.controls.stop(); //stop the menu music

            Form f = this.FindForm();
            f.Controls.Remove(this);

            Levels.Level_1 l1 = new Levels.Level_1();
            f.Controls.Add(l1);
        }

        private void controlsButton_Click(object sender, EventArgs e)
        {
            musicTimer.Enabled = false; //stop looping of music
            menuMusic.controls.stop(); //stop the menu music

            Form f = this.FindForm();
            f.Controls.Remove(this);

            controlScreen cs = new controlScreen();
            f.Controls.Add(cs);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void musicTimer_Tick(object sender, EventArgs e)
        {
            menuMusic.controls.play(); //play the menu theme on repeat
        }
    }
}
