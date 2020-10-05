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
    public partial class LevelFailedScreen : UserControl
    {
        WindowsMediaPlayer gameOver = new WindowsMediaPlayer(); //create a player for the game over tune

        public LevelFailedScreen()
        {
            InitializeComponent();
            gameOver.URL = "Shadow - Game Over.wav"; //set the file to play to the game over tune
        }

        private void tryButton_Click(object sender, EventArgs e)
        {
            gameOver.controls.stop(); //stop music (in case the button is pressed before the game over tune is done playing)

            Form f = this.FindForm();
            f.Controls.Remove(this);

            Levels.Level_1 l1 = new Levels.Level_1();
            f.Controls.Add(l1);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void LevelFailedScreen_Load(object sender, EventArgs e)
        {
            gameOver.controls.play(); //play the game over tune
        }
    }
}
