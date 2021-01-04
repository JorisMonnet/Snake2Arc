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

        //things to eat
        private List<Point> foodPoints = new List<Point>();

        private Snake snake1;
        private Snake snake2;

        //refresh delay
        private TimeSpan REFRESHDELAY = new TimeSpan(1000000);


        //snakes thick
        public const int SNAKETHICK = 10;
 

        //random number for food spawning
        private Random rand = new Random();

        public GameWindow(){
            InitializeComponent();

            snake1 = new Snake(Brushes.BlueViolet,true);
            snake2 = new Snake(Brushes.DarkGreen,false);
            
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();
            snake1.Eat();


            //refresh managment
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);

            //snakes speed
            timer.Interval = REFRESHDELAY;
            timer.Start();

            //keyboard managment
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            AddFood();
        }


        private void DrawFoods()
        {
            for(int i=0;i<foodPoints.Count;i++)
            {
                DrawFood(i,foodPoints[i]);
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
            foreach(Point p in snake.snakeBody)
            {
                Ellipse snakeEllipse = new Ellipse();
                snakeEllipse.Fill = snake.snakeColor;
                snakeEllipse.Width = SNAKETHICK;
                snakeEllipse.Height = SNAKETHICK;

                Canvas.SetTop(snakeEllipse, p.Y);
                Canvas.SetLeft(snakeEllipse, p.X);

                paintCanvas.Children.Add(snakeEllipse);
            }
        }

        private void TimerTick(object sender, EventArgs e){

            paintCanvas.Children.Clear();
            snake1.UpdateSnake();
            snake2.UpdateSnake();
            DrawSnakes();
            DrawFoods();


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

        private void EndGame(){
            MessageBox.Show("Some One Losed","Snake2Arc Over",MessageBoxButton.OK,MessageBoxImage.Hand);
            this.Close();
        }
    }
}
