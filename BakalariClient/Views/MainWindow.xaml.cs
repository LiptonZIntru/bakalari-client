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
        private LogService logService;
        public MainWindow()
        {
            InitializeComponent();
            logService = new LogService();
            logService.Add("\n\nProgram started\n");

            try
            {
                logService.Add("Getting credentials...");

                CredentialService credentialService = new CredentialService();
                config = credentialService.GetCredentials();

                logService.Add("Success");
            }
            catch (Exception e)
            {
                logService.Add("Failed");
                logService.Add("Creating credentials file");

                CredentialWindow credentialWindow = new CredentialWindow();
                credentialWindow.ShowDialog();
            }

            try
            {
                LoadSchedule();
            }
            catch
            {
                logService.Add("Schedule loading failed");
            }
        }
        

        private void LoadSchedule()
        {
            logService.Add("Reading credentials file...");

            // Login user and set cookies
            CredentialService credentialService = new CredentialService();
            config = credentialService.GetCredentials();

            logService.Add("Success");
            logService.Add("Logging in user...");

            LoginService loginService = new LoginService(config.LoginUrl);
            cookieContainer = loginService.Authorize(config.Credential);

            logService.Add("Success");
            logService.Add("Parsing schedule...");

            // Get Schedule (rozvrh)
            ScheduleService scheduleService = new ScheduleService(cookieContainer, config.ScheduleUrl);
            scheduleService.GetHtmlPage();
            Schedule schedule = scheduleService.GetSchedule();

            logService.Add("Success");
            logService.Add("Generating schedule...");

            ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule, ScheduleContentGrid);
            scheduleGenerator.GenerateSchedule();

            logService.Add("Success");
        }
    }
}
