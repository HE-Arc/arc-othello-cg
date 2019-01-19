using System;

using IPlayableAlias = IPlayable.IPlayable;

namespace arc_othello_cg
{
    [Serializable]
    class Board : IPlayableAlias
    {
        private int[,] board = new int[Constants.NbRow, Constants.NbColumn];

        // +----------------------------------------------------------------------
        // | Constructors
        // +----------------------------------------------------------------------

        public Board()
        {
            InitBoard();
        }

        // +----------------------------------------------------------------------
        // | Public functions
        // +----------------------------------------------------------------------


        /// <summary>
        /// Give the pawn at the wanted position
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="line">Line</param>
        /// <returns></returns>
        public int GetPawn(int column, int line)
        {
            return board[line, column];
        }

        /// <summary>
        /// Return the whithe score
        /// </summary>
        /// <returns>whithe score</returns>
        public int GetWhiteScore()
        {
            return GetScore(true);
        }

        /// <summary>
        /// Return the black score
        /// </summary>
        /// <returns>black score</returns>
        public int GetBlackScore()
        {
            return GetScore(false);
        }
        
        // Interface IPlayable

        /// <summary>
        /// Returns the IA's name
        /// </summary>
        /// <returns>true or false</returns>
        public string GetName()
        {
            return "IACG";
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
            if (board[line, column] != Pawn.Empty)
            {
                return false;
            }

            for (int y = -1; y <= 1; y++)
            {
                int currentTestY = line + y;

                if (currentTestY < 0 || currentTestY >= Constants.NbRow)
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

                    if (board[currentTestY, currentTestX] != Pawn.Empty)
                    {
                        if (IsValidDirection(column, line, x, y, isWhite))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Play the pawn at the given position
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="line">Line</param>
        /// <param name="isWhite">Player who's playing</param>
        /// <returns></returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (IsPlayable(column, line, isWhite))
            {
                for (int y = -1; y <= 1; y++)
                {
                    int currentTestY = line + y;

                    if (currentTestY < 0 || currentTestY >= Constants.NbRow)
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

                        if (IsValidDirection(column, line, x, y, isWhite))
                        {
                            SwapPawns(column, line, x, y, isWhite);
                        }
                    }
                }
                return false;
            }

            return false;
        }

        /// <summary>
        /// Returns a reference to a 2D array with the board status
        /// </summary>
        /// <returns>The 7x9 tiles status</returns>
        public int[,] GetBoard()
        {
            return board;
        }

        /// <summary>
        /// IA play
        /// </summary>
        /// <param name="game">The board</param>
        /// <param name="level">level</param>
        /// <param name="whiteTurn">IA's color</param>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        // +----------------------------------------------------------------------
        // | Private functions
        // +----------------------------------------------------------------------

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
        
        private bool IsValidDirection(int column, int line, int dX, int dY, bool isWhite)
        {
            line += dY;
            column += dX;

            if(board[line, column] == Pawn.getPawn(isWhite))
            {
                return false;
            }

            bool enemyCrossed = false;
            
            while(IsOnBoard(column, line) && board[line, column] != Pawn.Empty)
            {
                if (board[line, column] != Pawn.getPawn(isWhite))
                {
                    enemyCrossed = true;
                }

                if(enemyCrossed && board[line, column] == Pawn.getPawn(isWhite))
                {
                    return true;
                }

                line += dY;
                column += dX;
            }

            return false;
        }
        
        private void SwapPawns(int column, int line, int dX, int dY, bool isWhite)
        {
            board[line, column] = Pawn.getPawn(isWhite);
            line += dY;
            column += dX;

            while(IsOnBoard(column, line) && board[line, column] != Pawn.getPawn(isWhite) && board[line, column] != Pawn.Empty)
            {
                board[line, column] = Pawn.getPawn(isWhite);

                line += dY;
                column += dX;
            }
        }
        
        private int GetScore(bool isWhite)
        {
            int score = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    score += board[i, j] == Pawn.getPawn(isWhite) ? 1 : 0;
                }
            }
            return score;
        }
        
        private bool IsOnBoard(int column, int line)
        {
            return line >= 0 && line < Constants.NbRow && column >= 0 && column < Constants.NbColumn;
        }

    }
}
