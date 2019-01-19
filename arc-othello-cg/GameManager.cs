using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace arc_othello_cg
{
    [Serializable]
    class GameManager : INotifyPropertyChanged
    {
        
        private Board board;
        private bool IsCurrentPlayerWhite { get; set; }
        private List<Point> playables = new List<Point>();

        // Players infos

        private int playerWhiteScore;
        private int playerBlackScore;

        [NonSerialized()] private DispatcherTimer timer;
        [NonSerialized()] private Stopwatch PlayerWhiteTimer;
        [NonSerialized()] private Stopwatch PlayerBlackTimer;

        private TimeSpan playerWhiteTime;
        private TimeSpan playerBlackTime;
        
        // +----------------------------------------------------------------------
        // | Constructors
        // +----------------------------------------------------------------------

        public GameManager()
        {
            this.board = new Board();
            this.IsCurrentPlayerWhite = true;

            ResetTime();
            Init();
        }

        // +----------------------------------------------------------------------
        // | PUBLIC methods
        // +----------------------------------------------------------------------

        /// <summary>
        /// Init the gamemanager
        /// </summary>
        public void Init()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            PlayerWhiteTimer = new Stopwatch();
            PlayerBlackTimer = new Stopwatch();

            Console.WriteLine(playerWhiteTime.TotalSeconds);
            Console.WriteLine(playerBlackTime.TotalSeconds);

            TimerStart();

            UpdatePlayables();
            UpdatePlayersInfo();

            DisplayBoard();
        }

        /// <summary>
        /// Play the move
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="line">Line</param>
        public void Play(int column, int line)
        {
            if(IsCurrentPlayerWhite)
            {
                PlayerWhiteTimer.Stop();
                PlayerBlackTimer.Start();
            }
            else
            {
                PlayerWhiteTimer.Start();
                PlayerBlackTimer.Stop();
            }

            if (playables.Contains(new Point(column, line)))
            {
                board.PlayMove(column, line, IsCurrentPlayerWhite);

                IsCurrentPlayerWhite = !IsCurrentPlayerWhite;

                UpdatePlayersInfo();
                UpdatePlayables();

                DisplayBoard();

                if (playables.Count == 0)
                {
                    IsCurrentPlayerWhite = !IsCurrentPlayerWhite;
                    UpdatePlayables();
                    DisplayBoard();

                    if (playables.Count == 0)
                    {
                        EndGame();
                    }
                }
            }
        }

        /// <summary>
        /// Update the display of the board
        /// </summary>
        public void DisplayBoard()
        {
            for (int line = 0; line < Constants.NbRow; line++)
            {
                for (int column = 0; column < Constants.NbColumn; column++)
                {
                    Brush brush = GetCaseBrush(column, line);
                    Constants.mainWindow.SetCaseImage(column, line, brush);
                }
            }
        }

        /// <summary>
        /// Get the brush to display depending on the position
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="line">Line</param>
        /// <returns>The brush</returns>
        public Brush GetCaseBrush(int column, int line)
        {
            float opactity = Constants.NormalOpacity;
            Brush brush = Constants.getPawnBrush(board.GetPawn(column, line), opactity);

            if (playables.Contains(new Point(column, line)))
            {
                opactity = Constants.PlayableOpacity;
                brush = Constants.getPawnBrush(Pawn.getPawn(IsCurrentPlayerWhite), opactity);
            }
            return brush;
        }
       
        /// <summary>
        /// Starts the timer
        /// </summary>
        public void TimerStart()
        {
            timer.Start();
            if (IsCurrentPlayerWhite)
            {
                PlayerWhiteTimer.Start();
            }
            else
            {
                PlayerBlackTimer.Start();
            }
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void TimerPause()
        {
            PlayerWhiteTimer.Stop();
            PlayerBlackTimer.Stop();
            timer.Stop();
        }

        /// <summary>
        /// ave the current players time in the timespans
        /// </summary>
        public void SaveTime()
        {
            playerWhiteTime += PlayerWhiteTimer.Elapsed;
            playerBlackTime += PlayerBlackTimer.Elapsed;
        }

        /// <summary>
        /// Resets the timers
        /// </summary>
        public void ResetTime()
        {
            playerBlackTime = new TimeSpan();
            playerWhiteTime = new TimeSpan();
        }

        // +----------------------------------------------------------------------
        // | PRIVATE methods
        // +----------------------------------------------------------------------

        private void UpdatePlayables()
        {
            playables.Clear();

            for (int line = 0; line < Constants.NbRow; line++)
            {
                for (int column = 0; column < Constants.NbColumn; column++)
                {
                    if (board.IsPlayable(column, line, IsCurrentPlayerWhite))
                    {
                        playables.Add(new Point(column, line));
                    }
                }
            }
        }

        private void UpdatePlayersInfo()
        {
            PlayerWhiteScore = board.GetWhiteScore();
            PlayerBlackScore = board.GetBlackScore();

            Constants.mainWindow.UpdateSpace(playerWhiteScore, playerBlackScore);
        }

        private void EndGame()
        {
            string winner = board.GetWhiteScore() >= board.GetBlackScore() ? "Joueur blanc" : "Joueur noir";
            
            if (Constants.mainWindow.ShowComfirm(winner + " gagne la partie ! Voulez vous rejouer ?"))
            {
                Constants.mainWindow.ResetGame();
            }
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            TimeSpan tmpTimeWhite = PlayerWhiteTimer.Elapsed + playerWhiteTime;
            TimeSpan tmpTimeBlack = PlayerBlackTimer.Elapsed + playerBlackTime;

            Constants.mainWindow.SetTimes(tmpTimeWhite, tmpTimeBlack);
        }

        // +----------------------------------------------------------------------
        // | DataBinding
        // +----------------------------------------------------------------------

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int PlayerWhiteScore
        {
            get { return playerWhiteScore; }
            set
            {
                playerWhiteScore = value;
                RaisePropertyChanged("PlayerWhiteScore");
            }
        }
        public int PlayerBlackScore
        {
            get { return playerBlackScore; }
            set
            {
                playerBlackScore = value;
                RaisePropertyChanged("PlayerBlackScore");
            }
        }
        
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
