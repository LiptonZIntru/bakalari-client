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
        private readonly string url;
        public MainWindow()
        {
            InitializeComponent();

            url = ConfigurationManager.AppSettings["url"];


            LoginService loginService = new LoginService();
            Task.Run(async () =>  await AuthorizeUser(loginService)).Wait();

            ScheduleService scheduleService = new ScheduleService(cookieContainer);
            Task.Run(async () => await scheduleService.GetSchedule()).Wait();
        }

        /// <summary>
        /// Wrapper method for log in user based on credentials amd store cookies
        /// </summary>
        /// <param name="loginService"> Instance of  LoginService</param>
        /// <returns> void </returns>
        private async Task AuthorizeUser(LoginService loginService)
        {
            JsonReaderService jsonReaderService = new JsonReaderService("credentials.json");
            Credential credentials = jsonReaderService.GetCredentials();
            cookieContainer = await loginService.Authorize(credentials);
        }
    }
}
