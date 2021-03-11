using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;


namespace pong
{
    public partial class Form1 : Form
    {
        Stopwatch stopWatch = new Stopwatch();
        Random rnd = new Random();

        SoundPlayer paddleHit = new SoundPlayer(Properties.Resources.paddleHit);
        SoundPlayer wallHit = new SoundPlayer(Properties.Resources.wallHit);
        SoundPlayer goal = new SoundPlayer(Properties.Resources.goalSound);

        int paddle1X = 50;
        int paddle1Y = 185;
        int player1Score = 0;

        int paddle2X = 520;
        int paddle2Y = 185;
        int player2Score = 0;

        int paddleWidth = 30;
        int paddleHeight = 30;
        int paddleSpeed = 6;

        int ballX = 288;
        int ballY = 188;
        int ballXSpeed = 0;
        int ballYSpeed = 0;

        int ballWidth = 25;
        int ballHeight = 25;


        bool slow = false;
        bool slower = false;
        bool stop = false;
        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftDown = false;
        bool rightDown = false;

        Rectangle player1RecTop;
        Rectangle player1RecBottom;
        Rectangle player1RecRight;
        Rectangle player1RecLeft;

        Rectangle player2RecTop;
        Rectangle player2RecBottom;
        Rectangle player2RecRight;
        Rectangle player2RecLeft;

        Rectangle player1Goal = new Rectangle(-25, 175, 50, 50);
        Rectangle player2Goal = new Rectangle(575, 175, 50, 50);

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush redBrush = new SolidBrush(Color.IndianRed);

        SolidBrush lBlueBrush = new SolidBrush(Color.LightBlue);
        SolidBrush lRedBrush = new SolidBrush(Color.LightPink);

        Pen whitePen = new Pen(Color.White, 5);

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font screenFont = new Font("Consolas", 12);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }



        private void gameTimer_Tick(object sender, EventArgs e)
        {
            stopWatch.Start();
            //move ball 
            ballX += ballXSpeed;
            ballY += ballYSpeed;

            //move player 1 
            if (wDown == true && paddle1Y > 0)
            {
                paddle1Y -= paddleSpeed;
            }

            if (sDown == true && paddle1Y < this.Height - paddleHeight)
            {
                paddle1Y += paddleSpeed;
            }

            if (aDown == true && paddle1X > 0)
            {
                paddle1X -= paddleSpeed;
            }

            if (dDown == true && paddle1X < 300 - paddleWidth)
            {
                paddle1X += paddleSpeed;
            }

            //move player 2 
            if (upArrowDown == true && paddle2Y > 0)
            {
                paddle2Y -= paddleSpeed;
            }

            if (downArrowDown == true && paddle2Y < this.Height - paddleHeight)
            {
                paddle2Y += paddleSpeed;
            }

            if (leftDown == true && paddle2X > 300)
            {
                paddle2X -= paddleSpeed;
            }

            if (rightDown == true && paddle2X < this.Width - paddleWidth)
            {
                paddle2X += paddleSpeed;
            }

            //check if ball hit top or bottom wall and change direction if it does 
            if (ballY < 0 || ballY > this.Height - ballHeight)
            {
                ballYSpeed *= -1;  // or: ballYSpeed = -ballYSpeed; 
                wallHit.Play();
            }

            //create Rectangles of objects on screen to be used for collision detection 

            player1RecTop = new Rectangle(paddle1X, paddle1Y, paddleWidth, 0);
            player1RecBottom = new Rectangle(paddle1X, (paddle1Y + 30), paddleWidth, 0);
            player1RecLeft = new Rectangle(paddle1X, paddle1Y, 0, paddleHeight);
            player1RecRight = new Rectangle((paddle1X + 30), paddle1Y, 1, paddleHeight);

            player2RecTop = new Rectangle(paddle2X, paddle2Y, paddleWidth, 0);
            player2RecBottom = new Rectangle(paddle2X, (paddle2Y + 30), paddleWidth, 0);
            player2RecLeft = new Rectangle(paddle2X, paddle2Y, 0, paddleHeight);
            player2RecRight = new Rectangle((paddle2X + 30), paddle2Y, 0, paddleHeight);





            Rectangle ballRec = new Rectangle(ballX, ballY, ballWidth, ballHeight);

            //check if ball hits either paddle. If it does change the direction 
            //and place the ball in front of the paddle hit 
            if (player1RecTop.IntersectsWith(ballRec) && wDown)
            {
                paddleHit.Play();
                ballYSpeed = -6;
                if (ballXSpeed > 0)
                {
                    ballXSpeed = 6;
                }
                else if (ballXSpeed < 0)
                {
                    ballXSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballXSpeed = 6;
                    }
                    else { ballXSpeed = -6; }
                }
                ballY = paddle1Y - paddleHeight - 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;
            }

