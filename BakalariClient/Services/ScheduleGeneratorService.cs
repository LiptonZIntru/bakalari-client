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
using System.Windows.Media;

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
                GenerateDayLabels();
            }

            if (timeLabel)
            {
                GenerateTimeLabels();
            }
        }

        /// <summary>
        /// Generate time labels (0. lesson, 1., 2., etc.)
        /// </summary>
        public void GenerateTimeLabels()
        {
            int i = 0;
            foreach (ScheduleTime scheduleTime in schedule.ScheduleTimes)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = $"{ scheduleTime.Num }\n{ scheduleTime.Begin } - { scheduleTime.End }",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 10,
                    TextAlignment = TextAlignment.Center,
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
                i++;
            }
        }

        /// <summary>
        /// Generate day labels (Mo, Tu, etc.)
        /// </summary>
        public void GenerateDayLabels()
        {
            int i = 0;
            foreach (ScheduleDay scheduleDay in schedule.ScheduleDays)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = $"{ scheduleDay.DayNameShort }\n{ scheduleDay.DateShort }",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 12,
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
                ToolTip = GenerateTooltip(scheduleSubject),
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

        public UIElement GenerateTooltip(ScheduleSubject scheduleSubject)
        {
            string tooltipContent = (scheduleSubject.Teacher != "" ?       $"Učitel: {scheduleSubject.Teacher}\n" : "") +
                                    (scheduleSubject.ClassLocation != "" ? $"Učebna: {scheduleSubject.ClassLocation}\n" : "") +
                                    (scheduleSubject.Group != "" ?         $"Skupina: {scheduleSubject.Group}\n" : "") +
                                    (scheduleSubject.LessonSubject != "" ? $"Téma: {scheduleSubject.LessonSubject}\n" : "") +
                                    (scheduleSubject.ChangeInfo != "" ?    $"Změny: {scheduleSubject.ChangeInfo}\n" : "") +
                                    (scheduleSubject.Notice != "" ?        $"Upozornění: {scheduleSubject.Notice}\n" : "");
            TextBlock textBlock = new TextBlock()
            {
                Text = tooltipContent.Trim(),
                Foreground = new SolidColorBrush()
                {
                    Color = Color.FromRgb(255, 255, 247),
                }
                
            };
            Card card = new Card()
            {
                Content = textBlock,
                Padding = new Thickness(3),
            };
            return card;
        }
    }
}
