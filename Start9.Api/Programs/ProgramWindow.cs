﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using Start9.Api.Objects;
using Start9.Api.Tools;

namespace Start9.Api.Programs
{ 
    /// <summary>
    /// Represents an open program's window.
    /// </summary>
    // TODO: Redo the entire ProgramItem thing to not be stupid
	public class ProgramWindow : IProgramItem
	{
		static ProgramWindow()
		{
			Automation.AddAutomationEventHandler(
				WindowPattern.WindowOpenedEvent,
				AutomationElement.RootElement,
				TreeScope.Children,
				(sender, e) => WindowOpened?.Invoke(null,
													new WindowEventArgs(
														new ProgramWindow(new IntPtr(((AutomationElement) sender).Current.NativeWindowHandle)))));

			Automation.AddAutomationEventHandler(
				WindowPattern.WindowClosedEvent,
				AutomationElement.RootElement,
				TreeScope.Subtree,
				(sender, e) => WindowClosed?.Invoke(null,
													new WindowEventArgs(
														new ProgramWindow(new IntPtr(((AutomationElement) sender).Cached.NativeWindowHandle)))));
		}

        /// <summary>
        /// Creates a new program from an Hwnd.
        /// </summary>
        /// <param name="hwnd">The handle to the window.</param>
		public ProgramWindow(IntPtr hwnd) { Hwnd = hwnd; }

        /// <summary>
        /// Gets the Hwnd of the window.
        /// </summary>
        /// <value>
        /// A platform-specific handle to the window.
        /// </value>
		public IntPtr Hwnd { get; }

        /// <summary>
        /// Gets the process that the window is from.
        /// </summary>
		public Process Process
		{
			get
			{
				WinApi.GetWindowThreadProcessId(Hwnd, out var pid);
				return Process.GetProcessById((Int32) pid);
			}
		}

