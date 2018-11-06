/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

//#define EXCEPTION_FREE_DESERIALISATION
using System;
using System.IO;
using System.Text;
using System.Threading;

using System.Runtime.CompilerServices;
using System.Collections.Generic;
using WDToolbox.Data.Text;
using WDToolbox.Data.DataStructures;

namespace WDToolbox.AplicationFramework
{

    /**
     * This is the logging class I have used since .NET was in beta.
     * There should be better logging out there now, but I know this class is reliable, and
     * have a lot of code that depends on it.
     */
    public class WDAppLog
    {
        //-------------------------------------------------------------------------------------------
        // Static Data
        //-------------------------------------------------------------------------------------------
        //this is where errors are written to
        private static TextWriter errorLog = null;

        public static TextWriter ErrorLog
        {
            get { return WDAppLog.errorLog; }
            set { WDAppLog.errorLog = value; }
        }

        //this contains all errors
        public static bool UseErrorLogString = false;
        private static StringBuilder _errorLogString = new StringBuilder();
        public static string ErrorLogString
        {
            get
            {
                if (_errorLogString == null)
                    return "";
                else
                    return _errorLogString.ToString();
            }
            set
            {
                if (value == null)
                    _errorLogString = new StringBuilder();
                else
                    _errorLogString = new StringBuilder(value);
            }
        }

        static Mutex lockStreams = new Mutex();
        //How long to wait before giving up on accessing a stream		
        private static int writeTimeOut = 1000;

        //-------------------------------------------------------------------------------------------
        // Static data
        //-------------------------------------------------------------------------------------------
        private static string[] erorDescriptions = new string[] { "Debug", "Warning", "Small Error", "Error", "Terminal Error", "System Error" };
        private static string[] errorTags = new string[] { "INFO", "WARNING", "ERROR_MINOR", "ERROR", "ERROR_TERMINAL", "ERROR_SYSTEM" };
        public static IReadOnlyList<string> ErorTags { get { return new List<string>(errorTags).AsReadOnly(); } }
        public static readonly string ExceptionTag = "EXCEPTION";
        public static readonly string FileAccessTag = "FILE_ACCESS";

        //-------------------------------------------------------------------------------------------
        // Constructors
        //-------------------------------------------------------------------------------------------
        static WDAppLog()
        {
#if DEBUG
            errorLog = Console.Error;
            //infoLog = Console.Out;
#else
            //errorLog = new StreamWriter("c:\todo_find_log_pos.txt", true);
#endif
        }

        //-------------------------------------------------------------------------------------------
        // Log
        //-------------------------------------------------------------------------------------------
        #region errors

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional info.</param>
        /// <param name="stackTrace">The stack trace.</param>
        public static void logError(ErrorLevel priority,
                                    string message,
                                    string additionalInfo,
                                    string tag,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            try
            {
                
                string messageString = makeStringNeetAndValid(message, "?");
                string additionalInfoString = makeStringNeetAndValid(additionalInfo, "N/A");

                string errorType = WDAppLog.errorTags[(int)priority];

                StringBuilder errorMessage = new StringBuilder();
                errorMessage.Append(errorType + ": ");
                if (!string.IsNullOrEmpty(tag))
                {
                    errorMessage.Append("[" + tag + "] ");
                }
                
                errorMessage.AppendLine(messageString);
                
                if(!string.IsNullOrEmpty(additionalInfo))
                {
                    errorMessage.AppendLine("\tAdditional Info: "+additionalInfo);
                }

                errorMessage.AppendLine("\tLocation: " + member + " in '" +  Path.GetFileName(file) + "' line " + line);

                if (lockStreams.WaitOne(writeTimeOut, false))
                {
                    try
                    {
                        writeToErrorStream(errorMessage.ToString());
                    }
                    finally
                    {
                        lockStreams.ReleaseMutex();
                    }
                }
            }
            catch
            {

                //this if is to prevent infinite recursion when the error stream is broken
                if (priority < ErrorLevel.Error)
                {
                    logError(ErrorLevel.Error, "Unable to log error message", line, file, member);
                    return;
                }

                //at this point the system is in a serious state and the error log is not functioning
                //lets do nothing so as not to antagonise the situation any furthur
            }
        }

         /// <summary>
        /// Logs an error to the current error output stream
        /// </summary>
        /// <param name="priority">The priority of the error.
        /// </param>
        /// <param name="message">Nature of the error</param>
        public static void logError(ErrorLevel priority, 
                                    string message,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            logError(priority, message, "", line, file, member);
        }

