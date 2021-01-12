using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WMPLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace Snake2Arc
{

    //all directions
    public enum DIRECTION
    {
        UP = 1, DOWN = 0, LEFT = -1, RIGHT = 2
    }

    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        //music side
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        //bool to adapt code
        public bool IsNotAlone { get; set; }
        private bool IsDisplayingEnd { get; set; }
        private bool IsPaused { get; set; }

        //things to eat
        private readonly List<Point> foodPoints = new List<Point>();
        private readonly List<Point> poisonPoints = new List<Point>();
        private readonly List<Point> speedPoints = new List<Point>();
        private readonly List<Point> slowPoints = new List<Point>();

        //snakes management
        private Snake snake1;
        private Snake snake2;
       
        //refresh delay
        private TimeSpan REFRESHDELAY = new TimeSpan(1000000);

        //stock wining score/lastScore
        private int finalScoreSolo;

        //snakes thick
        public static int SNAKETHICK = 10;

        //random number for food spawning
        private readonly Random rand = new Random();

        private DispatcherTimer timer;
     

        public ObservableCollection<Score> LeaderBoardList              //observable -> get notification when list change
        {
            get; set;
        } = new ObservableCollection<Score>();
        public bool IsWantedMusic { get; private set; }

        public GameWindow()
        {
            InitializeComponent();
            player.URL = "mainGameMusic.mp3";
            player.settings.setMode("loop",true);
            IsWantedMusic = true;
            IsPaused = false;
        }

        //to restart and relaunch a game
        private void RunGame(Boolean IsNotAlone)
        {
            this.IsNotAlone = IsNotAlone;
            IsDisplayingEnd = false;
            IsPaused = false;
            snake1 = new Snake(Brushes.BlueViolet, true);
            if (IsNotAlone)
            {
                snake2 = new Snake(Brushes.DarkGreen, false);
            }

            //refresh managment
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = REFRESHDELAY;
            timer.Start();
            
            //reset things to eat
            foodPoints.Clear();
            poisonPoints.Clear();
            speedPoints.Clear();
            slowPoints.Clear();

            //keyboard managment
            KeyDown += new KeyEventHandler(OnButtonKeyDown);

            //add first set of items
            AddFood();
            AddFood();
            AddFoodOrPoison();
            AddFoodOrPoison();
            AddSlowOrSpeed();
            AddSlowOrSpeed();
            AddSlowOrSpeed();
        }

        //draw all items
        private void DrawFoodsAndPoisonsAndSlowAndSpeed()
        {
            for (int i = 0; i < foodPoints.Count; i++)
            {
                DrawFood(i, foodPoints[i]);
            }
            for (int i = 0; i < poisonPoints.Count; i++)
            {
                DrawPoison(i, poisonPoints[i]);
            }
            for (int i = 0; i < speedPoints.Count; i++)
            {
                DrawSlowOrSpeed(i, speedPoints[i]);
            }
            for (int i = 0; i < slowPoints.Count; i++)
            {
                DrawSlowOrSpeed(i, slowPoints[i]);
            }
        }

        //draw slow or speed
        private void DrawSlowOrSpeed(int index, Point point)
        {
            Ellipse speedOrSlowEllipse = new Ellipse
            {
                Fill = Brushes.DeepSkyBlue,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(speedOrSlowEllipse, point.Y);
            Canvas.SetLeft(speedOrSlowEllipse, point.X);

            paintCanvas.Children.Insert(index, speedOrSlowEllipse);
        }

        //draw food
        private void DrawFood(int index, Point foodPoint)
        {
            Ellipse foodEllipse = new Ellipse
            {
                Fill = Brushes.IndianRed,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(foodEllipse, foodPoint.Y);
            Canvas.SetLeft(foodEllipse, foodPoint.X);

            paintCanvas.Children.Insert(index, foodEllipse);
        }
        
        //draw poison
        private void DrawPoison(int index, Point poisonPoint)
        {
            Ellipse foodEllipse = new Ellipse
            {
                Fill = Brushes.Yellow,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(foodEllipse, poisonPoint.Y);
            Canvas.SetLeft(foodEllipse, poisonPoint.X);

            paintCanvas.Children.Insert(index, foodEllipse);
        }

        //generate and add slow or speed
        private void AddSlowOrSpeed()
        {
            int alea = rand.Next(0, 50);
            if (alea%5==0)
            {
                //slow
                Point slowPoint = new Point(SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Width - 2*SNAKETHICK))), SnakeCeiling(rand.Next(0 + 2*SNAKETHICK, (int)(paintCanvas.Height - 2*SNAKETHICK))));
                slowPoints.Add(slowPoint);
            }
            else
            {
                //speed
                Point speedPoint = new Point(SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Width - 2 * SNAKETHICK))), SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Height - 2 * SNAKETHICK))));
                speedPoints.Add(speedPoint);
            }
        }

        //generate and add food or poison
        private void AddFoodOrPoison()
        {
            int alea = rand.Next(0, 10);
            if (alea % 4 == 2 && foodPoints.Count != 0)
            {
                //malus
                Point poisonPoint = new Point(SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Width - 2 * SNAKETHICK))), SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Height - 2 * SNAKETHICK))));
                poisonPoints.Add(poisonPoint);
            }
            else
            {
                Point foodPoint = new Point(SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Width - 2 * SNAKETHICK))), SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Height - 2 * SNAKETHICK))));
                foodPoints.Add(foodPoint);
            }
        }

        //ceiling int to fit into canvas & be at right spot
        private int SnakeCeiling(int entry)
        {
            return (int)Math.Ceiling(entry / (SNAKETHICK * 1.0)) * SNAKETHICK;
        }

        //generate and add a food
        private void AddFood()
        {
            Point foodPoint = new Point(SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Width - 2 * SNAKETHICK))), SnakeCeiling(rand.Next(0 + 2 * SNAKETHICK, (int)(paintCanvas.Height - 2 * SNAKETHICK))));
            foodPoints.Add(foodPoint);
        }

        //draw the snakes
        private void DrawSnakes()
        {
            DrawASnake(snake1);
            if (IsNotAlone)
            {
                DrawASnake(snake2);
            }
        }

        //draw only one snake
        private void DrawASnake(Snake snake)
        {
            foreach (Point p in snake.SnakeBody)
            {
                Ellipse snakeEllipse = new Ellipse
                {
                    Fill = snake.SnakeColor,
                    Width = SNAKETHICK,
                    Height = SNAKETHICK
                };

                Canvas.SetTop(snakeEllipse, p.Y);
                Canvas.SetLeft(snakeEllipse, p.X);

                paintCanvas.Children.Add(snakeEllipse);
            }
        }

        
        //refresh on each timer end
        private void TimerTick(object sender, EventArgs e)
        {
            if (!IsPaused)
            {
                paintCanvas.Children.Clear();
                for (int i = 0; i < snake1.Speed; i++)
                {
                    snake1.UpdateSnake(true);
                    finalScoreSolo = snake1.Score;
                    CheckColisions();
                    CheckFood(snake1);
                    CheckPoison(snake1);
                    CheckSpeedOrSlow(snake1);
                }
                if (snake1.Speed == 0)
                {
                    snake1.UpdateSnake(false);
                }
                if (IsNotAlone)
                {
                    for (int i = 0; i < snake2.Speed; i++)
                    {
                        snake2.UpdateSnake(true);
                        CheckColisions();
                        CheckFood(snake2);
                        CheckPoison(snake2);
                        CheckSpeedOrSlow(snake2);
                    }
                    if (snake2.Speed == 0)
                    {
                        snake2.UpdateSnake(false);
                    }
                }
                DrawSnakes();
                DrawFoodsAndPoisonsAndSlowAndSpeed();
            
                scoreBoard.Text = $"- Score : Player1(purple)={snake1.Score}" + (IsNotAlone ? $"  | Player2(green)={snake2.Score} -" : " -");
            }
            else
            {
                scoreBoard.Text = $"PAUSED - Score : Player1(purple)={snake1.Score}" + (IsNotAlone ? $"  | Player2(green)={snake2.Score} -" : " -");
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(pauseMenu);
            }
        }

        //check colisions between given snake and speed or slow
        private void CheckSpeedOrSlow(Snake snake)
        {
            Point head = snake.SnakeBody[0];

            foreach (Point p in slowPoints)
            {
                if ((Math.Abs(p.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head.Y) < (SNAKETHICK)))
                {
                    snake.Speed = snake.Speed < 0 ? 1 : snake.Speed - 1;
                    slowPoints.Remove(p);
                    Random r = new Random();
                    int t = r.Next(0, 3);
                    for (int i = 0; i < t; i++)
                    {
                        AddSlowOrSpeed();
                    }
                    break;
                }
            }
            foreach (Point p in speedPoints)
            {
                if ((Math.Abs(p.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head.Y) < (SNAKETHICK)))
                {
                    snake.Speed = snake.Speed >= 2 ? 2 : snake.Speed + 1;
                    speedPoints.Remove(p);
                    Random r = new Random();
                    int t = r.Next(0, 3);
                    for (int i = 0; i < t; i++)
                    {
                        AddSlowOrSpeed();
                    }
                    break;
                }
            }
        }


        //check colision between given snake and poison
        private void CheckPoison(Snake snake)
        {
            Point head = snake.SnakeBody[0];

            foreach (Point p in poisonPoints)
            {
                if ((Math.Abs(p.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head.Y) < (SNAKETHICK)))
                {
                    snake.PoisonSnake(this);
                    snake.PoisonSnake(this);//poison Twice to be punitive
                    poisonPoints.Remove(p);
                  
                    Random r = new Random();
                    int t = r.Next(0, 3);
                    for (int i = 0; i < t; i++)
                    {
                        AddFoodOrPoison();
                    }
                    break;
                }
            }
        }

        //check colision between given snake and food
        private void CheckFood(Snake snake)
        {
            Point head = snake.SnakeBody[0];

            foreach (Point p in foodPoints)
            {
                if ((Math.Abs(p.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head.Y) < (SNAKETHICK)))
                {
                    snake.Eat();
                    foodPoints.Remove(p);
                    
                    Random r = new Random();
                    int t = r.Next(0, 4);
                    for (int i = 0; i < t; i++)
                    {
                        AddFoodOrPoison();
                    }
                    break;
                }
            }
        }

        //general collision check
        private void CheckColisions()
        {
            CheckHeadOfSnake(snake1);
            CheckSelfCollision(snake1);
            if (IsNotAlone)
            {
                CheckHeadOfSnake(snake2);
                CheckSelfCollision(snake2);

                Point head2 = snake2.SnakeBody[0];

                //collisions between snakes
                Point head1 = snake1.SnakeBody[0];

                foreach (Point p in snake2.SnakeBody)
                {
                    if ((Math.Abs(p.X - head1.X) < (SNAKETHICK)) &&
                         (Math.Abs(p.Y - head1.Y) < (SNAKETHICK)))
                    {
                        if (!IsDisplayingEnd)
                        {
                            EndGame(true);
                        }
                        break;
                    }
                }
                foreach (Point p in snake1.SnakeBody)
                {
                    if ((Math.Abs(p.X - head2.X) < (SNAKETHICK)) &&
                         (Math.Abs(p.Y - head2.Y) < (SNAKETHICK)))
                    {
                        if (!IsDisplayingEnd)
                        {
                            EndGame(false);
                        }
                        break;
                    }
                }
            }
        }

        //check colision between given snake and himself
        private void CheckSelfCollision(Snake snake)
        {
            Point head = snake.SnakeBody[0];
            for (int i = 1; i < snake.SnakeBody.Count; i++)
            {
                Point point = new Point(snake.SnakeBody[i].X, snake.SnakeBody[i].Y);
                if ((Math.Abs(point.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(point.Y - head.Y) < (SNAKETHICK)))
                {
                    if (!IsDisplayingEnd)
                    {
                        EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2");
                    }
                    break;
                }
            }
        }

        //check colision between head of snake and the gameboard borders
        private void CheckHeadOfSnake(Snake snake)
        {
                if (snake.SnakeBody[0].X < 0 + SNAKETHICK
                    || snake.SnakeBody[0].X >= paintCanvas.ActualWidth - 1 * SNAKETHICK
                    || snake.SnakeBody[0].Y < 0 + SNAKETHICK
                    || snake.SnakeBody[0].Y >= paintCanvas.ActualHeight - 1 * SNAKETHICK)
                {
                    if (!IsDisplayingEnd)
                    {
                        EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2");
                    }
                }
        }

        //keyboard management
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //pause treatment
                case Key.Escape:
                    if (IsPaused)
                    {
                        IsPaused = false;
                        pauseMenu.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        IsPaused = true;
                        pauseMenu.Visibility = Visibility.Visible;
                    }
                    break;
                //player1
                case Key.Down:
                    snake1.ChangeSnakeDirection(DIRECTION.DOWN);
                    break;
                case Key.Up:
                    snake1.ChangeSnakeDirection(DIRECTION.UP);
                    break;
                case Key.Left:
                    snake1.ChangeSnakeDirection(DIRECTION.LEFT);
                    break;
                case Key.Right:
                    snake1.ChangeSnakeDirection(DIRECTION.RIGHT);
                    break;
            }
            if (IsNotAlone)
            {
                switch (e.Key)
                {
                    //player2
                    case Key.S:
                        snake2.ChangeSnakeDirection(DIRECTION.DOWN);
                        break;
                    case Key.Z:
                        snake2.ChangeSnakeDirection(DIRECTION.UP);
                        break;
                    case Key.Q:
                        snake2.ChangeSnakeDirection(DIRECTION.LEFT);
                        break;
                    case Key.D:
                        snake2.ChangeSnakeDirection(DIRECTION.RIGHT);
                        break;
                }
            }
        }

        //end game management
        public void EndGame(bool isFirstLooser)
        {
            //stop refresh
            timer.Tick -= new EventHandler(TimerTick);
            if (IsNotAlone) { 
                gameOver2Player.Visibility = Visibility.Visible;
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(gameOver2Player);
                whoLose2Player.Text = "GAME OVER, Player " + (isFirstLooser ? "1 " : "2 ")+"\nLoosed\n Player "+ (!isFirstLooser ? "1 " : "2 ")+" wins.";
            }
            else
            {
                gameOver1Player.Visibility = Visibility.Visible;
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(gameOver1Player);
                scoreWin1Player.Text = finalScoreSolo.ToString();
            }

        }

        //button event management

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Play_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            RunGame(false);
        }

        private void Button_Play_Double_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            RunGame(true);
        }

        private void Button_LeaderBoard_Click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            leaderBoard.Visibility = Visibility.Visible;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(leaderBoard);
        }

        private void Button_Options_Click(object sender, RoutedEventArgs e)
        {
            optionMenu.Visibility = Visibility.Visible;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(optionMenu);
        }

        private void Button_return_menu_click(object sender, RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Visible;
            leaderBoard.Visibility = Visibility.Collapsed;
            IsPaused = false;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(mainMenu);
            scoreBoard.Text = "Snake2Arc";
        }
        
        private void Add_new_score(object sender, RoutedEventArgs e)
        {

        }

        private void Button_resume_click(object sender, RoutedEventArgs e)
        {
            IsPaused = false;
            pauseMenu.Visibility = Visibility.Collapsed;
        }
        
        private void Button_leave_click(object sender, RoutedEventArgs e)
        {
            pauseMenu.Visibility = Visibility.Collapsed;
            mainMenu.Visibility = Visibility.Visible;
            timer.Tick -= new EventHandler(TimerTick);
            KeyDown -= new KeyEventHandler(OnButtonKeyDown);
            IsPaused = false;
            paintCanvas.Children.Add(mainMenu);
            scoreBoard.Text = "Snake2Arc";
        }

        private void Change_Snake_Thick(object sender, RoutedEventArgs e)
        {
            if ((int)thickSlider.Value != 15) { 
            GameWindow.SNAKETHICK = (int)thickSlider.Value;
            }
            else
            {
                thickSlider.Value = 10;
            }
        }

        //music management
        private void Change_Check_Music(object sender, RoutedEventArgs e)
        {
            IsWantedMusic = !IsWantedMusic;
            if (!IsWantedMusic)
            {
                player.controls.pause();
            }
            else
            {
                player.controls.play();
            }
        }
    }
}
