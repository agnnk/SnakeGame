using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form2 : Form
    {
        
        private List<Circle> Snake = new List<Circle>();
        private Circle fruit = new Circle();
        public Form2()
        {
            InitializeComponent();
            new Settings();
            timer.Interval = 1500 / Settings.Speed;
            timer.Tick += updateScreen;
            timer.Start();
            startGame();
        }
        private void updateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if(Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }
                movePlayer();
            }
            pbArea.Invalidate();
        }
        private void movePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Up:
                            Snake[i].Y--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }
                    int maxXpos = pbArea.Size.Width / Settings.Width;
                    int maxYpos = pbArea.Size.Height / Settings.Height;
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXpos || Snake[i].Y > maxYpos)
                    {
                        death();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            death();
                        }
                    }
                    if (Snake[0].X == fruit.X && Snake[0].Y == fruit.Y)
                    {
                        eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                } 
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            Graphics area = e.Graphics;
            if(Settings.GameOver == false)
            {
                Brush snakeColor;
                for(int i = 0; i <Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.OliveDrab;
                    }
                    else
                    {
                        snakeColor = Brushes.GreenYellow;
                    }
                    area.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));
                    area.FillEllipse(Brushes.Red, new Rectangle(fruit.X * Settings.Width, fruit.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "Игра окончена \n" + "Ваш счет:" + Settings.Score + "\nНажмите Enter для повтора";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }
        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);
            label2.Text = Settings.Score.ToString();
            generateFruit();
        }
        private void generateFruit()
        {
            int maxXpos = pbArea.Size.Width / Settings.Width;
            int maxYpos = pbArea.Size.Height / Settings.Height;
            Random r = new Random();
            fruit = new Circle { X = r.Next(0, maxXpos), Y = r.Next(0, maxYpos) };
        }
        private void eat()
        {
            Circle body = new Circle { X = Snake[Snake.Count - 1].X, Y = Snake[Snake.Count - 1].Y };
            Snake.Add(body);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            generateFruit();
        }
        private void death()
        {
            Settings.GameOver = true;
        }
    }
}
