using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace arc_othello_cg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() : this(9, 7)
        {

        }

        public MainWindow(int width, int height)
        {
            InitializeComponent();

            GameManager gameManager = new GameManager();
            gameManager.Run();

            Box.Stretch = Stretch.Uniform;

            PlayGrid.Columns = width;
            PlayGrid.Rows = height;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Image btn = new Image();
                    btn.Source = new BitmapImage(new Uri(@"/Resources/two.png", UriKind.Relative));

                    PlayGrid.Children.Add(btn);
                    Grid.SetColumn(btn, col);
                    Grid.SetRow(btn, row);
                }
            }
        }
    }
}
