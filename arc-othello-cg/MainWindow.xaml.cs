using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace arc_othello_cg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameManager gameManager;

        public MainWindow()
        {
            InitializeComponent();
            InitBoard();
            gameManager = new GameManager(this);
            gameManager.DisplayBoard();
        }

        private void InitBoard()
        {
            Box.Stretch = Stretch.Uniform;
            PlayGrid.Rows = Constants.NbRow;
            PlayGrid.Columns = Constants.NbColumn;

            for (int row = 0; row < Constants.NbRow; row++)
            {
                for (int col = 0; col < Constants.NbColumn; col++)
                {

                    Label lbl = new Label();
                    lbl.BorderBrush = new SolidColorBrush(Color.FromRgb(73, 136, 64));
                    lbl.BorderThickness = new Thickness(Constants.BorderThickness);
                
                    lbl.MouseDown += ImageMouseDown;
                    lbl.MouseEnter += ImageMouseEnter;
                    lbl.MouseLeave += ImageMouseLeave;

                    PlayGrid.Children.Add(lbl);

                    Grid.SetColumn(lbl, col);
                    Grid.SetRow(lbl, row);
                }
            }
        }

        public void SetCaseImage(int column, int line, bool isWhite)
        {
            Label img = (Label)PlayGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == line && Grid.GetColumn(e) == column);

            if (!gameManager.IsPlayabe(column, line))
            {
                img.Background = Constants.getPawnBrush(gameManager.GetPawn(column, line), Constants.NormalOpacity);
            }
            else
            {
                img.Background = Constants.getPawnBrush(Pawn.getPawn(gameManager.IsCurrentPlayerWhite()), Constants.PlayableOpacity);
            }
        }

        // +----------
        // | 
        // +----------

        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label img = (Label)sender;
            int colume = Grid.GetColumn(img);
            int line = Grid.GetRow(img);

            if(gameManager.IsPlayabe(colume, line))
            {
                gameManager.Play(colume, line);
            }
        }
        
        private void ImageMouseEnter(object sender, MouseEventArgs e)
        {
            Label img = (Label)sender;
            int column = Grid.GetColumn(img);
            int line = Grid.GetRow(img);
            
            if (gameManager.GetPawn(column, line) == Pawn.Empty)
            {
                img.Background = Constants.getPawnBrush(Pawn.getPawn(gameManager.IsCurrentPlayerWhite()), Constants.NormalOpacity);
            }
        }
        
        private void ImageMouseLeave(object sender, MouseEventArgs e)
        {
            Label img = (Label)sender;
            int column = Grid.GetColumn(img);
            int line = Grid.GetRow(img);
            

            if (!gameManager.IsPlayabe(column, line))
            {
                img.Background = Constants.getPawnBrush(gameManager.GetPawn(column, line), Constants.NormalOpacity);
            }
            else
            {
                img.Background = Constants.getPawnBrush(Pawn.getPawn(gameManager.IsCurrentPlayerWhite()), Constants.PlayableOpacity);
            }
            
        }
    }
}
