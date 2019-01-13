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
        private bool isCurrentPlayerWhite;
        private MainWindow mainWindow;
        
        public GameManager(MainWindow mainWindow)
        {
            this.board = new Board();
            this.isCurrentPlayerWhite = false;
            this.mainWindow = mainWindow;
        }
        
        public void Play(int column, int line)
        {
            board.PlayMove(column, line, isCurrentPlayerWhite);
            
            isCurrentPlayerWhite = !isCurrentPlayerWhite;
            DisplayBoard();
            Console.WriteLine(board);
        }

        public void DisplayBoard()
        {
            for (int line = 0; line < Constants.NbRow; line++)
            {
                for (int column = 0; column < Constants.NbColumn; column++)
                {
                    mainWindow.SetCaseImage(column, line, isCurrentPlayerWhite);
                }
            }

            Console.WriteLine(board.GetWhiteScore() + " : " + board.GetBlackScore());
        }

        public bool IsCurrentPlayerWhite()
        {
            return isCurrentPlayerWhite;
        }

        public bool IsGameOver()
        {
            
            
            // @todo
            return false;
        }

        public int[,] GetBoard()
        {
            return board.GetBoard();
        }

        public int GetPawn(int column, int line)
        {
            return board.GetPawn(column, line);
        }

        public bool IsPlayabe(int column, int line)
        {
            return board.IsPlayable(column, line, isCurrentPlayerWhite);
        }

    }
}
