using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Snake2Arc
{
    class Snake
    {
        public List<Point> SnakeBody { get; set; }
        public Brush SnakeColor { get; set; }
        private int direction = -1;
        public int Score { get; set; }
        public int Speed { get; set; }
        public int SpeedDelay { get; set; }
        public Snake(Brush color,bool oneOrTwo)
        {
            Speed = 1;
            SpeedDelay = 20;
            SnakeColor = color;
            SnakeBody = new List<Point>();
            int size = oneOrTwo ? 100 : 300;
            SnakeBody.Add(new Point(size,size));
            SnakeBody.Add(new Point(size + GameWindow.SNAKETHICK,size));
        }

        public void UpdateSnake(bool isNotWaiting)
        {
            if (isNotWaiting) { 
                if (Speed != 1 && SpeedDelay>0)
                {
                    SpeedDelay--;
                }    
                else if (SpeedDelay <= 0)
                {
                    SpeedDelay = 20;
                    Speed = 1;
                }
            
                List<Point> newBody = new List<Point>();
                if(direction == (int)DIRECTION.UP)
                {
                    newBody.Add(new Point(SnakeBody[0].X,SnakeBody[0].Y - GameWindow.SNAKETHICK));
                }
                else if(direction == (int)DIRECTION.DOWN)
                {
                    newBody.Add(new Point(SnakeBody[0].X,SnakeBody[0].Y + GameWindow.SNAKETHICK));
                }
                else if(direction == (int)DIRECTION.LEFT)
                {
                    newBody.Add(new Point(SnakeBody[0].X - GameWindow.SNAKETHICK,SnakeBody[0].Y));
                }
                else if(direction == (int)DIRECTION.RIGHT)
                {
                    newBody.Add(new Point(SnakeBody[0].X + GameWindow.SNAKETHICK,SnakeBody[0].Y));
                }
                SnakeBody.RemoveAt(SnakeBody.Count - 1);
                foreach(Point p in SnakeBody)
                {
                    newBody.Add(p);
                }
                SnakeBody = newBody;
                Score = SnakeBody.Count;
            }
            else
            {
                if (Speed ==0 && SpeedDelay > 0)
                {
                    SpeedDelay--;
                }
                else if (SpeedDelay <= 0)
                {
                    SpeedDelay = 20;
                    Speed = 1;
                }
            }
        }

        public void PoisonSnake(GameWindow gW)
        {
            if(SnakeBody.Count - 2 > 0)
            {
                SnakeBody.RemoveAt(SnakeBody.Count - 1);
                UpdateSnake(Speed!=0);
            }
            else
            {
                gW.EndGame(SnakeColor.ToString() == "#FF8A2BE2" ? "Purple snake" : "Green  snake");
            }
        }

        public void Eat()
        {
            if(SnakeBody[SnakeBody.Count - 1].X > SnakeBody[SnakeBody.Count - 2].X)
            {
                //cas on dernier a droite
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X + GameWindow.SNAKETHICK,SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if(SnakeBody[SnakeBody.Count - 1].X < SnakeBody[SnakeBody.Count - 2].X)
            {
                //cas on est dernier sur la gauche
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X - GameWindow.SNAKETHICK,SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if(SnakeBody[SnakeBody.Count - 1].Y < SnakeBody[SnakeBody.Count - 2].Y)
            {
                //cas on est dernier en haut
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X,SnakeBody[SnakeBody.Count - 1].Y - GameWindow.SNAKETHICK));
            }
            else if(SnakeBody[SnakeBody.Count - 1].Y > SnakeBody[SnakeBody.Count - 2].Y)
            {
                //cas on est dernier  en bas
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X,SnakeBody[SnakeBody.Count - 1].Y + GameWindow.SNAKETHICK));
            }
        }

        public void ChangeSnakeDirection(DIRECTION dir)
        {
            if((dir == DIRECTION.DOWN && SnakeBody[1].Y <= SnakeBody[0].Y) ||
                (dir == DIRECTION.UP && SnakeBody[1].Y >= SnakeBody[0].Y) ||
                (dir == DIRECTION.LEFT && SnakeBody[1].X >= SnakeBody[0].X) ||
                (dir == DIRECTION.RIGHT && SnakeBody[1].X <= SnakeBody[0].X))
            {
                direction = (int)dir;
            }
        }

        internal void Reset(bool oneOrTwo)
        {
            SnakeBody.Clear();
            int size = oneOrTwo ? 300 : 100;
            SnakeBody.Add(new Point(size,size));
            SnakeBody.Add(new Point(size + GameWindow.SNAKETHICK,size));
        }
    }
}
