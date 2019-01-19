using System.Windows;

namespace arc_othello_cg
{
    /// <summary>
    /// Logique d'interaction pour ComfirmWindows.xaml
    /// </summary>
    public partial class ComfirmWindows : Window
    {
        // +----------------------------------------------------------------------
        // | CONSTRUCTORS
        // +----------------------------------------------------------------------

        public ComfirmWindows(string message, string yesOption = "Oui", string noOption = "Non")
        {
            InitializeComponent();

            txb_message.Text = message;
            btn_yes.Content = yesOption;
            btn_no.Content = noOption;

        }

        // +----------------------------------------------------------------------
        // | PRIVATE methods
        // +----------------------------------------------------------------------

        private void Yes_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void No_Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
