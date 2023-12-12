using System.Windows;

namespace HW_25._11._23_client_
{
    public partial class LoginWindow : Window
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Password;

            DialogResult = true;
        }
    }
}
