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
        private int leftHeadSize;
        private int topHeadSize;
        private Grid Grid;

        public ScheduleGeneratorService(Schedule schedule, Grid grid, int leftHeadSize = 0, int topHeadSize = 0, bool dayLabel = true, bool timeLabel = true)
        {
            this.schedule = schedule;
            this.leftHeadSize = leftHeadSize;
            this.topHeadSize = topHeadSize;
            Grid = grid;


            if (dayLabel)
            {
                this.leftHeadSize += 1;
            }
            if (timeLabel)
            {
                this.topHeadSize += 1;
            }

            // Generate definition
            this.GenerateColumnDefinitions();
            this.GenerateColumnDefinitions(this.leftHeadSize);
            this.GenerateRowDefinitions(5 + this.topHeadSize);

            if (dayLabel)
            {
                GenerateDayLabel();
            }

            if (timeLabel)
            {
                GenerateTimeLabel();
            }
        }

        /// <summary>
        /// Generate time labels (0. lesson, 1., 2., etc.)
        /// </summary>
        public void GenerateTimeLabel()
        {
            for (int i = 0; i < schedule.ScheduleDays[0].ScheduleSubjects.Count; i++)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };

                Card card = new Card()
                {
                    Content = textBlock,
                    Margin = new Thickness(2),
                    UniformCornerRadius = 3,
                    Opacity = 0.5,
                    Height = 30,
                };
                Grid.SetRow(card, topHeadSize - 1);
                Grid.SetColumn(card, i + leftHeadSize);

                Grid.Children.Add(card);
            }
        }

        /// <summary>
        /// Generate day labels (Mo, Tu, etc.)
        /// </summary>
        public void GenerateDayLabel()
        {
            int i = 0;
            foreach (string dayLabel in ScheduleParserService.dayLabels)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = dayLabel.Substring(0,2),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };

                Card card = new Card()
                {
                    Content = textBlock,
                    Margin = new Thickness(2),
                    UniformCornerRadius = 3,
                    Opacity = 0.5,
                };
                Grid.SetRow(card, i + topHeadSize);
                Grid.SetColumn(card, leftHeadSize - 1);

                i++;
                Grid.Children.Add(card);
            }
        }
        

        /// <summary>
        /// Create column sizes
        /// </summary>
        public void GenerateColumnDefinitions()
        {
            int maxLessons = schedule.ScheduleDays[0].ScheduleSubjects.Count;
            for (int i = 0; i < maxLessons; i++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
            }
        }

        /// <summary>
        /// Create column sizes
        /// </summary>
        /// <param name="count"> Number of columns </param>
        public void GenerateColumnDefinitions(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
            }
        }

        /// <summary>
        /// Create row sizes
        /// </summary>
        /// /// <param name="count"> Number of rows </param>
        public void GenerateRowDefinitions(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Grid.RowDefinitions.Add(new RowDefinition()
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
        /// Generate Grid subjects
        /// </summary>
        /// <returns></returns>
        public Grid GenerateSchedule()
        {
            int dayNumber = 0;
            foreach (ScheduleDay scheduleDay in schedule.ScheduleDays)
            {
                int subjectNumber = 0;
                foreach (ScheduleSubject scheduleSubject in scheduleDay.ScheduleSubjects)
                {
                    UIElement uiElement = this.GenerateCell(scheduleSubject);
                    Grid.SetRow(uiElement, dayNumber + topHeadSize);
                    Grid.SetColumn(uiElement, subjectNumber + leftHeadSize);
                    Grid.Children.Add(uiElement);
                    subjectNumber++;
                }
                dayNumber++;
            }

            return Grid;
        }


    }
}
