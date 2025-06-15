using System.Windows;
using WpfEfAuthen.Services;

namespace WpfEfAuthen
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public (bool isAuthenticated, string userRole) LoginFunction(string username, string password)
        {
            using var context = new MyContext();
            var user = context.tblUser.SingleOrDefault(u => u.Username == username);

            if (user != null && AuthService.VerifyPassword(password, user.PasswordHash))
            {
                return (true, user.UserRole); // Returns authentication status & role
            }
            return (false, null);
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLoginUsername.Text;
            string password = txtLoginPassword.Password;

            using var context = new MyContext();
            var user = context.tblUser.SingleOrDefault(u => u.Username == username);

            if (user != null && AuthService.VerifyPassword(password, user.PasswordHash))
            {
                MessageBox.Show("Login Successful!");
                if (user.UserRole == "Admin")
                {
                    // Open Admin Dashboard
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else
                {
                    // Open User Dashboard
                    var userWindow = new UserWindow();
                    userWindow.Show();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials.");
            }
        }
    }
}
