using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc_othello_cg
{
    class Pawn
    {
        public const int Empty = -1;
        public const int White = 0;
        public const int Black = 1;

        /// <summary>
        /// Return the pawns value (int)
        /// </summary>
        /// <param name="isWhite">Boolean isWhite</param>
        /// <returns>The pawn value</returns>
        public static int getPawn(bool isWhite)
        {
            return isWhite ? Pawn.White : Pawn.Black;
        }
    }
}
