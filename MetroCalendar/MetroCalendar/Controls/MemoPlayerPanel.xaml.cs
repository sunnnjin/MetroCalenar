// <copyright file="MemoPlayerPanel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Controls
{
    public partial class MemoPlayerPanel : UserControl
    {
        #region internal data
        private MemoPlayerViewModel _memoPlayerViewModel;
        private string _extraLoadingStr = "";
        private DispatcherTimer _timer;

        private const double MaxTimer = 500; // milliseconds
        private static readonly string[] ExtraLoadingArray = {
                ".", "..", "...", "" };
        #endregion

        #region external methods
        public void Start()
        {
            foreach (UIElement element in BarsStackPanel.Children)
            {
                Bar bar = element as Bar;
                bar.Start();
            }

            micophonePanel.StartAnimation();

            _timer.Start();
        }

        public void Stop()
        {
            foreach (UIElement element in BarsStackPanel.Children)
            {
                Bar bar = element as Bar;
                bar.Stop();
            }

            micophonePanel.StopAnimation();

            _timer.Stop();
            statusTextBlock.Text = AppResources.PauseText;
        }
        #endregion

        #region constructor
        public MemoPlayerPanel()
        {
            InitializeComponent();

            _memoPlayerViewModel = new MemoPlayerViewModel();
            this.DataContext = _memoPlayerViewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(MaxTimer);
            _timer.Tick += new EventHandler(OnTimerTick);
            _timer.Start();

            for (int i = 0; i < 32; ++i)
            {
                Bar bar = new Bar()
                {
                    Margin = new Thickness(8, 0, 0, 0),
                    Height = 220
                };

                BarsStackPanel.Children.Add(bar);
            }
        }
        #endregion

        #region internal methods
        private void OnTimerTick(object sender, EventArgs e)
        {
            _extraLoadingStr = ExtraLoadingArray[_extraLoadingStr.Length];
            statusTextBlock.Text = AppResources.PlayingText + _extraLoadingStr;
        }
        #endregion
    }
}
