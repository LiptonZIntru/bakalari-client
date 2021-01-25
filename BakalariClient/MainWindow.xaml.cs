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
        private Config config;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            // Login user and set cookies
            string filename = "config.json";
#if DEBUG
            filename = @"..\..\" + filename;
#endif
            GetCredentials(filename);

            LoginService loginService = new LoginService(config.LoginUrl);
            cookieContainer = loginService.Authorize(config.Credential);


            // Get Schedule (rozvrh)
            ScheduleService scheduleService = new ScheduleService(cookieContainer, config.ScheduleUrl);
            scheduleService.GetHtmlPage();
            Schedule schedule = scheduleService.GetSchedule();

            ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule, leftHeadSize: 1, topHeadSize: 1);
            scheduleGenerator.GenerateSchedule(ScheduleContentGrid);
        }

        private Config GetCredentials(string filename)
        {
            CredentialReaderService jsonReaderService = new CredentialReaderService(filename);
            config = jsonReaderService.GetConfig();
            return config;
        }
    }
}
