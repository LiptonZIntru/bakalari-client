using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace BakalariClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CookieContainer cookieContainer;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Log in user based on credentials and store cookies
        /// </summary>
        /// <param name="loginService"> Instance of  LoginService</param>
        /// <returns> void </returns>
        private void AuthorizeUser(LoginService loginService)
        {
            CredentialReaderService jsonReaderService = new CredentialReaderService("credentials.json");
            Credential credentials = jsonReaderService.GetCredentialsFromFile();
            cookieContainer = loginService.Authorize(credentials);
        }

        private void Init()
        {
            // Login user and set cookies
            LoginService loginService = new LoginService(ConfigurationManager.AppSettings["loginUrl"]);
            AuthorizeUser(loginService);

            // Get Schedule (rozvrh)
            ScheduleService scheduleService = new ScheduleService(cookieContainer, ConfigurationManager.AppSettings["scheduleUrl"]);
            scheduleService.GetHtmlPage();
            Schedule schedule = scheduleService.GetSchedule();

            ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule);
            scheduleGenerator.GenerateSchedule(ScheduleGrid);
        }
    }
}
