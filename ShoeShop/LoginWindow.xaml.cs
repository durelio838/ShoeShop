using System.Linq;
using System.Windows;

namespace ShoeShop
{
    public partial class LoginWindow : Window
    {
        public static User CurrentUser { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Enter login and password";
                return;
            }

            try
            {
                using (var db = new ShoeShopContext())
                {
                    var user = db.Users
                        .Include("Role")
                        .FirstOrDefault(u => u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        CurrentUser = user;
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        txtError.Text = "Wrong login or password";
                    }
                }
            }
            catch (System.Exception ex)
            {
                txtError.Text = "Error: " + ex.Message;
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}