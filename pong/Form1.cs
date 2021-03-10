using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace pong
{
    public partial class Form1 : Form
    {

        int paddle1X = 10;
        int paddle1Y = 100;
        int player1Score = 0;

        int paddle2X = 10;
        int paddle2Y = 300;
        int player2Score = 0;

        int paddleWidth = 10;
        int paddleHeight = 60;
        int paddleSpeed = 6;

        int ballX = 550;
        int ballY = 195;
        int ballXSpeed = -6;
        int ballYSpeed = -6;
        int ballIncSpeed = 2;
        int ballWidth = 10;
        int ballHeight = 10;


        bool activePaddle;
        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftDown = false;
        bool rightDown = false;

        Rectangle player1Rec;
        Rectangle player2Rec;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush redBrush = new SolidBrush(Color.IndianRed);

        SolidBrush lBlueBrush = new SolidBrush(Color.LightBlue);
        SolidBrush lRedBrush = new SolidBrush(Color.LightPink);

        Pen whitePen = new Pen(Color.White);

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

            if (dDown == true && paddle1X < this.Width - paddleWidth)
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

            if (leftDown == true && paddle2X > 0)
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
            }

            //create Rectangles of objects on screen to be used for collision detection 
            if (activePaddle)
            {
                player1Rec = new Rectangle(paddle1X, paddle1Y, paddleWidth, paddleHeight);
                player2Rec = new Rectangle(paddle2X, paddle2Y, 0, 0);
            }
            else
            {
                player2Rec = new Rectangle(paddle2X, paddle2Y, paddleWidth, paddleHeight);
                player1Rec = new Rectangle(paddle1X, paddle1Y, 0, 0);
            }

            Rectangle ballRec = new Rectangle(ballX, ballY, ballWidth, ballHeight);

            //check if ball hits either paddle. If it does change the direction 
            //and place the ball in front of the paddle hit 
            if (player1Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle1X + paddleWidth + 1;
                activePaddle = !activePaddle;
            }
            else if (player2Rec.IntersectsWith(ballRec))
            {
                ballXSpeed *= -1;
                ballX = paddle2X + ballWidth + 1;
                activePaddle = !activePaddle;
            }

            //check if a player missed the ball and if true add 1 to score of other player  
            if (ballX < 0)
            {
                if (activePaddle)
                {
                    player2Score++;

                    p2ScoreLabel.Text = $"{player2Score}";
                }
                else
                {
                    player1Score++;

                    p1ScoreLabel.Text = $"{player1Score}";
                }


                ballX = 550;
                ballY = 195;

                ballXSpeed = -6;
                ballYSpeed = -6;

                paddle1Y = 100;
                paddle2Y = 300;
                paddle1X = 10;
                paddle2X = 10;
                activePaddle = !activePaddle;
            }
            else if (ballX > 600)
            {
                ballXSpeed = ballXSpeed + ballIncSpeed;
                ballYSpeed = ballYSpeed + ballIncSpeed;

                ballXSpeed *= -1;

             

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
            e.Graphics.FillRectangle(whiteBrush, ballX, ballY, ballWidth, ballHeight);



            

            if (activePaddle)
            {
                e.Graphics.FillRectangle(blueBrush, paddle2X, paddle2Y, paddleWidth, paddleHeight);

                e.Graphics.FillRectangle(redBrush, paddle1X, paddle1Y, paddleWidth, paddleHeight);
                e.Graphics.DrawRectangle(whitePen, paddle1X, paddle1Y, paddleWidth, paddleHeight);
            }
            else
            {
                e.Graphics.FillRectangle(redBrush, paddle1X, paddle1Y, paddleWidth, paddleHeight);

                e.Graphics.FillRectangle(blueBrush, paddle2X, paddle2Y, paddleWidth, paddleHeight);
                e.Graphics.DrawRectangle(whitePen, paddle2X, paddle2Y, paddleWidth, paddleHeight);
            }
        }
    }
}
