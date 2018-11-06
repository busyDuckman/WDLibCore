/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System;
using System.Drawing;

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    /// There is a convenience to writing complicated tasks as if they
    /// were running from a command line.
    /// 
    /// This class gives a command line type environment for complex tasks.
    /// Allows for printing to a log and GUI agnostic status updates.
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OOUT"></typeparam>
    public interface IWorker<OOUT> : IStatusProvider
    {
        OOUT Process();

        Action<string> OnPrintLine { get; set; }
        Action<string> OnPrint { get; set; }

        Action<OOUT> OnUpdatedData { get; set; }
    }

    /// <summary>
    /// There is a convenience to writing complicated tasks as if they
    /// were running from a command line.
    /// 
    /// This class gives a command line type environment for complex tasks.
    /// Allows for printing to a log and GUI agnostic status updates.
    /// </summary>
    public interface IWorker<IN, OOUT> : IWorker<OOUT>
    {
        OOUT Process(IN input);
    }

    public interface IImageWorker:  IWorker<Bitmap, Bitmap>
    {
    }

    public static class IWorkerExtension
    {
        public static void Print(this IImageWorker worker, string text)
        {
            if (worker.OnPrint != null)
            {
                worker.OnPrint(text);
            }
            else
            {
                Console.Write(text);
            }
        }
        public static void Print(this IImageWorker worker, string format, params object[] args)
        {
            Print(worker, string.Format(format, args));
        }

        public static void PrintLine(this IImageWorker worker, string text)
        {
            if (worker.OnPrintLine != null)
            {
                worker.OnPrintLine(text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }
        public static void PrintLine(this IImageWorker worker, string format, params object[] args)
        {
            PrintLine(worker, string.Format(format, args));
        }
    }
}