        /// <summary>
        /// Logs an error to the current error output stream
        /// </summary>
        /// <param name="priority">The priority of the error.
        /// </param>
        /// <param name="message">Nature of the error</param>
        /// <param name="additionalInfo">info such as line of code or exception message</param>
        public static void logError(ErrorLevel priority,
                                    string message, 
                                    string additionalInfo,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
#if DEBUG
            logError(priority, message, additionalInfo, null, line, file, member);
#else
            logError(priority, message, additionalInfo, null, line, file, member);
#endif
        }


        /// <summary>
        /// Writes information to the current error stream
        /// </summary>
        /// <param name="message"></param>
        private static void writeToErrorStream(string message)
        {
            if (errorLog != null)
            {
                errorLog.WriteLine(message);
                errorLog.FlushAsync();
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);
#endif

            if (UseErrorLogString)
            {
                _errorLogString.AppendLine(message);
            }
        }
        #endregion

        //-------------------------------------------------------------------------------------------
        // Very specific log events
        //-------------------------------------------------------------------------------------------
        public static void logCallToMethodStub([CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            logError(ErrorLevel.Error, "Stub function was called", null, line, file, member);
        }

        public static void logException(ErrorLevel errorLevel, 
                                    Exception ex,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            logTaggedError(errorLevel, ex.Message, ex.GetADecentExplination(), ExceptionTag, line, file, member);
        }

        private static void logTaggedError(ErrorLevel errorLevel,
            string error, 
            string extraData, 
            string tag,
            [CallerLineNumber] int line = 0,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "")
        {
            string innerError = null;
            logError(errorLevel, error, extraData ?? innerError, tag, line, file, member);
        }

        public static void TryCatchLogged(Action tryBlock, 
                                    ErrorLevel errorLevelIfCatch,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            if (tryBlock != null)
            {
                try
                {
                    tryBlock();
                }
                catch (Exception ex)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex, line, file, member);
                }
            }
            else
            {
                WDAppLog.logError(ErrorLevel.Warning, "TryCatchLogged was given a null tryBlock", line, file, member);
            }
        }

        public static void logFileOpenError(ErrorLevel errorLevel,
                                            string filePath,
                                            [CallerFilePath] string file = "",
                                            [CallerLineNumber] int line = 0,
                                            [CallerMemberName] string member = "")
        {
            logFileOpenError(errorLevel, null, filePath, file, line, member);
        }

        public static void logFileOpenError(ErrorLevel errorLevel, 
                                            Exception ex, 
                                            string filePath,
                                            [CallerFilePath] string file = "",
                                            [CallerLineNumber] int line = 0,
                                            [CallerMemberName] string member = "")
        {
            string message = (ex != null) ? ex.Message : "(no-exception)";
            logTaggedError(errorLevel, message, filePath, FileAccessTag, line, file, member);
        }

        public static void logFileSaveError(ErrorLevel errorLevel,
                                            string filePath,
                                            [CallerFilePath] string file = "",
                                            [CallerLineNumber] int line = 0,
                                            [CallerMemberName] string member = "")
        {
            logFileSaveError(errorLevel, null, filePath, file, line, member);
        }

        public static void logFileSaveError(ErrorLevel errorLevel,
                                            Exception ex,
                                            string filePath,
                                            [CallerFilePath] string file = "",
                                            [CallerLineNumber] int line = 0,
                                            [CallerMemberName] string member = "")
        {
            string message = (ex != null) ? ex.Message : "(no-exception)";
            logTaggedError(errorLevel, message, filePath, FileAccessTag, line, file, member);
        }

        public static void Debug(
                string message,
                [CallerLineNumber] int line = 0,
                [CallerFilePath] string file = "",
                [CallerMemberName] string member = "")
        {
            //string msg = string.Format("(file:{0}, func:{2}, line:{1}): {3}", Path.GetFileName(file), line, member, message);
            logTaggedError(ErrorLevel.Debug, message, null, "DEBUG", line, file, member);
        }


        //-------------------------------------------------------------------------------------------
        // Private functions
        //-------------------------------------------------------------------------------------------
        private static string neetenString(string message)
        {
            string result;
            if (message != null)
            {
                result = message;
                if (result.Length > 0)
                {
                    result = result.Trim();
                }
                return result;
            }
            return "";
        }

        private static string makeStringNeetAndValid(string message, string onError)
        {
            string result;
            try		//is either string valid
            {
                try
                {
                    result = neetenString(message);
                }
                catch
                {
                    return neetenString(onError);
                }
                return result;
            }
            catch (Exception ex)
            {
                return "Invalid Error String (" + ex.Message + ")";
            }
        }


