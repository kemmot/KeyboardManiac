using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Plugins.TaskSwitcher
{
    public class TaskSwitcherPlugin : CommandPluginBase
    {
        private const string ResultType_Process = "Process";
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSwitcherPlugin"/> class.
        /// </summary>
        public TaskSwitcherPlugin(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Gives the plugin an opportunity to handle the command text.
        /// </summary>
        /// <param name="item">The item to assess.</param>
        /// <returns>
        /// A result object with details on whether the plugin can handle the command.
        /// </returns>
        override public CommandRequest CanHandleCommand(SearchResultItem item)
        {
            return CanHandleCommand(item.Path);
        }
        public override CommandRequest CanHandleCommand(string commandText)
        {
            Process[] processes = Process.GetProcessesByName(commandText);

            CommandRequest request = new CommandRequest();
            request.AliasCleansedCommandText = commandText;
            request.CanHandleCommand = processes.Length > 0;
            request.CommandText = commandText;
            request.MatchingAlias = string.Empty;
            return request;
        }
        protected override CommandResult DoExecute(CommandRequest commandRequest)
        {
            CommandResult result = new CommandResult();
            result.CommandText = commandRequest.CommandText;
            try
            {
                InnerSetForegroundWindow(commandRequest.AliasCleansedCommandText);
            }
            catch (Exception ex)
            {
                result.Output = ex.ToString();
                result.Success = false;
            }
            return result;
        }
        private static void InnerSetForegroundWindow(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                InnerSetForegroundWindow(processes[0]);
            }
            else
            {
                string message = string.Format(
                    "Process not found: {0}",
                    processName);
                throw new PluginException(message);
            }
        }
        private static void InnerSetForegroundWindow(Process process)
        {
            InnerSetForegroundWindow(process.MainWindowHandle);
        }
        private static void InnerSetForegroundWindow(IntPtr windowHandle)
        {
            if (!SetForegroundWindow(windowHandle))
            {
                Exception win32Exception = new Win32Exception();
                string message = string.Format(
                    "Failed calling SetForegroundWindow");
                throw new PluginException(message, win32Exception);
            }

            if (!ShowWindow(windowHandle, ShowWindowCommands.Normal))
            {
                Exception win32Exception = new Win32Exception();
                string message = string.Format(
                    "Failed calling ShowWindow");
                throw new PluginException(message, win32Exception);
            }
        }
        
        enum ShowWindowCommands : int
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window 
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>       
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value 
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position. 
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level 
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is 
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position. 
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
            /// that owns the window is not responding. This flag should only be 
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }
    }
}
