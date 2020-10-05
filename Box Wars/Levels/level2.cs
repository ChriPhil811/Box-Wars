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
    public partial class level2 : UserControl
    {
        //game music player and sound players
        WindowsMediaPlayer gameMusic = new WindowsMediaPlayer();
        SoundPlayer levelCompleteSound = new SoundPlayer(Properties.Resources.level_complete);
        SoundPlayer heroFootsteps = new SoundPlayer(Properties.Resources.footsteps);

        //hero key press variables
        Boolean wDown, aDown, sDown, dDown;
        Boolean mute;

        //rectangle list for the level layout
        List<Rectangle> level1Collision = new List<Rectangle>();

        //lists for enemies
        List<Rectangle> enemies = new List<Rectangle>();
        List<Enemy> moveEnemies = new List<Enemy>();

        //create a rectangle for the finish area
        Rectangle finish = new Rectangle(650, 50, 150, 150);

        int radius = 50; //int for enemy spotting radius size
        int enemyCount = 3; //number of enemies in stage (**VERY IMPORTANT**)
        int heroSoundTimer = 0;

        //integers for enemy move counts. 
        //move count is for basic up-down enemies
        //move count 2 is for quad directional enemies
        int moveCount = 50;
        int moveCount2 = 60;

        //integers for enemy move speeds
        //move speed is for basic up-down enemies
        //move speed 2 is for quad directional enemies
        int moveSpeed = 5;
        int moveSpeed2 = 5;

        Boolean leftRight = false; //true-false for enemy left-right movement

        //brushes
        SolidBrush levelBrush = new SolidBrush(Color.MidnightBlue);
        SolidBrush radiusBrush = new SolidBrush(Color.Khaki);
        SolidBrush finishBrush = new SolidBrush(Color.LightGreen);

        //hero creation
        Hero hero = new Hero(5, 240, 20);

        //enemy sprites
        Image moveEnemySprite;
        Image heroSprite;

        public level2()
        {
            InitializeComponent();
            OnStart();

            gameMusic.URL = "Shadow - Gameplay Theme.wav"; //set the file to play to the gameplay theme

            //setup sprites
            moveEnemySprite = Properties.Resources.OrangeEnemy;
            heroSprite = Properties.Resources.Shadow;
        }

        #region level setup

        public void OnStart()
        {
            //make rectangles for the stage layout
            Rectangle top = new Rectangle(0, 0, this.Width, 50);
            Rectangle bottom = new Rectangle(0, this.Height - 50, this.Width, 50);
            Rectangle obstacle1 = new Rectangle(100, 50, 50, 350);
            Rectangle obstacle2 = new Rectangle(300, 50, 50, 200);
            Rectangle obstacle3 = new Rectangle(300, 300, 50, 200);
            Rectangle obstacle4 = new Rectangle(475, 150, 50, 200);
            Rectangle obstacle5 = new Rectangle(650, 100, 150, 350);
            
            //add stage rectangles to level collision list
            level1Collision.Add(top);
            level1Collision.Add(bottom);
            level1Collision.Add(obstacle1);
            level1Collision.Add(obstacle2);
            level1Collision.Add(obstacle3);
            level1Collision.Add(obstacle4);
            level1Collision.Add(obstacle5);

            //enemy spawning
            Enemy moveEnemy1 = new Enemy(210, 150, 30);
            Enemy moveEnemy2 = new Enemy(400, 100, 30);
            Enemy moveEnemy3 = new Enemy(575, 380, 30);

            //add the enemies to the move enemies list
            moveEnemies.Add(moveEnemy1);
            moveEnemies.Add(moveEnemy2);
            moveEnemies.Add(moveEnemy3);
        }

        #endregion level setup

        #region key down and up

        private void level2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
                case Keys.M:
                    if (mute == true)
                    {
                        gameMusic.settings.volume = 100;
                        mute = false;
                    }
                    else
                    {
                        gameMusic.settings.volume = 0;
                        mute = true;
                    }
                    break;
            }
        }

        private void level2_KeyUp(object sender, KeyEventArgs e)
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

        #region movement and collisions (game loop)

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            gameMusic.controls.play(); //play the game music on repeat

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

            #region enemy movement

            //enemy movement for enemy 1 (up and down only)
            if (moveCount == 0)
            {
                moveSpeed = -moveSpeed;
                moveCount = 60;
            }
            else
            {
                moveEnemies[0].MoveEnemyUpDown(moveSpeed);
                moveCount--;
            }

            //enemy movement for enemies 2 and 3 (following a square path)
            if(leftRight == true && moveCount2 == 0)
            {
                moveCount2 = 60;
                moveSpeed2 = -moveSpeed2;
                leftRight = false;
            }
            else if (moveCount2 == 0)
            {
                moveCount2 = 35;
                leftRight = true;
            }
            else if(leftRight == true)
            {
                moveEnemies[1].MoveEnemyLeftRight(moveSpeed2);
                moveEnemies[2].MoveEnemyLeftRight(-moveSpeed2);
                moveCount2--;
            }
            else
            {
                moveEnemies[1].MoveEnemyUpDown(moveSpeed2);
                moveEnemies[2].MoveEnemyUpDown(-moveSpeed2);
                moveCount2--;
            }

            #endregion enemy movement

            #region hero collisions

            //hero collision rectangle
            Rectangle heroRec = new Rectangle(hero.x, hero.y, hero.size, hero.size);

            //hero collision with level layout
            foreach (Rectangle r in level1Collision)
            {
                if (heroRec.IntersectsWith(r))
                {
                    if (wDown)
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
            for (int i = 0; i < moveEnemies.Count; i++)
            {
                Rectangle enemy = new Rectangle(moveEnemies[i].x - radius, moveEnemies[i].y - radius, moveEnemies[i].size + radius * 2, moveEnemies[i].size + radius * 2);

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
                }
            }

            //hero collision with finish area 
            //brings player to the next level
            if (heroRec.IntersectsWith(finish))
            {
                gameLoop.Enabled = false;
                levelCompleteSound.Play(); //play the level complete sound
                gameMusic.controls.stop(); //stop the game music

                Thread.Sleep(2000); //2 second pause before next level

                Form f = this.FindForm();
                f.Controls.Remove(this);

                level3 l3 = new level3();
                f.Controls.Add(l3);
            }

            #endregion hero collisions

            Refresh();

            enemies.Clear(); //clear enemy rectangle list
        }

        #endregion movement and collisions (game loop)

        #region paint graphics

        private void level2_Paint(object sender, PaintEventArgs e)
        {
            //draw enemy spotting radius
            foreach (Rectangle r in enemies)
            {
                e.Graphics.FillRectangle(radiusBrush, r);
            }

            //draw enemies
            foreach (Enemy r in moveEnemies)
            {
                e.Graphics.DrawImage(moveEnemySprite, r.x, r.y, r.size, r.size);
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
