// <copyright file="AnniversaryPage.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MetroCalendar.Resources.Localizations;
using MetroCalendar.Utilities;
using MetroCalendar.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroCalendar
{
    public partial class AnniversaryPage : PhoneApplicationPage
    {
        #region internal data
        private AnniversaryViewModel _AnniversaryViewModel;
        private const int MaxLength = 26;
        private bool _isPageLoaded;
        private DateTime _startTime;
        #endregion

        public AnniversaryPage()
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

            _AnniversaryViewModel = new AnniversaryViewModel();
            this.DataContext = _AnniversaryViewModel;
        }

        private void OnRemindChecked(object sender, RoutedEventArgs e)
        {
            _alarmStackPanel.Visibility = Visibility.Visible;
        }

        private void OnRemindUnchecked(object sender, RoutedEventArgs e)
        {
            _alarmStackPanel.Visibility = Visibility.Collapsed;
        }

        private void OnDoneClick(object sender, EventArgs e)
        {
            if (AssertApply())
            {
                _AnniversaryViewModel.Store(_startTime);
                
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
                _AnniversaryViewModel.Update();

                DoBack();
            }
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            _AnniversaryViewModel.Remove();

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

                _AnniversaryViewModel.Load(NavigationContext.QueryString["EntryId"]);

                _startTimeTextBlock.Text = string.Format(" ({0})", _AnniversaryViewModel.AlarmTime.ToShortDateString());
                _limitTextBlock.Text = (MaxLength - _AnniversaryViewModel.Subject.Length).ToString();
            }
            else if (NavigationContext.QueryString.ContainsKey("StartTime"))
            {
                _startTime = Convert.ToDateTime(NavigationContext.QueryString["StartTime"]);
                _startTimeTextBlock.Text = string.Format(" ({0})", _startTime.ToShortDateString());
                _AnniversaryViewModel.AlarmTime = new DateTime(
                                                                _startTime.Year,
                                                                _startTime.Month,
                                                                _startTime.Day,
                                                                DateTime.Now.Hour,
                                                                DateTime.Now.Minute,
                                                                DateTime.Now.Second);
                _AnniversaryViewModel.ExpirationTime = _startTime;

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

            if (_AnniversaryViewModel.AlarmOn)
            {
                _alarmStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _alarmStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != AppResources.EnterSubjectText)
            {
                TextBox tb = (TextBox)sender;
                tb.Foreground = new SolidColorBrush(Colors.Black);

                _AnniversaryViewModel.Subject = ((TextBox)sender).Text;
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

            AnniversaryViewModel viewModel = (AnniversaryViewModel)_ringtoneListPicker.DataContext;

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

        private void OnRepeatCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                _expirationTimeStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void OnRepeatCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                _expirationTimeStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private bool AssertApply()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(_AnniversaryViewModel.Subject) ||
                _AnniversaryViewModel.Subject == AppResources.EnterSubjectText)
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.EmptySubjectWarningText,
                    AppResources.WarningTitleText,
                    MessageBoxButton.OK
                    );
            } else if (_AnniversaryViewModel.AlarmOn && 
                ((_AnniversaryViewModel.AlarmTime < DateTime.Now &&
                _AnniversaryViewModel.RepeatType == RepeatType.NotRepeated) ||
                _AnniversaryViewModel.ExpirationTime < DateTime.Now))
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.AlarmExpiredWarningText,
                    AppResources.WarningTitleText,
                    MessageBoxButton.OK
                    );
            }
            else if (_AnniversaryViewModel.AlarmOn &&
              _startTime.Date > _AnniversaryViewModel.ExpirationTime)
            {
                SoundUtilities.Instance().Play(SoundType.Note);
                VibrateUtilities.Instance().Vibrate(VibrateType.ShortVibrate);

                MessageBox.Show(
                    AppResources.AnniversaryExpirationWarningText,
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
