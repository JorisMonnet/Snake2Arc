using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake2Arc{


   
    //all directions
    public enum DIRECTION
    {
        UP = 1, DOWN = 0, LEFT = -1, RIGHT = 2
    }

    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window{

        //bool to adapt code
        public bool IsNotAlone { get; set; }
        private bool IsDisplayingEnd { get; set; }
        private bool IsPaused { get; set; }
        //things to eat
        private readonly List<Point> foodPoints = new List<Point>();
        private readonly List<Point> poisonPoints = new List<Point>();

        //snakes management
        private Snake snake1;
        private Snake snake2;

        //refresh delay
        private TimeSpan REFRESHDELAY = new TimeSpan(1000000);


        //snakes thick
        public const int SNAKETHICK = 10;

        //random number for food spawning
        private readonly Random rand = new Random();

        

        public GameWindow() 
        {
            InitializeComponent();
            IsPaused = true;
        }

        private void RunGame(Boolean IsNotAlone)
        {
            this.IsNotAlone = IsNotAlone;//set To TRUE to with 2 snakes
            IsDisplayingEnd = false;
            IsPaused = false;
            snake1 = new Snake(Brushes.BlueViolet,true);
            if(IsNotAlone)
            {
                snake2 = new Snake(Brushes.DarkGreen,false);
            }

            //refresh managment
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            //snakes speed
            timer.Interval = REFRESHDELAY;
            timer.Start();

            //keyboard managment
            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            AddFood();
            AddFoodOrPoison();
            AddFoodOrPoison();
        }
        private void DrawFoodsAndPoisons()
        {
            for(int i=0;i<foodPoints.Count;i++)
            {
                DrawFood(i,foodPoints[i]);
            }
            for (int i = 0; i < poisonPoints.Count; i++)
            {
                DrawPoison(i, poisonPoints[i]);
            }
        }

        private void DrawFood(int index,Point foodPoint)
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

        private void AddFoodOrPoison()
        {
            int alea = rand.Next(0, 10);
            if (alea % 4 == 0 && foodPoints.Count!=0)
            {
                //malus
                Point poisonPoint = new Point(SnakeCeiling(rand.Next(0 + SNAKETHICK, (int)(paintCanvas.Width - SNAKETHICK))), SnakeCeiling(rand.Next(0 + SNAKETHICK, (int)(paintCanvas.Height - SNAKETHICK))));
                poisonPoints.Add(poisonPoint);
            }
            else
            {
                Point foodPoint = new Point(SnakeCeiling(rand.Next(0+SNAKETHICK, (int)(paintCanvas.Width-SNAKETHICK))), SnakeCeiling(rand.Next(0 + SNAKETHICK, (int)(paintCanvas.Height - SNAKETHICK))));
                foodPoints.Add(foodPoint);
            }
        }
        private int SnakeCeiling(int entry)
        {
            return (int)Math.Ceiling(entry /(SNAKETHICK*1.0)) * SNAKETHICK;
        }
        private void AddFood() {
            Point foodPoint = new Point(SnakeCeiling(rand.Next(0 + SNAKETHICK, (int)(paintCanvas.Width - SNAKETHICK))), SnakeCeiling(rand.Next(0 + SNAKETHICK, (int)(paintCanvas.Height - SNAKETHICK))));
            foodPoints.Add(foodPoint);
        }

        private void DrawSnakes(){
            DrawASnake(snake1);
            if (IsNotAlone){
                DrawASnake(snake2);
            }
        }

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
        private void TimerTick(object sender, EventArgs e)
        {
            if (!IsPaused)
            {
                paintCanvas.Children.Clear();
                snake1.UpdateSnake();
                if (IsNotAlone)
                {
                    snake2.UpdateSnake();
                }
                DrawSnakes();
                DrawFoodsAndPoisons();
                CheckColisions();
                CheckFood(snake1);
                CheckPoison(snake1);
                if (IsNotAlone) 
                {
                    CheckFood(snake2);
                    CheckPoison(snake2);
                }
                scoreBoard.Text = $"- Score : Player1(purple)={snake1.Score}" + (IsNotAlone ? $"  | Player2(green)={snake2.Score} -" : " -");
            }
            else 
            { 
               scoreBoard.Text = $"PAUSED - Score : Player1(purple)={snake1.Score}"+ (IsNotAlone?$"  | Player2(green)={snake2.Score} -":" -");
            }
        }

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
                    AddFoodOrPoison();
                    break;
                }
            }
        }

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
                    AddFoodOrPoison();
                    break;
                }
            }
        }

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

                foreach(Point p in snake2.SnakeBody)
                { 
                    if ((Math.Abs(p.X - head1.X) < (SNAKETHICK)) &&
                         (Math.Abs(p.Y - head1.Y) < (SNAKETHICK)))
                    {
                        if (!IsDisplayingEnd) 
                        { 
                            EndGame("Purple  snake");
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
                            EndGame("Green  snake");
                        }
                        break;
                    }
                }
            }
        }


        private void CheckSelfCollision(Snake snake)
        {
            Point head = snake.SnakeBody[0];
            for (int i = 1; i < snake.SnakeBody.Count; i++)
            {
                Point point = new Point(snake.SnakeBody[i].X, snake.SnakeBody[i].Y);
                if ((Math.Abs(point.X - head.X) < (SNAKETHICK)) &&
                     (Math.Abs(point.Y - head.Y) < (SNAKETHICK)))
                {
                    if (!IsDisplayingEnd){
                        EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2" ? "Purple  snake" : "Green  snake");
                    }
                    break;
                }
            }
        }

        private void CheckHeadOfSnake(Snake snake)
        {
            if(snake.SnakeBody[0].X<0+SNAKETHICK 
                || snake.SnakeBody[0].X>550-2*SNAKETHICK 
                || snake.SnakeBody[0].Y<0+SNAKETHICK 
                || snake.SnakeBody[0].Y > 450 - 2*SNAKETHICK)
            {
                if (!IsDisplayingEnd){
                    EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2" ? "Purple snake" : "Green  snake");
                }
            }
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e){

            switch (e.Key)
            {
                case Key.P:
                    IsPaused = !IsPaused;
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
            if (IsNotAlone) { 
                switch (e.Key) { 
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

        public void EndGame(string s ="BG "){
            int result = (int)MessageBox.Show(s+" made a mistake ! \n Wanna Play Again ? ","Snake2Arc Over",MessageBoxButton.YesNo,MessageBoxImage.Information);
            if (result == 6){//for yes
                RunGame(IsNotAlone);
            }
            else{
                Close();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
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
        }

        private void Button_Options_Click(object sender,RoutedEventArgs e)
        {

        }
    }
}
