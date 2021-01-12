using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake2Arc
{
    /// <summary>
    /// Data class to store the name and score value of the 5 best players in the leaderboard
    /// </summary>
    public class Score
    {
        public int ScoreValue { get; set; }
        public string Name { get; set; }
    }
}
