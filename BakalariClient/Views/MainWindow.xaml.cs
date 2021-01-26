using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
using BakalariClient.Models;
using BakalariClient.Services;
using BakalariClient.Views;
using Newtonsoft.Json;

namespace BakalariClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CookieContainer cookieContainer;
        private Config config;
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                CredentialService credentialService = new CredentialService();
                config = credentialService.GetCredentials();
            }
            catch (Exception e)
            {
                CredentialWindow credentialWindow = new CredentialWindow();
                credentialWindow.ShowDialog();
            }
            LoadSchedule();
        }
        

        private void LoadSchedule()
        {
            // Login user and set cookies
            CredentialService credentialService = new CredentialService();
            config = credentialService.GetCredentials();

            LoginService loginService = new LoginService(config.LoginUrl);
            cookieContainer = loginService.Authorize(config.Credential);


            // Get Schedule (rozvrh)
            ScheduleService scheduleService = new ScheduleService(cookieContainer, config.ScheduleUrl);
            scheduleService.GetHtmlPage();
            Schedule schedule = scheduleService.GetSchedule();

            ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule, ScheduleContentGrid);
            scheduleGenerator.GenerateSchedule();
        }
    }
}
