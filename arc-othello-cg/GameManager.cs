using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IPlayableAlias = IPlayable.IPlayable;

namespace arc_othello_cg
{
    // @todo Implement it as a singleton ?
    class GameManager
    {
        private Board board;
        private Player[] players = new Player[Constants.NbPlayers];

        public void Run()
        {
            board = new Board();

            Console.WriteLine(board);
            //while (!IsGameOver())
            //{
            //    foreach (IPlayableAlias p in players)
            //    {
            //        // @todo
            //    }
            //}
        }

        public bool IsGameOver()
        {
            // @todo
            return false;
        }
    }
}
