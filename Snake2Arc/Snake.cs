using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Snake2Arc{
    class Snake
    {
        public List<Point> SnakeBody { get; set; }
        public Brush SnakeColor { get; set; }
        private int direction = -1;
        public int Score { get; set; }

        public Snake(Brush color,bool oneOrTwo)
        {
            SnakeColor = color;
            SnakeBody = new List<Point>();
            if (oneOrTwo)
            {
                SnakeBody.Add(new Point(100, 100));
                SnakeBody.Add(new Point(100 + GameWindow.SNAKETHICK, 100));
            }
            else
            {   
                SnakeBody.Add(new Point(300, 300));
                SnakeBody.Add(new Point(300 + GameWindow.SNAKETHICK, 300));
            }
        }

        public void UpdateSnake()
        {
            List<Point> newBody = new List<Point>();
            if (direction == (int)DIRECTION.UP)
            {
                newBody.Add(new Point(SnakeBody[0].X,SnakeBody[0].Y-GameWindow.SNAKETHICK));
            }
            else if (direction == (int)DIRECTION.DOWN)
            {
                newBody.Add(new Point(SnakeBody[0].X, SnakeBody[0].Y+ GameWindow.SNAKETHICK));
            }
            else if (direction == (int)DIRECTION.LEFT)
            {
                newBody.Add(new Point(SnakeBody[0].X- GameWindow.SNAKETHICK, SnakeBody[0].Y));
            }
            else if (direction == (int)DIRECTION.RIGHT)
            {
                newBody.Add(new Point(SnakeBody[0].X + GameWindow.SNAKETHICK, SnakeBody[0].Y));
            }
            SnakeBody.RemoveAt(SnakeBody.Count-1);
            foreach(Point p in SnakeBody)
            {
                newBody.Add(p);
            }
            SnakeBody = newBody;
            Score = SnakeBody.Count;
        }

        public void PoisonSnake(GameWindow gW)
        {
            if (SnakeBody.Count - 2 > 0) 
            { 
                SnakeBody.RemoveAt(SnakeBody.Count - 1);
                UpdateSnake();
            }
            else
            {
                gW.EndGame(SnakeColor.ToString() == "#FF8A2BE2" ? "Purple snake" : "Green  snake" );
            }
        }

        public void Eat()
        {
            if(SnakeBody[SnakeBody.Count - 1].X > SnakeBody[SnakeBody.Count - 2].X)
            {
                //cas on dernier a droite
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X+GameWindow.SNAKETHICK, SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if (SnakeBody[SnakeBody.Count - 1].X < SnakeBody[SnakeBody.Count - 2].X)
            {
                //cas on est dernier sur la gauche
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X-GameWindow.SNAKETHICK, SnakeBody[SnakeBody.Count - 1].Y));
            }
            else if (SnakeBody[SnakeBody.Count - 1].Y < SnakeBody[SnakeBody.Count - 2].Y)
            {
                //cas on est dernier en haut
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X , SnakeBody[SnakeBody.Count - 1].Y- GameWindow.SNAKETHICK));
            }
            else if (SnakeBody[SnakeBody.Count - 1].Y > SnakeBody[SnakeBody.Count - 2].Y)
            {
                //cas on est dernier  en bas
                SnakeBody.Add(new Point(SnakeBody[SnakeBody.Count - 1].X , SnakeBody[SnakeBody.Count - 1].Y + GameWindow.SNAKETHICK));
            }
        }

        public void ChangeSnakeDirection(DIRECTION dir){
            bool valid = false;
            if(dir == DIRECTION.DOWN && SnakeBody[1].Y <= SnakeBody[0].Y) { valid = true; }
            if(dir == DIRECTION.UP && SnakeBody[1].Y >= SnakeBody[0].Y) { valid = true; }
            if(dir == DIRECTION.LEFT && SnakeBody[1].X >= SnakeBody[0].X){ valid = true; }
            if(dir == DIRECTION.RIGHT && SnakeBody[1].X <= SnakeBody[0].X){ valid = true; }

            if (valid)
            {
                direction = (int)dir;
            }
        }

        internal void Reset(bool oneOrTwo)
        {
            SnakeBody.Clear();
            if (!oneOrTwo){
                SnakeBody.Add(new Point(100, 100));
                SnakeBody.Add(new Point(100 + GameWindow.SNAKETHICK, 100));
            }
            else{
                SnakeBody.Add(new Point(300, 300));
                SnakeBody.Add(new Point(300 + GameWindow.SNAKETHICK, 300));
            }
        }
    }
}
