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
        public Shadow()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bring up the menu screen
            MenuScreen ms = new MenuScreen();
            this.Controls.Add(ms);
        }
    }
}
