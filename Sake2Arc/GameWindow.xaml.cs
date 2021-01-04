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
        //snake1 body
        private List<Point> snake1Points = new List<Point>();
        //snake2 body
        private List<Point> snake2Points = new List<Point>();

        private Brush snake1Color = Brushes.AliceBlue;
        private Brush snake2Color = Brushes.GreenYellow;

        

        //refresh delay
        private TimeSpan REFRESHDELAY = new TimeSpan(10000);

        //starting points
        private Point startPointSnake1 = new Point(50,50);
        private Point startPointSnake2 = new Point(50,50);
        //acutal position
        private Point pointSnake1 = new Point();
        private Point pointSnake2 = new Point();

        //directions values
        private int directionSake1 = 0;
        private int directionSake2 = 0;
        //oldDirection to not back on himself
        private int oldDirectionSake1 = 0;
        private int oldDirectionSake2 = 0;

        //snakes thick
        private int snakeThick = 10;
        //snakes size
        private int snake1Lenght = 50;
        private int snake2Lenght = 50;

        //scores
        private int snake1Score = 0;
        private int snake2Score = 0;

        //random number for food spawning
        private Random rand = new Random();

        public GameWindow(){
            InitializeComponent();

            //refresh managment
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);

            //snakes speed
            timer.Interval = REFRESHDELAY;
            timer.Start();

            //keyboard managment
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            drawSnakes(startPointSnake1, startPointSnake2);
            drawFood();
            drawFood();
            drawFood();
            drawFood();
        }

        private void drawFood()
        {
            Point foodPoint = new Point(rand.Next(10, 540), rand.Next(10, 440));
            Ellipse foodEllipse = new Ellipse();
            foodEllipse.Fill = Brushes.IndianRed;
            foodEllipse.Width = snakeThick;
            foodEllipse.Height = snakeThick;

            Canvas.SetTop(foodEllipse, foodPoint.Y);
            Canvas.SetLeft(foodEllipse, foodPoint.X);

            paintCanvas.Children.Add(foodEllipse);
            foodPoints.Add(foodPoint);
        }

        private void drawSnakes(Point startPointSnake1, Point startPointSnake2)
        {
            
        }

        private void timerTick(object sender, EventArgs e){

        }

        private void OnButtonKeyDown(object sender, EventArgs e){

        }
    }
}
