using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

namespace HW_25._11._23_client_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        public MainWindow()
        {
            InitializeComponent();
            Curr1.SelectedItem = Curr1.Items[0];
            Curr2.SelectedItem = Curr2.Items[1];
            DisConnect.IsEnabled = false;
            Calculate.IsEnabled=false;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginWindow = new LoginWindow();
                if (loginWindow.ShowDialog() == true)
                {
                    string login = loginWindow.Username;
                    string password = loginWindow.Password;
                    client = new TcpClient("127.0.0.1", 8000);
                    stream = client.GetStream();

                    string message = $"{login} {password}";
                    byte[] messageData = Encoding.Unicode.GetBytes(message);
                    await stream.WriteAsync(messageData, 0, messageData.Length);

                    byte[] data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = await stream.ReadAsync(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    string result = builder.ToString();
                    Result.Text = result;
                    Connect.IsEnabled = false;
                    DisConnect.IsEnabled = true;
                    Calculate.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Result.Text = $"Помилка підключення: {ex.Message}";
            }
        }
           
          
            private void DisConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    stream?.Close();
                    client?.Close();
                    Result.Text = "Вiдключено вiд сервера";
                    Connect.IsEnabled = true;
                    DisConnect.IsEnabled = false;
                    Calculate.IsEnabled = false;
                }
                else
                {
                    Result.Text = "З'єднання неактивне.";
                    Connect.IsEnabled = true;
                    DisConnect.IsEnabled = false;
                    Calculate.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Result.Text = $"Помилка вiдключення: {ex.Message}";
            }
        }
        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currency1 = ((ComboBoxItem)Curr1.SelectedItem).Content.ToString();
                string currency2 = ((ComboBoxItem)Curr2.SelectedItem).Content.ToString();
                string request = $"{currency1} {currency2}";
                byte[] data = Encoding.Unicode.GetBytes(request);
                await stream.WriteAsync(data, 0, data.Length);
                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = await stream.ReadAsync(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                string result = builder.ToString();
                if (result.Contains("Кiлькiсть")) { Result.Text = result; }
                else { Result.Text = $"Курс валют: 1 {currency1} = {result} {currency2}"; }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка взаємодiї з сервером: {ex.Message}");
            }
        }
       
    }
}
