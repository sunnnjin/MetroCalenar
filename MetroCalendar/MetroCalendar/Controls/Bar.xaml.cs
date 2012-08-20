// <copyright file="Bar.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MetroCalendar.Controls
{
    public partial class Bar : UserControl
    {
        #region internal data
        DispatcherTimer _timer;
        Random _rand;
        bool _isRect1MoveUp; // true for up, false for down
        bool _isRect2MoveUp; // true for up, false for down

        double _rect1TargetTop;
        double _rect2TargetTop;

        const int MoveSpeed = 10;
        const int Rect2Margin = 2;
        #endregion

        #region external methods
        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
        #endregion

        #region constructor
        public Bar()
        {
            InitializeComponent();

            long tick = DateTime.Now.Ticks;
            _rand = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(OnTimerTick);
            _timer.Interval = TimeSpan.FromMilliseconds(111);
        }
        #endregion

        #region internal methods
        private void OnTimerTick(object sender, EventArgs e)
        {
            double rect1Top = Canvas.GetTop(rect1);
            double rect2Top = Canvas.GetTop(rect2);

            if (rect2Top + rect2.ActualHeight + Rect2Margin >= rect1Top)
            {
                // collision
                _isRect1MoveUp = false;
                _isRect2MoveUp = true;

                _rect1TargetTop = _rand.Next((int)rect1Top, (int)canvas.Height);
                _rect2TargetTop = _rand.Next((int)(rect2Top / 2), (int)rect2Top);
            }

            if (_isRect1MoveUp) // Move up
            {
                SetRect1Top(rect1Top - MoveSpeed);
            }
            else // Move down
            {
                double newTop = rect1Top + MoveSpeed;
                if (newTop >= _rect1TargetTop)
                {
                    _isRect1MoveUp = true;
                    SetRect1Top(_rect1TargetTop);
                }
                else
                {
                    SetRect1Top(newTop);
                }
            }

            if (_isRect2MoveUp) // Move up
            {
                double newTop = rect2Top - MoveSpeed;
                if (newTop <= _rect2TargetTop)
                {
                    _isRect2MoveUp = false;
                    SetRect2Top(Math.Max(0,_rect2TargetTop - rect2.Height));
                }
                else
                {
                    SetRect2Top(newTop);
                }
            }
            else // move down
            {
                SetRect2Top(rect2Top + MoveSpeed);
            }
        }

        private void SetRect1Top(double top)
        {
            rect1.Height = canvas.Height - top;
            Canvas.SetTop(rect1, top);
        }

        private void SetRect2Top(double top)
        {
            Canvas.SetTop(rect2, top);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            double rect1Top = _rand.Next((int)(rect2.Height + 1), (int)canvas.Height);
            double rect2Top = _rand.Next(0, (int)(rect1Top - rect2.Height));

            SetRect1Top(rect1Top);
            SetRect2Top(rect2Top);

            _isRect1MoveUp = _rand.Next(0, 5) >= 3 ? true : false;
            _isRect2MoveUp = _rand.Next(0, 5) >= 3 ? true : false;

            if (!_isRect1MoveUp) // Move down
            {
                _rect1TargetTop = _rand.Next((int)rect1Top, (int)canvas.Height);
            }

            if (_isRect2MoveUp) // Move up
            {
                _rect2TargetTop = _rand.Next(0, (int)rect2Top);
            }
        }
        #endregion
    }
}
