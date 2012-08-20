// <copyright file="MicrophonePanel.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace MetroCalendar.Controls
{
    public partial class MicrophonePanel : UserControl
    {
        public MicrophonePanel()
        {
            InitializeComponent();

            storyboard.Begin();
        }

        public void StartAnimation()
        {
            ellipse.Visibility = Visibility.Visible;
            ellipse1.Visibility = Visibility.Visible;
            ellipse2.Visibility = Visibility.Visible;
        }

        public void StopAnimation()
        {
            ellipse.Visibility = Visibility.Collapsed;
            ellipse1.Visibility = Visibility.Collapsed;
            ellipse2.Visibility = Visibility.Collapsed;
        }
    }
}
