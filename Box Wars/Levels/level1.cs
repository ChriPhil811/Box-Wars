using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Box_Wars.Classes;
using WMPLib;
using System.Threading;

namespace Box_Wars.Levels
{
    public partial class Level_1 : UserControl
    {
        #region variables

        //game music player and sound players
        WindowsMediaPlayer gameMusic = new WindowsMediaPlayer();
        SoundPlayer levelCompleteSound = new SoundPlayer(Properties.Resources.level_complete);
        SoundPlayer heroFootsteps = new SoundPlayer(Properties.Resources.footsteps);

        Boolean wDown, aDown, sDown, dDown; //hero key press variables
        Boolean mute = false; //boolean for mute
        Boolean fullscreen = false; //boolean for fullscreen 

        //rectangle list for the level layout
        List<Rectangle> level1Collision = new List<Rectangle>();

        //lists for enemies
        List<Rectangle> enemies = new List<Rectangle>();
        List<Enemy> stationaryEnemies = new List<Enemy>();

        //create a rectangle for the finish area
        Rectangle finish = new Rectangle(650, 50, 150, 150);

        int radius = 60; //int for enemy spotting radius size
        int enemyCount = 2; //number of enemies in stage (**VERY IMPORTANT**)
        int heroSoundTimer = 0; //hero movement sound timer

        //brushes
        SolidBrush levelBrush = new SolidBrush(Color.MidnightBlue);
        SolidBrush radiusBrush = new SolidBrush(Color.Khaki);
        SolidBrush finishBrush = new SolidBrush(Color.LightGreen);

        //hero creation
        Hero hero = new Hero(5, 240, 20);

        //enemy sprites
        Image stationaryEnemySprite;
        Image heroSprite;

        #endregion variables

        #region level on load

        public Level_1()
        {
            InitializeComponent();
            OnStart();
            
            gameMusic.URL = "Shadow - Gameplay Theme.wav"; //set the file to play to the gameplay theme

            Cursor.Hide(); //hide the cursor

            deathLabel.Text = "Deaths: " + Shadow.deathCount; //update the death counter

            //setup sprites
            stationaryEnemySprite = Properties.Resources.RedEnemy;
            heroSprite = Properties.Resources.Shadow;
        }

        #endregion level on load

        #region level setup

        public void OnStart()
        {
            //make rectangles for the stage layout
            Rectangle top = new Rectangle(0, 0, this.Width, 50);
            Rectangle bottom = new Rectangle(0, this.Height - 50, this.Width, 50);
            Rectangle obstacle1 = new Rectangle(this.Width / 2, this.Height / 2 - 25, 200, 50);
            Rectangle obstacle2 = new Rectangle(250, 300, 50, 300);

            //add stage rectangles to level collision list
            level1Collision.Add(top);
            level1Collision.Add(bottom);
            level1Collision.Add(obstacle1);
            level1Collision.Add(obstacle2);

            //enemy spawning
            Enemy enemy1 = new Enemy(500, 125, 30);
            Enemy enemy2 = new Enemy(125, 350, 30);

            //add the enemies to the stationary enemies list
            stationaryEnemies.Add(enemy1);
            stationaryEnemies.Add(enemy2);
        }

        #endregion level setup

        #region key down and up

        private void Level_1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;

