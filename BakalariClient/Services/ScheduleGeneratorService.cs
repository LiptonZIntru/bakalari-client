using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BakalariClient.Models;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace BakalariClient.Services
{
    class ScheduleGeneratorService
    {
        public readonly Schedule schedule;
        private readonly int leftHeadSize;
        private readonly int topHeadSize;

        public ScheduleGeneratorService(Schedule schedule, int leftHeadSize = 0, int topHeadSize = 0)
        {
            this.schedule = schedule;
            this.leftHeadSize = leftHeadSize;
            this.topHeadSize = topHeadSize;

            
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
        /// Create column sizes
        /// </summary>
        /// <param name="grid"> Grid where column definitions are created </param>
        /// <param name="count"> Number of columns </param>
        public void GenerateColumnDefinitions(Grid grid, int count)
        {
            for (int i = 0; i < count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
            }
        }

        /// <summary>
        /// Create row sizes
        /// </summary>
        /// <param name="grid"> Grid where row definitions are created </param>
        /// /// <param name="count"> Number of rows </param>
        public void GenerateRowDefinitions(Grid grid, int count)
        {
            for (int i = 0; i < count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star),
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
            if (scheduleSubject == null)
            {
                return new TextBlock();
            }
            TextBlock textBlock = new TextBlock
            {
                Text = scheduleSubject.ShortName,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
            };

            Card card = new Card()
            {
                Content = textBlock,
                Margin = new Thickness(2),
                UniformCornerRadius = 3,
            };

            return card;
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
            this.GenerateColumnDefinitions(grid, leftHeadSize);
            this.GenerateRowDefinitions(grid, topHeadSize);

            int dayNumber = 0;
            foreach (ScheduleDay scheduleDay in schedule.ScheduleDays)
            {
                int subjectNumber = 0;
                foreach (ScheduleSubject scheduleSubject in scheduleDay.ScheduleSubjects)
                {
                    UIElement uiElement = this.GenerateCell(scheduleSubject);
                    Grid.SetRow(uiElement, dayNumber + topHeadSize);
                    Grid.SetColumn(uiElement, subjectNumber + leftHeadSize);
                    grid.Children.Add(uiElement);
                    subjectNumber++;
                }
                dayNumber++;
            }

            return grid;
        }


    }
}
