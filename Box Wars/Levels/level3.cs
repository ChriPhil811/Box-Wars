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
    public partial class level3 : UserControl
    {
        //game music player and sound effect players
        WindowsMediaPlayer gameMusic = new WindowsMediaPlayer();
        SoundPlayer keyPickupSound = new SoundPlayer(Properties.Resources.key_pickup);
        SoundPlayer heroFootsteps = new SoundPlayer(Properties.Resources.footsteps);

        //hero key press variables
        Boolean wDown, aDown, sDown, dDown;

        //rectangle list for the level layout
        List<Rectangle> level1Collision = new List<Rectangle>();

        //lists for enemies
        List<Rectangle> enemies = new List<Rectangle>();
        List<Enemy> stationaryEnemies = new List<Enemy>();
        List<Enemy> moveEnemies = new List<Enemy>();

        //create a rectangle for the finish area, key, and door
        Rectangle finish = new Rectangle(0, 50, 100, 100);
        Rectangle key = new Rectangle(750, 90, 20, 20);
        Rectangle door = new Rectangle(0, 150, 100, 50);

        int radius = 20; //int for enemy spotting radius size
        int enemyCount = 8; //number of enemies in stage (**VERY IMPORTANT**)
        int heroSoundTimer = 0;

        //integers for enemy move counts. 
        //move count is for basic up-down enemies
        //move count 2 and 3 are for quad directional enemies
        int moveCount = 20;
        int moveCount2 = 70;
        int moveCount3 = 24;

        //integers for enemy move speeds
        //move speed is for basic up-down or left-right enemies
        //move speed 2 and 3 are for quad directional enemies
        int moveSpeed = 5;
        int moveSpeed2 = 5;
        int moveSpeed3 = 5;

        Boolean leftRight = false; //true-false for enemy left-right movement
        Boolean upDown = false; //true-false for enemy up-down movement
        Boolean keyCollected = false; //true-false for key collection

        //brushes
        SolidBrush levelBrush = new SolidBrush(Color.MidnightBlue);
        SolidBrush radiusBrush = new SolidBrush(Color.Khaki);
        SolidBrush finishBrush = new SolidBrush(Color.LightGreen);

        //hero creation
        Hero hero = new Hero(5, 240, 20);

        //enemy sprites
        Image stationaryEnemySprite;
        Image moveEnemySprite;
        Image heroSprite;
        Image keySprite;

        public level3()
        {
            InitializeComponent();
            OnStart();

            gameMusic.URL = "Shadow - Gameplay Theme.wav"; //set the file to play to the gameplay theme

            //setup sprites
            stationaryEnemySprite = Properties.Resources.RedEnemy;
            moveEnemySprite = Properties.Resources.OrangeEnemy;
            heroSprite = Properties.Resources.Shadow;
            keySprite = Properties.Resources.Key;
        }

        #region level setup

        public void OnStart()
        {
            //make rectangles for the stage layout
            Rectangle top = new Rectangle(0, 0, this.Width, 50);
            Rectangle bottom = new Rectangle(0, this.Height - 50, this.Width, 50);
            Rectangle obstacle1 = new Rectangle(100, 50, 30, 300);
            Rectangle obstacle2 = new Rectangle(215, 100, 30, 300);
            Rectangle obstacle3 = new Rectangle(330, 150, 30, 300);
            Rectangle obstacle4 = new Rectangle(560, 50, 30, 300);
            Rectangle obstacle5 = new Rectangle(630, 150, 170, 30);
            Rectangle obstacle6 = new Rectangle(630, 250, 170, 30);
            Rectangle obstacle7 = new Rectangle(660, 315, 30, 100);

            //add stage rectangles to level collision list
            level1Collision.Add(top);
            level1Collision.Add(bottom);
            level1Collision.Add(door);
            level1Collision.Add(obstacle1);
            level1Collision.Add(obstacle2);
            level1Collision.Add(obstacle3);
            level1Collision.Add(obstacle4);
            level1Collision.Add(obstacle5);
            level1Collision.Add(obstacle6);
            level1Collision.Add(obstacle7);

            //enemy spawning
            Enemy stationaryEnemy1 = new Enemy(390, 170, 30);
            Enemy stationaryEnemy2 = new Enemy(500, 300, 30);
            Enemy moveEnemy1 = new Enemy(155, 60, 30);
            Enemy moveEnemy2 = new Enemy(275, 410, 30);
            Enemy moveEnemy3 = new Enemy(155, 410, 30);
            Enemy moveEnemy4 = new Enemy(275, 60, 30);
            Enemy moveEnemy5 = new Enemy(610, 300, 30);
            Enemy moveEnemy6 = new Enemy(610, 200, 30);

            //add the enemies to the move enemies list
            stationaryEnemies.Add(stationaryEnemy1);
            stationaryEnemies.Add(stationaryEnemy2);
            moveEnemies.Add(moveEnemy1);
            moveEnemies.Add(moveEnemy2);
            moveEnemies.Add(moveEnemy3);
            moveEnemies.Add(moveEnemy4);
            moveEnemies.Add(moveEnemy5);
            moveEnemies.Add(moveEnemy6);
        }

        #endregion level setup

        #region key down and up

        private void level3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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

        private void level3_KeyUp(object sender, KeyEventArgs e)
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

            #region enemies 1-4

            //enemy movement for enemies 1 and 2 (following a square path starting with up-down)
            if (leftRight == true && moveCount2 == 0)
            {
                moveCount2 = 70;
                moveSpeed2 = -moveSpeed2;
                leftRight = false;
            }
            else if (moveCount2 == 0)
            {
                moveCount2 = 24;
                leftRight = true;
            }
            else if (leftRight == true)
            {
                moveEnemies[0].MoveEnemyLeftRight(moveSpeed2);
                moveEnemies[1].MoveEnemyLeftRight(-moveSpeed2);
                moveCount2--;
            }
            else
            {
                moveEnemies[0].MoveEnemyUpDown(moveSpeed2);
                moveEnemies[1].MoveEnemyUpDown(-moveSpeed2);
                moveCount2--;
            }

            //enemy movement for enemies 3 and 4 (following 1 & 2 square path but starting with left-right)
            if (upDown == true && moveCount3 == 0)
            {
                moveCount3 = 24;
                moveSpeed3 = -moveSpeed3;
                upDown = false;
            }
            else if (moveCount3 == 0)
            {
                moveCount3 = 70;
                upDown = true;
            }
            else if (upDown == true)
            {
                moveEnemies[2].MoveEnemyUpDown(-moveSpeed3);
                moveEnemies[3].MoveEnemyUpDown(moveSpeed3);
                moveCount3--;
            }
            else
            {
                moveEnemies[2].MoveEnemyLeftRight(moveSpeed3);
                moveEnemies[3].MoveEnemyLeftRight(-moveSpeed3);
                moveCount3--;
            }

            #endregion enemies 1-4

            #region enemies 5 and 6

            //enemy movement for enemy 5 (up and down only)
            if (moveCount == 0)
            {
                moveSpeed = -moveSpeed;
                moveCount = 20;
            }
            else
            {
                moveEnemies[4].MoveEnemyUpDown(moveSpeed);
                moveEnemies[5].MoveEnemyLeftRight(moveSpeed);
                moveCount--;
            }

            #endregion enemies 5 and 6

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

            #region enemy collisions

            //hero collision with stationary enemy sight radius
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
                }
            }

            //hero collision with move enemy sight radius
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

            #endregion enemy collisions

            //hero collision with key
            if (heroRec.IntersectsWith(key))
            {
                keyCollected = true;
                gameMusic.controls.pause(); //pause the game music
                keyPickupSound.Play(); //play key pickup sound

                key = new Rectangle(0, 0, 0, 0); //get rid of the key collision

                Thread.Sleep(1000); //pause for a second

                gameMusic.controls.play(); //resume game music

            }

            //hero collision with finish area 
            //game complete!
            if (heroRec.IntersectsWith(finish))
            {
                gameLoop.Enabled = false;
                gameMusic.controls.stop(); //stop the game music

                Form f = this.FindForm();
                f.Controls.Remove(this);

                GameCompleteScreen gcs = new GameCompleteScreen();
                f.Controls.Add(gcs);
            }

            #endregion hero collisions

            Refresh();

            enemies.Clear(); //clear enemy rectangle list (nessecary for enemy sight graphics to update properly)
        }

        #endregion movement and collisions (game loop)

        #region paint graphics

        private void level3_Paint(object sender, PaintEventArgs e)
        {
            //draw enemy spotting radius
            foreach (Rectangle r in enemies)
            {
                e.Graphics.FillRectangle(radiusBrush, r);
            }

            //draw stationary enemies
            foreach (Enemy r in stationaryEnemies)
            {
                e.Graphics.DrawImage(stationaryEnemySprite, r.x, r.y);
            }

            //draw moving enemies
            foreach (Enemy r in moveEnemies)
            {
                e.Graphics.DrawImage(moveEnemySprite, r.x, r.y, r.size, r.size);
            }

            //draw level design
            foreach (Rectangle r in level1Collision)
            {
                e.Graphics.FillRectangle(levelBrush, r);
            }

            //draw key and door
            if (keyCollected == true)
            {
                level1Collision.Remove(door);
            }
            else
            {
                e.Graphics.DrawImage(keySprite, key.X, key.Y);
            }

            //draw the finish area
            e.Graphics.FillRectangle(finishBrush, finish);

            //draw the hero
            e.Graphics.DrawImage(heroSprite, hero.x, hero.y, hero.size, hero.size);
        }
    }

        #endregion paint graphics
}
