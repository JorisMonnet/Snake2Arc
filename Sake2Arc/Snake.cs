using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sake2Arc{
    class Snake
    {
        public List<Point> snakeBody;
        public Brush snakeColor;
        public int snakeLen;
        private int direction = -1;


        public Snake(Brush color,bool oneOrTwo)
        {
            this.snakeColor = color;
            this.snakeBody = new List<Point>();
            this.snakeLen = 5;
            if (oneOrTwo)
            {
                snakeBody.Add(new Point(100, 100));
                snakeBody.Add(new Point(100 + GameWindow.SNAKETHICK, 100));
            }
            else
            {   
                snakeBody.Add(new Point(300, 300));
                snakeBody.Add(new Point(300 + GameWindow.SNAKETHICK, 300));
            }
        }

        public void UpdateSnake()
        {
            List<Point> newBody = new List<Point>();
            if (direction == (int)DIRECTION.UP)
            {
                newBody.Add(new Point(snakeBody[0].X,snakeBody[0].Y-GameWindow.SNAKETHICK));
            }
            else if (direction == (int)DIRECTION.DOWN)
            {
                newBody.Add(new Point(snakeBody[0].X, snakeBody[0].Y+ GameWindow.SNAKETHICK));
            }
            else if (direction == (int)DIRECTION.LEFT)
            {
                newBody.Add(new Point(snakeBody[0].X- GameWindow.SNAKETHICK, snakeBody[0].Y));
            }
            else if (direction == (int)DIRECTION.RIGHT)
            {
                newBody.Add(new Point(snakeBody[0].X + GameWindow.SNAKETHICK, snakeBody[0].Y));
            }
            snakeBody.RemoveAt(snakeBody.Count-1);
            foreach(Point p in snakeBody)
            {
                newBody.Add(p);
            }
            snakeBody = newBody;
        }

        public void Eat()
        {
            if(snakeBody[snakeBody.Count - 1].X > snakeBody[snakeBody.Count - 2].X)
            {
                //cas on dernier a droite
                this.snakeBody.Add(new Point(snakeBody[snakeBody.Count - 1].X+GameWindow.SNAKETHICK, snakeBody[snakeBody.Count - 1].Y));
            }
            else if (snakeBody[snakeBody.Count - 1].X < snakeBody[snakeBody.Count - 2].X)
            {
                //cas on est dernier sur la gauche
                this.snakeBody.Add(new Point(snakeBody[snakeBody.Count - 1].X-GameWindow.SNAKETHICK, snakeBody[snakeBody.Count - 1].Y));
            }
            else if (snakeBody[snakeBody.Count - 1].Y < snakeBody[snakeBody.Count - 2].Y)
            {
                //cas on est dernier en haut
                this.snakeBody.Add(new Point(snakeBody[snakeBody.Count - 1].X , snakeBody[snakeBody.Count - 1].Y- GameWindow.SNAKETHICK));
            }
            else if (snakeBody[snakeBody.Count - 1].Y > snakeBody[snakeBody.Count - 2].Y)
            {
                //cas on est dernier  en bas
                this.snakeBody.Add(new Point(snakeBody[snakeBody.Count - 1].X , snakeBody[snakeBody.Count - 1].Y + GameWindow.SNAKETHICK));
            }
        }

        public void ChangeSnakeDirection(DIRECTION dir){
            bool valid = false;
            if(dir==DIRECTION.DOWN && snakeBody[1].Y <= snakeBody[0].Y) { valid = true; }
            if(dir == DIRECTION.UP && snakeBody[1].Y >= snakeBody[0].Y) { valid = true; }
            if(dir == DIRECTION.LEFT && snakeBody[1].X >= snakeBody[0].X){ valid = true; }
            if(dir == DIRECTION.RIGHT && snakeBody[1].X <= snakeBody[0].X){ valid = true; }

            if (valid)
            {
                this.direction = (int)dir;
            }
        }
    }
}
