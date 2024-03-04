using System;
using System.Drawing;
using System.Windows.Forms;

namespace spaceShooter
{
    public partial class Form1 : Form
    {
        PictureBox[] enemiesMunition;
        int enemiesMunitionSpeed;
        
        PictureBox[] stars;
        int backgroundspeed;
        int playerSpeed;

        PictureBox[] munitions;
        int MunitionSpeed;

        PictureBox[] enemies;
        int enemiesSpeed;

        PictureBox Player;
        Random rnd;

        int score;
        int level;
        int deficulty;
        bool pause;
        bool gameIsOver;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameIsOver = false;
            score = 0;
            level = 1;
            deficulty = 9;
            
            backgroundspeed = 4;
            playerSpeed = 4;
            enemiesSpeed = 4;
            MunitionSpeed = 20;
            enemiesMunitionSpeed = 4;

            munitions = new PictureBox[3];
            
            //load images
            Image munition = Image.FromFile(@"asserts\munition.png");

            Image enemies1 = Image.FromFile(@"asserts\\E1.png");
            Image enemies2 = Image.FromFile(@"asserts\E2.png");
            Image enemies3 = Image.FromFile(@"asserts\E3.png");
            Image boss1 = Image.FromFile(@"asserts\Boss1.png");
            Image boss2 = Image.FromFile(@"asserts\Boss2.png");

            enemies = new PictureBox[10];

            //initialiase enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + i) *30, -30);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemies2;
            enemies[2].Image = enemies3;
            enemies[3].Image = enemies3;
            enemies[4].Image = enemies1;
            enemies[5].Image = enemies3;
            enemies[6].Image = enemies2;
            enemies[7].Image = enemies3;
            enemies[8].Image = enemies3;
            enemies[9].Image = boss2;

            //initialiase munitions
            for (int i = 0; i<munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            stars = new PictureBox[15];
            rnd = new Random();
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if(i%2==1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);
            }

            //Enemies munitions
            enemiesMunition = new PictureBox[10];
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2, 25);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Red;
                int X = rnd.Next(0, 10);
                this.Controls.Add(enemiesMunition[i]);
            }

            Player = new PictureBox();
            Player.Size = new Size(50, 50);
            Player.Image = Image.FromFile(@"asserts\player.png");
            Player.SizeMode = PictureBoxSizeMode.Zoom;
            Player.Location = new Point(300, 300);
            Player.BorderStyle = BorderStyle.None;
            this.Controls.Add(Player);

        }

        //moving
        private void MoveBackground_Tick(object sender, EventArgs e)
        {
            for(int i = 0; i<stars.Length/2; i++)
            {
                stars[i].Top += backgroundspeed;

                if (stars[i].Top>= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length/2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed - 2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(!pause)
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if(!gameIsOver)
                {
                    if(pause)
                    {
                        StartTimer();
                        label1.Visible = false;
                        StartTimer();
                        pause = true;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 - 120, 150);
                        label1.Text = "PAUSED";
                        label1.Visible = true;
                        StopTimer();
                        pause = true;
                    }
                }
            }
        }

        //shooting bullets And inside of this function, we also call Collision Function
        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= MunitionSpeed;

                    //distroying enemies of player function call hear
                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        //enemies movements main function
        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiesSpeed);
        }
        
        //enemies movements action function
        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length;  i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if(array[i].Top > this.Height)
                {
                    array[i].Location = new Point(i * 50, -200);
                }
            }
        }

        //distroying enemies of player
        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds)  || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds) )
                {
                    score += 1;
                    scorelbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString();

                    if(score % 30 == 0)
                    {
                        level += 1;
                        levellbl.Text = (level < 10) ? "0" + level.ToString() : level.ToString();

                        if (enemiesSpeed <= 10 && enemiesMunitionSpeed <= 10 && deficulty >= 0)
                        {
                            deficulty--;
                            enemiesSpeed++;
                            enemiesMunitionSpeed++;
                        }

                        if (level == 10)
                        {
                            GameOver("NICE DONE");
                        }
                    }
                    enemies[i].Location = new Point((i + i) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    Player.Visible = false;
                    GameOver("");
                }
            }
        }

        private void GameOver(String str)
        {
            label1.Text = str;
            label1.Location = new Point(120, 120);
            label1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;

            StopTimer();
        }

        //Stop timers
        private void StopTimer()
        {
            MoveBackground.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemiesMunitionTimer.Stop();
        }

        //Start timers
        private void StartTimer()
        {
            MoveBackground.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
            EnemiesMunitionTimer.Start();
        }

        //enemies bullets and also call CollisionWithEnemisMunition function which use to show what happen we hit a bullet
        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesMunition.Length - deficulty); i++)
            {
                if (enemiesMunition[i].Top <this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMunitionSpeed;

                    CollisionWithEnemisMunition();
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemiesMunition[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        //what happen if we hit by enemies bullet
        private void CollisionWithEnemisMunition()
        {
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Exit button
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        //Replay button
        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }
    }
}
