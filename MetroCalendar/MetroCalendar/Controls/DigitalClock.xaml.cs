// <copyright file="DigitalClock.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetroCalendar.Controls
{
    public partial class DigitalClock : UserControl
    {
        private int _currentHour;
        private int _currentMintue;

        public DigitalClock()
        {
            InitializeComponent();

            CompositionTarget.Rendering += OnRendering;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (_currentHour != DateTime.Now.Hour
                || _currentMintue != DateTime.Now.Minute)
            {
                _currentHour = DateTime.Now.Hour;
                _currentMintue = DateTime.Now.Minute;
                TimeText.Text = string.Format("{0}:{1}", _currentHour, _currentMintue);
            }
        }
    }
}
