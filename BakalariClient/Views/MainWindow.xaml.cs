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
using System.Windows.Threading;
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
        private DispatcherTimer dispatcherTimer;
        private Schedule schedule;
        public MainWindow()
        {
            InitializeComponent();
            logService = new LogService();
            logService.Add("\n\nProgram started\n");

            try
            {
                LoadSchedule();
            }
            catch
            {
                logService.Add("Schedule loading failed");
            }
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            LoadSchedule();
        }

        private void LoadScheduleFromServer()
        {
            logService.Add("Reading credentials file...");

            // Login user and set cookies
            ConfigService credentialService = new ConfigService();
            config = credentialService.GetCredentials();

            logService.Add("Success");
            logService.Add("Logging in user...");

            LoginService loginService = new LoginService(config.LoginUrl);
            cookieContainer = loginService.Authorize(config.Credential);

            logService.Add("Success");
            logService.Add("Parsing schedule...");

            // Get Schedule (rozvrh)
            string url = config.ScheduleUrl;
            if ((DateTime.Today.DayOfWeek.ToString() == "Saturday" || DateTime.Today.DayOfWeek.ToString() == "Sunday") && config.DisplayNextWeekFromSaturday == true)
            {
                url += "?s=next";
            }
            ScheduleService scheduleService = new ScheduleService(cookieContainer, url);

            scheduleService.GetHtmlPage();
            schedule = scheduleService.GetSchedule();


            logService.Add("Success");
            logService.Add("Generating schedule...");

            ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule, ScheduleContentGrid);
            scheduleGenerator.GenerateSchedule();

            logService.Add("Success");
        }

        private void LoadSchedule()
        {
            if (ScheduleContentGrid.Children.Count != 0)
            {
                ScheduleContentGrid.Children.RemoveRange(0, ScheduleContentGrid.Children.Count);
            }
            if (ScheduleContentGrid.ColumnDefinitions.Count != 0)
            {
                ScheduleContentGrid.ColumnDefinitions.RemoveRange(0, ScheduleContentGrid.ColumnDefinitions.Count);
            }
            if (ScheduleContentGrid.RowDefinitions.Count != 0)
            {
                ScheduleContentGrid.RowDefinitions.RemoveRange(0, ScheduleContentGrid.RowDefinitions.Count);
            }

            try
            {
                LoadScheduleFromServer();
            }
            catch
            {
                LoadScheduleFromFile();
            }
        }

        private void LoadScheduleFromFile()
        {
            try
            {
                logService.Add("Loading schedule from file...");

                ScheduleService scheduleService = new ScheduleService();
                schedule = scheduleService.LoadScheduleFromFile();

                ScheduleGeneratorService scheduleGenerator = new ScheduleGeneratorService(schedule, ScheduleContentGrid);
                scheduleGenerator.GenerateSchedule();
            }
            catch
            {
                logService.Add("Schedule loading from file failed");
            }
        }
    }
}
