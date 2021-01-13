using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Media;


namespace Snake2Arc
{
    /// <summary>
    /// class representing a snake 
    /// </summary>
    class Snake
    {
        public List<Point> SnakeBody { get; set; }
        public Brush SnakeColor { get; set; }
        private int direction = -1;
        public int Score { get; set; }
        public int Speed { get; set; }
        public int SpeedDelay { get; set; }
        public int Size { get; set; }

        public Snake(Brush color,bool oneOrTwo)
        {
            Speed = 1;
            SpeedDelay = 20;
            SnakeColor = color;
            SnakeBody = new List<Point>();
            Size = oneOrTwo ? 100 : 300;
            SnakeBody.Add(new Point(Size,Size));
            SnakeBody.Add(new Point(Size + GameWindow.SNAKETHICK,Size));
        }

        /// <summary>
        /// Update the position of the snake to move it
        /// </summary>
        /// <param name="isNotWaiting">boolean if the snake is frozen by a slowFood </param>
        public void UpdateSnake(bool isNotWaiting)
        {
            if(isNotWaiting)
            {
                if(Speed != 1 && SpeedDelay > 0)
                {
                    SpeedDelay--;
                }
                else if(SpeedDelay <= 0)
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
                if(Speed == 0 && SpeedDelay > 0)
                {
                    SpeedDelay--;
                }
                else if(SpeedDelay <= 0)
                {
                    SpeedDelay = 20;
                    Speed = 1;
                }
            }
        }

        /// <summary>
        /// Remove point of a snake, manage if he dies because of it
        /// </summary>
        /// <param name="gameWindow"></param>
        public void PoisonSnake(GameWindow gameWindow)
        {
            if(SnakeBody.Count - 3 > 0)
            {
                SnakeBody.RemoveAt(SnakeBody.Count - 2); //poison Twice to be punitive
                UpdateSnake(Speed != 0);
            }
            else
            {
                gameWindow.EndGame(SnakeColor.ToString() == "#FF8A2BE2");
            }
        }

        /// <summary>
        /// Add a point to the body of the snake
        /// </summary>
        public void Eat()
        {
            if(SnakeBody[SnakeBody.Count - 1].X > SnakeBody[SnakeBody.Count - 2].X)
            {
                //add on right
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X + GameWindow.SNAKETHICK,SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if(SnakeBody[SnakeBody.Count - 1].X < SnakeBody[SnakeBody.Count - 2].X)
            {
                //add on left
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X - GameWindow.SNAKETHICK,SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if(SnakeBody[SnakeBody.Count - 1].Y < SnakeBody[SnakeBody.Count - 2].Y)
            {
                //add on top
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X,SnakeBody[SnakeBody.Count - 1].Y - GameWindow.SNAKETHICK));
            }
            else if(SnakeBody[SnakeBody.Count - 1].Y > SnakeBody[SnakeBody.Count - 2].Y)
            {
                //add on bottom
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X,SnakeBody[SnakeBody.Count - 1].Y + GameWindow.SNAKETHICK));
            }
        }

        /// <summary>
        /// Change direction of the snake if it's possible
        /// </summary>
        /// <param name="dir">the direction asked</param>
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
    }
}
