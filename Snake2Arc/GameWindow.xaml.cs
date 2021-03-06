﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WMPLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;

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
    public partial class GameWindow: Window
    {

        //music side
        private readonly WindowsMediaPlayer player = new WindowsMediaPlayer();

        //bool to adapt code
        public bool IsNotAlone { get; set; }
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

        //observable -> get notification when list change
        public ObservableCollection<Score> LeaderBoardList
        {
            get; set;
        } = new ObservableCollection<Score>();
        public bool IsWantedMusic { get; private set; }

        public GameWindow()
        {
            InitializeComponent();
            LoadLeaderboardList();
            player.URL = "mainGameMusic.mp3";
            player.settings.setMode("loop",true);
            IsWantedMusic = true;
            IsPaused = false;
        }

        /// <summary>
        /// To restart and launch a game
        /// </summary>
        /// <param name="IsNotAlone">used to know if there is one ore two snakes(s)</param>
        private void RunGame(Boolean IsNotAlone)
        {
            this.IsNotAlone = IsNotAlone;
            IsPaused = false;
            snake1 = new Snake(Brushes.BlueViolet,true);
            if(IsNotAlone)
            {
                snake2 = new Snake(Brushes.DarkGreen,false);
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
            foodPoints.Add(GenerateItemPoint());
            foodPoints.Add(GenerateItemPoint());
            AddFoodOrPoison();
            AddFoodOrPoison();
            AddSlowOrSpeed();
            AddSlowOrSpeed();
            AddSlowOrSpeed();
        }

        /// <summary>
        /// Draw all items eatable by the snake
        /// </summary>
        private void DrawFoodsAndPoisonsAndSlowAndSpeed()
        {
            for(int i = 0;i < foodPoints.Count;i++)
            {
                DrawFood(i,foodPoints[i]);
            }
            for(int i = 0;i < poisonPoints.Count;i++)
            {
                DrawPoison(i,poisonPoints[i]);
            }
            for(int i = 0;i < speedPoints.Count;i++)
            {
                DrawSlowOrSpeed(i,speedPoints[i]);
            }
            for(int i = 0;i < slowPoints.Count;i++)
            {
                DrawSlowOrSpeed(i,slowPoints[i]);
            }
        }

        /// <summary>
        /// Draw blue points : slow or speed items
        /// </summary>
        /// <param name="index">index to insert in paintCanvas</param>
        /// <param name="point">point to change in ellipse</param>
        private void DrawSlowOrSpeed(int index,Point point)
        {
            Ellipse speedOrSlowEllipse = new Ellipse
            {
                Fill = Brushes.DeepSkyBlue,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(speedOrSlowEllipse,point.Y);
            Canvas.SetLeft(speedOrSlowEllipse,point.X);

            paintCanvas.Children.Insert(index,speedOrSlowEllipse);
        }

        /// <summary>
        /// Draw Food
        /// </summary>
        /// <param name="index">index to insert in paintCanvas</param>
        /// <param name="foodPoint">point to change in ellipse</param>
        private void DrawFood(int index,Point foodPoint)
        {
            Ellipse foodEllipse = new Ellipse
            {
                Fill = Brushes.IndianRed,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(foodEllipse,foodPoint.Y);
            Canvas.SetLeft(foodEllipse,foodPoint.X);

            paintCanvas.Children.Insert(index,foodEllipse);
        }

        /// <summary>
        /// Draw poison
        /// </summary>
        /// <param name="index">index to insert in paintCanvas</param>
        /// <param name="poisonPoint">point to change in ellipse</param>
        private void DrawPoison(int index,Point poisonPoint)
        {
            Ellipse foodEllipse = new Ellipse
            {
                Fill = Brushes.Yellow,
                Width = SNAKETHICK,
                Height = SNAKETHICK
            };

            Canvas.SetTop(foodEllipse,poisonPoint.Y);
            Canvas.SetLeft(foodEllipse,poisonPoint.X);

            paintCanvas.Children.Insert(index,foodEllipse);
        }

        /// <summary>
        /// Generate and add slow or speed
        /// </summary>
        private void AddSlowOrSpeed()
        {
            int alea = rand.Next(0,50);
            if(alea % 5 == 0)
            {
                //slow
                slowPoints.Add(GenerateItemPoint());
            }
            else
            {
                //speed
                speedPoints.Add(GenerateItemPoint());
            }
        }

        /// <summary>
        /// generate the point wher will be placed the item
        /// </summary>
        /// <returns></returns>
        private Point GenerateItemPoint()
        {
            return new Point(SnakeCeiling(rand.Next(2 * SNAKETHICK,(int)(paintCanvas.Width - 2 * SNAKETHICK))),SnakeCeiling(rand.Next(2 * SNAKETHICK,(int)(paintCanvas.Height - 2 * SNAKETHICK))));
        }

        /// <summary>
        /// Generate and add food or poison
        /// </summary>
        private void AddFoodOrPoison()
        {
            int alea = rand.Next(0,10);
            if(alea % 4 == 1 && foodPoints.Count != 0)
            {
                //malus
                poisonPoints.Add(GenerateItemPoint());
            }
            else
            {
                //bonus
                foodPoints.Add(GenerateItemPoint());
            }
        }

        /// <summary>
        /// ceiling int to fit into canvas & be at right spot
        /// </summary>
        /// <param name="entry">value to fit</param>
        /// <returns></returns>
        public static int SnakeCeiling(int entry)
        {
            return (int)(Math.Ceiling(entry / (SNAKETHICK * 1.0)) * SNAKETHICK);
        }

        /// <summary>
        /// Draw the snakes
        /// </summary>
        private void DrawSnakes()
        {
            DrawASnake(snake1);
            if(IsNotAlone)
            {
                DrawASnake(snake2);
            }
        }

        /// <summary>
        /// draw only one snake
        /// </summary>
        /// <param name="snake"></param>
        private void DrawASnake(Snake snake)
        {
            foreach(Point p in snake.SnakeBody)
            {
                Ellipse snakeEllipse = new Ellipse
                {
                    Fill = snake.SnakeColor,
                    Width = SNAKETHICK,
                    Height = SNAKETHICK
                };

                Canvas.SetTop(snakeEllipse,p.Y);
                Canvas.SetLeft(snakeEllipse,p.X);

                paintCanvas.Children.Add(snakeEllipse);
            }
            //snake's head
            Point pHead = snake.SnakeBody[0];
            Ellipse snakeHeadEllipse = new Ellipse
            {
                Fill = Brushes.White,
                Width = SNAKETHICK/2,
                Height = SNAKETHICK/2
            };

            Canvas.SetTop(snakeHeadEllipse, pHead.Y+ SNAKETHICK / 4);
            Canvas.SetLeft(snakeHeadEllipse, pHead.X + SNAKETHICK / 4);

            paintCanvas.Children.Add(snakeHeadEllipse);
        }


        /// <summary>
        /// refresh on each timer end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender,EventArgs e)
        {
            if(!IsPaused)
            {
                paintCanvas.Children.Clear();
                for(int i = 0;i < snake1.Speed;i++)
                {
                    snake1.UpdateSnake(true);
                    finalScoreSolo = snake1.Score;
                    CheckColisions();
                    CheckItems(snake1);
                }
                if(snake1.Speed == 0)
                {
                    snake1.UpdateSnake(false);
                }
                if(IsNotAlone)
                {
                    for(int i = 0;i < snake2.Speed;i++)
                    {
                        snake2.UpdateSnake(true);
                        CheckColisions();
                        CheckItems(snake2);
                    }
                    if(snake2.Speed == 0)
                    {
                        snake2.UpdateSnake(false);
                    }
                }
                DrawSnakes();
                DrawFoodsAndPoisonsAndSlowAndSpeed();

                scoreBoard.Text = $"- Score : purple = {snake1.Score}" + (IsNotAlone ? $"  | green = {snake2.Score} -" : " -");
            }
            else
            {
                scoreBoard.Text = $"PAUSED - Score : purple = {snake1.Score}" + (IsNotAlone ? $"  | green = {snake2.Score} -" : " -");
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(pauseMenu);
            }
        }

        /// <summary>
        /// Check colisions between given snake and speed or slow
        /// </summary>
        /// <param name="snake"></param>
        private void CheckItems(Snake snake)
        {
            Point head = snake.SnakeBody[0];
            
            //slow
            slowPoints.Where(p => Math.Abs(p.X - head.X) < SNAKETHICK && Math.Abs(p.Y - head.Y) < SNAKETHICK).FirstOrDefault(p =>
            {
                ApplySpeedOrSlow(snake,snake.Speed < 0 ? 1 : snake.Speed - 1);
                return slowPoints.Remove(p);
            });

            //speed
            speedPoints.Where(p => Math.Abs(p.X - head.X) < SNAKETHICK && Math.Abs(p.Y - head.Y) < SNAKETHICK).FirstOrDefault(p =>
            {
                ApplySpeedOrSlow(snake,snake.Speed >= 2 ? 2 : snake.Speed + 1);
                return speedPoints.Remove(p);
            });

            //poison
            poisonPoints.Where(p => Math.Abs(p.X - head.X) < SNAKETHICK && Math.Abs(p.Y - head.Y) < SNAKETHICK).FirstOrDefault(p =>
            {
                snake.PoisonSnake(this);
                AddNewFoodOrPoison();
                return poisonPoints.Remove(p);
            });

            //food
            foodPoints.Where(p => Math.Abs(p.X - head.X) < SNAKETHICK && Math.Abs(p.Y - head.Y) < SNAKETHICK).FirstOrDefault(p =>
            {
                snake.Eat();
                AddNewFoodOrPoison();
                return foodPoints.Remove(p);
            });
        }

        /// <summary>
        /// apply speed effect on the snake and create new speed or slow items
        /// </summary>
        /// <param name="snake"></param>
        /// <param name="speed"></param>
        private void ApplySpeedOrSlow(Snake snake,int speed)
        {
            snake.Speed = speed;
            Random r = new Random();
            int t = r.Next(0,3);
            for(int i = 0;i < t;i++)
            {
                AddSlowOrSpeed();
            }
        }
        
        /// <summary>
        /// Add all new item food or poison
        /// </summary>
        private void AddNewFoodOrPoison()
        {
            Random r = new Random();
            int t = r.Next(1,4);
            for(int i = 0;i < t;i++)
            {
                AddFoodOrPoison();
            }
        }

        /// <summary>
        /// General collision check
        /// </summary>
        private void CheckColisions()
        {
            CheckHeadOfSnake(snake1);
            CheckSelfCollision(snake1);

            if(IsNotAlone) //collisions between snakes
            {
                CheckHeadOfSnake(snake2);
                CheckSelfCollision(snake2);

                Point head2 = snake2.SnakeBody[0];
                Point head1 = snake1.SnakeBody[0];

                snake2.SnakeBody.ForEach(p => CheckOnePointCollisionSnake(p,head1,true));
                snake1.SnakeBody.ForEach(p => CheckOnePointCollisionSnake(p,head2,false));
            }
        }

        /// <summary>
        /// Check collision between one snake point and the head of a snake
        /// </summary>
        /// <param name="p"></param>
        /// <param name="head"></param>
        /// <param name="isFirstLooser"></param>
        private void CheckOnePointCollisionSnake(Point p,Point head,Boolean isFirstLooser)
        {
            if(Math.Abs(p.X - head.X) < SNAKETHICK && Math.Abs(p.Y - head.Y) < SNAKETHICK)
            {
                EndGame(isFirstLooser);
            }
        }

        /// <summary>
        /// Check colision between given snake and himself
        /// </summary>
        /// <param name="snake"></param>
        private void CheckSelfCollision(Snake snake)
        {
            Point head = snake.SnakeBody[0];
            for(int i = 1;i < snake.SnakeBody.Count;i++)
            {
                Point point = new Point(snake.SnakeBody[i].X,snake.SnakeBody[i].Y);
                CheckOnePointCollisionSnake(point,head,snake.SnakeColor.ToString() == "#FF8A2BE2");
            }
        }

        /// <summary>
        /// Check colision between head of snake and the gameboard borders
        /// </summary>
        /// <param name="snake"></param>
        private void CheckHeadOfSnake(Snake snake)
        {
            if(snake.SnakeBody[0].X < 0 + SNAKETHICK
                || snake.SnakeBody[0].X >= paintCanvas.ActualWidth - SNAKETHICK
                || snake.SnakeBody[0].Y < 0 + SNAKETHICK
                || snake.SnakeBody[0].Y >= paintCanvas.ActualHeight - SNAKETHICK)
            {
                EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2");
            }
        }

        /// <summary>
        /// keyboard management
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonKeyDown(object sender,KeyEventArgs e)
        {
            switch(e.Key)
            {
                //pause treatment
                case Key.Escape:
                    if(IsPaused)
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
            if(IsNotAlone)
            {
                switch(e.Key)
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

        /// <summary>
        /// End game management
        /// </summary>
        /// <param name="isFirstLooser"></param>
        public void EndGame(bool isFirstLooser)
        {
            //stop refresh
            timer.Tick -= new EventHandler(TimerTick);
            if(IsNotAlone)
            {
                gameOver2Player.Visibility = Visibility.Visible;
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(gameOver2Player);
                whoLose2Player.Text = "GAME OVER, " + (isFirstLooser ? "Purple" : "Green") + " Loosed\n" + (!isFirstLooser ? "Purple" : "Green") + " wins.";
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

        private void BtnCloseClick(object sender,RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Play_Click(object sender,RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            RunGame(false);
        }

        private void Button_Play_Double_Click(object sender,RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            RunGame(true);
        }

        private void Button_LeaderBoard_Click(object sender,RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Collapsed;
            leaderBoard.Visibility = Visibility.Visible;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(leaderBoard);
        }

        private void Button_Options_Click(object sender,RoutedEventArgs e)
        {
            optionMenu.Visibility = Visibility.Visible;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(optionMenu);
        }

        private void Button_return_menu_click(object sender,RoutedEventArgs e)
        {
            mainMenu.Visibility = Visibility.Visible;
            leaderBoard.Visibility = Visibility.Collapsed;
            IsPaused = false;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(mainMenu);
            scoreBoard.Text = "Snake2Arc";
        }

        /// <summary>
        /// Check if the score must be stored in leaderboard before returning to the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_return_menu_click_after_game(object sender,RoutedEventArgs e)
        {
            gameOver1Player.Visibility = Visibility.Collapsed;
            if(LeaderBoardList.Count < 5 || finalScoreSolo > LeaderBoardList.Min(x => x.ScoreValue))
            {
                addNewScore.Visibility = Visibility.Visible;
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(addNewScore);
            }
            else
            {
                mainMenu.Visibility = Visibility.Visible;
                IsPaused = false;
                paintCanvas.Children.Clear();
                paintCanvas.Children.Add(mainMenu);
                scoreBoard.Text = "Snake2Arc";
            }
        }

        private void Button_add_new_score(object sender,RoutedEventArgs e)
        {
            playerNameAdded.Focus();
            if(LeaderBoardList.Count == 5)
            {
                Score scoreAbove = LeaderBoardList.OrderByDescending(x => x.ScoreValue).FirstOrDefault(x => x.ScoreValue > finalScoreSolo);
                if(scoreAbove != null)
                {
                    LeaderBoardList.Insert(LeaderBoardList.IndexOf(scoreAbove) + 1,new Score() { Name = playerNameAdded.Text,ScoreValue = finalScoreSolo });
                }
                else if(LeaderBoardList.Count == 5 && finalScoreSolo > LeaderBoardList.Max(x => x.ScoreValue))
                {
                    LeaderBoardList.Insert(0,new Score() { Name = playerNameAdded.Text,ScoreValue = finalScoreSolo });
                }
            }
            else if(LeaderBoardList.Count < 5)
            {
                LeaderBoardList.Add(new Score() { Name = playerNameAdded.Text,ScoreValue = finalScoreSolo });
            }
            if(LeaderBoardList.Count > 5)
                LeaderBoardList.Remove(LeaderBoardList.Last());
            SaveLeaderBoardList();

            playerNameAdded.Text = "";
            addNewScore.Visibility = Visibility.Collapsed;
            leaderBoard.Visibility = Visibility.Visible;
            paintCanvas.Children.Clear();
            paintCanvas.Children.Add(leaderBoard);
        }

        private void Button_resume_click(object sender,RoutedEventArgs e)
        {
            IsPaused = false;
            pauseMenu.Visibility = Visibility.Collapsed;
        }

        private void Button_leave_click(object sender,RoutedEventArgs e)
        {
            pauseMenu.Visibility = Visibility.Collapsed;
            mainMenu.Visibility = Visibility.Visible;
            timer.Tick -= new EventHandler(TimerTick);
            KeyDown -= new KeyEventHandler(OnButtonKeyDown);
            IsPaused = false;
            paintCanvas.Children.Add(mainMenu);
            scoreBoard.Text = "Snake2Arc";
        }

        /// <summary>
        /// Change the snake Thickness with slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Snake_Thick(object sender,RoutedEventArgs e)
        {
            SNAKETHICK = (int)thickSlider.Value;
        }

        /// <summary>
        /// Music management
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Check_Music(object sender,RoutedEventArgs e)
        {
            IsWantedMusic = !IsWantedMusic;
            if(!IsWantedMusic)
            {
                player.controls.pause();
            }
            else
            {
                player.controls.play();
            }
        }
        /// <summary>
        /// Load leaderBoardList stored in XML
        /// </summary>
        private void LoadLeaderboardList()
        {
            if(File.Exists("leadeBoardList.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                using(Stream reader = new FileStream("leadeBoardList.xml",FileMode.Open))
                {
                    List<Score> temporaryList = (List<Score>)serializer.Deserialize(reader);
                    LeaderBoardList.Clear();
                    foreach(var score in temporaryList.OrderByDescending(x => x.ScoreValue))
                        LeaderBoardList.Add(score);
                }
            }
        }
        /// <summary>
        /// save LeaderBoardList in XML
        /// </summary>
        private void SaveLeaderBoardList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Score>));
            using(Stream writer = new FileStream("leadeBoardList.xml",FileMode.Create))
            {
                serializer.Serialize(writer,LeaderBoardList);
            }
        }
    }
}
