using System.Windows;
using WpfEfAuthen.Services;


namespace WpfEfAuthen
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtRegisterUsername.Text;
            string password = txtRegisterPassword.Password;

            using var context = new MyContext();
            var user = new tblUser
            {
                Username = username,
                PasswordHash = AuthService.HashPassword(password)
            };
            context.tblUser.Add(user);
            context.SaveChanges();

            MessageBox.Show("Registration Successful!");
            this.Close();
        }
    }
}