            else if (player1RecBottom.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballYSpeed = 6;
                if (ballXSpeed > 0)
                {
                    ballXSpeed = 6;
                }
                else if (ballXSpeed < 0)
                {
                    ballXSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballXSpeed = 6;
                    }
                    else { ballXSpeed = -6; }
                }
                ballY = paddle1Y + paddleHeight + 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;

            }

            else if (player1RecLeft.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballXSpeed = -6;
                if (ballYSpeed > 0)
                {
                    ballYSpeed = 6;
                }
                else if (ballYSpeed < 0)
                {
                    ballYSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballYSpeed = 6;
                    }
                    else { ballYSpeed = -6; }
                }
                ballX = paddle1X - paddleWidth - 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;

            }

            else if (player1RecRight.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballXSpeed = 6;
                if (ballYSpeed > 0)
                {
                    ballYSpeed = 6;
                }
                else if (ballYSpeed < 0)
                {
                    ballYSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballYSpeed = 6;
                    }
                    else { ballYSpeed = -6; }
                }
                ballX = paddle1X + paddleWidth + 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;

            }

            else if (player2RecTop.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                if (ballXSpeed > 0)
                {
                    ballXSpeed = 6;
                }
                else if (ballXSpeed < 0)
                {
                    ballXSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballXSpeed = 6;
                    }
                    else { ballXSpeed = -6; }
                }
                ballYSpeed = -6;
                ballY = paddle2Y - paddleHeight - 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;
            }

            else if (player2RecBottom.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballYSpeed = 6;
                if (ballXSpeed > 0)
                {
                    ballXSpeed = 6;
                }
                else if (ballXSpeed < 0)
                {
                    ballXSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballXSpeed = 6;
                    }
                    else { ballXSpeed = -6; }
                }
                ballY = paddle2Y + paddleHeight + 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;
            }

            else if (player2RecLeft.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballXSpeed = -6;
                if (ballYSpeed > 0)
                {
                    ballYSpeed = 6;
                }
                else if (ballYSpeed < 0)
                {
                    ballYSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballYSpeed = 6;
                    }
                    else { ballYSpeed = -6; }
                }
                ballX = paddle2X - paddleWidth - 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;
            }

            else if (player2RecRight.IntersectsWith(ballRec))
            {
                paddleHit.Play();
                ballXSpeed = 6;
                if (ballYSpeed > 0)
                {
                    ballYSpeed = 6;
                }
                else if (ballYSpeed < 0)
                {
                    ballYSpeed = -6;
                }
                else
                {
                    int speed = rnd.Next(0, 1);
                    if (speed == 1)
                    {
                        ballYSpeed = 6;
                    }
                    else { ballYSpeed = -6; }
                }
                ballX = paddle2X + paddleWidth + 1;
                stopWatch.Reset();
                slow = false;
                slower = false;
                stop = false;
            }
            // change ball speed to slow down
            if (stopWatch.ElapsedMilliseconds >= 4000 && !slow)
            {
                if (ballXSpeed == 6)
                {
                    ballXSpeed = 3;
                }
                else if (ballXSpeed == -6)
                {
                    ballXSpeed = -3;
                }
                if (ballYSpeed == 6)
                {
                    ballYSpeed = 3;
                }
                else if (ballYSpeed == -6)
                {
                    ballYSpeed = -3;
                }
                slow = true;
            }

            if (stopWatch.ElapsedMilliseconds >= 5000 && !slower)
            {
                if (ballXSpeed == 3)
                {
                    ballXSpeed = 1;
                }
                else if (ballXSpeed == -3)
                {
                    ballXSpeed = -1;
                }
                if (ballYSpeed == 3)
                {
                    ballYSpeed = 1;
                }
                else if (ballYSpeed == -3)
                {
                    ballYSpeed = -1;
                }
                slower = true;
            }

            if (stopWatch.ElapsedMilliseconds >= 5500 && !stop)
            {
                ballXSpeed = 0;
                ballYSpeed = 0;
                stop = true;
            }

            //check if the side walls have been hit 
            if (ballX < 0)
            {
                ballXSpeed *= -1;
                wallHit.Play();
            }

            else if (ballX > 600)
            {
                ballXSpeed *= -1;
                wallHit.Play();
            }

            // check to see if ball hits goal
            if (ballRec.IntersectsWith(player1Goal))
            {
                goal.Play();
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";
                paddle1X = 50;
                paddle1Y = 185;

                paddle2X = 520;
                paddle2Y = 185;

                ballX = 300;
                ballY = 195;
                ballXSpeed = -6;
                ballYSpeed = -6;
            }

            else if (ballRec.IntersectsWith(player2Goal))
            {
                goal.Play();
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";
                paddle1X = 50;
                paddle1Y = 185;

                paddle2X = 520;
                paddle2Y = 185;

                ballX = 300;
                ballY = 195;
                ballXSpeed = -6;
                ballYSpeed = -6;
            }
            // check score and stop game if either player is at 3 
            if (player1Score == 3 || player2Score == 3)
            {
                gameTimer.Enabled = false;
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawEllipse(whitePen, -25, 175, 50, 50);
            e.Graphics.DrawEllipse(whitePen, 575, 175, 50, 50);
            e.Graphics.DrawLine(whitePen, 300, 0, 300, 400);
            e.Graphics.DrawLine(whitePen, 0, 0, 0, 175);
            e.Graphics.DrawLine(whitePen, 0, 400, 0, 225);
            e.Graphics.DrawLine(whitePen, 0, 0, 600, 0);
            e.Graphics.DrawLine(whitePen, 0, 400, 600, 400);
            e.Graphics.DrawLine(whitePen, 600, 0, 600, 175);
            e.Graphics.DrawLine(whitePen, 600, 400, 600, 225);

            e.Graphics.FillRectangle(whiteBrush, ballX, ballY, ballWidth, ballHeight);
            e.Graphics.FillRectangle(blueBrush, paddle2X, paddle2Y, paddleWidth, paddleHeight);
            e.Graphics.FillRectangle(redBrush, paddle1X, paddle1Y, paddleWidth, paddleHeight);
        }
    }
}
