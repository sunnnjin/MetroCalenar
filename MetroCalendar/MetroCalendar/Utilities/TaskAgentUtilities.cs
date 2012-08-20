#define DEBUG_AGENT

// <copyright file="TaskAgentUtilities.cs" author="Sun Jinbo">
//     Copyright (c) Panda work studio. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;

namespace MetroCalendar.Utilities
{
    public static class TaskAgentUtilities
    {
        private const string TaskName = "MetroCalendar_TaskAgent";

        public static void StartTaskAgent()
        {
            PeriodicTask task = 
                ScheduledActionService.Find(TaskName) as PeriodicTask;
            if (task != null)
            {
                RemoveAgent(TaskName);
            }

            task = new PeriodicTask(TaskName);
            task.Description = "Screen Tile description";

            try
            {
                ScheduledActionService.Add(task);

#if(DEBUG_AGENT)
                ScheduledActionService.LaunchForTest(TaskName, TimeSpan.FromSeconds(20));
#endif
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
