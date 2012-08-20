// <copyright file="AboutPage.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MetroCalendar
{
    public partial class AboutPage : PhoneApplicationPage
    {
        #region internal data
        private DispatcherTimer _timer;
        private DispatcherTimer _twinkleTimer;
        private int _currentFrame;
        private const int MaxFrame = 18;
        private BitmapImage[] bitmapFrame = new BitmapImage[MaxFrame];
        private bool _reverse = false;

        private const double OpenEyeTimeSpan = 4.0; // secs
        private const double CloseEyeTimeSpan = 0.5; // secs

        private BitmapImage _openEyeBitmapImage;
        private BitmapImage _closeEyeBitmapImage;
        #endregion

        #region constructor
        public AboutPage()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.IsMenuEnabled = true;
            this.ApplicationBar.IsVisible = true;
            this.ApplicationBar.Opacity = 1.0;
            this.ApplicationBar.ForegroundColor = Colors.White;
            this.ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 0, 140);

            ApplicationBarIconButton okIconButton =
                new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Ok.png", UriKind.Relative));
            okIconButton.Text = AppResources.OkAppbarText;
            okIconButton.Click += new EventHandler(OnOkClick);

            this.ApplicationBar.Buttons.Add(okIconButton);

            _openEyeBitmapImage = new BitmapImage(
                new Uri("/Resources/Images/Panda.png", 
                        UriKind.Relative));
            _closeEyeBitmapImage = new BitmapImage(
                new Uri("/Resources/Images/Panda_twinkle.png",
                        UriKind.Relative));

            _pandaHeadImage.Source = _openEyeBitmapImage;

            _twinkleTimer = new DispatcherTimer();
            _twinkleTimer.Tick += new EventHandler(OnTwinkleTimerTick);
            _twinkleTimer.Interval = TimeSpan.FromSeconds(OpenEyeTimeSpan);
            _twinkleTimer.Start();

            for(int i = 0; i < MaxFrame; ++i)
            {
                string imgUri = string.Format("/Resources/Images/About/{0}.png", i + 1);
                bitmapFrame[i] = new BitmapImage(new Uri(imgUri, UriKind.Relative));
            }

            _currentFrame = 0;
            _pandaImage.Source = bitmapFrame[_currentFrame];

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(OnTimerTick);
            _timer.Interval = TimeSpan.FromTicks(2222222);
            _timer.Start();
        }
        #endregion

        #region internal methods
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_reverse)
            {
                --_currentFrame;
                if (_currentFrame < 0)
                {
                    _currentFrame = 0;
                    _reverse = false;
                }
            }
            else
            {
                ++_currentFrame;
                if (_currentFrame >= MaxFrame)
                {
                    _currentFrame = MaxFrame - 1;
                    _reverse = true;
                }
            }

            _pandaImage.Source = bitmapFrame[_currentFrame];
        }

        private void OnTwinkleTimerTick(object sender, EventArgs e)
        {
            _twinkleTimer.Stop();

            if (_pandaHeadImage.Source == _openEyeBitmapImage)
            {
                _pandaHeadImage.Source = _closeEyeBitmapImage;
                _twinkleTimer.Interval = 
                    TimeSpan.FromSeconds(CloseEyeTimeSpan);
            }
            else
            {
                _pandaHeadImage.Source = _openEyeBitmapImage;
                _twinkleTimer.Interval =
                    TimeSpan.FromSeconds(OpenEyeTimeSpan);
            }

            _twinkleTimer.Start();
        }

        private void OnOkClick(object sender, EventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            this.NavigationService.GoBack();
        }

        private void OnButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            WebBrowserTask wbTask = new WebBrowserTask();
            wbTask.Uri = new Uri("https://www.twitter.com/PandaWorkStudio",
                UriKind.RelativeOrAbsolute);
            wbTask.Show();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            base.OnBackKeyPress(e);
        }
        #endregion
    }
}
