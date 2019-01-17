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
using System.Windows.Shapes;

namespace arc_othello_cg
{
    /// <summary>
    /// Logique d'interaction pour ComfirmWindows.xaml
    /// </summary>
    public partial class ComfirmWindows : Window
    {
        public ComfirmWindows(string message, string yesOption = "Oui", string noOption = "Non")
        {
            InitializeComponent();

            txb_message.Text = message;
            btn_yes.Content = yesOption;
            btn_no.Content = noOption;

        }

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
