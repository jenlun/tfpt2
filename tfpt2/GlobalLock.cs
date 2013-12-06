namespace TFSPowerTools2
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Threading;

    public sealed class GlobalLock : IDisposable
    {
        private readonly bool hasHandle;

        private Mutex mutex;

        public GlobalLock(int timeOut)
        {
            this.InitMutex();
            try
            {
                this.hasHandle = this.mutex.WaitOne(timeOut <= 0 ? Timeout.Infinite : timeOut, false);
                if (this.hasHandle == false)
                {
                    throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
                }
            }
            catch (AbandonedMutexException)
            {
                this.hasHandle = true;
            }
        }

        public void Dispose()
        {
            if (this.mutex != null)
            {
                if (this.hasHandle)
                {
                    this.mutex.ReleaseMutex();
                }

                this.mutex.Dispose();
            }
        }

        private void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value;
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);
            this.mutex = new Mutex(false, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            this.mutex.SetAccessControl(securitySettings);
        }
    }
}
