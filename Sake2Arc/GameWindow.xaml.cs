using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sake2Arc{


   
    //all directions
    public enum DIRECTION
    {
        UP = 1, DOWN = 0, LEFT = -1, RIGHT = 2
    }

    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window{

        public bool IsAlone { get; set; }

        //things to eat
        private readonly List<Point> foodPoints = new List<Point>();
        private readonly List<Point> poisonPoints = new List<Point>();

        private readonly Snake snake1;
        private readonly Snake snake2;

        //refresh delay
        private TimeSpan REFRESHDELAY = new TimeSpan(1000000);


        //snakes thick
        public const int SNAKETHICK = 10;

        //random number for food spawning
        private readonly Random rand = new Random();

        public GameWindow(){
            InitializeComponent();

            snake1 = new Snake(Brushes.BlueViolet,true);
            snake2 = new Snake(Brushes.DarkGreen,false);
            
         /*   snake1.Eat();
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();
        

            snake2.Eat();
            snake2.Eat();
            snake2.Eat();
            snake2.Eat();*/

            //refresh managment
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            //snakes speed
            timer.Interval = REFRESHDELAY;
            timer.Start();

            //keyboard managment
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            AddFood();
            addFoodOrPoison();
            addFoodOrPoison();
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
            Ellipse foodEllipse = new Ellipse();
            foodEllipse.Fill = Brushes.IndianRed;
            foodEllipse.Width = SNAKETHICK;
            foodEllipse.Height = SNAKETHICK;

            Canvas.SetTop(foodEllipse, foodPoint.Y);
            Canvas.SetLeft(foodEllipse, foodPoint.X);

            paintCanvas.Children.Insert(index, foodEllipse);
        }
        private void DrawPoison(int index, Point poisonPoint)
        {
            Ellipse foodEllipse = new Ellipse();
            foodEllipse.Fill = Brushes.Yellow;
            foodEllipse.Width = SNAKETHICK;
            foodEllipse.Height = SNAKETHICK;

            Canvas.SetTop(foodEllipse, poisonPoint.Y);
            Canvas.SetLeft(foodEllipse, poisonPoint.X);

            paintCanvas.Children.Insert(index, foodEllipse);
        }

        private void addFoodOrPoison()
        {
            int alea = rand.Next(0, 10);
            if (alea % 4 == 0)
            {
                //malus
                Point poisonPoint = new Point(rand.Next(10, 540), rand.Next(10, 440));
                poisonPoints.Add(poisonPoint);
            }
            else
            {
                Point foodPoint = new Point(rand.Next(10, 540), rand.Next(10, 440));
                foodPoints.Add(foodPoint);
            }
        }
        private void AddFood() {
            Point foodPoint = new Point(rand.Next(10, 540), rand.Next(10, 440));
            
            foodPoints.Add(foodPoint);
        }

        private void DrawSnakes(){
            DrawASnake(snake1);
            DrawASnake(snake2);
        }

        private void DrawASnake(Snake snake)
        {
        foreach (Point p in snake.SnakeBody)
            {
                Ellipse snakeEllipse = new Ellipse();
                snakeEllipse.Fill = snake.SnakeColor;
                snakeEllipse.Width = SNAKETHICK;
                snakeEllipse.Height = SNAKETHICK;

                Canvas.SetTop(snakeEllipse, p.Y);
                Canvas.SetLeft(snakeEllipse, p.X);

                paintCanvas.Children.Add(snakeEllipse);
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            paintCanvas.Children.Clear();
            snake1.UpdateSnake();
            snake2.UpdateSnake();
            DrawSnakes();
            DrawFoodsAndPoisons();
            CheckColisions();
            CheckFood(snake1);
            CheckPoison(snake1);
            CheckFood(snake2);
            CheckPoison(snake2);
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
                    addFoodOrPoison();
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
                    addFoodOrPoison();
                    break;
                }
            }
        }

        private void CheckColisions()
        {
            CheckHeadOfSnake(snake1);
            CheckHeadOfSnake(snake2);
            CheckSelfCollision(snake1);
            CheckSelfCollision(snake2);
            Point head1 = snake1.SnakeBody[0];
            Point head2 = snake2.SnakeBody[0];

            foreach(Point p in snake2.SnakeBody)
            { 
                if ((Math.Abs(p.X - head1.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head1.Y) < (SNAKETHICK)))
                {
                    EndGame("Purple  snake");
                    break;
                }
            }
            foreach (Point p in snake1.SnakeBody)
            {
                if ((Math.Abs(p.X - head2.X) < (SNAKETHICK)) &&
                     (Math.Abs(p.Y - head2.Y) < (SNAKETHICK)))
                {
                    EndGame("Green  snake");
                    break;
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
                    EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2" ? "Purple  snake" : "Green  snake");
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
                EndGame(snake.SnakeColor.ToString() == "#FF8A2BE2" ? "Purple snake" : "Green  snake");
            }
        }


        private void OnButtonKeyDown(object sender, KeyEventArgs e){

            switch (e.Key)
            {
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

        public void EndGame(String s="BG "){
            MessageBox.Show(s+" made a mistake ! ","Snake2Arc Over",MessageBoxButton.OK,MessageBoxImage.Hand);
            this.Close();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
