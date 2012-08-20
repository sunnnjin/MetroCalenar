// <copyright file="SoundUtilities.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Windows.Resources;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MetroCalendar.Utilities
{
    public enum SoundType
    {
        AppbarClicked,
        Note,
        Slipping,
        CalendarPanelStart,
        CalendarPanelEnd,
        SchoolBell,
        Chaotic,
        DingTone
    }

    public class SoundUtilities
    {
        private static SoundUtilities _soundUtilities;
        private SoundEffectInstance _appbarClickedSound;
        private SoundEffectInstance _noteSound;
        private SoundEffectInstance _slippingSound;
        private SoundEffectInstance _calendarPanelStart;
        private SoundEffectInstance _calendarPanelEnd;
        private SoundEffectInstance _schoolBellSound;
        private SoundEffectInstance _chaoticSound;
        private SoundEffectInstance _dingToneSound;

        private SoundEffectInstance _currentSound;

        private bool mute = false;

        public void Play(SoundType type)
        {
            if (mute) return;

            Stop();

            try
            {
                switch (type)
                {
                    case SoundType.AppbarClicked:
                        _currentSound = _appbarClickedSound;
                        break;
                    case SoundType.Note:
                        _currentSound = _noteSound;
                        break;
                    case SoundType.Slipping:
                        _currentSound = _slippingSound;
                        break;
                    case SoundType.CalendarPanelStart:
                        _currentSound = _calendarPanelStart;
                        break;
                    case SoundType.CalendarPanelEnd:
                        _currentSound = _calendarPanelEnd;
                        break;
                    case SoundType.SchoolBell:
                        _currentSound = _schoolBellSound;
                        break;
                    case SoundType.Chaotic:
                        _currentSound = _chaoticSound;
                        break;
                    case SoundType.DingTone:
                        _currentSound = _dingToneSound;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            _currentSound.Play();
        }

        public void Stop()
        {
            if (_currentSound != null)
            {
                _currentSound.Stop();
            }
        }

        public static SoundUtilities Instance()
        {
            if (_soundUtilities == null)
            {
                _soundUtilities = new SoundUtilities();
            }
            return _soundUtilities;
        }

        private SoundUtilities()
        {
            LoadSound("Resources/Sounds/appbar.clicked.wav", out _appbarClickedSound);
            LoadSound("Resources/Sounds/note.wav", out _noteSound);
            LoadSound("Resources/Sounds/slipping.wav", out _slippingSound);
            LoadSound("Resources/Sounds/calendar.panel.start.wav", out _calendarPanelStart);
            LoadSound("Resources/Sounds/calendar.panel.end.wav", out _calendarPanelEnd);
            LoadSound("Resources/Sounds/school.bell.wav", out _schoolBellSound);
            LoadSound("Resources/Sounds/chaotic.wav", out _chaoticSound);
            LoadSound("Resources/Sounds/ding.tone.wav", out _dingToneSound);

            // Timer to simulate the XNA game loop (SoundEffect classes are from the XNA Framework)
            DispatcherTimer XnaDispatchTimer = new DispatcherTimer();
            XnaDispatchTimer.Interval = TimeSpan.FromMilliseconds(50);

            // Call FrameworkDispatcher.Update to update the XNA Framework internals.
            XnaDispatchTimer.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };

            // Start the DispatchTimer running.
            XnaDispatchTimer.Start();
        }

        /// <summary>
        /// Loads a wav file into an XNA Framework SoundEffect.
        /// </summary>
        /// <param name="SoundFilePath">Relative path to the wav file.</param>
        /// <param name="Sound">The SoundEffect to load the audio into.</param>
        private void LoadSound(String SoundFilePath, out SoundEffectInstance sound)
        {
            // For error checking, assume we'll fail to load the file.
            sound = null;
            SoundEffect soundEffect = null;

            try
            {
                // Holds informations about a file stream.
                StreamResourceInfo SoundFileInfo = 
                    App.GetResourceStream(new Uri(SoundFilePath, UriKind.Relative));

                // Create the SoundEffect from the Stream
                soundEffect = SoundEffect.FromStream(SoundFileInfo.Stream);

                // Create sound instance
                sound = soundEffect.CreateInstance();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
