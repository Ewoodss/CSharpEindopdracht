   using System;
using System.Runtime.InteropServices;

namespace Client
{
    public class PowerActions
    {
        [DllImport("user32.dll")]
        private static extern bool LockWorkStation();

        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(int uFlags, int dwReason);

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);


        public PowerActions(Actions actions)
        {
            actions.AddAction("Lock", _ => LockWorkStation());
            actions.AddAction("LogOff", _ => ExitWindowsEx(0, 0));
            actions.AddAction("Shutdown", _ => ExitWindowsEx(1, 0));
            actions.AddAction("Sleep", _ => SetSuspendState(false, true, true));
        }
    }
}