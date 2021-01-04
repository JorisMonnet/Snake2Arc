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

namespace Sake2Arc{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window{

        //things to eat
        private List<Point> eatPoints = new List<Point>();
        //snake1 body
        private List<Point> snake1Points = new List<Point>();
        //snake2 body
        private List<Point> snake2Points = new List<Point>();

        private Brush snake1Color = Brushes.AliceBlue;
        private Brush snake2Color = Brushes.GreenYellow;


        public GameWindow(){
            InitializeComponent();
        }
    }
}
