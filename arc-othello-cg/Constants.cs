using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace arc_othello_cg
{
    class Constants
    {
        public static int NbRow = 7;
        public static int NbColumn = 9;
        public static int NbPlayers = 2;

        public static float BorderThickness = 0.3f;
        public static float NormalOpacity = 1.0f;
        public static float PlayableOpacity = 0.2f;

        public static Brush EmptyBrush = new SolidColorBrush(Color.FromArgb(0,0,0,0));
        public static Brush WhiteBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/white.png")));
        public static Brush BlackBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/black.png")));
        
        public static Brush getPawnBrush(int pawn, float opacity)
        {
            Brush newBrush;
            switch (pawn)
            {
                case Pawn.Empty:
                    newBrush = EmptyBrush.Clone();
                    break;
                case Pawn.White:
                    newBrush = WhiteBrush.Clone();
                    break;
                case Pawn.Black:
                    newBrush = BlackBrush.Clone();
                    break;
                default:
                    throw new Exception();
            }

            newBrush.Opacity = opacity;
            return newBrush;
        }
    }
}
