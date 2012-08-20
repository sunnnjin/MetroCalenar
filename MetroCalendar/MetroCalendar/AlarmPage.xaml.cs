// <copyright file="AlarmPage.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using MetroCalendar.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroCalendar
{
    public partial class AlarmPage : PhoneApplicationPage
    {
        #region internal data
        private AlarmViewModel _alarmViewModel;
        private const int MaxLength = 26;
        private bool _isPageLoaded;
        private DateTime _startTime;
        #endregion

        public AlarmPage()
        {
            InitializeComponent();

            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.IsMenuEnabled = true;
            this.ApplicationBar.IsVisible = true;
            this.ApplicationBar.Opacity = 1.0;
            this.ApplicationBar.ForegroundColor = Colors.White;
            this.ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 0, 140);

            _subjectTextBox.MaxLength = MaxLength;
            _limitTextBlock.Text = MaxLength.ToString();

            _alarmViewModel = new AlarmViewModel();
            this.DataContext = _alarmViewModel;
        }

        private void OnDoneClick(object sender, EventArgs e)
        {
            if (AssertApply())
            {
                _alarmViewModel.Store(_startTime);

                DoBack();
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            DoBack();
        }

        private void OnUpdateClick(object sender, EventArgs e)
        {
            if (AssertApply())
            {
                _alarmViewModel.Update();

                DoBack();
            }
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            _alarmViewModel.Remove();

            DoBack();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            if (_isPageLoaded)
            {
                return;
            }
            else
            {
                _isPageLoaded = true;
            }

            if (NavigationContext.QueryString.ContainsKey("EntryId"))
            {
                ApplicationBarIconButton updateIconButton =
                    new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Update.png", UriKind.Relative));
                updateIconButton.Text = AppResources.UpdateAppbarText;
                updateIconButton.Click += new EventHandler(OnUpdateClick);

                ApplicationBarIconButton deleteIconButton =
                    new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Delete.png", UriKind.Relative));
                deleteIconButton.Text = AppResources.DeleteAppbarText;
                deleteIconButton.Click += new EventHandler(OnDeleteClick);

                this.ApplicationBar.Buttons.Add(updateIconButton);
                this.ApplicationBar.Buttons.Add(deleteIconButton);

                _alarmViewModel.Load(NavigationContext.QueryString["EntryId"]);

                _startTimeTextBlock.Text = string.Format(" ({0})", _alarmViewModel.AlarmTime.ToShortDateString());
                _limitTextBlock.Text = (MaxLength - _alarmViewModel.Subject.Length).ToString();
            }
            else if (NavigationContext.QueryString.ContainsKey("StartTime"))
            {
                _startTime = Convert.ToDateTime(NavigationContext.QueryString["StartTime"]);
                _startTimeTextBlock.Text = string.Format(" ({0})", _startTime.ToShortDateString());

                _alarmViewModel.AlarmTime = new DateTime(
                                                    _startTime.Year,
                                                    _startTime.Month,
                                                    _startTime.Day,
                                                    DateTime.Now.Hour,
                                                    DateTime.Now.Minute,
                                                    DateTime.Now.Second);
                _alarmViewModel.ExpirationTime = _startTime;

                ApplicationBarIconButton doneIconButton =
                    new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Ok.png", UriKind.Relative));
                doneIconButton.Text = AppResources.DoneAppbarText;
                doneIconButton.Click += new EventHandler(OnDoneClick);

                ApplicationBarIconButton cancelIconButton =
                    new ApplicationBarIconButton(new Uri("/Resources/Images/Appbar/Cancel.png", UriKind.Relative));
                cancelIconButton.Text = AppResources.CancelAppbarText;
                cancelIconButton.Click += new EventHandler(OnCancelClick);

                this.ApplicationBar.Buttons.Add(doneIconButton);
                this.ApplicationBar.Buttons.Add(cancelIconButton);
            }
            else
            {
                throw new ArgumentException("Wrong argument in Navigation Context!");
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != AppResources.EnterSubjectText)
            {
                TextBox tb = (TextBox)sender;
                tb.Foreground = new SolidColorBrush(Colors.Black);

                _alarmViewModel.Subject = ((TextBox)sender).Text;
                _limitTextBlock.Text = (MaxLength - ((TextBox)sender).Text.Length).ToString();
            }
            else
            {
                TextBox tb = (TextBox)sender;
                tb.Foreground = new SolidColorBrush(Colors.LightGray);

                _limitTextBlock.Text = MaxLength.ToString();
            }
        }

        private void OnTextGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender.GetType().Name == "TextBox")
            {
                TextBox tb = (TextBox)sender;
                String text = tb.Text;
                if (text == AppResources.EnterSubjectText)
                {
                    tb.Text = "";
                    tb.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void OnTextLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender.GetType().Name == "TextBox")
            {
                TextBox tb = (TextBox)sender;
                String text = tb.Text;
                if (text.Length == 0)
                {
                    tb.Text = AppResources.EnterSubjectText;
                }
                if (tb.Text == AppResources.EnterSubjectText)
                {
                    tb.Foreground = new SolidColorBrush(Colors.LightGray);
                }
            }
        }

        private void OnImageManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            BitmapImage img = new BitmapImage(new Uri(
                "/Resources/Images/TryToPlay_press.png", UriKind.Relative));
            _tryToPlayImage.Source = img;

            AlarmViewModel viewModel = (AlarmViewModel)_ringtoneListPicker.DataContext;

            switch (viewModel.RingTone)
            {
                case RingTone.SchoolBell:
                    SoundUtilities.Instance().Play(SoundType.SchoolBell);
                    break;
                case RingTone.Chaotic:
                    SoundUtilities.Instance().Play(SoundType.Chaotic);
                    break;
                case RingTone.DingTone:
                    SoundUtilities.Instance().Play(SoundType.DingTone);
                    break;
                default:
                    break;
            }

            base.OnManipulationStarted(e);
        }

        private void OnImageManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _tryToPlayImage.Source = new BitmapImage(new Uri(
                "/Resources/Images/TryToPlay.png", UriKind.Relative));

            base.OnManipulationCompleted(e);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            base.OnBackKeyPress(e);
        }

        private void OnListPickerTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListPicker lp = (ListPicker)sender;
            lp.Open();
        }

        private void OnRepeatSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_alarmViewModel.RepeatType == RepeatType.NotRepeated)
            {
                _expirationTimeStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                _expirationTimeStackPanel.Visibility = Visibility.Visible;
            }
        }

        private bool AssertApply()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(_alarmViewModel.Subject) ||
                            _alarmViewModel.Subject == AppResources.EnterSubjectText)
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.EmptySubjectWarningText,
                    AppResources.WarningTitleText,
                    MessageBoxButton.OK
                    );
            }
            else if ((_alarmViewModel.AlarmTime < DateTime.Now &&
                _alarmViewModel.RepeatType == RepeatType.NotRepeated) ||
                (_alarmViewModel.ExpirationTime < DateTime.Now))
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.AlarmExpiredWarningText,
                    AppResources.WarningTitleText,
                    MessageBoxButton.OK
                    );
            }
            else if (_startTime.Date > _alarmViewModel.ExpirationTime)
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.AlarmExpirationWarningText,
                    AppResources.WarningTitleText,
                    MessageBoxButton.OK
                    );
            }
            else
            {
                ret = true;
            }

            return ret;
        }

        private void DoBack()
        {
            SoundUtilities.Instance().Play(SoundType.AppbarClicked);

            this.NavigationService.GoBack();
        }
    }
}