        /// <summary>
        /// Gets a list of all open program windows.
        /// </summary>
		public static IEnumerable<ProgramWindow> RealProgramWindows
		{
			get
			{
				var collection = new List<IntPtr>();

                Boolean Filter(IntPtr hWnd, Int32 lParam)
				{
					var strbTitle = new StringBuilder(WinApi.GetWindowTextLength(hWnd));
					WinApi.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
					var strTitle = strbTitle.ToString();


					if (WinApi.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
						collection.Add(hWnd);

					return true;
				}

				if (!WinApi.EnumDesktopWindows(IntPtr.Zero, Filter, IntPtr.Zero)) yield break;

				foreach (var hwnd in collection)
					yield return new ProgramWindow(hwnd);
			}
		}

        /*public static IEnumerable<ProgramWindow> UserPerceivedProgramWindows => ProgramWindows.Where(
			hwnd => TASKSTYLE == (TASKSTYLE & WinApi.GetWindowLong(hwnd.Hwnd, GWL_STYLE).ToInt32()) &
					(WinApi.GetWindowLong(hwnd.Hwnd, GWL_EXSTYLE).ToInt32() & WS_EX_TOOLWINDOW) != WS_EX_TOOLWINDOW);*/

            /// <summary>
            /// Gets a list of open windows that are shown to the user.
            /// </summary>
        public static IEnumerable<ProgramWindow> ProgramWindows
        {
            get
            {
                var collection = new List<IntPtr>();

                Boolean Filter(IntPtr hWnd, Int32 lParam)
                {
                    var strbTitle = new StringBuilder(WinApi.GetWindowTextLength(hWnd));
                    WinApi.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                    var strTitle = strbTitle.ToString();


                    if (WinApi.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                        collection.Add(hWnd);

                    return true;
                }

                if (!WinApi.EnumDesktopWindows(IntPtr.Zero, Filter, IntPtr.Zero)) yield break;

                foreach (var hwnd in collection)
                {
                    try
                    {
                        if (Environment.Is64BitProcess)
                        {
                            if ((TASKSTYLE == (TASKSTYLE & WinApi.GetWindowLong(hwnd, GWL_STYLE).ToInt64())) && ((WinApi.GetWindowLong(hwnd, GWL_EXSTYLE).ToInt64() & WS_EX_TOOLWINDOW) != WS_EX_TOOLWINDOW))
                            {
                                yield return new ProgramWindow(hwnd);
                            }
                        }
                        else
                        {
                            if ((TASKSTYLE == (TASKSTYLE & WinApi.GetWindowLong(hwnd, GWL_STYLE).ToInt32())) && ((WinApi.GetWindowLong(hwnd, GWL_EXSTYLE).ToInt32() & WS_EX_TOOLWINDOW) != WS_EX_TOOLWINDOW))
                            {
                                yield return new ProgramWindow(hwnd);
                            }
                        }
                    }
                    finally
                    {

                    }
                }
            }
        }

        static void AddClosedHandler(IntPtr handle)
		{
			Automation.AddAutomationEventHandler(
				WindowPattern.WindowClosedEvent,
				AutomationElement.FromHandle(handle),
				TreeScope.Subtree,
				(sender, e) =>
				{
					Debug.WriteLine(handle.ToString() + " IS KILL");
					WindowClosed?.Invoke(null, new WindowEventArgs(new ProgramWindow(handle)));
				});
		}
        /// <summary>
        /// Occurs when a window is opened.
        /// </summary>
		public static event EventHandler<WindowEventArgs> WindowOpened;

        /// <summary>
        /// Occurs when a window is closed.
        /// </summary>
		public static event EventHandler<WindowEventArgs> WindowClosed;

		/// <summary>
		///     Moves the window to the front.
		/// </summary>
		public void Show()
		{
			try
			{
				WinApi.ShowWindow(Hwnd, 5);
                WinApi.WINDOWPLACEMENT placement = new WinApi.WINDOWPLACEMENT();
                WinApi.GetWindowPlacement(Hwnd, ref placement);
                //var style = WinApi.GetWindowLong(Hwnd, GWL_STYLE).ToInt64();
                //if ((style & Convert.ToInt64(WinApi.WsMinimize)) == WinApi.WsMinimize)
                //{
                    //if ((style & Convert.ToInt64(WinApi.WsMaximize)) == WinApi.WsMaximize)
                    if ((placement.showCmd == (3 & 6)) | (placement.showCmd == 3)) 
                    {
                        WinApi.ShowWindow(Hwnd, 3);
                    }
                    else
                    {
                        WinApi.ShowWindow(Hwnd, 1);
                    }
                //}
                WinApi.SetForegroundWindow(Hwnd);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Window not focused!\r\n" + ex);
			}
		}

        /// <summary>
        ///     Minimizes the window.
        /// </summary>
        public void Minimize()
		{
			try
			{
                WinApi.ShowWindow(Hwnd, 11);
			}
			catch (Exception e)
            {
                try
                {
                    Debug.WriteLine("Window not minimized on first attempt!\r\n" + e);
                    WinApi.ShowWindow(Hwnd, 6);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Window not minimized on second attempt!\r\n" + ex);
                }
			}
		}

		/// <summary>
		///     Maximizes the Window.
		/// </summary>
		public void Maximize() { WinApi.ShowWindow(Hwnd, 3); }

        /// <summary>
        ///     Restores (un-minimizes) the window.
        /// </summary>
        public void Restore() { WinApi.ShowWindow(Hwnd, 1); }


        /// <summary>
        ///     Displays the windows's system menu.
        /// </summary>
        public void ShowSystemMenu()
        {
            //https://stackoverflow.com/questions/21825352/how-to-open-window-system-menu-on-right-click
            WinApi.GetWindowRect(Hwnd, out WinApi.Rect pos);
            IntPtr hMenu = WinApi.GetSystemMenu(Hwnd, false);
            var cmd = WinApi.TrackPopupMenu(hMenu, 0x0004 & 0x0020, (Int32)SystemScaling.CursorPosition.X, (Int32)SystemScaling.CursorPosition.Y, 0, Hwnd, IntPtr.Zero);
            WinApi.SendMessage(Hwnd, 0x112, cmd, 0);
        }

        /// <summary>
        ///     Closes the window.
        /// </summary>
        public void Close() { WinApi.SendMessage(Hwnd, 0x0010, 0, 0); }

        /// <summary>
        /// Gets the name of the window.
        /// </summary>
		public String Name
		{
			get
			{
				var strbTitle = new StringBuilder(WinApi.GetWindowTextLength(Hwnd));
				WinApi.GetWindowText(Hwnd, strbTitle, strbTitle.Capacity + 1);
				return strbTitle.ToString();
			}
		}

        /// <summary>
        /// Gets the icon of thr window.
        /// </summary>
		public Icon Icon
		{
			get
			{
				var iconHandle = WinApi.SendMessage(Hwnd, WmGeticon, IconSmall2, 0);

				if (iconHandle == IntPtr.Zero)
					iconHandle = WinApi.SendMessage(Hwnd, WmGeticon, IconSmall, 0);
				if (iconHandle == IntPtr.Zero)
					iconHandle = WinApi.SendMessage(Hwnd, WmGeticon, IconBig, 0);
				if (iconHandle == IntPtr.Zero)
					iconHandle = WinApi.GetClassLongPtr(Hwnd, GclHicon);
				if (iconHandle == IntPtr.Zero)
					iconHandle = WinApi.GetClassLongPtr(Hwnd, GclHiconsm);

				if (iconHandle == IntPtr.Zero)
					return null;

				try
				{
					return Icon.FromHandle(iconHandle);
				}
				finally
				{
					WinApi.DestroyIcon(iconHandle);
				}
			}
		}

		/// <summary>
		/// Opens a new instance of the application that owns this window.
		/// </summary>
		public void Open()
		{
			try
			{
				if (File.Exists(Process.MainModule.FileName))
					Process.Start(Process.MainModule.FileName);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("New Instance not started:\r\n" + ex);
			}
		}

		#region p/invoke stuff don't touch lpz

		const Int32 GclHiconsm       = -34;
		const Int32 GclHicon         = -14;
		const Int32 IconSmall        = 0;
		const Int32 IconBig          = 1;
		const Int32 IconSmall2       = 2;
		const Int32 WmGeticon        = 0x7F;
		const Int32 GWL_STYLE        = -16;
		const Int32 GWL_EXSTYLE      = -20;
		const Int32 TASKSTYLE        = 0x10000000 | 0x00800000;
		const Int32 WS_EX_TOOLWINDOW = 0x00000080;

		#endregion
	}
}