using final_pr15.Service;
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

namespace final_pr15.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrPage.xaml
    /// </summary>
    public partial class RegistrPage : Page
    {
        public RegistrPage()
        {
            InitializeComponent();
        }

        private void AdminLogin_Click(object sender, RoutedEventArgs e)
        {
            string pin = PinBox.Password;

            if (AuthService.LoginAsAdmin(pin))
            {
                NavigationService.Navigate(new ProductsPage());
            }
            else
            {
                ErrorText.Text = "Неверный пароль";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void GuestLogin_Click(object sender, RoutedEventArgs e)
        {
            AuthService.LoginAsGuest();
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