                //mute and fullscreen controls
                case Keys.F:
                    if (fullscreen == true)
                    {
                        Form f = this.FindForm();
                        f.WindowState = FormWindowState.Normal;
                        this.Location = new Point((f.Width - this.Width) / 2, (f.Height - this.Height) / 2); //center the screen
                        fullscreen = false;
                    }
                    else
                    {
                        Form f = this.FindForm();
                        f.WindowState = FormWindowState.Maximized;
                        this.Location = new Point((f.Width - this.Width) / 2, (f.Height - this.Height) / 2); //center the screen
                        fullscreen = true;
                    }
                    break;
                case Keys.M:
                    if (mute == true)
                    {
                        Shadow.gameVolume = 100;
                        mute = false;
                    }
                    else
                    {
                        Shadow.gameVolume = 0;
                        mute = true;
                    }
                    break;
            }
        }

        private void Level_1_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
            }
        }

        #endregion key down and up

        #region movement and collisions (Game Loop)

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            gameMusic.controls.play(); //play game music on loop
            gameMusic.settings.volume = Shadow.gameVolume; //update game volume

            //hero movement
            if (dDown == true && hero.x < this.Width - hero.size)
            {
                hero.MoveRightLeft(5);
            }
            if (aDown == true && hero.x > 0)
            {
                hero.MoveRightLeft(-5);
            }
            if (wDown == true)
            {
                hero.MoveUpDown(-5);
            }
            if (sDown == true)
            {
                hero.MoveUpDown(5);
            }

            //hero movement sounds
            if (heroSoundTimer == 0 && (wDown == true || aDown == true || sDown == true || dDown == true))
            {
                heroFootsteps.Play();
                heroSoundTimer = 100;
            }
            else if (wDown == false && aDown == false && sDown == false && dDown == false)
            {
                heroFootsteps.Stop();
                heroSoundTimer = 0;
            }
            else if (heroSoundTimer > 0)
            {
                heroSoundTimer--;
            }

            #region hero collisions

            //hero collision rectangle
            Rectangle heroRec = new Rectangle(hero.x, hero.y, hero.size, hero.size);

            //hero collision with level layout
            foreach(Rectangle r in level1Collision)
            {
                if (heroRec.IntersectsWith(r))
                {
                    if(wDown)
                    {
                        hero.Stop("w");
                    }
                    if (aDown)
                    {
                        hero.Stop("a");
                    }
                    if (sDown)
                    {
                        hero.Stop("s");
                    }
                    if (dDown)
                    {
                        hero.Stop("d");
                    }
                }
            }
            
            //hero collision with enemy sight radius
            for (int i = 0; i < stationaryEnemies.Count; i++)
            {
                Rectangle enemy = new Rectangle(stationaryEnemies[i].x - radius, stationaryEnemies[i].y - radius, stationaryEnemies[i].size + radius * 2, stationaryEnemies[i].size + radius * 2);
                if (enemies.Count < enemyCount)
                {
                    enemies.Add(enemy);
                }

                if (heroRec.IntersectsWith(enemy))
                {
                    gameLoop.Enabled = false;
                    gameMusic.controls.stop(); //stop the game music

                    //switch to level failed screen
                    Form f = this.FindForm();
                    f.Controls.Remove(this);

                    LevelFailedScreen lf = new LevelFailedScreen();
                    f.Controls.Add(lf);

                    lf.Location = new Point((f.Width - lf.Width) / 2, (f.Height - lf.Height) / 2); //center the screen
                }
            }

            //hero collision with finish area 
            //brings player to the next level
            if(heroRec.IntersectsWith(finish))
            {
                gameLoop.Enabled = false;
                levelCompleteSound.Play(); //play the level complete sound
                gameMusic.controls.stop(); //stop the game music

                Thread.Sleep(2000); //2 second pause before next level

                Form f = this.FindForm();
                f.Controls.Remove(this);

                level2 l2 = new level2();
                f.Controls.Add(l2);

                l2.Location = new Point((f.Width - l2.Width) / 2, (f.Height - l2.Height) / 2); //center the screen
                l2.Focus();
            }

            #endregion hero collisions

            Refresh();
        }

        #endregion movement and collisions (Game Loop)

        #region paint graphics

        private void Level_1_Paint(object sender, PaintEventArgs e)
        {
            //draw enemy spotting radius
            foreach (Rectangle r in enemies)
            {
                e.Graphics.FillRectangle(radiusBrush, r);
            }

            //draw enemies
            foreach (Enemy r in stationaryEnemies)
            {
                e.Graphics.DrawImage(stationaryEnemySprite, r.x, r.y);
            }

            //draw level design
            foreach (Rectangle r in level1Collision)
            {
                e.Graphics.FillRectangle(levelBrush, r);
            }

            //draw the finish area
            e.Graphics.FillRectangle(finishBrush, finish);

            //draw the hero
            e.Graphics.DrawImage(heroSprite, hero.x, hero.y, hero.size, hero.size);
        }

        #endregion paint graphics
    }
}
