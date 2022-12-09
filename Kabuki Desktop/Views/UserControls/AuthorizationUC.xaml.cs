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

namespace Kabuki_Desktop.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationUC.xaml
    /// </summary>
    public partial class AuthorizationUC : UserControl
    {
        public AuthorizationUC()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text,0))
                e.Handled = true;
        }
    }
}
