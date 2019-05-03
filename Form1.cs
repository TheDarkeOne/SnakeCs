using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle Food = new Circle(), Food2 = new Circle(), Food3 = new Circle();
        private bool cont1 = true, cont2 = true;
        private int i = 0,i2 = 1, tempInt = 0, tempInt2;


        public Form1()
        {
            InitializeComponent();

            //Set settings to defualt
            new Settings();

            //Set game speed and Start Timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += new EventHandler(UpdateScreen);
            
            //Starts the game timer
            gameTimer.Start();

            //Start the game
            StartGame();
        }

        private void StartGame()
        {
            //Reset the changed variables to original values
            lblGameOver.Visible = false;
            i = 0;
            gameTimer.Interval = 1000 / Settings.Speed;

            //Temporarily saves the score and max score
            tempInt = Settings.Score;
            tempInt2 = Settings.maxScore;


            
            //Set settings to defualt
            new Settings();

            //Sets the win condition
            Settings.winPoints = (5000 * i2);

            //Checks to see if the new score was greater than the high score
            if (tempInt > tempInt2) {
                //Sets max score to tempInt and then sets the text of maxlbl to the max score
                Settings.maxScore = tempInt;
                maxlbl.Text = Settings.maxScore.ToString();
            }
            else
            {
                //Sets max score to tempInt2 and then sets the text of maxlbl to the max score
                Settings.maxScore = tempInt2;
                maxlbl.Text = Settings.maxScore.ToString();
            }

            //Create a new Player object
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            //Set the text box to the beginning score and generate a food to eat
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        //Place a Food at a random place on the board
        private void GenerateFood()
        {

            //Initializes the variables
            cont1 = true;
            cont2 = true;
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            //Puts food on a random place on the board
            Random random = new Random();
            Food = new Circle();
            Food.X = random.Next(1, maxXPos-1);
            Food.Y = random.Next(1, maxYPos-1);


            while (cont1 == true) {
                //Puts food on a random place on the board if the generate place isn't the same as the already generated food
                Food2 = new Circle();
                Food2.X = random.Next(1, maxXPos - 1);
                Food2.Y = random.Next(1, maxYPos - 1);
                if (Food2.X != Food.X || Food2.Y != Food.Y) {
                    cont1 = false;
                }
            }

            while (cont2 == true)
            {
                //Puts food on a random place on the board if the generate place isn't the same as the already generated food
                Food3 = new Circle();
                Food3.X = random.Next(1, maxXPos - 1);
                Food3.Y = random.Next(1, maxYPos - 1);
                if (Food3.X != Food.X || Food3.Y != Food.Y && Food3.X != Food2.X || Food3.Y != Food2.Y)
                {
                    cont2 = false;
                }
            }
        }

        
        private void UpdateScreen(object sender, EventArgs e)
        {
            //Keeps track of the key Up events
            KeyUp += new KeyEventHandler(OnKeyUp);

            //Check if game over has happened
            if (Settings.GameOver != true)
            {
                //moves the player
                movePlayer();
            }

            //Invalidates the game screen
            pbCanvas.Invalidate();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            //Keeps track of what button was pressed
            int key = (int)e.KeyCode;

            //Checks if the game is over
            if (Settings.GameOver == true)
            {
                //Checks if the enter key has been pressed
                if ((key == (int)Keys.Enter))
                {
                    //Starts the game
                    StartGame();
                }
            }
            else
            {
                //Checks which key has been pressed and then sets the direction to that one
                if ((key == (int)Keys.Left) && (Settings.direction != Direction.Right))
                {
                    Settings.direction = Direction.Left;
                }
                if ((key == (int)Keys.Right) && (Settings.direction != Direction.Left))
                {
                    Settings.direction = Direction.Right;
                }
                if ((key == (int)Keys.Up) && (Settings.direction != Direction.Down))
                {
                    Settings.direction = Direction.Up;
                }
                if ((key == (int)Keys.Down) && (Settings.direction != Direction.Up))
                {
                    Settings.direction = Direction.Down;
                }
            }
        }


        private void PbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            //checks if the game isn't over
            if (!Settings.GameOver)
            {

                Brush snakeColor;

                //Loops through the snake
                for (int i = 0; i < Snake.Count; i++) {
                    if (i == 0)
                    {
                        //sets the head color to black
                        snakeColor = Brushes.Black;
                    }
                    else {
                        //sets the body colors to blue
                        snakeColor = Brushes.Blue;
                    }

                    //Draws the snake to the screen
                    canvas.FillEllipse(snakeColor, new Rectangle(Snake[i].X * Settings.Width,
                                                                 Snake[i].Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));

                    //Draws the Foods to the screen
                    canvas.FillEllipse(Brushes.Red, new Rectangle(Food.X * Settings.Width,
                                                                 Food.Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));

                    canvas.FillEllipse(Brushes.Silver, new Rectangle(Food2.X * Settings.Width,
                                                                 Food2.Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));

                    canvas.FillEllipse(Brushes.Goldenrod, new Rectangle(Food3.X * Settings.Width,
                                                                 Food3.Y * Settings.Height,
                                                                 Settings.Width, Settings.Height));
                }
            }
            else {
                //checks if you have won
                if (!Settings.Win)
                {
                    //Displays the game over message to the screen
                    string gameOver = "Game over! \nYour final score is: " + Settings.Score + " \nReach "+ Settings.winPoints + " To win." + " \nPress Enter to play again.";
                    lblGameOver.Text = gameOver;
                }
                else {
                    //Displays the win message to the screen
                    string Win = "You Won! \nYour final score is: " + Settings.Score + " \nYou reached " + Settings.winPoints + " \nPress Enter to play again.";
                    lblGameOver.Text = Win;
                }
                //sets the game over message to visible
                lblGameOver.Visible = true;
            }
        }

        private void movePlayer() {
            //moves the snake in the direction specified in settings
            for (int i = Snake.Count - 1; i >= 0; i--) {
                if (i == 0)
                {
                    //Changes the position based on what direction is set
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    //Finds the max X position and max Y position
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //Checks if the snake has collided with the walls and if it has game over
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos) {
                        Die();
                    }


                    for (int j = 1; j < Snake.Count; j++) {
                        //Checks if the snake has run into itself
                        if (Snake[i].X == Snake[j].X &&
                            Snake[i].Y == Snake[j].Y) {
                            Die();
                        }
                    }

                    //Checks if the snake has collided with food if it has eat it
                    if (Snake[0].X == Food.X && Snake[0].Y == Food.Y || Snake[0].X == Food2.X && Snake[0].Y == Food2.Y || Snake[0].X == Food3.X && Snake[0].Y == Food3.Y) {
                        Eat();
                    }
                }
                else {
                    //Move the snakes body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Die() {
            //Sets the interval time to the original speed and then sets game over to true
            gameTimer.Interval = 1000 / Settings.Speed;
            Settings.GameOver = true;
        }

        private void Eat() {
            //creates a new circle and sets its position behind the last snake pieces coordinates
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            //Checks which food you have eaten and then sets the speed and score to the appropriate ones
            if (Snake[0].X == Food.X && Snake[0].Y == Food.Y) {
                gameTimer.Interval = 1000 / Settings.Speed;
                if (i > 0) {
                    i--;
                }
                Settings.Score += Settings.Points;
            } else if (Snake[0].X == Food2.X && Snake[0].Y == Food2.Y)
            {
                gameTimer.Interval = 1000 / (Settings.Speed*2);
                Settings.Score += Settings.Points*3;

            } else if (Snake[0].X == Food3.X && Snake[0].Y == Food3.Y)
            {
                gameTimer.Interval = 1000 / (Settings.Speed * (2 + i));
                if (i < 1)
                {
                    i++;
                }
                Settings.Score += Settings.Points * 6;
            }


            //Adds the new circle to the snake
            Snake.Add(food);

            //Updates the score and then checks if you have won
            lblScore.Text = Settings.Score.ToString();
            if (Settings.Score >= Settings.winPoints) {
                //sets win to true and then sets game over to true
                Settings.Win = true;
                Settings.GameOver = true;

                //increments winScore multiplier
                i2++;
            }

            //Generates food
            GenerateFood();
        }
    }
}