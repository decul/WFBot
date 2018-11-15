using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WFManager {
    static class WakeTimer {

        public delegate void TimerCompleteDelegate();
        static TimerCompleteDelegate timerComplete = Lel;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime, int lPeriod, TimerCompleteDelegate pfnCompletionRoutine, IntPtr pArgToCompletionRoutine, bool fResume);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);



        public static IntPtr SetTimer(DateTime dt, TimeSpan period, TimerCompleteDelegate method) {

            // read the manual for SetWaitableTimer to understand how this number is interpreted.
            long time = dt.ToFileTimeUtc();
            IntPtr handle = CreateWaitableTimer(IntPtr.Zero, true, "WaitableTimer");
            SetWaitableTimer(
                handle, 
                ref time, 
                (int) period.TotalMilliseconds, 
                method, 
                IntPtr.Zero,
                true
            );
            return handle;
        }

        private static void Lel() {
            Logger.Info("WakeUp");
        }
    }
}
