using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Box_Wars
{
    public partial class Shadow : Form
    {
        public static int gameVolume = 100; //volume int for global use

        public Shadow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bring up the menu screen
            MenuScreen ms = new MenuScreen();
            this.Controls.Add(ms);
            ms.Location = new Point((this.Width - ms.Width) / 2, (this.Height - ms.Height) / 2); //center the menu screen
        }
    }
}
