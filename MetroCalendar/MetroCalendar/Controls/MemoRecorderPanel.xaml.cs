// <copyright file="MemoRecorderPanel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MetroCalendar.ViewModels;

namespace MetroCalendar.Controls
{
    public delegate void RecordingTimeoutEventHandler();

    public partial class MemoRecorderPanel : UserControl
    {
        #region event
        public event RecordingTimeoutEventHandler RecordingTimeout;
        #endregion

        #region internal data
        private DispatcherTimer _timeoutTimer;
        private DispatcherTimer _approachTimer;
        private MemoRecorderViewModel _viewModel;
        private bool _isApproach;
        private const double MaxTimeout = 1.5; // minutes
        private const double ApproachTimer = 1.0; // minutes
        private const int BarNumber = 32;

        public static readonly SolidColorBrush RedBrush =
                new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static readonly SolidColorBrush BlackBrush =
            new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        #endregion

        #region external methods
        public void Reset()
        {
            _viewModel.InitRecording();
        }

        public void Start(DateTime dateTime)
        {
            foreach (UIElement element in BarsStackPanel.Children)
            {
                Bar bar = element as Bar;
                bar.Start();
            }

            _viewModel.StartRecording(dateTime);

            _timeoutTimer.Start();

            _isApproach = false;
            _approachTimer.Interval = TimeSpan.FromMinutes(ApproachTimer);
            _approachTimer.Start();
        }

        public void Stop()
        {
            foreach (UIElement element in BarsStackPanel.Children)
            {
                Bar bar = element as Bar;
                bar.Stop();
            }

            _viewModel.StopRecording();

            _timeoutTimer.Stop();

            _approachTimer.Stop();
            timeTextBlock.Foreground = BlackBrush;
        }
        #endregion

        #region constructor
        public MemoRecorderPanel()
        {
            InitializeComponent();

            try
            {
                _viewModel = new MemoRecorderViewModel();
            }
            catch
            {
            }

            this.DataContext = _viewModel;

            _timeoutTimer = new DispatcherTimer();
            _timeoutTimer.Interval = TimeSpan.FromMinutes(MaxTimeout);
            _timeoutTimer.Tick += new EventHandler(OnTimeoutTimerTick);

            _approachTimer = new DispatcherTimer();
            _approachTimer.Tick += new EventHandler(OnApproachTimerTick);

            for (int i = 0; i < BarNumber; ++i)
            {
                Bar bar = new Bar()
                {
                    Margin = new Thickness(8, 0, 0, 0),
                    Height = 220
                };

                BarsStackPanel.Children.Add(bar);
            }

            timeTextBlock.Foreground = BlackBrush;
        }
        #endregion

        #region internal methods

        private void OnTimeoutTimerTick(object sender, EventArgs e)
        {
            if (RecordingTimeout != null)
            {
                RecordingTimeout();
            }
        }

        private void OnApproachTimerTick(object sender, EventArgs e)
        {
            if (!_isApproach)
            {
                _isApproach = true;

                _approachTimer.Stop();
                _approachTimer.Interval = TimeSpan.FromSeconds(1);
                _approachTimer.Start();
            }
            else
            {
                if (timeTextBlock.Foreground == BlackBrush)
                {
                    timeTextBlock.Foreground = RedBrush;
                }
                else
                {
                    timeTextBlock.Foreground = BlackBrush;
                }
            }
        }
        #endregion
    }
}
