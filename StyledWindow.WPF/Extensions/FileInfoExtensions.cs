using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    internal static class FileInfoExtensions
    {
        #region  Почучение процессов которыми занят файл

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        private static extern uint RmStartSession(out uint pSessionHandle, uint dwSessionFlags,
            string strSessionKey);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        private static extern uint RmRegisterResources(uint dwSessionHandle,
            uint nFiles, string[] rgsFilenames, uint nApplications,
            uint rgApplications, uint nServices, uint rgsServiceNames);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        private static extern uint RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded,
            ref uint pnProcInfo, [In, Out] ProcessInfo[] rgAffectedApps, ref uint lpdwRebootReasons);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true, ExactSpelling = true)]
        private static extern uint RmEndSession(uint dwSessionHandle);

        private const uint __LockFileProcessRebootReasonNone = 0x0;
        private const int __LockFileProcessErrorMoreData = 234;

        /// <summary>Получить массив процессов, блокирующих файл</summary>
        /// <param name="file">Файл, который требуется проверить</param>
        /// <returns>Массив процессов, заблокировавших файл</returns>
        public static Process[] GetLockProcesses(this FileSystemInfo file)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));
            if (!file.Exists) return Array.Empty<Process>();

            var path = file.FullName;
            var key = Guid.NewGuid().ToString();
            if (RmStartSession(out var handle, 0, key) != 0)
                throw new InvalidOperationException("Невозможно перезапустить сессию. Невозможно определить блокирующий процесс");

            try
            {
                uint proc_info = 0;
                var reboot_reasons = __LockFileProcessRebootReasonNone;
                var resources = new[] { path };
                if (RmRegisterResources(handle, (uint)resources.Length, resources, 0, 0, 0, 0) != 0)
                    throw new InvalidOperationException("Невозможно определить дескриптор файла в системе");

                switch (RmGetList(handle, out var proc_info_needed, ref proc_info, null, ref reboot_reasons))
                {
                    case 0: return Array.Empty<Process>();
                    case __LockFileProcessErrorMoreData:
                        {
                            var process_info = new ProcessInfo[proc_info_needed];
                            proc_info = proc_info_needed;
                            if (RmGetList(handle, out proc_info_needed, ref proc_info, process_info, ref reboot_reasons) != 0)
                                throw new InvalidOperationException("Невозможно выполнить перечисление процессов, блокирующих файл");

                            var processes = new Process[(int)proc_info];
                            for (var i = 0; i < proc_info; i++)
                                try
                                {
                                    processes[i] = Process.GetProcessById(process_info[i].Process.ProcessId);
                                }
                                catch (ArgumentException) { }

                            return processes;
                        }
                    default:
                        throw new InvalidOperationException("Невозможно выполнить перечисление прцоессов, блокирующих файл",
                   new InvalidOperationException("Невозможно определить размер результата"));
                }
            }
            finally
            {
                RmEndSession(handle);
            }
        }

        /// <summary>Перечисление процессов, блокирующих файл</summary>
        /// <param name="file">Файл, который требуется проверить</param>
        /// <returns>Перечисление процессов, заблокировавших файл</returns>
        public static IEnumerable<Process> EnumLockProcesses(this FileSystemInfo file)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));

            if (!file.Exists) yield break;
            var path = file.FullName;
            var key = Guid.NewGuid().ToString();
            if (RmStartSession(out var handle, 0, key) != 0)
                throw new InvalidOperationException("Невозможно перезапустить сессию. Невозможно определить блокирующий процесс");

            try
            {
                uint proc_info = 0;
                var reboot_reasons = __LockFileProcessRebootReasonNone;
                var resources = new[] { path };
                if (RmRegisterResources(handle, (uint)resources.Length, resources, 0, 0, 0, 0) != 0)
                    throw new InvalidOperationException("Невозможно определить дескриптор файла в системе");

                switch (RmGetList(handle, out var proc_info_needed, ref proc_info, null, ref reboot_reasons))
                {
                    case 0: break;
                    case __LockFileProcessErrorMoreData:
                        {
                            var process_info = new ProcessInfo[proc_info_needed];
                            proc_info = proc_info_needed;
                            if (RmGetList(handle, out proc_info_needed, ref proc_info, process_info, ref reboot_reasons) != 0)
                                throw new InvalidOperationException("Невозможно выполнить перечисление процессов, блокирующих файл");

                            foreach (var id in process_info.Select(p => p.Process.ProcessId))
                            {
                                Process process = null;
                                try
                                {
                                    process = Process.GetProcessById(id);
                                }
                                catch (ArgumentException) { }

                                if (process != null)
                                    yield return process;
                            }
                            break;
                        }
                    default:
                        throw new InvalidOperationException("Невозможно выполнить перечисление прцоессов, блокирующих файл",
                            new InvalidOperationException("Невозможно определить размер результата"));
                }
            }
            finally
            {
                RmEndSession(handle);
            }
        }

        public static Task WaitFileLockAsync(this FileInfo file, CancellationToken Cancel = default) => file
           .EnumLockProcesses()
           .Select(process => process.WaitAsync(Cancel))
           .WhenAll();

        public static async Task<bool> WaitFileLockAsync(this FileInfo file, int Timeout, CancellationToken Cancel = default)
        {
            var processes = file.EnumLockProcesses().Select(process => process.WaitAsync(Cancel));
            var process_wait = Task.WhenAll(processes);
            var delay_task = Task.Delay(Timeout, Cancel);
            var task = await Task.WhenAny(process_wait, delay_task).ConfigureAwait(false);
            return task != delay_task;
        }

        /// <summary>Заблокирован ли файл другим процессом?</summary>
        /// <param name="file">Проверяемый файл</param>
        /// <returns>Истина, если файл заблокирован другим процессом</returns>
        public static bool IsLocked(this FileInfo file) => file.EnumLockProcesses().Any();

        [StructLayout(LayoutKind.Sequential)]
        private struct UniqueProcess
        {
            // The product identifier (PID). 
            public readonly int ProcessId;
            // The creation time of the process. 
            private readonly Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }
        /// <summary>Describes an application that is to be registered with the Restart Manager</summary> 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct ProcessInfo
        {
            private const int __MaxAppName = 255;
            private const int __MaxSvcName = 63;

            // Contains an UniqueProcess structure that uniquely identifies the 
            // application by its PID and the time the process began. 
            public readonly UniqueProcess Process;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = __MaxAppName + 1)]
            // If the process is a service, this parameter returns the  
            // long name for the service. 
            private readonly string AppName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = __MaxSvcName + 1)]
            // If the process is a service, this is the short name for the service. 
            private readonly string ServiceShortName;
            // Contains an AppType enumeration value. 
            private readonly AppType ApplicationType;
            // Contains a bit mask that describes the current status of the application. 
            private readonly uint AppStatus;
            // Contains the Terminal Services session ID of the process. 
            private readonly uint SessionId;
            // TRUE if the application can be restarted by the  
            // Restart Manager; otherwise, FALSE. 
            [MarshalAs(UnmanagedType.Bool)] private readonly bool Restartable;
        }
        /// <summary> 
        /// Specifies the type of application that is described by 
        /// the ProcessInfo structure. 
        /// </summary> 
        private enum AppType
        {
            // The application cannot be classified as any other type. 
            Unknown = 0,
            // A Windows application run as a stand-alone process that 
            // displays a top-level window. 
            MainWindow = 1,
            // A Windows application that does not run as a stand-alone 
            // process and does not display a top-level window. 
            OtherWindow = 2,
            // The application is a Windows service. 
            Service = 3,
            // The application is Windows Explorer. 
            Explorer = 4,
            // The application is a stand-alone console application. 
            Console = 5,
            // A system restart is required to complete the installation because 
            // a process cannot be shut down. 
            Critical = 1000
        }

        #endregion
        internal static Task<int> WaitAsync(this Process process, CancellationToken Cancel)
        {
            if (Cancel.IsCancellationRequested) return Task.FromCanceled<int>(Cancel);

            var tcs = new TaskCompletionSource<int>();
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => tcs.TrySetResult(process.ExitCode);
            if (process.HasExited) tcs.TrySetResult(process.ExitCode);
            else if (Cancel.CanBeCanceled)
                Cancel.Register(cancel => tcs.TrySetCanceled((CancellationToken)cancel), Cancel);

            return tcs.Task;
        }
        internal static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) => Task.WhenAll<T>(tasks);

    }
}