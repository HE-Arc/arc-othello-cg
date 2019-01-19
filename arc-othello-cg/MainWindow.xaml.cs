using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
            InitWindows();
            
            gameManager = new GameManager();
            this.DataContext = gameManager;
        }

        // +----------------------------------------------------------------------
        // | PUBLIC methods
        // +----------------------------------------------------------------------

        /// <summary>
        /// Set the board label brush
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="line">Line</param>
        /// <param name="brush">The new brush</param>
        public void SetCaseImage(int column, int line, Brush brush)
        {
            Label img = (Label)PlayGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == line && Grid.GetColumn(e) == column);
            img.Background = brush;
        }
        
        /// <summary>
        /// Update the domination sapce
        /// </summary>
        /// <param name="whiteScore">White score</param>
        /// <param name="blackScore">Black score</param>
        public void UpdateSpace(int whiteScore, int blackScore)
        {
            WhiteSpace.Height = new GridLength(whiteScore, GridUnitType.Star);
            BlackSpace.Height = new GridLength(blackScore, GridUnitType.Star);
        }

        /// <summary>
        /// Restart the game
        /// </summary>
        public void ResetGame()
        {
            gameManager.TimerPause();
            gameManager = new GameManager();
            this.DataContext = gameManager;
        }

        /// <summary>
        /// Show a comfirm windows
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="yesOption">Yes option text</param>
        /// <param name="noOption">No option text</param>
        /// <returns></returns>
        public bool ShowComfirm(string message, string yesOption="Oui", string noOption="Non")
        {
            ComfirmWindows comfirm = new ComfirmWindows(message, yesOption, noOption);
            
            comfirm.Owner = Application.Current.MainWindow;
            comfirm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            rct_shadow.Visibility = Visibility.Visible;

            gameManager.TimerPause();
            bool? result = comfirm.ShowDialog();
            gameManager.TimerStart();
            
            rct_shadow.Visibility = Visibility.Hidden;

            return result == true;
        }

        /// <summary>
        /// Display the times
        /// </summary>
        /// <param name="whiteTime">White time</param>
        /// <param name="blackTime">Black time</param>
        public void SetTimes(TimeSpan whiteTime, TimeSpan blackTime)
        {
            PlayerWhiteTime.Content = whiteTime.Minutes + ":" + whiteTime.Seconds;
            PlayerBlackTime.Content = blackTime.Minutes + ":" + blackTime.Seconds;
        }

        // +----------------------------------------------------------------------
        // | PRIVATE methods
        // +----------------------------------------------------------------------

        private void InitWindows()
        {
            rct_shadow.Visibility = Visibility.Hidden;
            Constants.mainWindow = this;
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
                    lbl.BorderBrush = new SolidColorBrush(Constants.BoardBorder);
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

        // +----------------------------------------------------------------------
        // | EVENTS
        // +----------------------------------------------------------------------

        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label img = (Label)sender;
            int colume = Grid.GetColumn(img);
            int line = Grid.GetRow(img);

            gameManager.Play(colume, line);
        }
        
        private void ImageMouseEnter(object sender, MouseEventArgs e)
        {
            Label img = (Label)sender;
            int column = Grid.GetColumn(img);
            int line = Grid.GetRow(img);

            Brush brush = gameManager.GetCaseBrush(column, line);
            brush.Opacity = Constants.NormalOpacity;

            SetCaseImage(column, line, brush);
        }
        
        private void ImageMouseLeave(object sender, MouseEventArgs e)
        {
            Label img = (Label)sender;
            int column = Grid.GetColumn(img);
            int line = Grid.GetRow(img);

            SetCaseImage(column, line, gameManager.GetCaseBrush(column, line));
        }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !ShowComfirm("Voulez-vous vraiment quitter la partie ?");
        }
        
        // Restart
        private void RestartGame(object sender, RoutedEventArgs e)
        {
            if(ShowComfirm("Voulez-vous vraiment recommencer la partie ?"))
            {
                ResetGame();
            }
        }

        // Save
        private void SaveGame(object sender, RoutedEventArgs e)
        {
            gameManager.TimerPause();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Othello save|*.oth";
            saveFileDialog1.Title = "Sauvegarder votre partie";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                gameManager.SaveTime();
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, gameManager);
                }
                gameManager.ResetTime();
            }
            gameManager.TimerStart();
        }
        
        //Open
        private void OpenGame(object sender, RoutedEventArgs e)
        {
            gameManager.TimerPause();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Othello save|*.oth";
            openFileDialog1.Title = "Charger une partie";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    gameManager = (GameManager)formatter.Deserialize(stream);
                    this.DataContext = gameManager;
                    gameManager.Init();
                }
            }
            gameManager.TimerStart();
        }
    }
}
