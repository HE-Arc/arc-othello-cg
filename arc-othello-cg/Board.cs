using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using IPlayableAlias = IPlayable.IPlayable;

namespace arc_othello_cg
{
    class Board : IPlayableAlias
    {
        private int[,] board = new int[Constants.NbRow, Constants.NbColumn];

        public Board()
        {
            InitBoard();
        }

        private void InitBoard()
        {
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = Pawn.Empty;
                }
            }

            board[4, 3] = board[3, 4] = Pawn.Black;
            board[3, 3] = board[4, 4] = Pawn.White;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    switch (board[i, j]) {
                        case Pawn.Empty:
                            stringBuilder.Append(". ");
                            break;
                        case Pawn.White:
                            stringBuilder.Append("X ");
                            break;
                        case Pawn.Black:
                            stringBuilder.Append("O ");
                            break;
                        case Pawn.Playable:
                            stringBuilder.Append("+ ");
                            break;
                        default:
                            throw new Exception();
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns the IA's name
        /// </summary>
        /// <returns>true or false</returns>
        public string GetName()
        {
            return "Ah que coucou";
        }

        /// <summary>
        /// Returns true if the move is valid for specified color
        /// </summary>
        /// <param name="column">value between 0 and 8</param>
        /// <param name="line">value between 0 and 6</param>
        /// <param name="isWhite"></param>
        /// <returns>true or false</returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {
            int playerPawn = isWhite ? Pawn.White : Pawn.Black;
            int enemyPawn = isWhite ? Pawn.Black : Pawn.White;

            // Bail early if tile is already occupied
            if (board[line, column] != Pawn.Empty)
            {
                return false;
            }
            
            for(int y = -1; y <= 1; y++)
            {
                int currentTestY = line + y;

                if(currentTestY < 0 || currentTestY >= Constants.NbRow)
                {
                    continue;
                }
                
                for (int x = -1; x <= 1; x++)
                {
                    int currentTestX = column + x;

                    if (currentTestX < 0 || currentTestX >= Constants.NbColumn || (x == 0 && y == 0))
                    {
                        continue;
                    }

                    if (board[currentTestY, currentTestX] != Pawn.Empty && board[currentTestY, currentTestX] != Pawn.Playable)
                    {
                        if (IsValidDirection(line, column, x, y, playerPawn, enemyPawn))
                        {
                            board[line, column] = Pawn.Playable;
                            
                        }
                    }
                }
            }
                        
            return true;
        }

        private bool IsValidDirection(int line, int columnm, int dX, int dY, int playerPawn, int enemyPawn)
        {
            line += dY;
            columnm += dX;

            bool enemyCrossed = false;

            while (board[line, columnm] != Pawn.Empty && board[line, columnm] != Pawn.Playable)
            {
                if (board[line, columnm] == enemyPawn)
                {
                    enemyCrossed = true;
                }

                if(enemyCrossed && board[line, columnm] == playerPawn)
                {
                    return true;
                }

                line += dY;
                columnm += dX;
            }
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a reference to a 2D array with the board status
        /// </summary>
        /// <returns>The 7x9 tiles status</returns>
        public int[,] GetBoard()
        {
            return board;
        }

        public int GetWhiteScore()
        {
            throw new NotImplementedException();
        }

        public int GetBlackScore()
        {
            throw new NotImplementedException();
        }

        private int GetScore(bool isWhite)
        {
            return 0;
        }
    }
}
