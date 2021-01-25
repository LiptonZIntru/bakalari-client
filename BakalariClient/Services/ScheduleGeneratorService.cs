using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BakalariClient.Models;

namespace BakalariClient.Services
{
    class ScheduleGeneratorService
    {
        public readonly Schedule schedule;

        public ScheduleGeneratorService(Schedule schedule)
        {
            this.schedule = schedule;
        }

        /// <summary>
        /// Create column sizes
        /// </summary>
        /// <param name="grid"> Grid where column definitions are created </param>
        public void GenerateColumnDefinitions(Grid grid)
        {
            int maxLessons = schedule.ScheduleDays[0].ScheduleSubjects.Count;
            for (int i = 0; i < maxLessons; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
            }
        }

        /// <summary>
        /// Generate single subject cell
        /// </summary>
        /// <param name="scheduleSubject"> ScheduleSubject from which cell is generated </param>
        /// <returns></returns>
        public UIElement GenerateCell(ScheduleSubject scheduleSubject)
        {
            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            TextBlock textBlock = new TextBlock
            {
                Text = scheduleSubject == null ? "" : scheduleSubject.ShortName
            };

            grid.Children.Add(textBlock);
            return grid;
        }

        /// <summary>
        /// Generate grid subjects
        /// </summary>
        /// <param name="grid"> Grid in which subjects are generated </param>
        /// <returns></returns>
        public Grid GenerateSchedule(Grid grid)
        {
            // Generate definition
            this.GenerateColumnDefinitions(grid);

            int dayNumber = 0;
            foreach (ScheduleDay scheduleDay in schedule.ScheduleDays)
            {
                int subjectNumber = 0;
                foreach (ScheduleSubject scheduleSubject in scheduleDay.ScheduleSubjects)
                {
                    UIElement uiElement = this.GenerateCell(scheduleSubject);
                    Grid.SetRow(uiElement, dayNumber);
                    Grid.SetColumn(uiElement, subjectNumber);
                    grid.Children.Add(uiElement);
                    subjectNumber++;
                }
                dayNumber++;
            }

            return grid;
        }
    }
}
