using System;
using System.Collections.Generic;
using System.IO;
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
using BakalariClient.Models;
using BakalariClient.Services;
using Newtonsoft.Json;

namespace BakalariClient.Views
{
    /// <summary>
    /// Interaction logic for CredentialWindow.xaml
    /// </summary>
    public partial class CredentialWindow : Window
    {
        private LogService logService;
        public CredentialWindow()
        {
            InitializeComponent();

            logService = new LogService();
            try
            {
                logService.Add("Getting credentials...");

                ConfigService credentialService = new ConfigService();
                credentialService.GetCredentials();

                logService.Add("Success");

                OpenMainWindow();
            }
            catch
            {
                logService.Add("Failed");
                logService.Add("Creating credentials file");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Config config = new Config()
            {
                Domain = UrlTextBox.Text,
                Credential = new Credential()
                {
                    Username = UsernameTextBox.Text,
                    Password = PasswordTextBox.Password,
                },
            };

            ConfigService credentialService = new ConfigService();
            credentialService.SetCredentials(config);

            logService.Add("Credentials file created");

            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
