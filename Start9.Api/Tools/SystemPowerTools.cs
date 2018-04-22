﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Start9.Api;

namespace Start9.Api
{
    public class SystemPower
    {
        public static void LockUserAccount()
        {
            WinApi.LockWorkStation();
        }

        public static void SignOut()
        {
            WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Force, 0);
        }

        public static void SleepSystem()
        {
            WinApi.SetSuspendState(false, true, true);
        }

        public static void ShutDownSystem()
        {
			WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Shutdown, 0);
        }

		public static void RestartSystem()
        {
			WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Reboot, 0);
        }
	}
}