        public static void LogNeverSupposedToBeHere( [CallerFilePath] string file = "",
                                            [CallerLineNumber] int line = 0,
                                            [CallerMemberName] string member = "")
        {
            logError(ErrorLevel.Error, "This point in the code should never have been reached.", null, line, file, member);
        }

        public static void RaiseTestErrors()
        {
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logCallToMethodStub();
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.Debug, "Error test (debug)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.Error, "Error test (Error)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.SmallError, "Error test (SmallError)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.SystemError, "Error test (SystemError)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.TerminalError, "Error test (TerminalError)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.Warning, "Error test (Warning)");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.LogNeverSupposedToBeHere();
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logException(ErrorLevel.Error, new NotImplementedException("Just a test"));
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logFileOpenError(ErrorLevel.Error, @"C:\just a test.txt");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logFileSaveError(ErrorLevel.Error, @"C:\just a test.txt");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logTaggedError(ErrorLevel.Error, "Test error", null, "TEST_TAG");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logTaggedError(ErrorLevel.Error, "Test error 2", Misc.LoremIpsum, "TEST_TAG");
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.logError(ErrorLevel.Error, "long (Error)" + Misc.LoremIpsum);
            WDAppLog.Debug("Next error is just a test");
            WDAppLog.Debug(Misc.LoremIpsum);            
        }

        static ConsoleColor ErrorLogColor = ConsoleColor.DarkYellow;
        static ConsoleColor ErrorLogBackColor = ConsoleColor.Black;
        public static TextWriter2Event NewConsoleLogHelper()
        {
            return new TextWriter2Event(delegate(object sender, string line)
            {
                if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.SmallError]))
                {
                    ErrorLogColor = ConsoleColor.Black;
                    ErrorLogBackColor = ConsoleColor.White;
                }
                else if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.SystemError]))
                {
                    ErrorLogColor = ConsoleColor.Magenta;
                    ErrorLogBackColor = ConsoleColor.White;
                }
                else if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.Warning]))
                {
                    ErrorLogColor = ConsoleColor.Yellow;
                    ErrorLogBackColor = ConsoleColor.DarkGray;
                }
                else if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.TerminalError]))
                {
                    ErrorLogColor = ConsoleColor.Red;
                    ErrorLogBackColor = ConsoleColor.White;
                }
                else if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.Debug]))
                {
                    ErrorLogColor = ConsoleColor.Gray;
                    ErrorLogBackColor = ConsoleColor.DarkGray;
                }
                //last because other tags often start with this tag
                else if (line.StartsWith(WDAppLog.ErorTags[(int)ErrorLevel.Error])) 
                {
                    ErrorLogColor = ConsoleColor.DarkRed;
                    ErrorLogBackColor = ConsoleColor.White;
                }

                ConsoleColor oldFore = Console.ForegroundColor;
                ConsoleColor oldBack = Console.BackgroundColor;

                //new colors
                Console.ForegroundColor = ErrorLogColor;
                Console.BackgroundColor = ErrorLogBackColor;

                int consoleWidth = Math.Min(512, Math.Max(Console.BufferWidth, 10));
                string indent = "    "; //dont indent more than 9!  See above line.

                //write line
                foreach (string logLine in line.GetLines())
                {
                    string msg = logLine.Replace("\t", "    ");
                    foreach (string messageLine in msg.Wrap(consoleWidth-indent.Length-1).GetLines())
                    {
                        string actualLine = indent + messageLine;
                        actualLine = actualLine.PadRight(consoleWidth-1);
                        Console.WriteLine(actualLine);
                    }
                }

                //restore previous colors
                Console.ForegroundColor = oldFore;
                Console.BackgroundColor = oldBack;
            });
        }
    }


    //-------------------------------------------------------------------------------------------
    // Extensions
    //-------------------------------------------------------------------------------------------
    public static class WDAppLogExtension
    {
        /// <summary>
        /// If False, Logs the reason via WDAppLog 
        /// </summary>
        public static void LogIfFalse(this Why why,
                                    ErrorLevel priority = ErrorLevel.Error,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            if (!why)
            {
                WDAppLog.logError(priority, why.Reason ?? "(no reason given)", "", line, file, member);
            }
        }

        /// <summary>
        /// If True, Logs the reason via WDAppLog 
        /// </summary>
        public static void LogIfTrue(this Why why,
                                    ErrorLevel priority = ErrorLevel.Error,
                                    [CallerLineNumber] int line = 0,
                                    [CallerFilePath] string file = "",
                                    [CallerMemberName] string member = "")
        {
            if (why)
            {
                WDAppLog.logError(priority, why.Reason ?? "(no reason given)", "", line, file, member);
            }
        }
    }

}
