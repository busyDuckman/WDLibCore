/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using WDToolbox.Maths.Range;

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// Job status
    /// </summary>
    public enum StatusType {Starting, Working, Done, Error};
    
    /// <summary>
    /// The status provided by an IStatusProvider
    /// </summary>
    public class Status
    {
        public string       Message   { get; set; }
        public double       Percent   { get; set; }
        public StatusType   Type      { get; set; }

        protected Status(string status, double percent, StatusType type)
        {
            Message = status;
            Percent = percent;
            Type    = type;
        }

        public static Status Error(string message, double percent = 0.0f)
        {
            return new Status(message, 0, StatusType.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent">1.0 = 100%</param>
        /// <returns></returns>
        public static Status Update(double percent)
        {
            return Update(String.Format("{0}% Complete.", (int)(percent * 100.0)), percent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="percent">1.00 = 100%</param>
        /// <returns></returns>
        public static Status UpdatePercentMessage(string task, double percent)
        {
            return Update(String.Format("{1}: {0}% Complete.", (int)(percent * 100.0), task), percent);
        }

        public static Status UpdateLoop(int i, int total)
        {
            return Update(i/(double)total);
        }

        public static Status UpdateLoop(int inner, int totalInner, int outer, int totalOuter)
        {
            int total = totalInner * totalOuter;
            int pos = (outer * totalInner) + inner;
            return UpdateLoop(pos, total);
        }

        public static Status UpdateLoopMessage(string task, int i, int total)
        {
            return UpdatePercentMessage(task, i / (double)total);
        }

        public static Status UpdateLoopMessage(string task, int inner, int totalInner, int outer, int totalOuter)
        {
            int total = totalInner * totalOuter;
            int pos = (outer * totalInner) + inner;
            return UpdateLoopMessage(task, pos, total);
        }

        public static Status Update(string message, double percent=0.0f)
        {
            return new Status(message, percent, StatusType.Working);
        }

        public static Status Done(string message="Done.")
        {
            return new Status(message, 1, StatusType.Done);
        }

        public static Status Starting(string message="Starting.")
        {
            return new Status(message, 0, StatusType.Starting);
        }

        public override string ToString()
        {
            switch (Type)
            {
                case StatusType.Starting:
                    return string.Format("INIT: {0}", Message);
                case StatusType.Working:
                    return string.Format(" MSG: {1}% - {0}", Message, Percent);
                case StatusType.Done:
                    return string.Format("DONE: {0}", Message);
                case StatusType.Error:
                    return string.Format(" ERR: {0}", Message);
                default:
                    return Message??"";
            }
        }
    }

    /// <summary>
    /// Delegate for responding to a status update.
    /// </summary>
    /// <param name="status"></param>
    public delegate void OnStatusChangeDelegate(Status status);

    /// <summary>
    /// Implement this to have a job report its status.
    /// </summary>
    public interface IStatusProvider
    {
        OnStatusChangeDelegate OnStatusChange {get; set;}
    }

    public static class StatusProviderExtension
    {
        public static void UpdateStatus(this IStatusProvider provider, Status status) 
        {
            if (provider == null)
            {
                return;
            }

            if (provider.OnStatusChange != null)
            {
                provider.OnStatusChange(status);
            }
        }

#if !DISABLE_WINFORMS
        public static void SetupStatusRespond(this Form form, TextBox txt, IStatusProvider provider)
        {
            provider.OnStatusChange += delegate(Status s)
            {
                MethodInvoker action = delegate { txt.Text = s.Message; };
                form.BeginInvoke(action);
            };
        }

        public static void SetupStatusRespond(this Form form, ToolStripStatusLabel txt, IStatusProvider provider)
        {
            provider.OnStatusChange += delegate(Status s)
            {
                MethodInvoker action = delegate { 
                    txt.Text = s.Message;
                    txt.Invalidate();
                    form.Refresh();
                };
                form.BeginInvoke(action);
            };
        }

        public static void SetupStatusRespond(this Form form, ToolStripStatusLabel label, ToolStripProgressBar progressBar, IStatusProvider provider)
        {
            provider.OnStatusChange += delegate(Status s)
            {
                MethodInvoker action = delegate
                {
                    label.Text = s.Message;
                    label.Invalidate();
                    int range = progressBar.Maximum - progressBar.Minimum;
                    double pos = progressBar.Minimum + (range * s.Percent);
                    pos = Range.clamp((int)pos, progressBar.Minimum, progressBar.Maximum);
                    progressBar.Value = (int)pos;
                    form.Refresh();
                };
                form.BeginInvoke(action);
            };
        }
#endif
    }
}
