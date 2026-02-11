using System.Windows;

namespace ShoeShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Setup();
            MainFrame.Navigate(new ProductsPage());
        }

        private void Setup()
        {
            User user = LoginWindow.CurrentUser;

            if (user == null)
            {
                txtUserInfo.Text = "Guest";
                btnOrders.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtUserInfo.Text = user.FullName + " (" + user.Role.RoleName + ")";

                if (user.RoleID == 1 || user.RoleID == 2)
                {
                    btnOrders.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow.CurrentUser = null;
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}