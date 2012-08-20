// <copyright file="XnaFrameworkDispatcherService.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace MetroCalendar.Memo
{
    public class XnaFrameworkDispatcherService : IApplicationService
    {
        DispatcherTimer _timer;

        public XnaFrameworkDispatcherService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromTicks(333333);
            _timer.Tick += OnTimerTick;
            FrameworkDispatcher.Update();
        }

        void OnTimerTick(object sender, EventArgs args)
        {
            FrameworkDispatcher.Update();
        }

        void IApplicationService.StartService(ApplicationServiceContext context)
        {
            _timer.Start();
        }

        void IApplicationService.StopService()
        {
            _timer.Stop();
        }
    }
}
